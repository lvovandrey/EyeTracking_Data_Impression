using AvalonDock.Layout;
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


        public DataTemplate FileViewTemplate
        {
            get;
            set;
        }

        public DataTemplate FileStatsViewTemplate
        {
            get;
            set;
        }

        public DataTemplate TestDiagramViewTemplate
        {
            get;
            set;
        }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var itemAsLayoutContent = item as LayoutContent;

            if (item is FileVM)
                return FileViewTemplate;

            if (item is FileStatsVM)
                return FileStatsViewTemplate;

            if (item is TestDiagramVM)
                return TestDiagramViewTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
