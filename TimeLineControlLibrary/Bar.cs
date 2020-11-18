using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimeLineControlLibrary
{
    public class Bar
    {

        #region ctor
        public Bar(TimeSpan timeBegin, TimeSpan timeEnd, string label, double height, Brush fillBrush, Brush strokeBrush, BarsArea barsArea)
        {
            
            if (timeBegin > timeEnd) //вадидация данных
                throw new ArgumentException("ctor TimeInterval: Argument timeBegin must be less than timeEnd");

            TimeBegin = timeBegin;
            TimeEnd = timeEnd;

            BarsArea = barsArea;
            Label = label;
            Height = height;
            FillBrush = fillBrush;
            StrokeBrush = strokeBrush;

            Body = new Rectangle();
            Body.Fill = FillBrush;
            Body.Height = Height;
            Body.Stroke = StrokeBrush;
            Body.Width = 10;
            Body.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Body.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            Body.StrokeThickness = 0;

            Body.MouseEnter += Body_MouseEnter;
            Body.MouseLeave += Body_MouseLeave;
        }

        private void Body_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Body.StrokeThickness = 0;
            if (BarLabel != null)
                BarsArea.RemoveBarLabel(BarLabel);
        }

        private void Body_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Body.StrokeThickness = 3;
            BarLabel = new BarLabel(this);
            BarsArea.AddBarLabel(BarLabel);
        }
        #endregion

        #region Fields
        public Rectangle Body;
        public BarsArea BarsArea;
        public BarLabel BarLabel;
        #endregion

        #region Properties
        /// <summary>
        /// Граница начала временного интервала
        /// </summary>
        public TimeSpan TimeBegin { get; set; }

        /// <summary>
        /// Граница конца временного интервала
        /// </summary>
        public TimeSpan TimeEnd { get; set; }

        /// <summary>
        /// Название столбца
        /// </summary>
        public String Label { get; set; }


        /// <summary>
        /// Высота столбца
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Номер категории столбца
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Цвет (кисть) столбца 
        /// </summary>
        public Brush FillBrush { get; set; }

        /// <summary>
        /// Цвет (кисть) обводки
        /// </summary>
        public Brush StrokeBrush { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Длительность временного интервала
        /// </summary>
        /// <returns></returns>
        public TimeSpan Duration()
        {
            return TimeEnd - TimeBegin;
        }
        #endregion

    }
}
