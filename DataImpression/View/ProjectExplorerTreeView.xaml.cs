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

namespace DataImpression.View
{
    /// <summary>
    /// Interaction logic for ProjectExplorerTreeView.xaml
    /// </summary>
    public partial class ProjectExplorerTreeView : UserControl
    {
        public ProjectExplorerTreeView()
        {
            InitializeComponent();
        }

        private void TreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }

    /// <summary>
    /// Отдельный элемент дерева
    /// </summary>
    public class PEElement:INPCBase
    {
        public PEElement()
        {

        }

        public PEElement(string title, string elementType)
        {
            Title = title;
            ElementType = elementType;
        }

        /// <summary>
        /// Нужно чтобы пробросить к вьюшке событие об изменении коллекции внутренних элементов, а то так она не хочет его ловить
        /// </summary>
        public void ItemsChanged()
        {
            OnPropertyChanged("PEElements");
        }

        public string Title { get; set; }
        public string ElementType { get; set; } = "Folder";
        public ObservableCollection<PEElement> PEElements { get; set; } = new ObservableCollection<PEElement>();
    }
}
