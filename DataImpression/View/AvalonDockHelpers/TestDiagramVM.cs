using DataImpression.Models;
using DataImpression.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataImpression.View.AvalonDockHelpers
{
    public class TestDiagramVM: PaneVM
    {
        static ImageSourceConverter ISC = new ImageSourceConverter();
        public TestDiagramVM(Model _model)
        {
            model = _model;

            //Set the icon only for open documents (just a test)
            IconSource = ISC.ConvertFromInvariantString(@"pack://application:,,/Images/Table-Add.png") as ImageSource;
            Title = CSVFilePath;
            OnPropertyChanged("FAOIDiagramVM");
        }

        private Model model;

        #region CONTENT
        public string CSVFilePath
        {
            get { OnPropertyChanged("FAOIDiagramVM"); return model.SourceData.CSVFileName; }
        }

        public int RecordingCount
        {
            get { OnPropertyChanged("FAOIDiagramVM"); return model.Results.TobiiCSVRecordsList.Count; }
        }

        public FAOIDiagramVM FAOIDiagramVM
        {
            get { if (model == null) return null;
                return new FAOIDiagramVM(model);  }
        }
        #endregion

    }
}
