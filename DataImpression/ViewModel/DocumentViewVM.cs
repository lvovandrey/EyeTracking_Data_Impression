using DataImpression.Models;
using DataImpression.View;
using DataImpression.ViewModel.AvalonDockHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DataImpression.ViewModel
{
    public class DocumentViewVM : PaneVM
    {
        public DocumentViewVM(Model _model, string DocumentType)
        {
            model = _model;
            documentType = DocumentType;
            Title = Path.GetFileName(model.SourceData.CSVFileName);
        }

        private Model model;
        private string documentType;

        #region CONTENT

        public IDocumentBodyVM DocumentBodyVM
        {
            get;
            set;
        }

        public DocumentViewVM THIS
        { get { return this; } }



        #endregion

        #region Methods
        public void ConstructDocumentView(DocumentView Body)
        {
            if (DocumentBodyVM == null)
            {
                if (documentType == "AverageFixationTimeDistribution")
                {
                    var diagram = new FAOIDistributedColumnChartView();
                    SettingsFAOIDistributedColumnChart settings = new SettingsFAOIDistributedColumnChart() { Fill = Brushes.Green };
                    DocumentBodyVM = new FAOIDistributedColumnChartVM<TimeSpan>(model, model.Results.AverageFixationTimeDistribution, settings);
                    diagram.DataContext = DocumentBodyVM;
                    Body.Container.Children.Add(diagram);
                }

                if (documentType == "FrequencyRequestsFAOIDistributionPerMinute")
                {
                    var diagram = new FAOIDistributedColumnChartView();
                    SettingsFAOIDistributedColumnChart settings = new SettingsFAOIDistributedColumnChart() { Fill = Brushes.Orange };
                    DocumentBodyVM = new FAOIDistributedColumnChartVM<double>(model, model.Results.FrequencyRequestsFAOIDistributionPerMinute, settings);
                    diagram.DataContext = DocumentBodyVM;
                    Body.Container.Children.Add(diagram);
                }

                if (documentType == "TimePercentDistribution")
                {
                    var diagram = new FAOIDiagramView();
                    DocumentBodyVM = new FAOIDiagramVM(model);
                    diagram.DataContext = DocumentBodyVM;
                    Body.Container.Children.Add(diagram);
                }

                if (documentType == "FixationsTimeline")
                {
                    DocumentBodyVM = new FixationsTimelineVM(model);
                    var diagram = new FixationsTimelineView();
                    diagram.DataContext = DocumentBodyVM;
                    Body.Container.Children.Add(diagram);
                }



                if (documentType == "TimePercentDistribution+AverageFixationTimeDistribution+FrequencyRequestsFAOIDistributionPerMinute")
                {
                    var complexdiagram = new FAOIDistributedComplexColumnChart(); 
                    DocumentBodyVM = new FAOIDistributedComplexColumnChartVM<object>(model);
                    complexdiagram.DataContext = DocumentBodyVM;

                    var diagram1 = new FAOIDistributedColumnChartView();
                    SettingsFAOIDistributedColumnChart settings = new SettingsFAOIDistributedColumnChart() { Fill = Brushes.Blue };
                    var DocumentBodyVM1 = new FAOIDistributedColumnChartVM<double>(model, model.Results.TimePercentDistribution, settings);
                    

                    var diagram2 = new FAOIDistributedColumnChartView();
                    SettingsFAOIDistributedColumnChart settings2 = new SettingsFAOIDistributedColumnChart() { Fill = Brushes.Green };
                    var DocumentBodyVM2 = new FAOIDistributedColumnChartVM<TimeSpan>(model, model.Results.AverageFixationTimeDistribution, settings2);

                    var diagram3 = new FAOIDistributedColumnChartView();
                    SettingsFAOIDistributedColumnChart settings3 = new SettingsFAOIDistributedColumnChart() { Fill = Brushes.Orange };
                    var DocumentBodyVM3 = new FAOIDistributedColumnChartVM<double>(model, model.Results.FrequencyRequestsFAOIDistributionPerMinute, settings3);


                    Body.Container.Children.Add(complexdiagram);
                    ((FAOIDistributedComplexColumnChartVM<object>)DocumentBodyVM).ChartVMs.Add(DocumentBodyVM1);
                    ((FAOIDistributedComplexColumnChartVM<object>)DocumentBodyVM).ChartVMs.Add(DocumentBodyVM2);
                    ((FAOIDistributedComplexColumnChartVM<object>)DocumentBodyVM).ChartVMs.Add(DocumentBodyVM3);

                }

            }
            else
            {

            }
        }
        #endregion
    }
}
