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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chopan.Controls
{
    /// <summary>
    /// DirectoryBox.xaml 的交互逻辑
    /// </summary>
    public partial class DirectoryBox : UserControl
    {
        public DirectoryBox(string Name)
        {
            InitializeComponent();
            this.Loaded += (sender, e) =>
            {
                this.FN.Text = Name;
            };
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除此文件夹？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ApplicationValues.ChopanClient.DeleteDirectory($"{ApplicationValues.Menu.DirectoryPathShow.Text}{FN.Text}");
                ApplicationValues.Menu.Flush();
            }
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            RenameDirectory renameDirectory = new RenameDirectory($"{ApplicationValues.Menu.DirectoryPathShow.Text}{FN.Text}");
            renameDirectory.ShowDialog();
        }
    }
}
