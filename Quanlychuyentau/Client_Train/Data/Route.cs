using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Train.Data
{
    public class Route : INotifyPropertyChanged
    {

        private int _MSC;
        public int MSC
        {
            get { return _MSC; }
            set
            {
                _MSC = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs("MSC"));
            }
        }

        private string _TenChuyen;
        public string TenChuyen
        {
            get { return _TenChuyen; }
            set
            {
                _TenChuyen = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs("TenChuyen"));
            }
        }

        private string _LoaiVe;
        public string LoaiVe
        {
            get { return _LoaiVe; }
            set
            {
                _LoaiVe = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs("LoaiVe"));
            }
        }

        private int _SoLuong;
        public int SoLuong
        {
            get { return _SoLuong; }
            set
            {
                _SoLuong = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs("SoLuong"));
            }
        }

        private float _GiaVe;
        public float Giave
        {
            get { return _GiaVe; }
            set
            {
                _GiaVe = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs("GiaVe"));
            }
        }

        public event PropertyChangedEventHandler
           PropertyChanged;
    
    }
}
