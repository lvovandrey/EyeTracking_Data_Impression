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

        public DocumentViewVM THIS 
            {get {return this;} }

        public DocumentView Body
        { get;
            set; }
        #endregion

    }
}
