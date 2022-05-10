using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Тренажёр_SQL
{
    /// <summary>
    /// Логика взаимодействия для Admin_ManageEducationDB.xaml
    /// </summary>
    public partial class Admin_ManageEducationDB : Page
    {
        TreeItem tree = new TreeItem() { Title = "Таблицы" };
        public Admin_ManageEducationDB()
        {
            InitializeComponent();

            GetTables();
            Tv.Items.Add(tree);
        }

        public void GetTables()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Theatres"].ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("", con);
                    dataAdapter.SelectCommand = new SqlCommand("select * from sysobjects Where xtype = 'U' OR xtype = 'PK' OR xtype = 'C' OR xtype='F' ", con);
                    DataSet ds = new DataSet();
                    dataAdapter.Fill(ds, "DB");
                    foreach (DataTable dt in ds.Tables)
                    {
                        if (dt.TableName == "DB")
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                if ("U" == Convert.ToString(row.ItemArray[2]).Trim(' '))
                                {
                                    TreeItem tr = new TreeItem() { Title = Convert.ToString(row.ItemArray[0]) };
                                    tree.Items.Add(tr);
                                }
                            }
                        }
                    }
                    con.Close();
                }

            }
            catch (SqlException exe)
            {
                MessageBox.Show(exe.Message);
            }
        }

        private void Tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e != null)
            {
                TreeItem tr = (TreeItem)e.NewValue;
                if ((tr != null)&&(tr.Title != "Таблицы"))
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Theatres"].ConnectionString))
                        {
                            con.Open();
                            SqlDataAdapter dataAdapter = new SqlDataAdapter("", con);
                            dataAdapter.SelectCommand = new SqlCommand("select * from " + tr.Title, con);
                            DataSet ds = new DataSet();
                            dataAdapter.Fill(ds, tr.Title);
                            foreach (DataTable dt in ds.Tables)
                            {
                                if (dt.TableName == tr.Title)
                                {
                                    Dg.ItemsSource = null;
                                    Dg.ItemsSource = dt.AsDataView();
                                }
                            }
                            con.Close();
                        }

                    }
                    catch (SqlException exe)
                    {
                        MessageBox.Show(exe.Message);
                    }
                }
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

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            tree.Items.Clear();
            GetTables();
            Tv.Items.Clear();
            Tv.Items.Add(tree);
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            if (Tb.Text.ToUpper().Contains("DROP DATABASE "))
                MessageBox.Show("Нельзя удалить базу данных");
            else
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Theatres"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(Tb.Text, con);
                    command.ExecuteNonQuery();
                    con.Close();
                }

            }
            catch (SqlException exe)
            {
                MessageBox.Show(exe.Message);
            }
        }
    }
}
