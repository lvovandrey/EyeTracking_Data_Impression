using DataImpression.AbstractMVVM;
using DataImpression.Models;

namespace DataImpression.ViewModel
{
    public class DocumentBodyVM : INPCBase, IDocumentBodyVM
    {
        protected Model model => Model.GetModel();

        public DocumentBodyVM()
        {
        }

        string IDocumentBodyVM.Title { get { return model.SourceData.CSVFileName; } }
    }
}