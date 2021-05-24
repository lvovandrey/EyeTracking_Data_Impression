﻿using System;
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
    /// Логика взаимодействия для FixationsTimelineStepChartView.xaml
    /// </summary>
    public partial class FixationsTimelineView : UserControl
    {
        public FixationsTimelineView()
        {
            InitializeComponent();
        }
                                      
        private void TimeLine_Zoom(double MousePositionX, double ZoomKoef)
        {
            Console.WriteLine("Zoom X={0} Koef={1}", MousePositionX, ZoomKoef);
        }
    }
}
