using DataImpression.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataImpression.ViewModel.AvalonDockHelpers
{
    /// <summary>
    /// Класс обеспечивает выбор (проброс через привязки) стиля Dock-окошка в зависимости от типа VM-ки
    /// </summary>
    class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle
        {
            get;
            set;
        }

        public Style DocumentViewStyle
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

            if (item is DocumentViewVM)
                return DocumentViewStyle;


            if (item is ProjectExplorerVM)
                return ProjectExplorerStyle;

            return base.SelectStyle(item, container);
        }
    }
}
