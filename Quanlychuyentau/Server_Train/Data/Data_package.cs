using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Train.Data
{
    public class Data_package
    {
        private int _ms;
        public int ms
        {
            get { return _ms; }
            set
            {
                _ms = value;
            }
        }
        private string _loai;
        public string loai
        {
            get { return _loai; }
            set
            {
                _loai = value;
            }
        }

        private int _soluong;
        public int soluong
        {
            get { return _soluong; }
            set
            {
                _soluong = value;
            }
        }
    }
}
