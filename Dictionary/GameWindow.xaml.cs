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
using static System.Net.Mime.MediaTypeNames;

namespace Tema1_Dictionar
{

    public partial class GameWindow : Window
    {
        private List<DictionaryEntry> Dictionary { get; set; }
        List<DictionaryEntry> Words;
        private int raspunsuriCorecte;
        private int numberWords ;

        private static string PathOf = "C:\\Users\\Ioana\\source\\repos\\MAP\\Tema1_Dictionar\\Tema1_Dictionar\\Resorce\\";


        public GameWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Dictionary = new List<DictionaryEntry>();
            Words = new List<DictionaryEntry>();
            InitializationDictionary();
            btFinish.Visibility = Visibility.Hidden;

        }

        private void InitializationDictionary()
        {
            Dictionary = DictionaryMethods.ReadFileWords(PathOf + "cuvinte.txt");
        }

        private List<int> Generate5Numbers()
        {
            List<int> numbers = new List<int>();
            Random random = new Random();
            int size = Dictionary.Count;

            for (int i = 0; i < 5; i++)
            {
                int number = random.Next(0, size - 1);
                if (!numbers.Contains(number))
                {
                    numbers.Add(number);
                    Debug.WriteLine(number);
                }
                else
                    i = i - 1;
            }

            return numbers;
        }

        private List<DictionaryEntry> Generate5Word()
        {
            List<DictionaryEntry> wordsGame = new List<DictionaryEntry>();
            List<int> numbers = Generate5Numbers();
            for (int i = 0; i < numbers.Count; i++)
            {
                wordsGame.Add(Dictionary[numbers[i]]);
            }

            return wordsGame;
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


        private void VerifyWord(string correctWord)
        {
            if (tbRaspunsCuvant.Text == correctWord)
            {
                raspunsuriCorecte++;
                tbRaspunsuriCorecte.Text = raspunsuriCorecte.ToString();
                MessageBox.Show("Corect");
            }
            else
            {
                MessageBox.Show("Gresit, raspunsul era: "+correctWord);
            }
            numberWords++;
            PrintWords();

        }

        private void PrintWords()
        {
            Initialization();

            Random random = new Random();
            int number = random.Next(2);
            if (number == 0 || Words[numberWords].ImagePath == PathOf + "No_Image_Available.jpg")
            {
                tbDescriere.Text = Words[numberWords].Description;
            }
            else
            {
                image.Visibility = Visibility.Visible;
                PrintImage(Words[numberWords].ImagePath);
            }
            if (numberWords == 4)
            {
                btNext.Visibility = Visibility.Hidden;
                btFinish.Visibility = Visibility.Visible;
            }
        }

        private void btStart_Click(object sender, RoutedEventArgs e)
        {

            InitializationStart();
            Words = Generate5Word();
            PrintWords();
            btStart.Visibility = Visibility.Hidden;
            btNext.Visibility= Visibility.Visible;
            btFinish.Visibility= Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Close();
            window.Show();
        }

        private void InitializationStart()
        {
            numberWords = 0;
            raspunsuriCorecte = 0;
            tbRaspunsuriCorecte.Text = "0";
            Initialization();
        }

        private void Initialization()
        {
            tbDescriere.Text = "";
            tbRaspunsCuvant.Text = "";
        }


        private void btFinish_Click(object sender, RoutedEventArgs e)
        {
            Initialization();
            image.Visibility = Visibility.Hidden;
            MessageBox.Show("Felicitari! Ai raspuns la " + tbRaspunsuriCorecte.Text + " cuvinte corect!");
            btStart.Visibility = Visibility.Visible;
        }

        private void btNext_Click(object sender, RoutedEventArgs e)
        {
            image.Visibility = Visibility.Hidden;
            VerifyWord(Words[numberWords].Word);
        }
    }
}
