using lab2.utils;
using lab2.viewmodel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            bool showStartupMessage = SettingsManager.LoadShowStartupMessageSetting();

            if (showStartupMessage)
            {
                Loaded += MainWindow_Loaded;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Показываем всплывающее окно
            MainViewModel.ShowStartupInfo();
        }

    }
}