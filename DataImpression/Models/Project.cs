using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    [Serializable]
    public class Project
    {
        public Project(string projectFilePath)
        {
            FilePath = projectFilePath;
        }

        public Project()
        {
        }

        public Model Model
        {
            get { return Model.GetModel(); }
            set { Model.SetModel(value); }
        }

        public string FilePath { get; set; }
        
        
        public string Name 
        { 
            get 
            {
                if (File.Exists(FilePath))
                    return Path.GetFileNameWithoutExtension(FilePath);
                else
                    return "Файл проекта не найден: " + FilePath;
            }
        }
    }
}
