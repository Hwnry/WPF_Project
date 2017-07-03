using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
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
        private List<Course> courseData;

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
            courseData = new List<Course>();

            if(txtCourseArea.Text == "" && txtCourseNumber.Text == "")
            {
                //get everything
                //connect to the database
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\WPF_Project\\Database\\Shannon.db");
                conn.Open();
                SQLiteCommand command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM COURSE;";
                SQLiteDataReader sdr = command.ExecuteReader();


                while (sdr.Read())
                {

                    courseData.Add(new Course { ID = sdr.GetInt32(0), Course_Number = sdr.GetString(1) });
                }
                sdr.Close();

                conn.Close();

                queryResults.ItemsSource = courseData;
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

        private void queryResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnRemove.IsEnabled = true;
            btnUpdate.IsEnabled = true;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            remove removeDialogue = new remove();

            if(removeDialogue.ShowDialog() == true)
            {
                //run query to remove the course
            }
        }
    }
}
