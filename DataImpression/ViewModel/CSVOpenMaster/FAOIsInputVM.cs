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
            FAOIsVM.Add(new FAOIVM(new FAOI(0, "No name"), _model, this));
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

                    var FAOIstmp_ = (ObservableCollection<FAOIVM>)formatter.Deserialize(fs);
                    if (!IsApproachFAOIVMCollection(FAOIstmp_))
                    {
                        var result = MessageBox.Show("Список AOI Hit в выбранном файле" + filename + " не совпадает со списком AOI Hit в csv-файле. Это может привести к непредсказуемым ошибкам при обработке. Все равно продолжить и загрузить выбранный файл?", "Неверный список AOI hit", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.OK)
                        {
                            FAOIstmp = FAOIstmp_;
                            OnPropertyChanged("FAOIsVM");
                        }
                    }
                    else
                    {
                        FAOIstmp = FAOIstmp_;
                        OnPropertyChanged("FAOIsVM");
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка открытия файла " + filename + ". \n Описание ошибки: " + e.Message + "    Stacktrace:" + e.StackTrace);
            }
        }

        private bool IsApproachFAOIVMCollection(IEnumerable<FAOIVM> fAOIVMs)
        {

            foreach (var item in fAOIVMs.First().AOIHitColumnsVM)
            {
                if (!_model.SourceData.CSVAOIHitsColumns.Contains(item.Column)) return false;
            }

            return true;
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
                FAOIstmp.Add(new FAOIVM(new FAOI(c.OrderedNumber, c.Name.Replace("AOI hit [", "").Replace("]", "")), _model, this));
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
            if (FirstFAOINotLinkedWithAOIhitColumns() != null)
            {
                MessageBox.Show("Не выбрано ни одной размеченной геометрической зоны (AOI-hit колонки) для функциональной зоны " + FirstFAOINotLinkedWithAOIhitColumns().Name);
                return false;
            }

            return true;
        }



        public bool RecordResultsToModel()
        {
            if (!ValidateResults()) return false;
            _model.SourceData.FAOIs.Clear();
            foreach (var fAOIVM in FAOIsVM)
            {
                FAOI faoi = new FAOI(fAOIVM.OrderedNumber, fAOIVM.Name);
                _model.SourceData.FAOIs.Add(faoi);
                foreach (var col in fAOIVM.AOIHitColumnsVM)
                {
                    if (col.IsChecked)
                        _model.SourceData.CSVColumnsToFAOIsConversionTable.Add(col.Column, faoi);
                }
            }
            return true;
        }

        public bool HaveRepeatedElements(IEnumerable<FAOIVM> fAOIVMs)
        {
            foreach (var f in fAOIVMs)
            {
                foreach (var f2 in fAOIVMs)
                {
                    if (f != f2 && f.Name == f2.Name) return true;
                }
            }
            return false;
        }

        private FAOIVM FirstFAOINotLinkedWithAOIhitColumns()
        {
            foreach (var fAOIVM in FAOIsVM)
            {
                if (fAOIVM.AOIHitColumnsVM.Where(t => t.IsChecked).Count() < 1)
                    return fAOIVM;
            }
            return null;
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
        [NonSerialized]
        Model _model;

        [NonSerialized]
        FAOIsInputVM FAOIsInputVM;

        public FAOIVM(FAOI _faoi, Model model, FAOIsInputVM fAOIsInputVM)
        {
            fAOI = _faoi;
            _model = model;
            FAOIsInputVM = fAOIsInputVM;

            AOIHitColumnsVM = new ObservableCollection<ColumnAndCheckFAOI_AOIVM>();
            foreach (var _column in _model.SourceData.CSVAOIHitsColumns)
            {
                bool _isChecked = false;
                var cc = new ColumnAndCheckFAOI_AOIVM(_column, _isChecked, (e) => { }, FAOIsInputVM, this);
                AOIHitColumnsVM.Add(cc);
            }
            OnPropertyChanged("AOIHitColumnsVM");
        }
        public FAOIVM()
        {
            fAOI = new FAOI(0, "");
            AOIHitColumnsVM = new ObservableCollection<ColumnAndCheckFAOI_AOIVM>();
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

        [NonSerialized]
        ObservableCollection<ColumnAndCheckFAOI_AOIVM> aOIHitColumnsVM;

        public ObservableCollection<ColumnAndCheckFAOI_AOIVM> AOIHitColumnsVM
        {
            get { return aOIHitColumnsVM; }
            set { aOIHitColumnsVM = value; OnPropertyChanged("AOIHitColumnsVM"); }
        }

    }



    public delegate void AcitonColumnAndCheckFAOI_AOIVMArgument(ColumnAndCheckFAOI_AOIVM newColumnAndCheckVM);
    /// <summary>
    /// ЖУТЬ ПРОСТО ЖУТЬ. ВСЕ ПЕРЕПИСАТЬ!
    /// </summary>
    [Serializable]
    public class ColumnAndCheckFAOI_AOIVM : INPCBase
    {
        [NonSerialized]
        FAOIsInputVM FAOIsInputVM;

        [NonSerialized]
        FAOIVM FAOIVM;

        public ColumnAndCheckFAOI_AOIVM(Column _column, bool _isChecked, AcitonColumnAndCheckFAOI_AOIVMArgument _checkColumn, FAOIsInputVM fAOIsInputVM, FAOIVM fAOIVM)
        {
            Column = _column;
            IsChecked = _isChecked;
            checkColumn = _checkColumn;
            FAOIsInputVM = fAOIsInputVM;
            FAOIVM = fAOIVM;
        }

        public ColumnAndCheckFAOI_AOIVM()
        {
            Column = new Column();
            IsChecked = false;
            checkColumn = null;
        }
        bool isChecked;
        AcitonColumnAndCheckFAOI_AOIVMArgument checkColumn;
        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; OnPropertyChanged("IsChecked"); checkColumn?.Invoke(this); }
        }

        public void Check(bool ch)
        {
            isChecked = ch;
            OnPropertyChanged("IsChecked");
            OnPropertyChanged("Occupied");
        }

        public Column Column;
        public string Name
        {
            get { return Column.Name; }
        }
        public int OrderedNumber
        {
            get { return Column.OrderedNumber; }
        }

        public string Occupied
        {
            get
            {
                foreach (var faoivm in FAOIsInputVM.FAOIsVM)
                {
                    if (!FAOIVM.Equals(faoivm))
                        foreach (var aoicolumn in faoivm.AOIHitColumnsVM)
                        {
                            if (OrderedNumber == aoicolumn.OrderedNumber && Name == aoicolumn.Name && aoicolumn.IsChecked)
                                return "Занято, стучаться надо";
                        }
                }
                return "Свободно";

            }
        }
    }


}

