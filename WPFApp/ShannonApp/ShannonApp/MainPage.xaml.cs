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

namespace ShannonApp
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Uri("AddCourse.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if(txtCourseArea.Text == "" && txtCourseNumber.Text == "")
            {
                //get everything
            }

            else if( txtCourseArea.Text != "" && txtCourseNumber.Text == "")
            {
                //get only matching course areas
            }

            else if(txtCourseArea.Text == "" && txtCourseNumber.Text != "")
            {
                //only search course number
            }

            else
            {
                //both fields are being searched
            }
        }
    }
}
