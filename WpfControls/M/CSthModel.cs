using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;


namespace WpfControls.M
{
    public class CSthModel:ObservableObject
    {
        private string _SNum;

        public string SNum
        {
            get { return _SNum; }
            set
            {
                _SNum = value;
                RaisePropertyChanged(nameof(SNum));
            }
        }

        private string _ErrCode;

        public string ErrCode
        {
            get { return _ErrCode; }
            set
            {
                _ErrCode = value;
                RaisePropertyChanged(nameof(ErrCode));
            }
        }
    }
}
