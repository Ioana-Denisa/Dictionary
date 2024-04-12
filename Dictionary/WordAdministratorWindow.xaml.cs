using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using Dictionary.Properties;
using Path = System.IO.Path;



namespace Tema1_Dictionar
{
    public partial class WordAdministratorWindow : Window
    {
        private static string PathOf = "C:\\Users\\Ioana\\source\\repos\\MAP\\Dictionary\\Dictionary\\Resorce\\";
        private static string NoCategory = "-Nici o categorie selectata-";

        private List<DictionaryEntry> Dictionary { get; set; }
        private List<Category> Category { get; set; }

        public WordAdministratorWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            Dictionary = new List<DictionaryEntry>();
            Category = new List<Category>();

            Commands();
            comboBox.DropDownClosed += comboBox_DropDownClosed;
            comboBox.SelectionChanged += comboBox_SelectionChanged;
        }
        public void Commands()
        {
            Dictionary = DictionaryMethods.ReadFileWords(PathOf + "cuvinte.txt");
            Category = DictionaryMethods.ListCategory(Dictionary);
            DictionaryMethods.PrintCategoriesInComboBox(cbCategory, Category);
            DictionaryMethods.PrintCategoriesInComboBox(cbCategorieNoua, Category);
            DictionaryMethods.PrintWordsInComboBox(comboBox, Dictionary);
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

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedWord = comboBox.SelectedItem as string;

            DictionaryEntry word = Dictionary.Where(c => c.Word == selectedWord).FirstOrDefault();
            if (word != null)
            {
                tbCuvantFind.Text = word.Word;
                tbDescriereFound.Text = word.Description;
                tbCategorieFound.Text = word.Category.Name;
                string caleImagine = word.ImagePath;

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

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            var combobox = (ComboBox)sender;
            CollectionView itemsViewOriginal = (CollectionView)CollectionViewSource.GetDefaultView(combobox.Items);
            itemsViewOriginal.Filter = null;
        }

        private void AddTextBoxInfo(string filePath, List<string> lines)
        {

            for (int i = 0; i < lines.Count; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length == 4 && parts[0] == comboBox.Text)
                {
                    if (tbCuvantNou.Text != "")
                    {
                        parts[0] = tbCuvantNou.Text;
                        tbCuvantFind.Text = tbCuvantNou.Text;
                    }
                    else tbCuvantFind.Text = parts[0];

                    if (cbCategorieNoua.Text != "" && cbCategorieNoua.Text != "-Nici o categorie selectata-")
                    {
                        parts[1] = cbCategorieNoua.Text;
                        tbCategorieFound.Text = cbCategorieNoua.Text;
                    }
                    else tbCategorieFound.Text = parts[1];


                    if (tbDescriereNoua.Text != "")
                    {
                        parts[2] = tbDescriereNoua.Text;
                        tbDescriereFound.Text = tbDescriereNoua.Text;
                    }
                    else tbDescriereFound.Text = parts[2];

                    if (tbImagineNoua.Text != "")
                    {
                        parts[3] = DictionaryMethods.PathOnFile(PathOf + tbImagineNoua.Text);
                        string caleImagine = parts[3];
                        DictionaryMethods.PrintImage(caleImagine, imageControl);
                    }
                    else DictionaryMethods.PrintImage(parts[3], imageControl);

                    lines[i] = string.Join(",", parts);

                    break;
                }
            }
        }
        private void btnModificareCuvant_Click(object sender, RoutedEventArgs e)
        {
            if (Dictionary.Exists(c => c.Word == comboBox.Text))
            {
                DictionaryEntry word = Dictionary.Where(c => c.Word == tbCuvantFind.Text).FirstOrDefault();
                string filePath = PathOf + "cuvinte.txt";
                List<string> lines = File.ReadAllLines(filePath).ToList();

                AddTextBoxInfo(filePath, lines);

                File.WriteAllLines(filePath, lines);
                Commands();
                MessageBox.Show("Modificarea a fost facuta cu succes!");
            }
            else
                MessageBox.Show("Cuvantul nu exista in dictionar!");
        }


        private void btnAdaugareCuvant_Click(object sender, RoutedEventArgs e)
        {
            if (tbCuvantAdaugare.Text == "" || tbDescriereAdaugare.Text == "" || cbCategory.Text == "")
                MessageBox.Show("Adăugați informațiile necesare!");
            else
            {
                if (!Dictionary.Exists(x => x.Word == tbCuvantAdaugare.Text))
                {
                    if (!Category.Exists(c => c.Name == cbCategory.Text))
                    {
                        Category.Add(new Category(cbCategory.Text));
                    }
                    if (cbCategory.Text == NoCategory)
                        MessageBox.Show("Nici o categorie selectată! Reîncercați.");
                    else
                    {
                        string path;
                        if (tbImagineAdaugare.Text == "")
                        {
                            path = DictionaryMethods.PathOnFile(PathOf + "No_Image_Available.jpg");
                        }
                        else path = DictionaryMethods.PathOnFile(PathOf + tbImagineAdaugare.Text);

                        Dictionary.Add(new DictionaryEntry(tbCuvantAdaugare.Text, tbDescriereAdaugare.Text, new Category(cbCategory.Text), path));
                        File.AppendAllText(PathOf + "cuvinte.txt", tbCuvantAdaugare.Text + "," + cbCategory.Text + "," + tbDescriereAdaugare.Text + "," + path + Environment.NewLine);
                        Commands();
                        DictionaryMethods.VerifyAfterAddWord(tbCuvantAdaugare.Text, Dictionary);
                        InitializareButoane(tbImagineAdaugare, tbDescriereAdaugare, tbCuvantAdaugare, cbCategory);
                    }
                }
                else
                {
                    MessageBox.Show("Cuvantul acesta exista in dictionar!");
                    InitializareButoane(tbImagineAdaugare, tbDescriereAdaugare, tbCuvantAdaugare, cbCategory);
                }
            }
        }

        private void InitializareButoane(TextBox t1, TextBox t2, TextBox t3, ComboBox cb)
        {
            t1.Text = "";
            t2.Text = "";
            t3.Text = "";
            cb.Text = "";
        }
        private string LineOfFile(DictionaryEntry word)
        {
            return word.Word + "," + word.Category.Name + "," + word.Description + "," + word.ImagePath;
        }

        private void btnStergereCuvant_Click(object sender, RoutedEventArgs e)
        {
            if (Dictionary.Exists(c => c.Word == tbCuvantStergere.Text))
            {
                DictionaryEntry word = Dictionary.Where(c => c.Word == tbCuvantStergere.Text).FirstOrDefault();
                Dictionary.Remove(word);
                string caleFisier = PathOf + "cuvinte.txt";
                string linieDeSters = LineOfFile(word);
                List<string> linii = File.ReadAllLines(caleFisier).ToList();
                if (linii.Contains(linieDeSters))
                {
                    linii.Remove(linieDeSters);
                }
                File.WriteAllLines(caleFisier, linii);
                Commands();
                if (!Dictionary.Exists(c => c.Word == tbCuvantStergere.Text))
                    MessageBox.Show("Stergerea a fost facuta cu succes!");
                else
                    MessageBox.Show("Stergerea a esuat!");
            }
            else
            {
                MessageBox.Show("Cuvantul nu exista in dictionar!");
            }
            tbCuvantStergere.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdministratorWindow window = new AdministratorWindow();
            this.Close();
            window.Show();
        }

    }
}
