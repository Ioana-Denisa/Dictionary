using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tema1_Dictionar
{
    public partial class SearchWindow : Window
    {
        private static string PathOf = "C:\\Users\\Ioana\\source\\repos\\MAP\\Tema1_Dictionar\\Tema1_Dictionar\\Resorce\\";
        private static string NoCategory = "-Nici o categorie selectata-";

        private List<DictionaryEntry> Dictionary { get; set; }
        private List<Category> Category { get; set; }
        public SearchWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            Dictionary = new List<DictionaryEntry>();
            Category = new List<Category>();

            Commands();
            ComboBoxCommands();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();   
            this.Close();
            window.Show();
        }

        private void comboBox_KeyUp(object sender, KeyEventArgs e)
        {
            var combobox = (ComboBox)sender;
            combobox.IsDropDownOpen = true;

            CollectionView itemsViewOriginal = (CollectionView)CollectionViewSource.GetDefaultView(combobox.Items);
            itemsViewOriginal.Filter = ((o) =>
            {
                if (String.IsNullOrEmpty(combobox.Text))
                {
                    return true;
                }
                else
                {
                    return o.ToString().StartsWith(combobox.Text, true, null);
                }
            });

            itemsViewOriginal.Refresh();

            if (string.IsNullOrEmpty(combobox.Text) && combobox.Items.Count > 0)
            {
                combobox.Items.Refresh();
            }
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            var combobox = (ComboBox)sender;
            CollectionView itemsViewOriginal = (CollectionView)CollectionViewSource.GetDefaultView(combobox.Items);
            itemsViewOriginal.Filter = null;
        }

        public void Commands()
        {
            Dictionary= DictionaryMethods.ReadFileWords(PathOf + "cuvinte.txt");
            Category=DictionaryMethods.ListCategory(Dictionary);
            DictionaryMethods.PrintCategoriesInComboBox(cbCategory, Category);
            DictionaryMethods.PrintWordsInComboBox(cbCuvinte, Dictionary);
        }

        private void ComboBoxCommands()
        {
            cbCategory.DropDownClosed += comboBox_DropDownClosed;
            cbCategory.SelectionChanged += cbCategory_SelectionChanged;
            cbCuvinte.DropDownClosed += comboBox_DropDownClosed;
            cbCuvinte.SelectionChanged += cbCuvinte_SelectionChanged;

        }
        private void PrintImage(string caleImagine)
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
                image.Source = bitmapImage;
            }
        }

        private void cbCuvinte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedWord = cbCuvinte.SelectedItem as string;
            if (selectedWord != null)
            {
                DictionaryEntry word = Dictionary.Where(c => c.Word == selectedWord).FirstOrDefault();
                tbCategorie.Text = word.Category.Name;
                tbCuvant.Text = word.Word;
                tbDescriere.Text = word.Description;
                PrintImage(word.ImagePath);
            }
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbCuvinte.Items.Clear();
            var selectedWordCategory = cbCategory.SelectedItem as string;
            if(selectedWordCategory!=null)
            {
                if(selectedWordCategory==NoCategory)
                {
                    DictionaryMethods.PrintWordsInComboBox(cbCuvinte, Dictionary);
                }
                else
                {
                    foreach (DictionaryEntry word in Dictionary)
                    {
                        if (word.Category.Name == selectedWordCategory)
                        {
                            cbCuvinte.Items.Add(word.Word);
                        }
                    }
                }  
            } 
        }

    }
}
