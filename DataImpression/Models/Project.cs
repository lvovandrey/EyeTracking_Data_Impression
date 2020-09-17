using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{

    public class Project
    {
        public Project(Model model, string projectFilePath)
        {
            Model = model;
            FilePath = projectFilePath;
        }

        public Model Model { get; private set; }
        public string FilePath { get; private set; }
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
