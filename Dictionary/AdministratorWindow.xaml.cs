using System;
using System.IO;
using System.Collections.Generic;
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
 
    public partial class AdministratorWindow : Window
    {
        private List<Admin> administrators;
        public AdministratorWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            administrators = new List<Admin>();
            readFileAdministrators("C:\\Users\\Ioana\\source\\repos\\MAP\\Tema1_Dictionar\\Tema1_Dictionar\\Resorce\\administratori.txt");

        }

        public void AddAdministrator(string newUsername, string newPassword)
        {
            if (!administrators.Exists(a => a.username == newUsername))
            {
                administrators.Add(new Admin(newUsername, newPassword));
            }
        }

        public void readFileAdministrators(string fileName)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] strings = line.Split(',');

                        if (strings.Length == 2)
                        {
                            string username = strings[0];
                            string password = strings[1];
                            AddAdministrator(username, password);

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
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;
            if (administrators.Exists(a => a.username == username) && administrators.Exists(a => a.password == password))
            {
                WordAdministratorWindow administrator = new WordAdministratorWindow();
                this.Close();
                administrator.Show();
            }
            else
            {
                MessageBox.Show($"Username sau password incorect.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Close();
            window.Show();
        }
    }

}

   


