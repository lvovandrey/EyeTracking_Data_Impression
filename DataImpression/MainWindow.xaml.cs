using DataImpression.ViewModel;
using System.Windows;
using DataImpression.Models;

namespace DataImpression
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainWindowViewModel(new Model(), this);
        }
    }
}
