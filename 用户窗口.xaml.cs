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

namespace 公交路线管理
{
    /// <summary>
    /// 用户窗口.xaml 的交互逻辑
    /// </summary>
    public partial class 用户窗口 : Window
    {
        // 创建后台系统对象
        private static BackendSystem backendSystem = new BackendSystem();
        // 创建前台服务对象
        private static FrontendService frontendService = new FrontendService(backendSystem.Getroutes());
        public 用户窗口()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen; // 设置窗口的启动位置为屏幕中央
         
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里是返回按钮点击事件的处理逻辑
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BusRouteButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里是公交线路信息查询按钮点击事件的处理逻辑
            string busRoute = BusRouteTextBox.Text;
            // 使用busRoute查询公交线路信息，并将结果显示在窗口上
            查询信息.Text = frontendService.QueryBusRoute(busRoute);
        }

        private void TravelPlanButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里是乘车线路方案查询按钮点击事件的处理逻辑
            string start = TravelPlanStartTextBox.Text;
            string end = TravelPlanEndTextBox.Text;
            // 使用start和end查询乘车线路方案，并将结果显示在窗口上
            查询信息.Text = frontendService.QueryTravelPlan(start, end);
        }

        private void OptimalTravelButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里是乘车线路最优查询按钮点击事件的处理逻辑
            string start = OptimalTravelStartTextBox.Text;
            string end = OptimalTravelEndTextBox.Text;
            // 使用start和end查询乘车线路最优方案，并将结果显示在窗口上
            查询信息.Text = frontendService.QueryOptimalTravel(start,end);
        }
    }
}
