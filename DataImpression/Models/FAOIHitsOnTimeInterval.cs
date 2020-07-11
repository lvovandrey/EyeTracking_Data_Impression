using System.Collections.Generic;

namespace DataImpression.Models
{
    /// <summary>
    /// Отдельная запись, в которой хранятся данные о том, какие на данном временном интервале были попадания маркера взгляда в FAOI
    /// Иначе говоря куда он смотрел в данный интервал времени
    /// </summary>
    public class FAOIHitsOnTimeInterval
    {
        #region ctor
        public FAOIHitsOnTimeInterval(TimeInterval timeInterval, List<FAOI> fAOIHits)
        {
            TimeInterval = timeInterval;
            FAOIHits = fAOIHits;
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Текущий интервал времени
        /// </summary>
        TimeInterval TimeInterval { get; set; }

        /// <summary>
        /// Список попаданий в FAOI в данном интервале времени
        /// </summary>
        List<FAOI> FAOIHits { get; set; }
        #endregion

        #region Methods
        #endregion

    }
}
