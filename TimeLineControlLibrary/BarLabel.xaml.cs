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

namespace TimeLineControlLibrary
{
    /// <summary>
    /// Interaction logic for BarLabel.xaml
    /// </summary>
    public partial class BarLabel : UserControl
    {
        public BarLabel(Bar bar)
        {
            InitializeComponent();

            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Margin = bar.Body.Margin;
            Panel.SetZIndex(this, Panel.GetZIndex(bar.Body) - 1);
        }
    }
}
