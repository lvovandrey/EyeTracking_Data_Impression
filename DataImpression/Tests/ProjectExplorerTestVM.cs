using DataImpression.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Tests
{
    public class ProjectExplorerTestVM
    {
        public ProjectExplorerTestVM(Project project)
        {
            root = new PEElement(project.Name,"Project");

            source = new PEElement()
            {
                Title = "Исходные данные",
                PEElements = new ObservableCollection<PEElement>()
                {
                    new PEElement(Path.GetFileName(project.Model.SourceData.CSVFileName), "FileCSV"),
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

 
    public class PEElement
    {
        public PEElement()
        {

        }

        public PEElement(string title, string elementType)
        {
            Title = title;
            ElementType = elementType;
        }

        public string Title { get; set; }
        public string ElementType { get; set; } = "Folder";
        public ObservableCollection<PEElement> PEElements { get; set; } = new ObservableCollection<PEElement>();
    }



}
