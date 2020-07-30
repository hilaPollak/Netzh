using Hits.Views.Grids;
using Hits.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hits
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Grid lastVisibleGrid;

        public MainWindow()
        {
            InitializeComponent();

            lastVisibleGrid = HomeGrid;
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SwitchGridEvent(object sender, MouseButtonEventArgs e)
        {
            lastVisibleGrid.Visibility = Visibility.Collapsed;
            lastVisibleGrid = ((sender as Control).Tag as Grid);
            lastVisibleGrid.Visibility = Visibility.Visible;
        }
        private void Facebook(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/%D7%A0%D7%A5-109057790474660/");
        }

    }
}
