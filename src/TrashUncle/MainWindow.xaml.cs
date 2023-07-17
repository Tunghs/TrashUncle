using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            this.PreviewDrop += MainWindow_PreviewDropAsync;
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
            }
            catch (Exception ex)
            {

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
            }
            catch (Exception ex)
            {

            }
        }
    }
}
