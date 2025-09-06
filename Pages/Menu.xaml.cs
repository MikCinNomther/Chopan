using Chopan.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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

namespace Chopan.Pages
{
    /// <summary>
    /// Menu.xaml 的交互逻辑
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
            ApplicationValues.Menu = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationValues.MainCF.Taout.Visibility = Visibility.Visible;
            Flush();
        }

        public async void Flush()
        {
            FileDialog.Children.Clear();
            int r = 0;
            foreach (string FN in await ApplicationValues.ChopanClient.GetDirectoriesAsync(DirectoryPathShow.Text))
            {
                if(FN == "") continue;
                DirectoryBox directoryBox = new Controls.DirectoryBox(FN)
                {
                    Margin = new Thickness(0, (r++) * 50, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top
                };
                directoryBox.MouseDown += (sender, e) =>
                {
                    DirectoryPathShow.Text += $"{FN}/";
                    Flush();
                };
                this.FileDialog.Children.Add(directoryBox);
            }
            foreach (string FN in ApplicationValues.ChopanClient.GetFiles(DirectoryPathShow.Text))
            {
                if (FN == "") continue;
                this.FileDialog.Children.Add(new Controls.FileBox(FN)
                {
                    Margin = new Thickness(0, (r++) * 50, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top
                });
            }
        }

        private void ToRoot_Click(object sender, RoutedEventArgs e)
        {
            DirectoryPathShow.Text = "/";
            Flush();
        }

        private void Perious_Click(object sender, RoutedEventArgs e)
        {
            string path = DirectoryPathShow.Text;
            if (path == "/" || string.IsNullOrEmpty(path))
                return;

            // 移除最後的斜線
            if (path.EndsWith("/"))
                path = path.Substring(0, path.Length - 1);

            int lastSlash = path.LastIndexOf('/');
            if (lastSlash <= 0)
                DirectoryPathShow.Text = "/";
            else
                DirectoryPathShow.Text = path.Substring(0, lastSlash + 1);

            Flush();
        }

        private void MoveFileToIt(object sender, DragEventArgs e)
        {
            try
            {
                var fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                string rz = DirectoryPathShow.Text;
                if (File.Exists(fileName))
                    e.Effects = DragDropEffects.Copy;
                else
                {
                    e.Effects = DragDropEffects.None;
                    return;
                }
                Drst.AllowDrop = false;
                new Thread(() =>
                {
                    ApplicationValues.ChopanClient.Upload(fileName, rz + System.IO.Path.GetFileName(fileName));
                    this.Dispatcher.Invoke(() =>
                    {
                        Flush();
                        MessageBox.Show("上传成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                        Drst.AllowDrop = true;
                    });
                }).Start();
            }
            catch
            {

            }
        }
    }
}
