using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace PupilDiameterControlLibrary
{
    public delegate void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);

    /// <summary>
    /// Interaction logic for PupilDiameterUI.xaml
    /// </summary>
    public partial class PupilDiameterUI : UserControl, INotifyPropertyChanged
    {
        public PupilDiameterUI()
        {
            InitializeComponent();
            VM = new VM(this);
        }

        public VM VM;

        private void PupilDiameterUI_OnPupilDiameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

     

        #region DependencyProperty PupilDiameter
        //DependencyProperty PupilDiameter - чтобы можно было подписаться на него
        public ObservableCollection<TimeSpan_PupilDiameters_Pair> PupilDiameter
        {
            get { return (ObservableCollection<TimeSpan_PupilDiameters_Pair>)GetValue(PupilDiameterProperty); }
            set
            {
                SetValue(PupilDiameterProperty, value);
            }
        }

        public static readonly DependencyProperty PupilDiameterProperty =
            DependencyProperty.Register("PupilDiameter", typeof(ObservableCollection<TimeSpan_PupilDiameters_Pair>), typeof(PupilDiameterUI), 
                new PropertyMetadata(new ObservableCollection<TimeSpan_PupilDiameters_Pair>(), new PropertyChangedCallback(PupilDiameterPropertyChangedCallback)));

        public event PropertyChanged OnPupilDiameterChanged;

        private static void PupilDiameterPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((PupilDiameterUI)d).OnPupilDiameterChanged != null)
                ((PupilDiameterUI)d).OnPupilDiameterChanged(d, e);
        }
        #endregion
        

        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


    }

}
