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
    [Serializable]
    public class Model
    {
        #region ctor
        public Model()
        {
            SourceData = new ProcessingTaskSourceData();
            Results = new ProcessingResults(SourceData);
            HaveData = false;
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Свойство показывает, что модель не пуста, т.е. что в нее уже какие-то данные загружены
        /// </summary>
        public bool HaveData { get; set; }


        /// <summary>
        /// Источник данных вычислительной задачи.... в общем он в себя вмещает все данные которые нужны для выполнения данной задачи.
        /// </summary>
        public ProcessingTaskSourceData SourceData { get; set; }

        public ProcessingResults Results { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Создает новую задачу обработки - это нужно когда мы например в новым файлом начинаем работать. 
        /// </summary>
        /// <returns>Возваращает true если создать задачу удалось, false - если не удалось</returns>
        public bool CreateNewProcessingTask()
        {
            SourceData = new ProcessingTaskSourceData();
            Results = new ProcessingResults(SourceData);
            return true;
        }
        #endregion
    }
}
