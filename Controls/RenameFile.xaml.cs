using Chopan.Pages;
using System;
using System.Collections.Generic;
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

namespace Chopan.Controls
{
    /// <summary>
    /// CreateNewDirectory.xaml 的交互逻辑
    /// </summary>
    public partial class RenameFile : Window
    {
        string path;
        public RenameFile(string DirectoryPath)
        {
            path = DirectoryPath;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Minz_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Cloiz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            if(FileName.Text == "")
            {
                MessageBox.Show("文件夹名称不能为空！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                ApplicationValues.ChopanClient.RenameFile(path, FileName.Text);
                ApplicationValues.Menu.Flush();
                this.Close();
            }
        }
    }
}
