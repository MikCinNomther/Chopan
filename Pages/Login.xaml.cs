using Chopan.Kernel;
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
using static Chopan.Kernel.ChopanClient;

namespace Chopan.Pages
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            this.Loaded += (sender, e) =>
            {
                ApplicationValues.MainCF.Taout.Visibility = Visibility.Hidden;
            };
        }
        Brush WhiteOp = new SolidColorBrush(Color.FromArgb(161, 255, 255, 255));
        Brush White = new SolidColorBrush(Color.FromArgb(245, 255, 255, 255));

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ApplicationValues.WindowState == WindowState.Normal)
            {
                this.Back.Background = WhiteOp;
            }
            else
            {
                this.Back.Background = White;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ChopanClient client = new ChopanClient(Username.Text, Password.Password);
            LoginState loginState = await client.GetStateAsync();
            if (loginState == ChopanClient.LoginState.Success)
            {
                ApplicationValues.ChopanClient = client;
                ApplicationValues.MainContent = new Menu();
            }
            else if (loginState == ChopanClient.LoginState.PasswordError)
            {
                MessageBox.Show("密码错误，请重试！", "登录失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (loginState == ChopanClient.LoginState.UserError)
            {
                MessageBox.Show("用户不存在，请检查用户名！", "登录失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (loginState == ChopanClient.LoginState.Locked)
            {
                MessageBox.Show("用户已被锁定，请联系管理员！", "登录失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (loginState == ChopanClient.LoginState.NetworkError)
            {
                MessageBox.Show("网络错误，请检查网络连接！", "登录失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (loginState == ChopanClient.LoginState.ServerError)
            {
                MessageBox.Show("服务器错误，请稍后重试！", "登录失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("未知错误，请稍后重试！", "登录失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
