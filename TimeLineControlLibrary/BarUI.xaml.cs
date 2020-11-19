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
    /// Interaction logic for BarUI.xaml
    /// </summary>
    public partial class BarUI : UserControl
    {
        public BarUI()
        {
            InitializeComponent();
        }

        private void Body_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Body.StrokeThickness = 0;
            //if (BarLabel != null)
            //{
            //    ((Panel)Body.Parent).Children.Remove(BarLabel);
            //    BarLabel = null;
            //}
        }

        private void Body_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Body.StrokeThickness = 3;
            //BarLabel = new BarLabel(this);
            //((Panel)Body.Parent).Children.Add(BarLabel);
        }

    }
}
