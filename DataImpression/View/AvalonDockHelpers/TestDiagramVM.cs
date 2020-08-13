﻿using DataImpression.Models;
using DataImpression.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataImpression.View.AvalonDockHelpers
{
    public class TestDiagramVM: PaneVM
    {
        static ImageSourceConverter ISC = new ImageSourceConverter();
        public TestDiagramVM(Model _model)
        {
            model = _model;

            //Set the icon only for open documents (just a test)
            IconSource = ISC.ConvertFromInvariantString(@"pack://application:,,/Images/Table-Add.png") as ImageSource;
            Title = CSVFilePath;
            FAOIDiagramVM = new FAOIDiagramVM(model);
        }

        private Model model;

        #region CONTENT
        public string CSVFilePath
        {
            get { return model.SourceData.CSVFileName; }
        }

        public int RecordingCount
        {
            get { return model.Results.TobiiCSVRecordsList.Count; }
        }

        public FAOIDiagramVM FAOIDiagramVM
        {
            get; set;
        }
        #endregion

    }
}
