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
    }
}
