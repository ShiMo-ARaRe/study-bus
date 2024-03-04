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
    /// 管理员窗口.xaml 的交互逻辑
    /// </summary>
    public partial class 管理员窗口 : Window
    {
        // 创建后台系统对象
        private static BackendSystem backendSystem = new BackendSystem();
        // 创建前台服务对象
        private static FrontendService frontendService = new FrontendService(backendSystem.Getroutes());
        public 管理员窗口()
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

        private void AddStopButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里是增加公交站点按钮点击事件的处理逻辑
            string stopName = AddStopTextBox.Text;
            // 使用stopName增加公交站点，并将结果显示在窗口上
            修改信息.Text = backendSystem.AddBusStop(stopName);
        }

        private void AddRouteButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里是增加公交线路按钮点击事件的处理逻辑
            string line = AddRouteTextBox.Text;
            // 使用routeName增加公交线路，并将结果显示在窗口上
  
                // 以逗号分隔路线信息
                string[] fields = line.Split(',');
                // 获取路线编号、司机姓名、站台总数、站台列表、起始时间、终止时间和票价
                string routeNumber = fields[0];
                string driverName = fields[1];
                int stopsCount = int.Parse(fields[2]);
                List<string> stopList = fields[3].Split(';').ToList(); // 以分号分隔站台名称
                string startTime = fields[4];
                string endTime = fields[5];
                double ticketFare = double.Parse(fields[6]);//类型转换
            
            // 调用添加路线的方法
            修改信息.Text = backendSystem.AddBusRoute(routeNumber, driverName, stopsCount, stopList, startTime, endTime, ticketFare);
            

        }

        private void RemoveRouteButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里是删除公交线路按钮点击事件的处理逻辑
            string routeName = RemoveRouteTextBox.Text;
            // 使用routeName删除公交线路，并将结果显示在窗口上
            修改信息.Text = backendSystem.RemoveBusRoute(routeName);
         
        }

        private void CommitTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里是提交事务按钮点击事件的处理逻辑
            // 提交事务，并将结果显示在窗口上
            修改信息.Text = backendSystem.CommitTransaction();
        }
    }
}
