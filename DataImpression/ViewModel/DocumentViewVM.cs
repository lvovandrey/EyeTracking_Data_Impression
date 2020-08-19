using DataImpression.Models;
using DataImpression.View;
using DataImpression.View.AvalonDockHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                    DocumentBodyVM = new FAOIDistributedColumnChartVM<TimeSpan>(model, model.Results.AverageFixationTimeDistribution);
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
            }
            else
            {

            }
        }
        #endregion
    }
}
