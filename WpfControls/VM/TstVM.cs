using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace WpfControls.VM
{
    public class TstVM:ViewModelBase
    {
        public TstVM()
        {
            BtContent = "BtContent_ABC";

            BtnCmd = new RelayCommand(ChangeContent);

            TxtCmd = new RelayCommand(ChangeContent2);

            txt = "";
        }

        private void ChangeContent()
        {
            LbContent = DateTime.Now.ToString();
            //txt = DateTime.Now.ToString();
        }

        private void ChangeContent2()
        {
            LbContent2 = txt;
        }

        private string _BtContent;
        public string BtContent
        {
            get
            {
                return _BtContent;
            }
            set
            {
                _BtContent = value;
                RaisePropertyChanged(nameof(BtContent));
            }
        }

        private string _LbContent;
        public string LbContent
        {
            get
            {
                return _LbContent;
            }
            set
            {
                _LbContent = value;
                RaisePropertyChanged(nameof(LbContent));
            }
        }

        private string _LbContent2;
        public string LbContent2
        {
            get
            {
                return _LbContent2;
            }
            set
            {
                _LbContent2 = value;
                RaisePropertyChanged(nameof(LbContent2));
            }
        }

        
        private string _txt;
        public string txt
        {
            get
            {
                return _txt;
            }
            set
            {
                _txt = value;
                RaisePropertyChanged(nameof(txt));
            }
        }

        #region cmd
        private RelayCommand _btnCmd;
        public RelayCommand BtnCmd
        {
            get { return _btnCmd; }
            set { _btnCmd = value; }
        }

        private RelayCommand _txtCmd;
        public RelayCommand TxtCmd
        {
            get { return _txtCmd; }
            set { _txtCmd = value; }
        }
        #endregion
    }
}
