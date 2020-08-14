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
    class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle
        {
            get;
            set;
        }

        public Style FileStyle
        {
            get;
            set;
        }

        public Style DiagramStyle
        {
            get;
            set;
        }

        public Style ProjectExplorerStyle
        {
            get;
            set;
        }

        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            if (item is ToolVM)
                return ToolStyle;

            if (item is FileVM)
                return FileStyle;

            if (item is TestDiagramVM)
                return DiagramStyle;

            if (item is ProjectExplorerVM)
                return ProjectExplorerStyle;

            return base.SelectStyle(item, container);
        }
    }
}
