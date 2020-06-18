using DataImpression.AbstractMVVM;
using DataImpression.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace DataImpression.ViewModel
{
    public class FAOIsInputVM : INPCBase
    {
        #region ctor
        public FAOIsInputVM(Model model, ListView fAOIsInputListView)
        {
            _model = model;
            FAOIsInputListView = fAOIsInputListView;
            OnPropertyChanged("FAOIsVM");
        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model;

        ListView FAOIsInputListView;
        #endregion


        #region Properties
        ObservableCollection<FAOIVM> FAOIstmp;
        //{
        //    get 
        //    {
        //        return new ObservableCollection<FAOIVM>(from f in _model.SourceData.FAOIs select new FAOIVM(f));
        //    }
        //    set 
        //    { 
                
        //    }
        //}
        public ObservableCollection<FAOIVM> FAOIsVM
        {
            get
            {
                if (FAOIstmp != null)
                {
                    return FAOIstmp;
                }
                FAOIstmp = new ObservableCollection<FAOIVM>();
                return FAOIstmp;
            }
        }

        FAOIVM selectedFAOIVM;

        public FAOIVM SelectedFAOIVM
        {
            get { return selectedFAOIVM; }
            set { selectedFAOIVM = value; OnPropertyChanged("SelectedFAOIVM"); }
        }


        Visibility visibility;
        public Visibility Visibility
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;
                OnPropertyChanged("Visibility");
            }
        }
        #endregion
        #region Methods
        public bool CanExecuteNextInputStage()
        {
            if (FAOIsVM?.Count > 0) return true; else return false; 
        }
        void Add()
        {
            FAOIsVM.Add(new FAOIVM(new FAOI(0, "No name")));
            OnPropertyChanged("FAOIsVM");
            RegularizeOrderedNumbers();
        }

        void Remove(FAOIVM faoivm)
        {
            FAOIsVM.Remove(faoivm);
            OnPropertyChanged("FAOIsVM");
            RegularizeOrderedNumbers();
        }

        private void Up(FAOIVM selectedFAOIVM)
        {
            int oldPos = FAOIsInputListView.Items.CurrentPosition;
            if (oldPos == 0) return;

            if (selectedFAOIVM == null) return;

            int i = FAOIsVM.IndexOf(selectedFAOIVM);
            FAOIsVM.RemoveAt(i);
            FAOIsVM.Insert(i - 1, selectedFAOIVM);

            RegularizeOrderedNumbers();
            FAOIsInputListView.SelectedIndex = oldPos - 1;
            OnPropertyChanged("FAOIsVM");
        }
        private void Down(FAOIVM selectedFAOIVM)
        {
            int oldPos = FAOIsInputListView.Items.CurrentPosition;
            if (oldPos >= FAOIsInputListView.Items.Count - 1) return;

            if (selectedFAOIVM == null) return;

            int i = FAOIsVM.IndexOf(selectedFAOIVM);
            FAOIsVM.RemoveAt(i);
            FAOIsVM.Insert(i + 1, selectedFAOIVM);

            RegularizeOrderedNumbers();
            FAOIsInputListView.SelectedIndex = oldPos + 1;
            OnPropertyChanged("FAOIsVM");
        }
        private void LoadFromXML()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Xml-flie(*.xml)|*.xml";
            bool? res = openFileDialog.ShowDialog();
            if (res == null || res == false) return;
            string filename = openFileDialog.FileName;
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<FAOIVM>));

                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    FAOIstmp = (ObservableCollection<FAOIVM>)formatter.Deserialize(fs);
                    OnPropertyChanged("FAOIsVM");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка открытия файла " + filename + ". \n Описание ошибки: " + e.Message + "    Stacktrace:" + e.StackTrace);
            }
        }
        private void SaveToXML()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Xml-flie(*.xml)|*.xml";
            bool? res = saveFileDialog.ShowDialog();
            if (res == null || res == false) return;
            string filename = saveFileDialog.FileName;

            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<FAOIVM>));

                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    formatter.Serialize(fs, FAOIsVM);
                }
                MessageBox.Show("Файл " + filename + " успешно сохранен");
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка сохранения файла " + filename + ". \n Описание ошибки: " + e.Message + "    Stacktrace:" + e.StackTrace);
            }
        }
        private void LoadFromAOIHitsColumns()
        {
            if (_model.SourceData.CSVAOIHitsColumns.Count < 0)
            {
                MessageBox.Show("Не выбрано ни одной колонки AOI hits на предыдущем шаге");
                return;
            }
            FAOIstmp.Clear();
            foreach (var c in _model.SourceData.CSVAOIHitsColumns)
            {
                FAOIstmp.Add(new FAOIVM(new FAOI(c.OrderedNumber, c.Name.Replace("AOI hit [", "").Replace("]", ""))));
            }
            RegularizeOrderedNumbers();
            OnPropertyChanged("FAOIsVM");
        }

        void RegularizeOrderedNumbers()
        {
            int i = 1;
            foreach (var f in FAOIsVM)
                f.OrderedNumber = i++;
            OnPropertyChanged("FAOIsVM");
        }

        bool ValidateResults()
        {
            if (FAOIsVM.Count < 1)
            {
                MessageBox.Show("Введите хотя бы одну функциональную зону");
                return false;
            }
            if (HaveRepeatedElements(FAOIsVM))
            {
                MessageBox.Show("Найдены одно или более одинаковых имен в перечне функциональных зон. Функциональные зоны должны иметь уникальные названия");
                return false;
            }
            return true;
        }

        public bool RecordResultsToModel()
        {
            if (!ValidateResults()) return false;
            _model.SourceData.FAOIs.Clear();
            foreach (var fAOIVM in FAOIsVM)
                _model.SourceData.FAOIs.Add(new FAOI(fAOIVM.OrderedNumber, fAOIVM.Name));
            return true;
        }

        public bool HaveRepeatedElements(IEnumerable<FAOIVM> fAOIVMs)
        {
            foreach (var f in fAOIVMs)
            {
                foreach (var f2 in fAOIVMs)
                {
                    if (f!=f2 && f.Name == f2.Name) return true;
                }
            }
            return false;
        }
        #endregion

        #region Commands

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(obj =>
                {
                    Add();
                }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ?? (removeCommand = new RelayCommand(obj =>
                {
                    Remove(SelectedFAOIVM);
                }));
            }
        }

        private RelayCommand upCommand;

        public RelayCommand UpCommand
        {
            get
            {
                return upCommand ?? (upCommand = new RelayCommand(obj =>
                {
                    Up(SelectedFAOIVM);
                }));
            }
        }


        private RelayCommand downCommand;
        public RelayCommand DownCommand
        {
            get
            {
                return downCommand ?? (downCommand = new RelayCommand(obj =>
                {
                    Down(SelectedFAOIVM);
                }));
            }
        }


        private RelayCommand loadFromXMLCommand;
        public RelayCommand LoadFromXMLCommand
        {
            get
            {
                return loadFromXMLCommand ?? (loadFromXMLCommand = new RelayCommand(obj =>
                {
                    LoadFromXML();
                }));
            }
        }


        private RelayCommand saveToXMLCommand;
        public RelayCommand SaveToXMLCommand
        {
            get
            {
                return saveToXMLCommand ?? (saveToXMLCommand = new RelayCommand(obj =>
                {
                    SaveToXML();
                }));
            }
        }


        private RelayCommand loadFromAOIHitsColumnsCommand;
        public RelayCommand LoadFromAOIHitsColumnsCommand
        {
            get
            {
                return loadFromAOIHitsColumnsCommand ?? (loadFromAOIHitsColumnsCommand = new RelayCommand(obj =>
                {
                    LoadFromAOIHitsColumns();
                }));
            }
        }


        //LoadFromXMLCommand
        //SaveToXMLCommand
        //LoadFromAOIHitsColumnsCommand
        #endregion
    }

    [Serializable]
    public class FAOIVM : INPCBase
    {
        public FAOIVM(FAOI _faoi)
        {
            fAOI = _faoi;
        }
        public FAOIVM()
        {
            fAOI = new FAOI(0, "");
        }

        [NonSerialized]
        public FAOI fAOI;

        public string Name
        {
            get { return fAOI.Name; }
            set { fAOI.Name = value; OnPropertyChanged("Name"); }
        }

        public int OrderedNumber
        {
            get { return fAOI.OrderedNumber; }
            set { fAOI.OrderedNumber = value; OnPropertyChanged("OrderedNumber"); }
        }
    }


}

