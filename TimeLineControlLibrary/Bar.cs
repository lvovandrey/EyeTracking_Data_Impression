﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimeLineControlLibrary
{
    public class Bar
    {

        #region ctor
        public Bar(TimeSpan timeBegin, TimeSpan timeEnd, string label, int orderNumber, double height, Brush fillBrush, Brush strokeBrush)
        {
            
            if (timeBegin > timeEnd) //вадидация данных
                throw new ArgumentException("ctor TimeInterval: Argument timeBegin must be less than timeEnd");

            TimeBegin = timeBegin;
            TimeEnd = timeEnd;

            Label = label;
            Height = height;
            FillBrush = fillBrush;
            StrokeBrush = strokeBrush;
            OrderNumber = orderNumber;

            Body = new BarUI();
            Body.Body.Fill = FillBrush;
            Body.Height = Height;
            Body.Body.Stroke = StrokeBrush;
            Body.Width = 10;
            Body.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Body.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            Body.Body.StrokeThickness = 0;
            Body.DataContext = this;
        }


        #endregion

        #region Fields
        public BarUI Body;
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
        /// Длительность временного интервала
        /// </summary>
        /// <returns></returns>
        public TimeSpan TimeDuration
        {
            get
            {
                return Duration();
            }
        }

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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Зона №" + OrderNumber+"\t");
            sb.Append(Label + "\t");
            sb.Append(TimeBegin.ToString(@"hh\:mm\:ss\,ff") + "\t");
            sb.Append(TimeEnd.ToString(@"hh\:mm\:ss\,ff") + "\t");
            sb.Append(TimeDuration.ToString(@"mm\:ss\,ff"));
            return sb.ToString();
        }
        #endregion

    }
}
