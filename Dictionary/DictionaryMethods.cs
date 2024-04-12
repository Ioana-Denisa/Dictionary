using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tema1_Dictionar
{
    internal class DictionaryMethods
    {
        private static string NoCategory = "-Nici o categorie selectata-";


        public static List<DictionaryEntry> ReadFileWords(string fileName)
        {
            List<DictionaryEntry> list = new List<DictionaryEntry>();
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] strings = line.Split(',');

                        if (strings.Length == 4)
                        {
                            Category category = new Category(strings[1]);
                            DictionaryEntry dictionary = new DictionaryEntry(strings[0], strings[2], category, strings[3]);
                            list.Add(dictionary);
                        }
                        else
                        {
                            Console.WriteLine($"Formatul liniei incorect: {line}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la citirea din fișier: {ex.Message}");
            }
            return list;
        }

        public static List<Category> ListCategory(List<DictionaryEntry> Dictionary)
        {
            List<Category> list = new List<Category>();
            if (Dictionary != null)
            {
                foreach (DictionaryEntry entry in Dictionary)
                {
                    if (!list.Exists(c=>c.Name==entry.Category.Name))
                    {
                        list.Add(entry.Category);
                    }
                }
            }

            return list;
        }

        public static void VerifyAfterAddWord(string text, List<DictionaryEntry> Dictionary)
        {
            if (Dictionary.Exists(c => c.Word ==text))
                MessageBox.Show("Adaugarea s-a facut cu succes!");
            else
                MessageBox.Show("Adaugarea a esuat!");
        }

        public static void PrintWordsInComboBox(ComboBox cbCuvinte, List<DictionaryEntry> Dictionary)
        {
            cbCuvinte.Items.Clear();
            foreach (DictionaryEntry word in Dictionary)
            {
                cbCuvinte.Items.Add(word.Word);
            }
        }

        public static void PrintCategoriesInComboBox(ComboBox cbCategory, List<Category> Category)
        {
            cbCategory.Items.Clear();
            cbCategory.Items.Add(NoCategory);
            foreach (Category category in Category)
            {
                cbCategory.Items.Add(category.Name);
            }
        }

        public static string PathOnFile(string imagePath)
        {
            string path = "";
            foreach (char c in imagePath)
            {
                if (c != '"')
                {
                    path += c;
                }
            }
            return path;
        }

        public static void PrintImage(string caleImagine, Image imageControl)
        {
            if (File.Exists(caleImagine))
            {
                BitmapImage bitmapImage = new BitmapImage();
                using (FileStream stream = new FileStream(caleImagine, FileMode.Open))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                }
                imageControl.Source = bitmapImage;
            }
        }

       

    }
}

