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
    /// Interaction logic for Slet.xaml
    /// </summary>
    public partial class Slet : Window
    {
        public Slet()
        {
            InitializeComponent();
            //starter ud med at alle input felter og labels er skjulte
            InputTitel.IsEnabled = false;
            InputInstruk.IsEnabled = false;
            InputUdgivelse.IsEnabled = false;
            InputOm.IsEnabled = false;
            InputNomi.IsEnabled = false;
            CheckVundet.IsEnabled = false;
            Annuller.IsEnabled = false;
            Sletfilm.IsEnabled = false;
        }

        private void BtnClickTilbage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClickSøg(object sender, RoutedEventArgs e)
        {
            bool FilmCheckBool = true;
            string sql, Output = "";
            SqlConnection cnn = new(@"Data Source=10.0.5.102,1433;Initial Catalog=LAX_DB;User ID=sa; Password=Guest1234");
            SqlCommand command;
            SqlDataReader dataReader;

            //Prøver at åbnne forbindelsen
            try { cnn.Open(); }
            catch { MessageBox.Show("DB connection error! Database could not be reached"); }


            //Prøver at finde filmen
            try
            {
                sql = $"SELECT FilmTitel FROM Film WHERE FilmTitel = '{InputSøg.Text}' ";
                command = new(sql, cnn);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    Output += dataReader.GetValue(0);

                if (Output.Length == 0)
                {
                    MessageBox.Show("Kunne ikke finde film");
                    FilmCheckBool = false;
                }

                dataReader.Close();
                command.Dispose();
            }
            catch
            {
                MessageBox.Show("Kunne ikke finde film");
                FilmCheckBool = false;
            }

            //Hvis filmen er fundet
            if (FilmCheckBool == true)
            {
                //sørger for at alle inputfelter er tomme
                {
                    InputTitel.Clear();
                    InputInstruk.Clear();
                    InputUdgivelse.Clear();
                    InputOm.SelectAll();
                    InputOm.Selection.Text = "";
                    InputNomi.Clear();
                    CheckVundet.IsChecked = false;
                }

                //gør knapperne synlige
                {
                    Annuller.IsEnabled = true;
                    Sletfilm.IsEnabled = true;
                }

                //sætter data fra databasen ind i felterne
                {
                    //FilmTitel
                    {
                        command = new($"SELECT FilmTitel FROM Film WHERE FilmTitel = '{InputSøg.Text}'", cnn);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                            InputTitel.Text = Convert.ToString(dataReader.GetValue(0));
                        dataReader.Close();
                    }

                    //FilmInstruk
                    {
                        command = new($"SELECT FilmInstruk FROM Film WHERE FilmTitel = '{InputSøg.Text}'", cnn);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                            InputInstruk.Text = Convert.ToString(dataReader.GetValue(0));
                        dataReader.Close();
                    }

                    //FilmUdgiv
                    {
                        command = new($"SELECT FilmUdgiv FROM Film WHERE FilmTitel = '{InputSøg.Text}'", cnn);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                            InputUdgivelse.Text = Convert.ToString(dataReader.GetValue(0));
                        dataReader.Close();
                    }

                    //FilmOm
                    {
                        command = new($"SELECT FilmOm FROM Film WHERE FilmTitel = '{InputSøg.Text}'", cnn);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                            Output = Convert.ToString(dataReader.GetValue(0));
                        dataReader.Close();
                        InputOm.SelectAll();
                        InputOm.Selection.Text = Output;
                    }

                    //FilmNomi
                    {
                        command = new($"SELECT FilmNomi FROM Film WHERE FilmTitel = '{InputSøg.Text}'", cnn);
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                            InputNomi.Text = Convert.ToString(dataReader.GetValue(0));
                        dataReader.Close();
                    }

                    //NomiVundet
                    {
                        command = new($"SELECT NomiVundet FROM Film WHERE FilmTitel = '{InputSøg.Text}'", cnn);
                        dataReader = command.ExecuteReader();
                        Nullable<int> Checkmark;
                        while (dataReader.Read())
                        {
                            try { Checkmark = Convert.ToInt32(dataReader.GetValue(0)); }
                            catch { Checkmark = null; }
                            if (Checkmark == 1)
                                CheckVundet.IsChecked = true;
                            else
                                CheckVundet.IsChecked = false;
                        }
                        dataReader.Close();
                    }
                }
            }

            //sletter al tekst i input felter og gør felterne og labels usynlige igen hvis film ikke kunne findes
            else
            {


                InputSøg.Clear();
                InputTitel.Clear();
                InputInstruk.Clear();
                InputUdgivelse.Clear();
                InputOm.SelectAll();
                InputOm.Selection.Text = "";
                InputNomi.Clear();
                CheckVundet.IsChecked = false;

                InputTitel.IsEnabled = false;
                InputInstruk.IsEnabled = false;
                InputUdgivelse.IsEnabled = false;
                InputOm.IsEnabled = false;
                InputNomi.IsEnabled = false;
                CheckVundet.IsEnabled = false;
                Annuller.IsEnabled = false;
                Sletfilm.IsEnabled = false;
            }

            cnn.Close();

        }

        private void BtnClickAnuller(object sender, RoutedEventArgs e)
        {

            InputTitel.Clear();
            InputInstruk.Clear();
            InputUdgivelse.Clear();
            InputOm.SelectAll();
            InputOm.Selection.Text = "";
            InputNomi.Clear();
            CheckVundet.IsChecked = false;
            Annuller.IsEnabled = false;
            Sletfilm.IsEnabled = false;

            //this.Close();
        }

        private void BtnClickSlet(object sender, RoutedEventArgs e)
        {
            MessageBoxResult slet = MessageBox.Show("Du er ved at slette denne film. Vil du fortsætte?", "Advarsel!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            switch (slet)
            {
                case MessageBoxResult.Yes:
                    string ValueSøg = InputSøg.Text;
                    string sql = $"DELETE FROM Film WHERE filmTitel = '{ValueSøg}'";
                    SqlConnection cnn = new(@"Data Source=10.0.5.102,1433;Initial Catalog=LAX_DB;User ID=sa; Password=Guest1234");
                    SqlCommand command = new SqlCommand(sql, cnn);
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    //Prøver at åbnne forbindelsen
                    try { cnn.Open(); }
                    catch { MessageBox.Show("DB connection error! Database could not be reached"); }

                    //Prøver at slette dataen
                    try
                    {
                        adapter.DeleteCommand = new SqlCommand(sql, cnn);
                        adapter.DeleteCommand.ExecuteNonQuery();


                        InputTitel.Clear();
                        InputInstruk.Clear();
                        InputUdgivelse.Clear();
                        InputOm.SelectAll();
                        InputOm.Selection.Text = "";
                        InputNomi.Clear();
                        CheckVundet.IsChecked = false;


                        MessageBox.Show("Alt data er nu slettet", "Slettet", MessageBoxButton.OK, MessageBoxImage.Information);

                        MainWindow objTilbage = new MainWindow();
                        this.Visibility = Visibility.Hidden;
                        objTilbage.Show();

                    }
                    catch
                    {
                        MessageBox.Show("Fejl! Data kunne ikke slettes");
                    }

                    cnn.Close();
                    break;

                case MessageBoxResult.No:
                    break;

            }
        }

        private void EnterPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                //skal opstarte BtnClickSøg - ved ikke hvordan
            }
        }

    }
}