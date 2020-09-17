using DataImpression.AbstractMVVM;
using DataImpression.Models;
using DataImpression.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.ViewModel
{
    public class ProjectExplorerTreeViewVM:INPCBase
    {
        public ProjectExplorerTreeViewVM(Project project, ResultsViewAreaVM resultsViewAreaVM)
        {
            Project = project;
            this.ResultsViewAreaVM = resultsViewAreaVM;

            root = new PEElement(Project.Name, "Project");

            source = new PEElement()
            {
                Title = "Исходные данные",
                PEElements = new ObservableCollection<PEElement>()
                {
                    new PEElement(Path.GetFileName(Project.Model.SourceData.CSVFileName), "FileCSV"),
                    new PEElement("Таблица AOI", "Table"),
                    new PEElement("Таблица FAOI-AOI", "Table")
                }
            };
            view = new PEElement()
            {
                Title = "Визуальные представления",
                PEElements = new ObservableCollection<PEElement>()
                {

                }
            };
            root.PEElements.Add(source);
            root.PEElements.Add(view);

            Items = new ObservableCollection<PEElement>() { root };

            ResultsViewAreaVM.DocumentViewVMsChanged += ViewsRefresh;
        }

        private void ViewsRefresh(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            view.PEElements = new ObservableCollection<PEElement>();
            foreach (var documentViewVM in ResultsViewAreaVM.DocumentViewVMs)
            {
                view.PEElements.Add(new PEElement(documentViewVM.documentType, "Diagramm", ResultsViewAreaVM.ActivateDocument, documentViewVM));
            }
            view.ItemsChanged();
        }


        public string ProjectName { get; private set; } = "Project1";

        PEElement root;
        PEElement source;
        PEElement view;
        private ResultsViewAreaVM ResultsViewAreaVM;

        public ObservableCollection<PEElement> Items { get; private set; }

        Project Project { get; set; }

    }

    
}
