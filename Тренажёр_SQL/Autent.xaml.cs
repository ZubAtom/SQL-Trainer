using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Тренажёр_SQL
{
    /// <summary>
    /// Логика взаимодействия для Autent.xaml
    /// </summary>
    public partial class Autent : Page
    {
        public NavigationService nav;
        public Autent()
        {
            InitializeComponent();
            BoxLogin.Text = "";
            BoxPassword.Password = "";
        }
        public void Enter(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "CheckLogin ";
                    command.Parameters.AddWithValue("@login", BoxLogin.Text);
                    command.Parameters.AddWithValue("@password", BoxPassword.Password);
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        MessageBox.Show("Некорректные логин и пароль");
                    while (reader.Read())
                    {
                        if (reader[0] != null)
                        {
                            bool dostup = Convert.ToBoolean(reader[0]);
                            int id = Convert.ToInt32(reader[1]);
                            nav = NavigationService.GetNavigationService(this);
                            if (dostup)
                            {
                                nav.Navigate(new Admin());
                            }
                            else
                            {
                                nav.Navigate(new RegUser(id));
                            }
                        }
                    }
                    reader.Close();
                    con.Close();
                }
            }
            catch (SqlException exe)
            {
                MessageBox.Show(exe.Message);
            }
        }
        private void Registrate(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Registration());
        }
    }
}
