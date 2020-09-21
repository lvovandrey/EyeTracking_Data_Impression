using DataImpression.ViewModel;
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

namespace DataImpression.View
{
    /// <summary>
    /// Логика взаимодействия для InformatoinColumnsChoiceView.xaml
    /// </summary>
    public partial class InformatoinColumnsChoiceView : UserControl
    {
        public InformatoinColumnsChoiceView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((InformationColumnsChoiceVM)DataContext).RaiseAllPropertyChanged();
                MessageBox.Show(((InformationColumnsChoiceVM)DataContext).TEST.ToString());
            }
        }
    }
}
