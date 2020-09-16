using DataImpression.AbstractMVVM;
using DataImpression.ViewModel;
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

        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as TreeViewItem;
            if (item != null)
            {
                var DC = item.DataContext as PEElement;
                if (DC != null && DC.SelectionPEElementCallback != null)
                    DC.SelectionPEElementCallback.Invoke(DC.DocumentViewVM);
            };
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

        public PEElement(string title, string elementType, SelectionPEElementCallbackDelegate selectionPEElementCallback, DocumentViewVM documentViewVM)
        {
            Title = title;
            ElementType = elementType;
            SelectionPEElementCallback = selectionPEElementCallback;
            DocumentViewVM = documentViewVM;
        }

        /// <summary>
        /// Нужно чтобы пробросить к вьюшке событие об изменении коллекции внутренних элементов, а то так она не хочет его ловить
        /// </summary>
        public void ItemsChanged()
        {
            OnPropertyChanged("PEElements");
        }

        public DocumentViewVM DocumentViewVM;

        public string Title { get; set; }
        public string ElementType { get; set; } = "Folder";
        public ObservableCollection<PEElement> PEElements { get; set; } = new ObservableCollection<PEElement>();

        public SelectionPEElementCallbackDelegate SelectionPEElementCallback;
    }


    public delegate void SelectionPEElementCallbackDelegate(object o);
}
