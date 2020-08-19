using DataImpression.AbstractMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataImpression.ViewModel.AvalonDockHelpers
{
    /// <summary>
    /// Базовый класс для VM-ок документов. (возможно его можно убрать, и все сложить в DocumentViewVM?)
    /// </summary>
    public class PaneVM : INPCBase
    {
        public PaneVM()
        { }


        #region Title

        private string _title = null;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        #endregion

        public ImageSource IconSource
        {
            get;
            protected set;
        }

        #region ContentId

        private string _contentId = null;
        public string ContentId
        {
            get { return _contentId; }
            set
            {
                if (_contentId != value)
                {
                    _contentId = value;
                    OnPropertyChanged("ContentId");
                }
            }
        }

        #endregion

        #region IsSelected

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion

        #region IsActive

        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged("IsActive");
                }
            }
        }

        #endregion


    }
}
