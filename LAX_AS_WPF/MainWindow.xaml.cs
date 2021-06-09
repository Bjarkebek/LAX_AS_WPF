using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LAX_AS_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void BtnClickOpret(object sender, RoutedEventArgs e)
        {
            Opret objOpret = new();
            objOpret.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            objOpret.Show();
        }

        private void BtnClickRediger(object sender, RoutedEventArgs e)
        {
            Rediger objRediger = new Rediger();
            objRediger.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            objRediger.Show();
        }

        private void BtnClickSlet(object sender, RoutedEventArgs e)
        {
            Slet objSlet = new Slet();
            objSlet.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            objSlet.Show();
        }

        private void BtnClickForbind(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection cnn = new(@"Data Source=10.0.5.102,1433;Initial Catalog=LAX_DB;User ID=sa; Password=Guest1234");
                cnn.Open();
                MessageBox.Show("Databasen åbnet");
                cnn.Close();
            }
            catch
            {
                MessageBox.Show("Fejl! Database kan ikke tilgås");
            }
        }
    }
}
