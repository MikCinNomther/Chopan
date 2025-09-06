using Chopan.Pages;
using Chopan.Source;
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
using System.Windows.Shapes;

namespace Chopan.Kernel
{
    /// <summary>
    /// Downloader.xaml 的交互逻辑
    /// </summary>
    public partial class Downloader : Window
    {
        string mp,sv;
        public Downloader(string Path,string SavePath)
        {
            sv = SavePath;
            mp = Path;
            InitializeComponent();
        }
        Thread DownloaderThread = null;
        private void Minz_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Cloiz_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("正在下载中，是否取消？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if(DownloaderThread != null && DownloaderThread.IsAlive)
                {
                    DownloaderThread.Abort();
                }
                Close();
            }
        }

        private void Titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Article.Text = Articlet.s[new Random().Next(0, Articlet.s.Length - 1)];
            DownloaderThread = new Thread(() =>
            {
                Thread.Sleep(1000);
                try
                {
                    byte[] buffer = ApplicationValues.ChopanClient.DownloadData(mp);
                    if (File.Exists(sv))
                    {
                        File.Delete(sv);
                    }
                    BinaryWriter binaryWriter = new BinaryWriter(File.Open(sv, FileMode.CreateNew));
                    binaryWriter.Write(buffer);
                    binaryWriter.Close();
                    this.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("下载完成", "完成", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    });
                }
                catch
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("下载失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            });
            DownloaderThread.Start();
        }
    }
}
