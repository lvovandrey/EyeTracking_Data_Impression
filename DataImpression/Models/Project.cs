﻿using System;
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
        public Project(Model model, string projectFilePath)
        {
            Model = model;
            FilePath = projectFilePath;
        }

        public Project()
        {
        }

        public Model Model { get; set; }
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
