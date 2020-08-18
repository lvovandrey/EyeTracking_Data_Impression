﻿using AvalonDock.Layout;
using DataImpression.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataImpression.View.AvalonDockHelpers
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        public PanesTemplateSelector()
        {

        }




        public DataTemplate ProjectExplorerTemplate
        {
            get;
            set;
        }

        public DataTemplate TestDiagramViewTemplate
        {
            get;
            set;
        }

        public DataTemplate DocumentViewTemplate
        {
            get;
            set;
        }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var itemAsLayoutContent = item as LayoutContent;


            if (item is TestDiagramVM)
                return TestDiagramViewTemplate;

            if (item is DocumentViewVM)
                return DocumentViewTemplate;

            if (item is ProjectExplorerVM)
                return ProjectExplorerTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
