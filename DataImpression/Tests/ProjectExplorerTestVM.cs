using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Tests
{
    public class ProjectExplorerTestVM
    {
        public ProjectExplorerTestVM()
        {
            source = new PEElement()
            {
                PEElements = new ObservableCollection<PEElement>()
                {
                    new PEElement("Файл.csv"),
                    new PEElement("Таблица AOI"),
                    new PEElement("Таблица FAOI-AOI")
                }
            };
            view = new PEElement()
            {
                //PEElements = new ObservableCollection<PEElement>()
                //{
                //    new PEElement("Список диаграмм")
                //    {
                //        PEElements = new ObservableCollection<PEElement>()
                //        {
                //            new PEDataView("Диаграмма 1"){IsDiagram=true },
                //            new PEDataView("Диаграмма 2"){IsDiagram=true },
                //            new PEDataView("Диаграмма 3"){IsDiagram=true },
                //        }
                //    },
                //    new PEElement("Список таблиц")
                //    {
                //        PEElements = new ObservableCollection<PEElement>()
                //        {
                //            new PEDataView("Таблица 1"){IsTable=true },
                //            new PEDataView("Таблица 2"){IsTable=true },
                //            new PEDataView("Таблица 3"){IsTable=true },
                //        }
                //    }
                //}
            };
            Items = new ObservableCollection<PEElement>() { source, view };
        }

        public string ProjectName { get; set; } = "Project1";

        PEElement source;
        PEElement view;
        public ObservableCollection<PEElement> Items { get; set; }

    }

 
    public class PEElement
    {
        public PEElement()
        {

        }

        public PEElement(string title)
        {
            Title = title;
        }

        public string Title { get; set; }
        public bool IsDiagram = false;
        public bool IsTable = false;
        public ObservableCollection<PEElement> PEElements { get; set; }
    }



}
