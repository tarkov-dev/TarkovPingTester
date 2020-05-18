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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;

namespace TarkovPingTester
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ObservableCollection<ServerPingData> pingData;
        private delegate void UpdatePingTableDelegate();

        public MainWindow()
        {
            InitializeComponent();

            pingData = getServerList();

            PingTable.DataContext = pingData;

            SortDataGrid(PingTable, 0, ListSortDirection.Ascending);

            Task.Delay(1000).ContinueWith((task) => {
                startPingTest();
            });
        }

        private void startPingTest()
        {
            foreach (var server in pingData)
            {
                startPingTestServer(server);
            }
        }

        private void startPingTestServer(ServerPingData server)
        {
            if (server.TestCount + server.ErrorCount >= 10) return;

            Ping pingSender = new Ping();

            pingSender.PingCompleted += new PingCompletedEventHandler((sender, e) => PingCompletedCallback(sender, e, server));

            int timeout = 1000;

            PingOptions options = new PingOptions(64, true);

            int index = server.TestCount % server.ServerAddress.Length;

            pingSender.SendAsync(server.ServerAddress[index], timeout, options);
        }

        private void PingCompletedCallback(object sender, PingCompletedEventArgs e, ServerPingData server)
        {
            if (e.Cancelled)
            {
                //Debug.Print("Ping canceled.");

                DisplayReply(null, server);
                return;
            }

            if (e.Error != null)
            {
                //Debug.Print("Ping failed:");
                //Debug.Print(e.Error.ToString());

                DisplayReply(null, server);
                return;
            }

            PingReply reply = e.Reply;

            DisplayReply(reply, server);
            startPingTestServer(server);
        }

        private void DisplayReply(PingReply reply, ServerPingData server)
        {
            if (reply == null)
            {
                server.ErrorCount++;
                return;
            }

            //Debug.Print("ping status: {0}", reply.Status);
            if (reply.Status == IPStatus.Success)
            {
                decimal totalPing = server.AvgPing * server.TestCount;

                totalPing += reply.RoundtripTime;

                server.TestCount++;

                server.AvgPing = totalPing / server.TestCount;

                if (server.MinPing == 0 || server.MinPing > reply.RoundtripTime) server.MinPing = reply.RoundtripTime;
                if (server.MaxPing == 0 || server.MaxPing < reply.RoundtripTime) server.MaxPing = reply.RoundtripTime;

                //Debug.Print("Address: {0}", reply.Address.ToString());
                //Debug.Print("RoundTrip time: {0}", reply.RoundtripTime);
                //Debug.Print("Time to live: {0}", reply.Options.Ttl);
                //Debug.Print("Don't fragment: {0}", reply.Options.DontFragment);
                //Debug.Print("Buffer size: {0}", reply.Buffer.Length);
            }
            else
            {
                server.ErrorCount++;
            }

            if (!PingTable.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(new UpdatePingTableDelegate(UpdatePingTable));
                return;
            }
        }

        private void UpdatePingTable()
        {
            //
        }

        public static void SortDataGrid(DataGrid dataGrid, int columnIndex = 0, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            var column = dataGrid.Columns[columnIndex];

            // Clear current sort descriptions
            dataGrid.Items.SortDescriptions.Clear();

            // Add the new sort description
            dataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, sortDirection));

            // Apply sort
            foreach (var col in dataGrid.Columns)
            {
                col.SortDirection = null;
            }
            column.SortDirection = sortDirection;

            // Refresh items to display sort
            dataGrid.Items.Refresh();
        }

        private ObservableCollection<ServerPingData> getServerList()
        {
            var serverList = new ObservableCollection<ServerPingData>();

            serverList.Add(new ServerPingData
            {
                ServerRegion = "USA",
                ServerName = "Beauharnois",
                ServerAddress = new string[] { "192.99.18.148", "51.161.118.44" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "EUR",
                ServerName = "Warsaw",
                ServerAddress = new string[] { "217.182.201.220", "51.83.237.15" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0,
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "EUR",
                ServerName = "Strasbourg",
                ServerAddress = new string[] { "92.42.105.134", "92.42.104.238" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "EUR",
                ServerName = "Germany",
                ServerAddress = new string[] { "176.57.181.247", "176.57.181.233" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "ASI",
                ServerName = "Seoul",
                ServerAddress = new string[] { "92.223.73.8", "92.223.73.159" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "RUS",
                ServerName = "Moscow",
                ServerAddress = new string[] { "176.99.3.77", "176.99.3.52" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "USA",
                ServerName = "Miami",
                ServerAddress = new string[] { "23.82.136.213", "192.155.108.142" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "USA",
                ServerName = "StLouis",
                ServerAddress = new string[] { "148.72.173.234", "148.72.168.82" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "RUS",
                ServerName = "Ekaterinburg",
                ServerAddress = new string[] { "92.223.91.17", "92.223.91.8" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "RUS",
                ServerName = "SaintPetersburg",
                ServerAddress = new string[] { "5.8.78.115", "5.8.78.114" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "USA",
                ServerName = "WashingtonDC",
                ServerAddress = new string[] { "172.107.197.169", "198.7.59.197" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "ASI",
                ServerName = "HongKong",
                ServerAddress = new string[] { "209.58.191.183", "103.66.180.142" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "USA",
                ServerName = "Chicago",
                ServerAddress = new string[] { "108.62.107.8", "108.62.106.1" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "USA",
                ServerName = "Dallas",
                ServerAddress = new string[] { "45.35.66.69", "172.241.25.144" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "EUR",
                ServerName = "London",
                ServerAddress = new string[] { "95.168.177.233", "81.17.62.129" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "RUS",
                ServerName = "Nursultan",
                ServerAddress = new string[] { "94.247.134.219" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "USA",
                ServerName = "Seattle",
                ServerAddress = new string[] { "108.62.5.69", "23.19.87.236" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "USA",
                ServerName = "California",
                ServerAddress = new string[] { "23.80.6.49", "23.19.74.27" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "ME",
                ServerName = "Turkey",
                ServerAddress = new string[] { "92.38.180.18", "92.38.180.19" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "ASI",
                ServerName = "Singapore",
                ServerAddress = new string[] { "209.58.171.67", "209.58.169.122" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "RUS",
                ServerName = "Khabarovsk",
                ServerAddress = new string[] { "92.223.80.206", "92.223.80.78" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "EUR",
                ServerName = "Helsinki",
                ServerAddress = new string[] { "95.217.105.51", "95.216.36.208" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "OCE",
                ServerName = "Sydney",
                ServerAddress = new string[] { "103.101.129.148", "146.185.237.5" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "SAM",
                ServerName = "SanPaulo",
                ServerAddress = new string[] { "92.38.150.27", "92.38.150.37" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "ME",
                ServerName = "TelAviv",
                ServerAddress = new string[] { "5.188.95.64" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "EUR",
                ServerName = "Madrid",
                ServerAddress = new string[] { "5.188.148.17", "5.188.148.14" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "EUR",
                ServerName = "Netherlands",
                ServerAddress = new string[] { "81.171.15.122", "92.38.140.36" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "ASI",
                ServerName = "Tokyo",
                ServerAddress = new string[] { "5.188.71.113", "5.188.71.31" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "RUS",
                ServerName = "Krasnoyarsk",
                ServerAddress = new string[] { "92.223.87.12", "92.223.87.6" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "EUR",
                ServerName = "Milan",
                ServerAddress = new string[] { "92.38.174.13" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "AF",
                ServerName = "Johannesburg",
                ServerAddress = new string[] { "5.188.1.27", "5.188.1.26" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });
            serverList.Add(new ServerPingData
            {
                ServerRegion = "ASI",
                ServerName = "Taiwan",
                ServerAddress = new string[] { "43.251.182.44", "43.251.182.48" },
                MinPing = 0,
                AvgPing = 0,
                MaxPing = 0,
                TestCount = 0,
                ErrorCount = 0
            });

            return serverList;
        }
    }
}
