using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace Тренажёр_SQL
{
    /// <summary>
    /// Логика взаимодействия для RegUser_Exercises.xaml
    /// </summary>
    public partial class RegUser_Exercises : Page
    {
        public int userid;
        public ExerciseForUser exercise  = null;
        public TreeItem ti;
        public TreeItem tr;
        public RegUser_Exercises(int i)
        {
            userid = i;
            InitializeComponent();
            ti = new TreeItem() { Title = "Темы"};
            List<TreeItem> items = new List<TreeItem>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "AllTemi";
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        TreeItem tree = new TreeItem() { id = Convert.ToInt32(reader[0]), Title = Convert.ToString(reader[1]), type="t" };
                        items.Add(tree);
                        ti.Items.Add(tree);
                    }
                    reader.Close();
                    command.CommandText = "AllExercises";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        TreeItem tree = new TreeItem() { type="e", id = Convert.ToInt32(reader[0])};
                        foreach (TreeItem tr in items)
                        {
                            if (tr.id == Convert.ToInt32(reader[1]))
                            {
                                tree.Title = "Упражнение №" + Convert.ToString(tr.Items.Count + 1);
                                tr.Items.Add(tree);
                            }   
                        }                      
                    }
                    reader.Close();
                    Tv.Items.Add(ti);
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
            exercise = null;
            if (e!= null)
            {
                tr = (TreeItem)e.NewValue;
                if (tr.type == "e")
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                        {
                            con.Open();
                            SqlCommand command = new SqlCommand("", con);
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "SelectExercise";
                            command.Parameters.AddWithValue("@id", userid);
                            command.Parameters.AddWithValue("@exerid", tr.id);
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                ExerciseForUser note = new ExerciseForUser();
                                note.exer_id = Convert.ToInt32(reader[0]);
                                note.tema_id = Convert.ToInt32(reader[1]);
                                note.description = Convert.ToString(reader[2]);
                                note.progress = Convert.ToString(reader[3]);
                                note.etal_query = Convert.ToString(reader[4]);
                                Tblock.Text = note.description;
                                Tbox.Text = note.progress;
                                exercise = note;
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
            if (exercise != null)
            {
                if (exercise.progress.Trim(' ') != "")
                {
                    Bt.Visibility = Visibility.Visible;
                }
                else
                {
                    Bt.Visibility = Visibility.Hidden;
                }
            }
        }

        public void UpdateProgress()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("", con);
                    command.CommandType = CommandType.Text;
                    command.CommandText = "update Results set Progress=@progress where Id_user=@id and Number_exercise=@exerid";
                    command.Parameters.AddWithValue("@id", userid);
                    command.Parameters.AddWithValue("@exerid", exercise.exer_id);
                    command.Parameters.AddWithValue("@progress", Tbox.Text.Trim(' '));
                    command.ExecuteNonQuery();
                    con.Close();
                    exercise.progress = Tbox.Text.Trim(' ');
                }
            }
            catch (SqlException exe)
            {
                MessageBox.Show(exe.Message);
            }
            if (Tbox.Text.Trim(' ') == "")
            {
                Bt.Visibility = Visibility.Hidden;
            }
            else
            {
                Bt.Visibility = Visibility.Visible;
            }
        }

        private void Tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (exercise != null)
            {
                UpdateProgress();
            }
        }

        private void Bt_Click(object sender, RoutedEventArgs e)
        {
            string etalon = " " + exercise.etal_query.Trim(' ').ToUpper();
            string query = " " + exercise.progress.Trim(' ').ToUpper();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Theatres"].ConnectionString))
                {
                    con.Open();
                    bool result = true;
                    SqlDataAdapter adapter = new SqlDataAdapter("", con);
                    using (DataSet ds = new DataSet())
                    {
                        int type1 = 0;
                        int type2 = 0;
                        SqlCommand command = new SqlCommand("", con);
                        command.CommandText = "ExecuteQuery";
                        command.CommandType = CommandType.StoredProcedure;
                        if (etalon.Contains(" INSERT INTO ")|| etalon.Contains(" DELETE ") || etalon.Contains(" UPDATE ")|| etalon.Contains(" DROP ") || etalon.Contains("\r\nINSERT INTO ") || etalon.Contains("\r\nDELETE ") || etalon.Contains("\r\nUPDATE ") || etalon.Contains("\r\nDROP "))
                        {
                            type1 = 1;
                            string str = "";
                            string[] strings = exercise.etal_query.Split(' ', '\r', '\n');
                            for (int i = 0; i+1 < strings.Length; i++)
                            {
                                if ((strings[i].ToUpper() == "INTO")|| (strings[i].ToUpper() == "FROM") || (strings[i].ToUpper() == "UPDATE") || (strings[i].ToUpper() == "DROP"))
                                {
                                    str = str+" select * from " + strings[i + 1];
                                }
                            }                            
                        command.Parameters.AddWithValue("@query", exercise.etal_query.Trim(' ') + str);
                        adapter.SelectCommand = command;
                        adapter.Fill(ds, "Etalon");
                        adapter.SelectCommand.Parameters.Clear();
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@query", exercise.etal_query.Trim(' '));
                            adapter.SelectCommand = command;
                            adapter.Fill(ds, "Etalon");
                            adapter.SelectCommand.Parameters.Clear();       
                        }
                        if (query.Contains(" INSERT INTO ") || query.Contains(" DELETE ") || query.Contains(" UPDATE ") || query.Contains(" DROP ") || query.Contains("\r\nINSERT INTO ") || query.Contains("\r\nDELETE ") || query.Contains("\r\nUPDATE ") || query.Contains("\r\nDROP "))
                        {
                            type2 = 1;
                            string str = "";
                            char[] chars = new char[3] { ' ', '\r', '\n' };
                            string[] strings = exercise.progress.Split(chars);
                            for (int i = 0; i+1 < strings.Length; i++)
                            {
                                if ((strings[i].ToUpper() == "INTO") || (strings[i].ToUpper() == "FROM") || (strings[i].ToUpper() == "UPDATE") || (strings[i].ToUpper() == "DROP"))
                                {
                                    str =  str +" select * from " + strings[i + 1];
                                }
                            }
                            command.Parameters.AddWithValue("@query", exercise.progress.Trim(' ') + str);
                            adapter.SelectCommand = command;
                            adapter.Fill(ds, "Query");
                            adapter.SelectCommand.Parameters.Clear();
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@query", exercise.progress.Trim(' '));
                            adapter.SelectCommand = command;
                            adapter.Fill(ds, "Query");
                            adapter.SelectCommand.Parameters.Clear();
                        }
                        if (type1 != type2)
                        {
                            result = false;
                        }
                        DataTable table1 = ds.Tables["Etalon"];
                        DataTable table2 = ds.Tables["Query"];
                        if (table1.Rows.Count != table2.Rows.Count)
                        {
                            result = false;
                        }
                        else
                        {
                            if (table1.Columns.Count != table2.Columns.Count)
                            {
                                result = false;
                            }
                            else
                            {
                                for (int i = 0; i < table1.Rows.Count; i++)
                                {
                                    for (int j = 0; j < table1.Columns.Count; j++)
                                    {
                                        if (!Equals(table1.Rows[i].ItemArray[j], table2.Rows[i].ItemArray[j]))
                                        {
                                            result = false;
                                        }
                                    }
                                }
                            }
                        }
                        if (result)
                        {
                            Updateresult();
                            MessageBox.Show("Верно");
                        }
                        else
                        MessageBox.Show("Неверно");
                    }
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

        private void Updateresult()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Trainer"].ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("", con);
                    command.CommandText = "Update Results set result=1 where Id_user=@id and Number_exercise=@exer_id";
                    command.Parameters.AddWithValue("@id", userid);
                    command.Parameters.AddWithValue("@exer_id", exercise.exer_id);
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
