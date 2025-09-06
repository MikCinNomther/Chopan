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
        }
        int r = 0;
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            r = 0;
            ApplicationValues.MainCF.Taout.Visibility = Visibility.Visible;
            foreach(string FN in await ApplicationValues.ChopanClient.GetDirectoriesAsync())
            {
                this.FileDialog.Children.Add(new Controls.DirectoryBox(FN)
                {
                    Margin = new Thickness(0, (r++) * 50, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top
                });
            }
            foreach (string FN in ApplicationValues.ChopanClient.GetFiles("/"))
            {
                this.FileDialog.Children.Add(new Controls.FileBox(FN)
                {
                    Margin = new Thickness(0, (r++) * 50, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top
                });
            }
        }
    }
}
