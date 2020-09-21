using DataImpression.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    /// <summary>
    /// Инкапсулирует все исходные данные для решения одной 
    /// задачи по обработке.
    /// </summary>
    [Serializable]
    public class ProcessingTaskSourceData
    {
        #region ctor
        public ProcessingTaskSourceData()
        {
            //может создаваться без данных и постепенно ими наполняться
            //TODO: продумать механизм предотвращающий использование незаполненного класса. ValidateDataCompleteness - так себе решение.
            CSVCaption = new List<Column>();
            CSVTimeColumn = new Column(0, null);
            CSVAOIHitsColumns = new List<Column>();
            CSVFullTimeInterval = new TimeInterval(TimeSpan.Zero, TimeSpan.Zero);
            WorkingTOI = new TOI(CSVFullTimeInterval, CSVFullTimeInterval);
            FAOIs = new List<FAOI>();
            CSVColumnsToFAOIsConversionTable = new XmlSerializableDictionary<Column, FAOI>();
            OptionalDataCSVColumns = new XmlSerializableDictionary<string, Column>();
        }

        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Имя файла csv (полный путь к нему)
        /// </summary>
        public string CSVFileName { get; set; }

        /// <summary>
        /// Набор колонок в csv-файле. Фактически это заголовочная строка файла
        /// </summary>
        public List<Column> CSVCaption { get; set; }

        /// <summary>
        /// Вторая строка в csv-файле. В ней содержатся всякие имена испытуемых и проч. доп. параметры. Иногда может быть полезной.
        /// </summary>
        public string CSVSecondString { get; set; }

        /// <summary>
        /// Колонка в файле csv в которой размещается время
        /// </summary>
        public Column CSVTimeColumn { get; set; }

        /// <summary>
        /// Коллекция колонок в файле csv в которой размещаются разные дополнительные данные
        /// </summary>
        public XmlSerializableDictionary<string, Column> OptionalDataCSVColumns { get; set; }


        /// <summary>
        /// Коллекция колонок в csv-файле в которых размещаются данные о попадании маркера взгляда в ту или иную AOI(зону)
        /// </summary>
        public List<Column> CSVAOIHitsColumns { get; set; }

        /// <summary>
        /// Весь интервал времени который представлен в csv -файле
        /// </summary>
        public TimeInterval CSVFullTimeInterval { get; set; } //TODO: В этих файлах могут быть и множественные интервалы времени.... 

        /// <summary>
        /// Интервал времени, в пределах которого будет проводиться анализ
        /// </summary>
        public TOI WorkingTOI { get; set; } //TODO: здесь больше подходит коллекция из TOI

        /// <summary>
        /// Коллекция функциональных зон, для которой будет проводиться анализ
        /// </summary>
        public List<FAOI> FAOIs { get; set; }

        /// <summary>
        /// Таблица соответствия колонки c csv-файле функциональной зоне
        /// </summary>
        public XmlSerializableDictionary<Column, FAOI> CSVColumnsToFAOIsConversionTable { get; set; } //TODO: мне не очень нравится то, что эта коллекция и FAOIs независимы друг от друга... надо их как-то жестко связать
        
        #endregion

        #region Methods
        /// <summary>
        /// Метод вызывается при использовании экземпляра класса. 
        /// В нем осуществляется проверка полноты всех данных, необходимых для корректной дальнейшей работы класса.
        /// </summary>
        public void ValidateDataCompleteness()
        {
            throw new NotImplementedException("ProcessingTaskSourceData.ValidateDataCompleteness");
            throw new Exception("ProcessingTaskSourceData.ValidateDataCompleteness: data is not complete for use this instance. Hash:" + this.GetHashCode());
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods


        #endregion

    }
}
