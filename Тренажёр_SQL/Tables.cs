using System.Collections.ObjectModel;

namespace Тренажёр_SQL
{

    public class Users
    {
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public bool dostup { get; set; }
        public string secondname { get; set; }
        public string name { get; set; }
        public string fathername { get; set; }
    }
    public class Temi
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class Exercise
    {
        public int exer_id { get; set; }
        public int tema_id { get; set; }
        public string description { get; set; }
        public string etal_query { get; set; }
    }

    public class ExerciseForUser
    {
        public int exer_id { get; set; }
        public int tema_id { get; set; }
        public string description { get; set; }
        public string progress { get; set; }
        public string etal_query { get; set; }
    }
    public class Results
    {
        public int id { get; set; }
        public int exer_id { get; set; }
        public int tema_id { get; set; }
        public bool result { get; set; }
    }
    public class FullResults
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public int tema_id { get; set; }
        public string tema_name { get; set; }
        public int exer_id { get; set; }
        public string exer_desc { get; set; }
        public bool result { get; set; }
    }
    public class TreeItem
    {
        public int id { get; set; }
        public string type { get; set; }
        public string Title { get; set; }
        public ObservableCollection<TreeItem> Items { get; set; }
        public TreeItem()
        {
            this.Items = new ObservableCollection<TreeItem>();
        }
    }
}
