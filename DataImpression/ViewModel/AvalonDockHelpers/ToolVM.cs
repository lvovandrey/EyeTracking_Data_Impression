﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.ViewModel.AvalonDockHelpers
{

    /// <summary>
    /// Базовый класс для VM-ок Dock-окошек панелей управления (типа панелей инструментов).
    /// </summary>
    public class ToolVM : PaneVM
    {
        public ToolVM(string name)
        {
            Name = name;
            Title = name;
        }

        public string Name
        {
            get;
            private set;
        }


        #region IsVisible

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnPropertyChanged("IsVisible");
                }
            }
        }

        #endregion


    }
}
