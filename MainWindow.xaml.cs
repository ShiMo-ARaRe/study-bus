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



namespace 公交路线管理
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen; // 设置窗口的启动位置为屏幕中央
        // 创建后台系统对象
        BackendSystem backendSystem = new BackendSystem();
        // 创建前台服务对象
        FrontendService frontendService = new FrontendService(backendSystem.Getroutes());
    }
        string adminPassword = "123456"; // 设置管理员密码
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里是登录按钮点击事件的处理逻辑
            if (PasswordTextBox.Password == adminPassword)
            {
                // 如果密码是"admin"，则打开管理员窗口
                管理员窗口 adminWindow = new 管理员窗口();
                adminWindow.Show();
                this.Close();
            }
            else
            {
                // 否则，显示密码输入错误的提示消息
                ErrorTextBlock.Text = "密码输入错误";
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            用户窗口 userWindow = new 用户窗口();
            userWindow.Show();
            this.Close();
        }
    }





}
