using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LAX_AS_WPF
{
    /// <summary>
    /// Interaction logic for Opret.xaml
    /// </summary>
    public partial class Opret : Window
    {
        public Opret()
        {
            InitializeComponent();
        }

        private void BtnClickTilbage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClickAnuller(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Er du sikker på du vil annullere?", "Advarsel!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            switch (result)
            {
                case MessageBoxResult.Yes: //sletter al input tekst for at annullere 
                    InputTitel.Clear();
                    InputInstruk.Clear();
                    InputUdgivelse.Clear();
                    InputOm.SelectAll();
                    InputOm.Selection.Text = "";
                    InputNomi.Clear();
                    CheckVundet.IsChecked = false;

                    //this.Close();
                    break;

                case MessageBoxResult.No:

                    break;
            }
        }

        private void BtnClickGem(object sender, RoutedEventArgs e)
        {
            //tager alle inputs og sætter dem ind i en variabel
            string Value1 = InputTitel.Text;
            string Value2 = InputInstruk.Text;
            Nullable<int> Value3;
            InputOm.SelectAll();
            string Value4 = InputOm.Selection.Text;
            string Value5 = InputNomi.Text;
            int Value6;
            if (CheckVundet.IsChecked == true)
                Value6 = 1;

            else
                Value6 = 0;




            string connection = @"Data Source=10.0.5.102,1433;Initial Catalog=LAX_DB;User ID=sa; Password=Guest1234";
            string sql;
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection cnn = new(connection);

            try { cnn.Open(); }
            catch { MessageBox.Show("DB connection error! Database could not be reached"); }


            try { Value3 = Convert.ToInt32(InputUdgivelse.Text); }
            catch
            {
                MessageBox.Show("Fejl! Udgivelsesår skal være tal");
                Value3 = null;
            }

            if (Value3 != null)
            {

                try
                {
                    //hvis filmen er blevet nomineret vil Value5 have text i sig, og derfor være længere end 1
                    if (Value5.Length > 1)
                    {
                        sql = $"INSERT INTO Film (FilmTitel, FilmInstruk, FilmUdgiv, FilmOm, FilmNomi, NomiVundet) " +
                                     $"VALUES('{Value1}', '{Value2}', '{Value3}', '{Value4}', '{Value5}', '{Value6}')";
                        SqlCommand command = new(sql, cnn);
                        adapter.InsertCommand = new SqlCommand(sql, cnn);
                        adapter.InsertCommand.ExecuteNonQuery();
                        command.Dispose();
                    }

                    //hvis filmen ikke har en nomination bliver Value5 til NULL da der så ikke er noget input
                    else
                    {
                        sql = $"INSERT INTO Film (FilmTitel, FilmInstruk, FilmUdgiv, FilmOm) " +
                                     $"VALUES('{Value1}', '{Value2}', '{Value3}', '{Value4}')";
                        SqlCommand command = new(sql, cnn);
                        adapter.InsertCommand = new SqlCommand(sql, cnn);
                        adapter.InsertCommand.ExecuteNonQuery();
                        command.Dispose();
                    }

                    MessageBox.Show("Din data er gemt!");
                }

                catch
                {
                    MessageBox.Show("Fejl! Data ikke gemt");
                }
            }

            cnn.Close();


        }
    }
}
