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
        private static ObservableCollection<Course> courseData;
        public static Course selectedCourse;

        public MainPage()
        {
            InitializeComponent();
            selectedCourse = new Course();
            courseData = new ObservableCollection<Course>();
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
                    command.CommandText = "DELETE FROM course WHERE ID = " + test.ID.ToString() + ";";
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

            courseData.Clear();
            //connect to the database


            if (txtCourseArea.Text == "" && txtCourseNumber.Text == "")
            {
                //get everything



                searchQuery("select course.id as 'ID' ,approval_date as 'Approval Date', course_area as 'Course Area'," +
                             " number as 'Course Number', title as 'Course Title', department as 'Department'," +
                             " instructor_last_name as 'Instructor Last Name', Instructor_first_name as " +
                             "'Instructor First Name', academic_org as 'Academic Organization', instruction_mode as" +
                             " 'Instruction Mode', color_code as 'Color Code', course_id as 'Course ID'," +
                             " student_verification_method as 'Student Verification Method', grandfather as" +
                             " 'Grandfather Color Code', notes as 'Notes', approved as 'Approved' from course left outer join course_area on" +
                             " course_area.id = course.FK_course_area left outer join department on department.id = " +
                             "course.FK_Department left outer join instruction_mode on instruction_mode.id = " +
                             "course.FK_Instruction_Mode left outer join grandfather_color_code on grandfather_color_code.id " +
                             "= course.FK_Grandfather_Color_Code left outer join instructor on instructor.id = " +
                             "course.FK_Instructor left outer join academic_org on academic_org.id = course.FK_Academic_ORG" +
                             " left outer join approval_date on approval_date.FK_Course = course.id;");


            }

            else if (txtCourseArea.Text != "" && txtCourseNumber.Text == "")
            {
                ////get only matching course areas

                searchQuery("select course.id ,approval_date as 'Approval Date', course_area as 'Course Area'," +
                    " number as 'Course Number', title as 'Course Title', department as 'Department'," +
                    " instructor_last_name as 'Instructor Last Name', Instructor_first_name as " +
                    "'Instructor First Name', academic_org as 'Academic Organization', instruction_mode as" +
                    " 'Instruction Mode', color_code as 'Color Code', course_id as 'Course ID'," +
                    " student_verification_method as 'Student Verification Method', grandfather as" +
                    " 'Grandfather Color Code', notes as 'Notes', approved as 'Approved' from course left outer join course_area on" +
                    " course_area.id = course.FK_course_area left outer join department on department.id = " +
                    "course.FK_Department left outer join instruction_mode on instruction_mode.id = " +
                    "course.FK_Instruction_Mode left outer join grandfather_color_code on grandfather_color_code.id " +
                    "= course.FK_Grandfather_Color_Code left outer join instructor on instructor.id = " +
                    "course.FK_Instructor left outer join academic_org on academic_org.id = course.FK_Academic_ORG" +
                    " left outer join approval_date on approval_date.FK_Course = course.id Where course_area = '" +
                    txtCourseArea.Text.ToUpper() + "';");

            }

            else if (txtCourseArea.Text == "" && txtCourseNumber.Text != "")
            {
                //only search course number
                searchQuery("select course.id as 'Course ID', approval_date as 'Approval Date', course_area as 'Course Area'," +
                    " number as 'Course Number', title as 'Course Title', department as 'Department'," +
                    " instructor_last_name as 'Instructor Last Name', Instructor_first_name as " +
                    "'Instructor First Name', academic_org as 'Academic Organization', instruction_mode as" +
                    " 'Instruction Mode', color_code as 'Color Code', course_id as 'Course ID'," +
                    " student_verification_method as 'Student Verification Method', grandfather as" +
                    " 'Grandfather Color Code', notes as 'Notes', approved as 'Approved' from course left outer join course_area on" +
                    " course_area.id = course.FK_course_area left outer join department on department.id = " +
                    "course.FK_Department left outer join instruction_mode on instruction_mode.id = " +
                    "course.FK_Instruction_Mode left outer join grandfather_color_code on grandfather_color_code.id " +
                    "= course.FK_Grandfather_Color_Code left outer join instructor on instructor.id = " +
                    "course.FK_Instructor left outer join academic_org on academic_org.id = course.FK_Academic_ORG" +
                    " left outer join approval_date on approval_date.FK_Course = course.id Where number = " + txtCourseNumber.Text + ";");

            }

            else
            {
                //both fields are being searched             
                searchQuery("select course.id as 'Course ID' ,approval_date as 'Approval Date', course_area as 'Course Area'," +
                    " number as 'Course Number', title as 'Course Title', department as 'Department'," +
                    " instructor_last_name as 'Instructor Last Name', Instructor_first_name as " +
                    "'Instructor First Name', academic_org as 'Academic Organization', instruction_mode as" +
                    " 'Instruction Mode', color_code as 'Color Code', course_id as 'Course ID'," +
                    " student_verification_method as 'Student Verification Method', grandfather as" +
                    " 'Grandfather Color Code', notes as 'Notes', approved as 'Approved' from course left outer join course_area on" +
                    " course_area.id = course.FK_course_area left outer join department on department.id = " +
                    "course.FK_Department left outer join instruction_mode on instruction_mode.id = " +
                    "course.FK_Instruction_Mode left outer join grandfather_color_code on grandfather_color_code.id " +
                    "= course.FK_Grandfather_Color_Code left outer join instructor on instructor.id = " +
                    "course.FK_Instructor left outer join academic_org on academic_org.id = course.FK_Academic_ORG" +
                    " left outer join approval_date on approval_date.FK_Course = course.id Where course_area = '" +
                    txtCourseArea.Text.ToUpper() + "' and number = " + txtCourseNumber.Text + ";");          
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            update updateDialogue = new update();
            updateDialogue.Show();
        }

        private void searchQuery(string query)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\WPF_Project\\Database\\Shannon.db");
            conn.Open();
            SQLiteCommand command = conn.CreateCommand();

            command.CommandText = query;

            SQLiteDataReader sdr = command.ExecuteReader();

            while (sdr.Read())
            {
                Course temp = new Course();

                //try to get each data entry

                // id
                try
                {
                    temp.ID = sdr.GetInt32(0);
                }
                catch
                {
                    temp.ID = -1;
                    txtErrorBlock.Text = "Error retrieving data from the database.";
                    return;
                }

                //approval date
                try
                {
                    temp.Approval_Date = sdr.GetDateTime(1);
                }
                catch
                {
                    temp.Approval_Date = new DateTime(1, 1, 1);
                }

                //course area
                try
                {
                    temp.Course_Area = sdr.GetString(2);
                }
                catch
                {
                    temp.Course_Area = "Error";
                    txtErrorBlock.Text = "Error retrieving data from the database.";
                    return;
                }
                //course number
                try
                {
                    temp.Course_Number = sdr.GetString(3);
                }
                catch
                {
                    temp.Course_Number = "Error";
                    txtErrorBlock.Text = "Error retrieving data from the database.";
                    return;
                }
                //title
                try
                {
                    temp.Title = sdr.GetString(4);
                }
                catch
                {
                    temp.Title = "NULL";
                }
                //department
                try
                {
                    temp.Department = sdr.GetString(5);
                }
                catch
                {
                    temp.Department = "NULL";
                }
                //instructor
                try
                {
                    temp.Instructor = sdr.GetString(6) + ", " + sdr.GetString(7);
                }
                catch
                {
                    temp.Instructor = "NULL";
                }
                //academic org
                try
                {
                    temp.Academic_Org = sdr.GetString(8);
                }
                catch
                {
                    temp.Academic_Org = "NULL";
                }
                //instruction mode
                try
                {
                    temp.Instruction_Mode = sdr.GetString(9);
                }
                catch
                {
                    temp.Instruction_Mode = "NULL";
                }
                //color code
                try
                {
                    temp.Grandfather_Color_Code = sdr.GetString(10);
                }
                catch
                {
                    temp.Grandfather_Color_Code = "NULL";
                }
                //course id
                try
                {
                    temp.Course_Id = sdr.GetInt32(11);
                }
                catch
                {
                    temp.Course_Id = -1;
                }
                //student verification method
                try
                {
                    temp.Student_Verification_Method = sdr.GetString(12);
                }
                catch
                {
                    temp.Student_Verification_Method = "NULL";
                }
                //grandfather
                try
                {
                    temp.Grandfather = Convert.ToBoolean(sdr.GetString(13));
                }
                catch
                {
                    temp.Grandfather = false;
                }
                //notes
                try
                {
                    temp.Notes = sdr.GetString(14);
                }
                catch
                {
                    temp.Notes = "NULL";
                }
                //approved
                try
                {
                    temp.Approved = Convert.ToBoolean(sdr.GetString(15));
                }
                catch
                {
                    temp.Approved = false;
                }

                courseData.Add(temp);
            }

            sdr.Close();

            conn.Close();

            queryResults.ItemsSource = courseData;

            return;
        }
    }
}
