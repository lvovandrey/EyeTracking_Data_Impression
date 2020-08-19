using DataImpression.AbstractMVVM;
using DataImpression.Models;

namespace DataImpression.ViewModel
{
    public class DocumentBodyVM : INPCBase, IDocumentBodyVM
    {
        protected Model model;

        public DocumentBodyVM(Model model)
        {
            this.model = model;
        }

        string IDocumentBodyVM.Title { get { return model.SourceData.CSVFileName; } }
    }
}