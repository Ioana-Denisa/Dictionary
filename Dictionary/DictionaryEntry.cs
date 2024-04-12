using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema1_Dictionar
{
    internal class DictionaryEntry
    {
        public string Word { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public string ImagePath { get; set; }

        public DictionaryEntry(string name, string description,Category category, string imagePath )
        {
            Word = name;
            Description = description;
            Category = category;
            ImagePath = imagePath;
        }
    }
}
