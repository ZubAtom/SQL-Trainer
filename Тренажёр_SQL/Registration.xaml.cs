using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Тренажёр_SQL
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public NavigationService nav;
        public Registration()
        {
            InitializeComponent();
        }
        private void FinishRegistrate(object sender, RoutedEventArgs e)
        {
            if ((NewName.Text.Trim(' ') != "") & (NewName.Text.Trim(' ') != "") & (NewLogin.Text.Trim(' ') != "") & (NewPassword.Text.Trim(' ') != ""))
            {
                if (CheckPassword(NewPassword.Text))
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                        {
                            con.Open();
                            SqlCommand command = new SqlCommand("", con);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@login", NewLogin.Text.Trim(' '));
                            command.CommandText = "CheckCreateLogin";
                            SqlDataReader reader = command.ExecuteReader();
                            bool check = true;
                            while (reader.Read())
                            {
                                if (reader[0] != null)
                                {
                                    check = false;
                                }
                            }
                            reader.Close();
                            if (check)
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                if (NewFathername.Text.Trim(' ') == "")
                                {
                                    command.CommandText = "AddRegUserWithoutFathername ";
                                }
                                else
                                {
                                    command.CommandText = "AddRegUserWithFathername ";
                                    command.Parameters.AddWithValue("@fathername", NewFathername.Text.Trim(' '));
                                }
                                command.Parameters.AddWithValue("@password", NewPassword.Text.Trim(' '));
                                command.Parameters.AddWithValue("@name", NewName.Text.Trim(' '));
                                command.Parameters.AddWithValue("@secondname", NewSecondname.Text.Trim(' '));
                                command.ExecuteNonQuery();
                                con.Close();
                                nav = NavigationService.GetNavigationService(this);
                                nav.GoBack();
                                while (nav.CanGoBack)
                                {
                                    nav.RemoveBackEntry();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Логин уже существует. Введите другой.");
                            }
                        }
                    }
                    catch (SqlException exe)
                    {
                        MessageBox.Show(exe.Message);
                    }
                }

            }
            else
            {
                MessageBox.Show("Заполните все обязательные поля");
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
        private bool CheckPassword (string password)
        {
            bool flag = true;
            if ((password.Trim(' ').Length<8)||(password == password.ToUpper())||(password == password.ToLower()))
            {
                Tip.Text = "Пароль должен быть не короче 8 символов и включать буквы верхнего и нижнего регистров";
                flag = false;
            }
            return flag;
        }
    }
}
