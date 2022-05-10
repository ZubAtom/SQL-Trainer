using System.Windows.Navigation;

namespace Тренажёр_SQL
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Navigate(new Autent());
        }
    }
}
