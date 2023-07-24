using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using static System.Net.Mime.MediaTypeNames;

namespace TrashUncle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.PreviewMouseLeftButtonDown += MainWindow_PreviewMouseLeftButtonDown;
            this.PreviewMouseRightButtonDown += MainWindow_PreviewMouseRightButtonDown;
            this.PreviewDrop += MainWindow_PreviewDropAsync;
            MessagePopup.CustomPopupPlacementCallback = new System.Windows.Controls.Primitives.CustomPopupPlacementCallback(SetPopupLocation);
        }

        private void MainWindow_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessagePopup.IsOpen = true;
        }

        /// <summary>
        /// popup의 위치를 설정하는 함수
        /// </summary>
        /// <param name="popupSize"></param>
        /// <param name="targetSize"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private CustomPopupPlacement[] SetPopupLocation(Size popupSize, Size targetSize, Point offset)
        {
            var centerX = 0.0;
            if (this.Left - ((MessagePopup.Width / 2) + 20) < 0)
            {
                centerX = (this.Width / 2) - 10;
            }
            else
            {
                centerX = (this.Width / 2) - 10;
            }

            CustomPopupPlacement placement1 = new CustomPopupPlacement(new Point(centerX, 55), PopupPrimaryAxis.Vertical);
            CustomPopupPlacement placement2 = new CustomPopupPlacement(new Point(0, 55), PopupPrimaryAxis.Horizontal);

            CustomPopupPlacement[] placementArray = new CustomPopupPlacement[] { placement1, placement2 };

            return placementArray;
        }

        private async void MainWindow_PreviewDropAsync(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            List<string> dropItemList = new List<string>();
            await Task.Run(() =>
            {
                var dropItems = (string[])e.Data.GetData(DataFormats.FileDrop);
                dropItemList = new List<string>(dropItems.ToList());
            });

            Delete(dropItemList[0]);
        }

        private void MainWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                MessagePopup.IsOpen = false;
                DragMove();
            }
        }

        private void Delete(string path)
        {
            if (File.Exists(path))
            {
                DeleteFile(path);
            }
            else if (Directory.Exists(path))
            {
                DeleteDirectory(path);
            }
        }

        private void DeleteFile(string filePath)
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                FileInfo fileInfo = new FileInfo(filePath);
                fileInfo.Delete();

                ShowMessage("삭제되었습니다.");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void DeleteDirectory(string dirPath)
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                dirInfo.Delete(true);

                ShowMessage("삭제되었습니다.");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void ShowMessage(string message)
        {
            MessageTbx.Text = message;
        }
    }
}
