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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OTONOTE
{
    /// <summary>
    /// Logique d'interaction pour MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void todayBtn_Click(object sender, RoutedEventArgs e)
        {
            NotePage notePage = new NotePage(DateTime.Now.Date);
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(notePage);
        }

        private void otherBtn_Click(object sender, RoutedEventArgs e)
        {
            EveryNotesOverviewPage everyNotesOverviewPage = new EveryNotesOverviewPage();
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(everyNotesOverviewPage);
        }
    }
}
