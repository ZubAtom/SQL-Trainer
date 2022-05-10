using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.SqlClient;
using System.Configuration;

namespace Тренажёр_SQL
{
    /// <summary>
    /// Логика взаимодействия для Admin_Users.xaml
    /// </summary>
    public partial class Admin_Users : Page
    {
        public Users user;
        public Admin_Users()
        {
            InitializeComponent();
            Up.Visibility = Visibility.Hidden;
            Down.Visibility = Visibility.Hidden;
            GetUsers();
        }

        public void Upgrade(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("", con);
                    command.CommandText = "Update Users set Level_dostup=1 where Id_user=@id";
                    command.Parameters.AddWithValue("@id", user.id);
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (SqlException exe)
            {
                MessageBox.Show(exe.Message);
            }
            GetUsers();
        }

        public void Regrade(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("", con);
                    command.CommandText = "Update Users set Level_dostup=0 where Id_user=@id";
                    command.Parameters.AddWithValue("@id", user.id);
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (SqlException exe)
            {
                MessageBox.Show(exe.Message);
            }
            GetUsers();
        }

        public void Lv_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                user = (Users)e.AddedItems[0];
                if (user.dostup)
                {
                    Down.Visibility = Visibility.Visible;
                    Up.Visibility = Visibility.Hidden;
                }
                else
                {
                    Up.Visibility = Visibility.Visible;
                    Down.Visibility = Visibility.Hidden;
                }
            }
        }
        public void GetUsers()
        {
            List<Users> users = new List<Users>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("select * from Users", con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Users note = new Users();
                        note.id = Convert.ToInt32(reader[0]);
                        note.login = Convert.ToString(reader[1]);
                        note.secondname = Convert.ToString(reader[4]);
                        note.name = Convert.ToString(reader[5]);
                        note.dostup = Convert.ToBoolean(reader[3]);
                        users.Add(note);
                    }
                    reader.Close();
                    Lv.ItemsSource = users;
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
