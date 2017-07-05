using System;
using System.Collections;
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
        public static Course selectedCourse;

        public MainPage()
        {
            InitializeComponent();
            selectedCourse = new Course();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Uri("AddCourse.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            populateData();
        }

        private void queryResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnRemove.IsEnabled = true;
            btnUpdate.IsEnabled = true;

            selectedCourse = queryResults.SelectedItem as Course;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            remove removeDialogue = new remove();

            if (removeDialogue.ShowDialog() == true)
            {
                //run query to remove the course
                SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\WPF_Project\\Database\\Shannon.db");
                conn.Open();
                SQLiteCommand command = conn.CreateCommand();

                Course test = queryResults.SelectedItem as Course;
                if (test != null)
                {
                    command.CommandText = "DELETE FROM course WHERE ID = " + test.ID.ToString() +";";
                    command.ExecuteNonQuery();
                    txtErrorBlock.Text = "Course Removed";
                    populateData();

                    //TO DO: remove the associated dates?
                }

                else
                {
                    txtErrorBlock.Text = "Error removing the course."; 
                }


            }
        }

        private void populateData()
        {
            courseData = new List<Course>();

            //connect to the database
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\WPF_Project\\Database\\Shannon.db");
            conn.Open();
            SQLiteCommand command = conn.CreateCommand();

            if (txtCourseArea.Text == "" && txtCourseNumber.Text == "")
            {
                //get everything
                command.CommandText = "SELECT Course.ID, Course_Area, number, title FROM COURSE LEFT JOIN Course_Area on course.fk_course_area = course_area.id;";
                SQLiteDataReader sdr = command.ExecuteReader();


                while (sdr.Read())
                {

                    courseData.Add(new Course { ID = sdr.GetInt32(0), courseArea = sdr.GetString(1) ,Course_Number = sdr.GetString(2),
                    Title = sdr.GetString(3)});
                }
                sdr.Close();

                conn.Close();

                queryResults.ItemsSource = courseData;
            }

            else if (txtCourseArea.Text != "" && txtCourseNumber.Text == "")
            {
                //get only matching course areas
            }

            else if (txtCourseArea.Text == "" && txtCourseNumber.Text != "")
            {
                //only search course number
            }

            else
            {
                //both fields are being searched             
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            update updateDialogue = new update();
            updateDialogue.Show();
        }
    }
}
