using Server_Train.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace Server_Train
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , INotifyPropertyChanged
    {
        List<Route> Routes;
        List<Route> temp;
        List<Data_package> list_receive;
        List<Route> Dataofclient;
        IPEndPoint ipe;
        Socket client;
        TcpListener tcpclient;
        string s;

        public ObservableCollection<string> trip
        {
            set;  get;
        }
        public ObservableCollection<string> kind
        {
            set; get;
        }
        public MainWindow()
        {
            InitializeComponent();
            Routes = Data.Read_Database.read_file();
            
            

            trip = new ObservableCollection<string>();
            for (int i = 0; i < Routes.Count; i++)
            {
                trip.Add(Routes[i].TenChuyen);
            }

            kind = new ObservableCollection<string>();
            for (int i = 0; i < Routes.Count; i++)
            {
                kind.Add(Routes[i].LoaiVe);
            }

            conect();
        }

        private void Accept_Train(object sender, RoutedEventArgs e)
        {
            float s = 0;
            for (int k = 0; k < Routes.Count; k++)
            {
                if (Dataofclient[Listdata.SelectedIndex].MSC == Routes[k].MSC && Dataofclient[Listdata.SelectedIndex].LoaiVe == Routes[k].LoaiVe)
                {
                    s = Routes[k].SoLuong - Dataofclient[Listdata.SelectedIndex].SoLuong;
                }
            }
            string notifi = "Accept - " + Dataofclient[Listdata.SelectedIndex].MSC.ToString() + " - " + Dataofclient[Listdata.SelectedIndex].LoaiVe + " - " + s.ToString();
            send(client,notifi);


        }

        private void Decline_Train(object sender, RoutedEventArgs e)
        {
            string notifi = "Decline - " + Dataofclient[Listdata.SelectedIndex].MSC.ToString() + " - " + Dataofclient[Listdata.SelectedIndex].LoaiVe;
            send(client,notifi);
        }

        

        private void loaichuyen_changes(object sender, SelectionChangedEventArgs e)
        {
            
            int id = Routes[loaichuyenchange.SelectedIndex].MSC;
            temp = new List<Route>();
            for (int i = 0; i < Dataofclient.Count; i++)
            {
                if(Dataofclient[i].MSC == id)
                {
                    temp.Add(Dataofclient[i]);
                }
            }

            Tenchuyentau.Text = Routes[loaichuyenchange.SelectedIndex].TenChuyen.Replace(" - ","----------->------------");
            Listdata.ItemsSource = temp;
        }


        private void loaive_changes(object sender, SelectionChangedEventArgs e)
        {
            string loai = Routes[loaivechange.SelectedIndex].LoaiVe;
            temp = new List<Route>();
            for (int i = 0; i < list_receive.Count; i++)
            {
                if (Dataofclient[i].LoaiVe == loai)
                {
                    temp.Add(Dataofclient[i]);
                }
            }

            Listdata.ItemsSource = temp;
        }

        public event PropertyChangedEventHandler
           PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Seen(object sender, MouseButtonEventArgs e)
        {
            Tenchuyentau.Text = Routes[Listdata.SelectedIndex].TenChuyen;
        }

        

        private void Load_data(object sender, RoutedEventArgs e)
        {
            list_receive = new List<Data_package>();
            for(int j = 1; j < listchat.Items.Count;j++)
            {
                var tokens = listchat.Items[j].ToString().Split(new string[] { " - " }, StringSplitOptions.None);

                var item = new Data_package()
                {
                    ms = int.Parse(tokens[0]),
                    loai = tokens[1],
                    soluong = int.Parse(tokens[2])
                };

                list_receive.Add(item);
                
            }    
            List<Route> list = new List<Route>();
            Dataofclient = new List<Route>();
            list = Data.Read_Database.read_file();
            for (int i = 0; i < list_receive.Count; i++)
            {
                for(int k = 0; k < list.Count; k++)
                {
                    if (list_receive[i].ms == list[k].MSC && list_receive[i].loai == list[k].LoaiVe)
                    {
                        Route a = new Route()
                        {
                            MSC = list[k].MSC,
                            LoaiVe = list[k].LoaiVe,
                            GiaVe = list[k].GiaVe,
                            TenChuyen = list[k].TenChuyen,
                            SoLuong = list_receive[i].soluong
                        };
                        Dataofclient.Add(a);
                    }
                }
            }

            Listdata.ItemsSource = Dataofclient;
            
        }

        

        void conect()
        {
            ipe = new IPEndPoint(IPAddress.Any, 9999);
            tcpclient = new TcpListener(ipe);



            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    tcpclient.Start();
                    client = tcpclient.AcceptSocket();
                    Thread rec = new Thread(receive);
                    rec.IsBackground = true;
                    rec.Start(client);
                }
            });

            thread.Start();
            thread.IsBackground = true;

        }

        void receive(object obj)
        {
            while (true)
            {
                Socket cli = obj as Socket;
                byte[] data = new byte[1024];

                client.Receive(data);

                s = Encoding.UTF8.GetString(data);
                addmessage(s);
            }

        }

        void send(Socket clie,string notify)
        {
            byte[] data = Encoding.UTF8.GetBytes(notify);
            clie.Send(data);
        }

        void addmessage(string message)
        {
            Dispatcher.BeginInvoke(
            new ThreadStart(() => listchat.Items.Add(message)));
        }
    }
    
}
