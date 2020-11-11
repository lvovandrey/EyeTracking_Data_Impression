using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    /// <summary>
    /// Основной класс модели. кстати это синглтон
    /// </summary>
    [Serializable]
    public class Model
    {
        #region ctor

        private Model()
        {
            SourceData = new ProcessingTaskSourceData();
            Results = new ProcessingResults(SourceData);
            HaveData = false;
        }

        private static Model model;
        public static Model GetModel()
        {
            if (model == null)
                model = new Model();
            return model;
        }

        public static void ClearModel()
        {
            model = new Model();
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
