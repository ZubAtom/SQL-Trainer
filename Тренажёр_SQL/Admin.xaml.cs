using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Тренажёр_SQL
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        NavigationService nav;
        public Admin()
        {
            InitializeComponent();
        }
        public void SeeResults(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Admin_Results());
        }
        public void UsersSetting(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Admin_Users());
        }

        public void ExercisesSetting(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Admin_TemiAndExercise());
        }

        public void ManageDataBase(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Admin_ManageEducationDB());
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
