using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tema1_Dictionar
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }
        private void Administrativ_Click_1(object sender, RoutedEventArgs e)
        {
            AdministratorWindow administrativ = new AdministratorWindow();
            this.Close();
            administrativ.Show();
        }

        private void CautareCuvinte_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow cautare = new SearchWindow();
            this.Close();
            cautare.Show();
        }

        private void Divertisment_Click(object sender, RoutedEventArgs e)
        {
            GameWindow divertisment = new GameWindow();
            this.Close();
            divertisment.Show();
        }


    }
}

  