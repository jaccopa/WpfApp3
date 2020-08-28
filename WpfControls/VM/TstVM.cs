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
        }

        private void ChangeContent()
        {
            LbContent = DateTime.Now.ToString();
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


        #region cmd
        private RelayCommand _btnCmd;
        public RelayCommand BtnCmd
        {
            get { return _btnCmd; }
            set { _btnCmd = value; }
        }
        #endregion
    }
}
