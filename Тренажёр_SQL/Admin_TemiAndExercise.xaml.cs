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
    /// Логика взаимодействия для Admin_Exercise.xaml
    /// </summary>
    public partial class Admin_TemiAndExercise : Page
    {
        public List<Exercise> exercises = new List<Exercise>();
        public List<Temi> temi = new List<Temi>();
        public List<int> tema_id = new List<int>();
        public Exercise exercise = null;
        public Temi tema = null;
        public Admin_TemiAndExercise()
        {
            InitializeComponent();
            List<string> lb = new List<string>();           
            lb.Add("Темы");
            lb.Add("Упражнения");
            Lb.ItemsSource = lb;
            GetTables();
            Cbe1.ItemsSource = tema_id;
            
        }

        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string s = e.AddedItems[0].ToString();
            if (s == "Темы")
            {
                Lve.Visibility = Visibility.Hidden;
                Lbe1.Visibility = Visibility.Hidden;
                Cbe1.Visibility = Visibility.Hidden;
                Lbe2.Visibility = Visibility.Hidden;
                Tbe2.Visibility = Visibility.Hidden;
                Lbe3.Visibility = Visibility.Hidden;
                Tbe3.Visibility = Visibility.Hidden;
                Lvt.Visibility = Visibility.Visible;
                Lbt.Visibility = Visibility.Visible;
                Tbt.Visibility = Visibility.Visible;
                Tbt.Text = "";
                BtAdd.Visibility = Visibility.Visible;
                BtClear.Visibility = Visibility.Hidden;
                BtDel.Visibility = Visibility.Hidden;
            }
            if (s == "Упражнения")
            {
                Lvt.Visibility = Visibility.Hidden;
                Lbt.Visibility = Visibility.Hidden;
                Tbt.Visibility = Visibility.Hidden;
                Lve.Visibility = Visibility.Visible;
                Lbe1.Visibility = Visibility.Visible;
                Cbe1.Visibility = Visibility.Visible;
                Cbe1.Text = "";
                Lbe2.Visibility = Visibility.Visible;
                Tbe2.Visibility = Visibility.Visible;
                Tbe2.Text = "";
                Lbe3.Visibility = Visibility.Visible;
                Tbe3.Visibility = Visibility.Visible;
                Tbe3.Text = "";
                BtAdd.Visibility = Visibility.Visible;
                BtClear.Visibility = Visibility.Hidden;
                BtDel.Visibility = Visibility.Hidden;

            }
            BtAdd.Visibility = Visibility.Visible;
        }

        public void GetTables()
        {
            Lvt.ItemsSource = null;
            Lve.ItemsSource = null;
            
            tema_id.Clear();
            exercises.Clear();
            temi.Clear();
            InitializeComponent();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("", con);
                    dataAdapter.SelectCommand = new SqlCommand("select * from Temi", con);
                    DataSet ds = new DataSet();
                    dataAdapter.Fill(ds, "Temi");
                    dataAdapter.SelectCommand = new SqlCommand("select * from Exercises", con);
                    dataAdapter.Fill(ds, "Exercises");
                    foreach (DataTable dt in ds.Tables)
                    {
                        if (dt.TableName == "Temi")
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                Temi note = new Temi();
                                var cells = row.ItemArray;
                                int i = 0;
                                foreach (object cell in cells)
                                {
                                    i++;
                                    switch (i)
                                    {
                                        case 1:
                                            note.id = Convert.ToInt32(cell);
                                            break;
                                        case 2:
                                            note.name = Convert.ToString(cell);
                                            break;
                                    }
                                }
                                tema_id.Add(note.id);
                                temi.Add(note);
                            }
                        }
                        if (dt.TableName == "Exercises")
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                Exercise note = new Exercise();
                                var cells = row.ItemArray;
                                int i = 0;
                                foreach (object cell in cells)
                                {
                                    i++;
                                    switch (i)
                                    {
                                        case 1:
                                            note.exer_id = Convert.ToInt32(cell);
                                            break;
                                        case 2:
                                            note.tema_id = Convert.ToInt32(cell);
                                            break;
                                        case 3:
                                            note.description = Convert.ToString(cell);
                                            break;
                                        case 4:
                                            note.etal_query = Convert.ToString(cell);
                                            break;
                                    }
                                }
                                exercises.Add(note);
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
            InitializeComponent();
            if (Lb.Text == "Темы")
            {
                Cbe1.ItemsSource = null;
                Cbe1.ItemsSource = tema_id;
            }
            Lvt.ItemsSource = temi;
            Lve.ItemsSource = exercises;
            if (exercise != null)
            InitializeComponent();
        }

        public void UpdateData(SelectionChangedEventArgs e)
        {
            if ((tema != null) || (exercise != null))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand("", con);
                        if (Lb.Text == "Темы")
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = "update Temi set Name_temi=@name where Number_temi=@id";
                            command.Parameters.AddWithValue("@name", Tbt.Text.Trim(' '));
                            command.Parameters.AddWithValue("@id", tema.id); 
                            tema.name = Tbt.Text.Trim(' ');
                        }
                        if (Lb.Text == "Упражнения")
                        {
                            command.CommandText = "UpdateExercise";
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@exer_id", exercise.exer_id);
                            command.Parameters.AddWithValue("@newtema", Convert.ToInt32(Cbe1.SelectedItem));
                            command.Parameters.AddWithValue("@exer_desc", Tbe2.Text.Trim(' '));
                            command.Parameters.AddWithValue("@etal_query", Tbe3.Text.Trim(' '));
                            exercise.tema_id = Convert.ToInt32(Cbe1.SelectedItem);
                            exercise.description = Tbe2.Text.Trim(' ');
                            exercise.etal_query = Tbe3.Text.Trim(' ');
                        }
                        command.ExecuteNonQuery();
                        con.Close();
                    }

                }
                catch (SqlException exe)
                {
                    MessageBox.Show(exe.Message);
                }
                GetTables();
            }
        }

        public void Delete()
        {
            if ((tema != null) || (exercise != null))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand("", con);
                        if (Lb.Text == "Темы")
                        {
                            command.CommandText = "delete Temi where Number_temi=@id update Temi set Number_temi=Number_temi-1 where Number_temi>@id";
                            command.Parameters.AddWithValue("@id", tema.id);
                        }
                        if (Lb.Text == "Упражнения")
                        {
                            command.CommandText = "delete Exercises where Number_exercise=@exer_id update Exercises set Number_exercise=Number_exercise-1 where Number_exercise>@exer_id";
                            command.Parameters.AddWithValue("@exer_id", exercise.exer_id);                         
                        }
                        command.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (SqlException exe)
                {
                    MessageBox.Show(exe.Message);
                }
                GetTables();                
            }
            exercise = null;
            tema = null;
        }

        private void BtClear_Click(object sender, RoutedEventArgs e)
        {
            tema = null;
            exercise = null;
            BtClear.Visibility = Visibility.Hidden;
            BtDel.Visibility = Visibility.Hidden;
            Tbt.Text = "";
            Tbe2.Text = "";
            Tbe3.Text = "";
            Cbe1.Text = "";
            BtAdd.Visibility = Visibility.Visible;
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if ((Tbt.Text.Trim(' ') != "") || ((Cbe1.Text != "") && (Tbe2.Text.Trim(' ') != "") && (Tbe3.Text.Trim(' ') != "")))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                    {
                        con.Open();
                        SqlCommand commamd = new SqlCommand("", con);
                        commamd.CommandType = CommandType.StoredProcedure;
                        if (Lb.Text == "Темы")
                        {
                            commamd.CommandText = "AddTema";
                            commamd.Parameters.AddWithValue("@name", Tbt.Text.Trim(' '));
                        }
                        if (Lb.Text == "Упражнения")
                        {
                            commamd.CommandText = "AddExercise";
                            commamd.Parameters.AddWithValue("@tema_id", Convert.ToInt32(Cbe1.Text));
                            commamd.Parameters.AddWithValue("@description", Tbe2.Text.Trim(' '));
                            commamd.Parameters.AddWithValue("@etal_query", Tbe3.Text.Trim(' '));
                        }
                        commamd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (SqlException exe)
                {
                    MessageBox.Show(exe.Message);
                }
                GetTables();
                Tbt.Text = "";
                Tbe2.Text = "";
                Tbe3.Text = "";
                Cbe1.Text = "";
            }
        }

        private void BtDel_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            exercise = null;
            tema = null;
            BtClear.Visibility = Visibility.Hidden;
            BtDel.Visibility = Visibility.Hidden;
            BtClear_Click(null, null);
            GetTables();
            BtAdd.Visibility = Visibility.Visible;
        }

        private void Lvt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtAdd.Visibility = Visibility.Hidden;
            BtClear.Visibility = Visibility.Visible;
            BtDel.Visibility = Visibility.Visible;
            if (e.AddedItems.Count > 0)
            {
                tema = (Temi)e.AddedItems[0];
            }
            Tbt.Text = tema.name;
        }

        private void Lve_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            BtAdd.Visibility = Visibility.Hidden;
            BtClear.Visibility = Visibility.Visible;
            BtDel.Visibility = Visibility.Visible;
            if (e.AddedItems.Count > 0)
            {
                exercise = (Exercise)e.AddedItems[0];
            }
            Cbe1.SelectedItem = exercise.tema_id;
            Tbe2.Text = exercise.description;
            Tbe3.Text = exercise.etal_query;

        }

        private void Tbt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Tbt.Text.Trim(' ') != "")
            {
                UpdateData(null);
            }
        }

        private void Cbe1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Cbe1.Text.Trim(' ') != "")&&(Tbe2.Text.Trim(' ') != "") &&(Tbe3.Text.Trim(' ') != ""))
            {
                UpdateData(e);
            }
        }

        private void Tbe2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((Cbe1.Text.Trim(' ') != "") && (Tbe2.Text.Trim(' ') != "") && (Tbe3.Text.Trim(' ') != ""))
            {
                UpdateData(null);
            }
        }

        private void Tbe3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((Cbe1.Text.Trim(' ') != "") && (Tbe2.Text.Trim(' ') != "") && (Tbe3.Text.Trim(' ') != ""))
            {
                UpdateData(null);
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
