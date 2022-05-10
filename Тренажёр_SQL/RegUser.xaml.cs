using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace Тренажёр_SQL
{
    /// <summary>
    /// Логика взаимодействия для RegUser.xaml
    /// </summary>
    public partial class RegUser : Page
    {
        public int id;
        NavigationService nav;
        public RegUser( int i)
        {
            id = i;
            InitializeComponent();
        }
        private void SeeResults(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new RegUser_Results(id));
        }

        private void Exercise(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new RegUser_Exercises(id));
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
