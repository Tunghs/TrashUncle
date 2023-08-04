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

            PopupManage popupManage = new PopupManage(this, MessagePopup);

            this.PreviewMouseLeftButtonDown += MainWindow_PreviewMouseLeftButtonDown;
            this.PreviewMouseRightButtonDown += MainWindow_PreviewMouseRightButtonDown;
            this.PreviewDrop += MainWindow_PreviewDropAsync;
            MessagePopup.CustomPopupPlacementCallback = new System.Windows.Controls.Primitives.CustomPopupPlacementCallback(popupManage.GetPopupLocation);
        }

        private void MainWindow_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessagePopup.IsOpen = true;
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
