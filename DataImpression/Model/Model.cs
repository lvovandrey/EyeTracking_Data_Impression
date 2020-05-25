using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    /// <summary>
    /// Основной класс модели.
    /// </summary>
    public class Model
    {
        #region ctor
        public Model()
        {
            SourceData = new ProcessingTaskSourceData();
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        public ProcessingTaskSourceData SourceData { get; }
        #endregion

        #region Methods
        #endregion
    }
}
