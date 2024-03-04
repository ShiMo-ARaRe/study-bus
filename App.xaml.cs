using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace 公交路线管理
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {


    }

    // 路线类
    public class BusRoute
    {
        // 路线的属性
        public string RouteNumber { get; set; } // 路线编号
        public string DriverName { get; set; } // 司机姓名
        public int TotalStops { get; set; } // 途经站台总数
        public List<string> Stops { get; set; } // 站台名称列表
        public string StartTime { get; set; } // 运营起始时间
        public string EndTime { get; set; } // 运营终止时间
        public double Fare { get; set; } // 票价

        // 构造函数，根据参数初始化公交线路对象
        public BusRoute(string number, string driver, int stopsCount, List<string> stopList, string start, string end, double ticketFare)
        {
            RouteNumber = number;
            DriverName = driver;
            TotalStops = stopsCount;
            Stops = stopList;
            StartTime = start;
            EndTime = end;
            Fare = ticketFare;
        }

        // 检查是否经过某站点
        public bool HasStop(string stop)
        {
            //Contains是一个字符串或集合类型的方法，用于确定目标字符串或元素是否存在于当前字符串或集合中。
            return Stops.Contains(stop);
        }

        // 公交线路的其他成员函数
        // ...
    }

    // 站点类
    public class BusStop
    {
        // 站点的属性
        public string StopName { get; set; } // 站点名称
        public List<BusRoute> Routes { get; set; } // 所属的公交线路列表

        // 构造函数，根据参数初始化公交站点对象
        public BusStop(string name)
        {
            StopName = name;
            Routes = new List<BusRoute>();
        }

        // 向站点的路线列表中添加路线
        public void AddRoute(BusRoute route)
        {
            if (!Routes.Contains(route))
            {
                Routes.Add(route);
            }
        }

        // 从站点的路线列表中删除路线
        public void RemoveRoute(BusRoute route)
        {
            Routes.Remove(route);
        }

        // 公交站点的其他成员函数
        // ...
    }

    // 管理员操作类
    public class BackendSystem
    {
        // 公交线路的哈希表，以路线编号为键
        private Dictionary<string, BusRoute> routes;
        // 公交站点的哈希表，以站点名称为键
        private Dictionary<string, BusStop> stops;

        // 构造函数，从CSV文件中读取数据
        public BackendSystem()
        {
            // 初始化哈希表
            routes = new Dictionary<string, BusRoute>();
            stops = new Dictionary<string, BusStop>();

            // 从 BusStop.csv 文件中读取站点数据
            using (StreamReader file = new StreamReader("BusStop.csv"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    // 以逗号分隔站点名称
                    string stopName = line.Split(',')[0];
                    // 调用添加站点的方法
                    AddBusStop(stopName);
                }
            }

            // 从 BusRoute.csv 文件中读取路线数据
            using (StreamReader file = new StreamReader("BusRoute.csv"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
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
                    AddBusRoute(routeNumber, driverName, stopsCount, stopList, startTime, endTime, ticketFare);
                }
            }
        }

        // 添加路线
        public string AddBusRoute(string number, string driver, int stopsCount, List<string> stopList, string start, string end, double ticketFare)
        {
            StringBuilder sb = new StringBuilder();

            // 检查站台列表中的每个站点是否存在
            foreach (string stop in stopList)
            {
                if (!stops.ContainsKey(stop))
                {
                    sb.AppendLine("操作失败！" + stop + " 不存在。");
                    return sb.ToString();
                }
            }

            // 创建公交线路对象并添加到哈希表
            if (routes.ContainsKey(number))
            {
                sb.AppendLine("操作失败！" + "路线编号已存在，请为新路线更换编号。");
                return sb.ToString();
            }

            BusRoute route = new BusRoute(number, driver, stopsCount, stopList, start, end, ticketFare);
            routes[number] = route;

            // 更新公交站点对象的路线列表
            foreach (string stop in stopList)
            {
                if (stops.ContainsKey(stop))
                {
                    stops[stop].AddRoute(route);
                }
                else
                {
                    sb.AppendLine(stop + " 不存在。");
                }
            }

            sb.AppendLine("路线 " + number + " 添加成功。");
            return sb.ToString();
        }

        // 添加站点
        public string AddBusStop(string name)
        {
            StringBuilder sb = new StringBuilder();

            // 创建公交站点对象并添加到哈希表
            if (!stops.ContainsKey(name))
            {
                BusStop stop = new BusStop(name);
                stops[name] = stop;
                sb.AppendLine("站点 " + name + " 添加成功。");
            }
            else
            {
                sb.AppendLine("操作失败！" + name + " 已存在。");
            }

            return sb.ToString();
        }

        // 删除路线
        public string RemoveBusRoute(string number)
        {
            StringBuilder sb = new StringBuilder();

            // 从哈希表中删除公交线路对象
            if (routes.ContainsKey(number))
            {
                BusRoute route = routes[number];
                routes.Remove(number);

                // 从公交站点对象的路线列表中删除对应路线
                foreach (string stop in route.Stops)
                {
                    if (stops.ContainsKey(stop))
                    {
                        stops[stop].RemoveRoute(route);
                    }
                }

                sb.AppendLine("路线 " + number + " 删除成功。");
            }
            else
            {
                sb.AppendLine("操作失败！" + "路线 " + number + " 不存在。");
            }

            return sb.ToString();
        }

        //打印所有路线信息
        public void PrintAllRoutes()
        {
            // 遍历哈希表，打印所有路线的信息
            foreach (var pair in routes)
            {
                Console.WriteLine("路线编号: " + pair.Key);
                BusRoute route = pair.Value;
                Console.WriteLine("司机: " + route.DriverName);
                Console.WriteLine("总站数: " + route.TotalStops);
                Console.WriteLine("站台列表: ");
                foreach (var stop in route.Stops)
                {
                    Console.Write(stop + " ");
                }
                Console.WriteLine("开始时间: " + route.StartTime);
                Console.WriteLine("结束时间: " + route.EndTime);
                Console.WriteLine("票价: " + route.Fare);
                Console.WriteLine();
            }
        }

        //打印所有站点信息
        public void PrintAllStops()
        {
            // 遍历哈希表，打印所有站点的信息
            foreach (var pair in stops)
            {
                Console.WriteLine("站点名称: " + pair.Key);
                BusStop stop = pair.Value;
                Console.Write("路线编号: ");
                foreach (var route in stop.Routes)
                {
                    Console.Write(route.RouteNumber + " ");
                }
                Console.WriteLine();
            }
        }

        //提交事务
        public string CommitTransaction()
        {
            // 写入路线数据
            using (StreamWriter routeFile = new StreamWriter("BusRoute.csv", false))
            {
                foreach (var entry in routes)
                {
                    BusRoute route = entry.Value;
                    routeFile.Write(route.RouteNumber + ",");
                    routeFile.Write(route.DriverName + ",");
                    routeFile.Write(route.TotalStops + ",");

                    List<string> routeStops = route.Stops;
                    for (int i = 0; i < routeStops.Count; i++)
                    {
                        routeFile.Write(routeStops[i]);
                        if (i < routeStops.Count - 1)
                        {
                            routeFile.Write(";");
                        }
                    }
                    routeFile.Write(",");
                    routeFile.Write(route.StartTime + ",");
                    routeFile.Write(route.EndTime + ",");
                    routeFile.Write(route.Fare + "\n");
                }
            }

            // 写入站台数据
            using (StreamWriter stopFile = new StreamWriter("BusStop.csv", false))
            {
                foreach (var entry in stops)
                {
                    BusStop stop = entry.Value;
                    stopFile.Write(stop.StopName + ",");
                    List<BusRoute> stopRoutes = stop.Routes;
                    for (int i = 0; i < stopRoutes.Count; i++)
                    {
                        stopFile.Write(stopRoutes[i].RouteNumber);
                        if (i < stopRoutes.Count - 1)
                        {
                            stopFile.Write(";");
                        }
                    }
                    stopFile.Write("\n");
                }
            }

            return "修改源文件成功！";
        }

        //用于初始化用户操作类
        public Dictionary<string, BusRoute> Getroutes()
        {
            return routes;
        }
        // 其他后台管理系统的功能函数
        // ...
    }

    // 用户操作类
    public class FrontendService
    {
        // 公交线路的哈希表，以路线编号为键
        private Dictionary<string, BusRoute> routes;

        // 构造函数，接收后台系统传递的路线信息
        public FrontendService(Dictionary<string, BusRoute> routes)
        {
            // 将路线信息赋值给本地变量
            this.routes = routes;
        }

        // 查询公交线路信息
        public string QueryBusRoute(string number)
        {
            StringBuilder sb = new StringBuilder();

            // 查询公交线路信息
            if (routes.ContainsKey(number))
            {
                BusRoute route = routes[number];
                List<string> stops = route.Stops;

                sb.AppendLine("线路号：" + route.RouteNumber);
                sb.AppendLine("司机：" + route.DriverName);
                sb.AppendLine("途经站台总数：" + route.TotalStops);
                sb.AppendLine("站台列表：");
                foreach (string stop in stops)
                {
                    sb.Append(stop + " ");
                }
                sb.AppendLine();
                sb.AppendLine("起始时间：" + route.StartTime);
                sb.AppendLine("终止时间：" + route.EndTime);
                sb.AppendLine("票价：" + route.Fare);
            }
            else
            {
                sb.AppendLine("线路 " + number + " 不存在。");
            }

            // 将输出文本赋值返回
            return sb.ToString();
        }

        // 乘车方案
        public string QueryTravelPlan(string start, string end)
        {
            StringBuilder sb = new StringBuilder();

            // 检查起点和终点是否在同一条公交线路上
            List<BusRoute> startRoutes = GetRoutesForStop(start);
            List<BusRoute> endRoutes = GetRoutesForStop(end);

            List<BusRoute> commonRoutes = new List<BusRoute>();
            foreach (BusRoute startRoute in startRoutes)
            {
                foreach (BusRoute endRoute in endRoutes)
                {
                    if (startRoute == endRoute)
                    {
                        commonRoutes.Add(startRoute);
                    }
                }
            }

            if (commonRoutes.Count == 0)
            {
                sb.AppendLine("无法找到从 " + start + " 到 " + end + " 的乘车方案");
            }
            else
            {
                sb.AppendLine("从 " + start + " 到 " + end + " 的乘车方案：");
                foreach (BusRoute route in commonRoutes)
                {
                    sb.AppendLine("线路：" + route.RouteNumber);
                    sb.AppendLine("司机：" + route.DriverName);
                    sb.AppendLine("途经站台总数：" + route.TotalStops);
                    sb.AppendLine("站台列表：");
                    List<string> stops = route.Stops;
                    for (int i = 0; i < stops.Count; i++)
                    {
                        sb.Append(stops[i]);
                        if (i < stops.Count - 1)
                        {
                            sb.Append(" -> ");
                        }
                    }
                    sb.AppendLine();
                    sb.AppendLine("运营起始时间：" + route.StartTime);
                    sb.AppendLine("运营终止时间：" + route.EndTime);
                    sb.AppendLine("票价：" + route.Fare);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        // 最短路线
        public string QueryOptimalTravel(string start, string end)
        {
            StringBuilder sb = new StringBuilder();

            // 输出最短乘车时间和最短乘车方案
            List<BusRoute> startRoutes = GetRoutesForStop(start);
            List<BusRoute> endRoutes = GetRoutesForStop(end);
            List<BusRoute> commonRoutes = new List<BusRoute>();
            foreach (BusRoute startRoute in startRoutes)
            {
                foreach (BusRoute endRoute in endRoutes)
                {
                    if (startRoute == endRoute)
                    {
                        commonRoutes.Add(startRoute);
                    }
                }
            }

            if (commonRoutes.Count == 0)
            {
                sb.AppendLine("无法找到从 " + start + " 到 " + end + " 的乘车方案");
            }
            else
            {
                long minDistance = long.MaxValue;
                BusRoute optimalRoute = null;

                foreach (BusRoute route in commonRoutes)
                {
                    List<string> stations = route.Stops;
                    int startIndex = stations.IndexOf(start);
                    int endIndex = stations.IndexOf(end);
                    long d = Math.Abs(startIndex - endIndex);

                    if (d < minDistance)
                    {
                        minDistance = d;
                        optimalRoute = route;
                    }
                }

                sb.AppendLine("最短距离：" + minDistance);
                if (optimalRoute != null)
                {
                    sb.AppendLine("最佳路线：" + optimalRoute.RouteNumber);
                    sb.AppendLine("司机：" + optimalRoute.DriverName);
                    sb.AppendLine("途经站台总数：" + optimalRoute.TotalStops);
                    sb.AppendLine("站台列表：");
                    List<string> stops = optimalRoute.Stops;
                    for (int i = 0; i < stops.Count; i++)
                    {
                        sb.Append(stops[i]);
                        if (i < stops.Count - 1)
                        {
                            sb.Append(" -> ");
                        }
                    }
                    sb.AppendLine();
                    sb.AppendLine("运营起始时间：" + optimalRoute.StartTime);
                    sb.AppendLine("运营终止时间：" + optimalRoute.EndTime);
                    sb.AppendLine("票价：" + optimalRoute.Fare);
                }
                else
                {
                    sb.AppendLine("没有找到最佳路线。");
                }
            }

            return sb.ToString();
        }

        // 找出所有经过某站点的路线
        public List<BusRoute> GetRoutesForStop(string stop)
        {
            // 找出所有经过某站点的路线
            List<BusRoute> result = new List<BusRoute>();
            foreach (var pair in routes)
            {
                BusRoute route = pair.Value;
                if (route.HasStop(stop))
                {
                    result.Add(route);
                }
            }
            return result;
        }

        // 其他前台服务的功能函数
        // ...
    }
}
