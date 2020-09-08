using DataImpression.AbstractMVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DataImpression.Tests
{
    /// <summary>
    /// Логика взаимодействия для FixationsTimeLine.xaml
    /// </summary>
    public partial class FixationsTimeLine : UserControl
    {
        public FixationsTimeLine()
        {
            InitializeComponent();
        }
    }

    public class FixationsTimeLineVM : INPCBase

    {
        public FixationsTimeLineVM()
        {
            RectanglesCollection = new ObservableCollection<double>();
            for (int i = 0; i < 300; i++)
            {
                RectanglesCollection.Add(1);
            }
        }

        public ObservableCollection<double> RectanglesCollection { get; set; }
    }

}
