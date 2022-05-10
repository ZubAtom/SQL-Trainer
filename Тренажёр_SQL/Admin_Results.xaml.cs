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
    /// Логика взаимодействия для Admin_Results.xaml
    /// </summary>
    public partial class Admin_Results : Page
    {
        public Admin_Results()
        {
            InitializeComponent();
            List<FullResults> res = new List<FullResults>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command= new SqlCommand("", con);
                    command.CommandText = "FullResults";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader(); 
                    while (reader.Read())
                    {
                        FullResults note = new FullResults();
                        note.user_id = Convert.ToInt32(reader[0]);
                        note.user_name = Convert.ToString(reader[1]);
                        note.tema_id = Convert.ToInt32(reader[2]);
                        note.tema_name = Convert.ToString(reader[3]);
                        note.exer_id = Convert.ToInt32(reader[4]);
                        note.exer_desc = Convert.ToString(reader[5]);
                        note.result = Convert.ToBoolean(reader[6]);
                        res.Add(note);
                    }
                    Lv.ItemsSource = res;
                    con.Close();
                }
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
