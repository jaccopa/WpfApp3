using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using WpfControls.M;

namespace WpfControls.VM
{
    public class DataGridVM:ViewModelBase
    {
        public ObservableCollection<CSthModel> _WarnInfos { get; set; } = new ObservableCollection<CSthModel>();

        public ObservableCollection<CSthModel> WarnInfos
        {

            get { return _WarnInfos; }
            set
            {
                _WarnInfos = value;
                RaisePropertyChanged("WarnInfos");
            }
        }

        public DataGridVM()
        {
            for (int i = 0; i < 10; i++)
            {
                WarnInfos.Add(new CSthModel { SNum = i.ToString(), ErrCode = (i * i).ToString() });
            }
        }

        

    }
}
