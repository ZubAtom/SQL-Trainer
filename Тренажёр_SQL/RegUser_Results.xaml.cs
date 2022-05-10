using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Тренажёр_SQL
{
    /// <summary>
    /// Логика взаимодействия для RegUser_Results.xaml
    /// </summary>
    public partial class RegUser_Results : Page
    {
        public int id;
        public NavigationService nav;
        public RegUser_Results(int i)
        {
            id = i;
            InitializeComponent();
            List<FullResults> res = new List<FullResults>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "FullResultsForRegUser";
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        FullResults note = new FullResults();
                        note.tema_id = Convert.ToInt32(reader[0]);
                        note.tema_name = Convert.ToString(reader[1]);
                        note.exer_id = Convert.ToInt32(reader[2]);
                        note.exer_desc = Convert.ToString(reader[3]);
                        note.result = Convert.ToBoolean(reader[4]);
                        res.Add(note);
                    }
                    reader.Close();
                    con.Close();
                }
                
                Lv.ItemsSource = res;
            }
            catch (SqlException exe)
            {
                MessageBox.Show(exe.Message);
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService service = NavigationService.GetNavigationService(this);
            if (service.CanGoBack)
            {
                service.GoBack();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService service = NavigationService.GetNavigationService(this);
            service.Navigate(new Autent());

        }
    }
}
