using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Train.Data
{
    public class Read_Database
    {
        public static List<Route> read_file()
        {
            List<Route> list = new List<Route>();

            string currentFolder = AppDomain.CurrentDomain.BaseDirectory;

            var baseFolder = currentFolder.Substring(0, currentFolder.Length - 23) + "Train.txt";
            var lines = File.ReadAllLines(baseFolder);
            int count = File.ReadAllLines(baseFolder).Count();

            for (var i = 0; i < count; i++)
            {
                var line = lines[i];
                var tokens = line.Split(new string[] { " * " }, StringSplitOptions.None);

                var route = new Route()
                {
                    MSC = int.Parse(tokens[0]),
                    TenChuyen = tokens[1],
                    LoaiVe = tokens[2],
                    SoLuong = int.Parse(tokens[3]),
                    GiaVe = float.Parse(tokens[4])

                };

                list.Add(route);
            }

            return list;
        }
    }
}
