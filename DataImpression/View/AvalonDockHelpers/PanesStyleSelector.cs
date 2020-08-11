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

        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            if (item is ToolVM)
                return ToolStyle;

            if (item is FileVM)
                return FileStyle;

            return base.SelectStyle(item, container);
        }
    }
}
