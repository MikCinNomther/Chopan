using Chopan.Kernel;
using Chopan.Pages;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chopan.Controls
{
    /// <summary>
    /// FileBox.xaml 的交互逻辑
    /// </summary>
    public partial class FileBox : UserControl
    {
        public FileBox(string Name)
        {
            InitializeComponent();
            this.Loaded += (sender, e) =>
            {
                this.FN.Text = Name;
            };
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("确认删除此文件？","确认",MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ApplicationValues.ChopanClient.DeleteFile($"{ApplicationValues.Menu.DirectoryPathShow.Text}{FN.Text}");
                ApplicationValues.Menu.Flush();
            }
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = FN.Text;
            saveFileDialog.Title = "选择保存位置";
            if ((bool)saveFileDialog.ShowDialog())
            {
                if(saveFileDialog.FileName != "")
                {
                    Downloader downloader = new Downloader($"{ApplicationValues.Menu.DirectoryPathShow.Text}{FN.Text}", saveFileDialog.FileName);
                    downloader.Show();
                }
            }
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            RenameFile renameFile = new RenameFile($"{ApplicationValues.Menu.DirectoryPathShow.Text}{FN.Text}");
            renameFile.ShowDialog();
        }
    }
}
