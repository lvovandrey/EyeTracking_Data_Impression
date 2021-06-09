using DataImpression.ViewModel;
using System.Windows;
using DataImpression.Models;
using DataImpression.Tests;
using MahApps.Metro.Controls;

namespace DataImpression
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainWindowViewModel(this);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new WindowForTests();
            wnd.DataContext = this.DataContext;
            wnd.Show();
        }
    }
}
