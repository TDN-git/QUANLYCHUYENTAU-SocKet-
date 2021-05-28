using Client_Train.Data;
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
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Client_Train
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Route> Listdata;
        IPEndPoint ipe;
        TcpClient tcpclient;
        Stream stream;
        bool isconect = false;
        List<string> listgia;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Showdata(object sender, MouseButtonEventArgs e)
        {
            int a =  data.SelectedIndex;

            for(int i = 0;i < Listdata.Count;i++)
            {
                if(i == a)
                {
                    machuyen.Text = Listdata[a].MSC.ToString();
                    tenchuyen.Text = Listdata[a].TenChuyen;
                    loaive.Text = Listdata[a].LoaiVe;
                    giave.Text = Listdata[a].Giave.ToString();
                    soluongveton.Text = Listdata[a].SoLuong.ToString();
                }    
            }
        }

        private void count_up(object sender, RoutedEventArgs e)
        {
            int s = 1;
            if(int.Parse(soluong.Text) < int.Parse(soluongveton.Text) && soluongveton.Text != "0")
            {
                soluong.Text = (s + int.Parse(soluong.Text)).ToString();
            }
            else
            {
                soluong.Text = soluongveton.Text;
            }
            
        }

        private void Count_down(object sender, RoutedEventArgs e)
        {
            int s = 1;
            if(int.Parse(soluong.Text) > 0)
            {
                soluong.Text = (int.Parse(soluong.Text) - s).ToString();
            }   
            
        }

        private void Buy_ticket(object sender, RoutedEventArgs e)
        {
            float tonggia = 0;
            float giagoc = 0;
            int sl = 0;
            if (soluong.Text == "0")
            {
                thongbao.Text = "CAN'T SEND: Chưa Nhập Số Lượng Mua !!!";
                
            }
            else
            {
                if(isconect == true)
                {
                    send();
                    thongbao.Text = "Successfuly !!!";
                    listgia = new List<string>();
                    tonggia = int.Parse(soluong.Text) * float.Parse(giave.Text);
                    gia.Text = tonggia.ToString();
                    listgia.Add(tonggia.ToString());
                }
                else
                {
                    thongbao.Text = "Chưa kết nối server";
                }
                
            }
        }

        void conect()
        {
            ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
            tcpclient = new TcpClient();
            tcpclient.Connect(ipe);
            stream = tcpclient.GetStream();

            Thread thread = new Thread(receive);
            thread.IsBackground = true;
            thread.Start();
        }

        void receive()
        {
            
            while (true)
            {
                byte[] data = new byte[1024];
                stream.Read(data, 0, data.Length);
                stream.Flush();
                string receive_data = Encoding.ASCII.GetString(data);
                addmessage(receive_data);
            }

        }

        void send()
        {
            if(tcpclient.Connected == true)
            {
                byte[] data = Encoding.ASCII.GetBytes(machuyen.Text + " - " + loaive.Text + " - " + soluong.Text);
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
        }

        
        List<Data_package> list_receive;
        void addmessage(string message)
        {
            Dispatcher.BeginInvoke(
            new ThreadStart(() => listchat.Items.Add(message)));

            int s = 0;
            float giagoc = 0;

            list_receive = new List<Data_package>();
            for (int j = 0; j < listchat.Items.Count; j++)
            {
                var tokens = listchat.Items[j].ToString().Split(new string[] { " - " }, StringSplitOptions.None);
                if(tokens[0] == "Accept")
                {
                    var item = new Data_package()
                    {
                        ms = int.Parse(tokens[1]),
                        loai = tokens[2],
                        soluong = int.Parse(tokens[3])
                    };

                    list_receive.Add(item);
                }

            }

            int n = Listdata.Count;
            for (int m = 0; m < list_receive.Count;m++)
            {
                    for (int i = 0; i < n; i++)
                    {
                        if (list_receive[m].ms == Listdata[i].MSC && list_receive[m].loai == Listdata[i].LoaiVe)
                        {
                            var dt = new Route()
                            {
                                MSC = Listdata[i].MSC,
                                TenChuyen = Listdata[i].TenChuyen,
                                SoLuong = list_receive[m].soluong,
                                Giave = Listdata[i].Giave,
                                LoaiVe = Listdata[i].LoaiVe
                            };
                            Listdata.RemoveAt(i);
                            Listdata.Add(dt);
                            n = n - 1;
                        }
                    }

            }

            Dispatcher.BeginInvoke(new ThreadStart(() => data.ItemsSource = Listdata));
        }

        private void show_cost(object sender, MouseButtonEventArgs e)
        {
            
            int index = listchat.SelectedIndex;
            for (int i = 0; i < listgia.Count(); i++)
            {
                if (i == index)
                {
                    gia.Text = listgia[i];
                }
            }

            
        }

        void disconnect()
        {
            //tcpclient.Close();
            return;
            
        }
        private void connect_server(object sender, RoutedEventArgs e)
        {
            conect();
            MessageBox.Show("Đã kết nối đến server");
            isconect = true;
        }

        private void disconnect_server(object sender, RoutedEventArgs e)
        {
            disconnect();
            MessageBox.Show("Đã ngắt kết nối đến server");
            isconect = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Listdata = new List<Route>();
            Listdata = Read_Database.read_file();
            data.ItemsSource = Listdata;
        }
    }
}
