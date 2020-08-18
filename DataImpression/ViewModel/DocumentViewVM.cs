using DataImpression.Models;
using DataImpression.View.AvalonDockHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.ViewModel
{
    public class DocumentViewVM : PaneVM
    {
        public DocumentViewVM(Model _model)
        {
            model = _model;
            Title = Path.GetFileName(model.SourceData.CSVFileName);
        }

        private Model model;

        #region CONTENT

        public IDocumentBodyVM DocumentBodyVM
        {
            get;
            set;
        }
        #endregion

    }
}
