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
    public class ProjectExplorerTreeViewVM
    {
        public ProjectExplorerTreeViewVM(Model model)
        {
            root = new PEElement(model.Project.Name, "Project");

            source = new PEElement()
            {
                Title = "Исходные данные",
                PEElements = new ObservableCollection<PEElement>()
                {
                    new PEElement(Path.GetFileName(model.SourceData.CSVFileName), "FileCSV"),
                    new PEElement("Таблица AOI", "Table"),
                    new PEElement("Таблица FAOI-AOI", "Table")
                }
            };
            view = new PEElement()
            {
                Title = "Визуальные представления",
                PEElements = new ObservableCollection<PEElement>()
                {
                    new PEElement("Диаграмма 1", "Diagram"),
                    new PEElement("Диаграмма 2", "Diagram"),
                    new PEElement("Диаграмма 3","Diagram")
                }
            };
            root.PEElements.Add(source);
            root.PEElements.Add(view);

            Items = new ObservableCollection<PEElement>() { root };
        }

        public string ProjectName { get; set; } = "Project1";

        PEElement root;
        PEElement source;
        PEElement view;
        public ObservableCollection<PEElement> Items { get; set; }

    }

    
}
