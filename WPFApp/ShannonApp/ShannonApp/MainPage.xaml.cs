using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Resources;
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
    /// 
    /// 
    /// This will run a query to show specific classes
    /// </summary>
    public partial class MainPage : Page
    {
        private static ObservableCollection<Course> courseData;
        public static Course selectedCourse;
        public const string defaultConnection = "Data Source = //storage.unr.edu/xs/Istudy/shannon/quality assurance/shannon.db"; 

        public MainPage()
        {
            InitializeComponent();
            selectedCourse = new Course();
            courseData = new ObservableCollection<Course>();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            txtErrorBlock.Text = "";

            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Uri("AddCourse.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            txtErrorBlock.Text = "";

           
            populateData();

            txtErrorBlock.Text = "Finished populating results.";
 
        }

        private void queryResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtErrorBlock.Text = "";

            btnRemove.IsEnabled = true;
            btnUpdate.IsEnabled = true;

            selectedCourse = queryResults.SelectedItem as Course;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            txtErrorBlock.Text = "";

            remove removeDialogue = new remove();

            if (removeDialogue.ShowDialog() == true)
            {
                //run query to remove the course
                SQLiteConnection conn = new SQLiteConnection(MainPage.defaultConnection);
                conn.Open();
                SQLiteCommand command = conn.CreateCommand();

                Course test = queryResults.SelectedItem as Course;
                if (test != null)
                {
                    command.CommandText = "DELETE FROM course WHERE ID = " + test.ID.ToString() + ";" + "DELETE FROM approval_date WHERE fk_course =" + test.ID.ToString() +";";
                    command.ExecuteNonQuery();                   
                    populateData();
                    txtErrorBlock.Text = "Course Removed";
                    btnRemove.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
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


            if (txtCourseArea.Text.Trim() == "" && txtCourseNumber.Text.Trim() == "")
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

            else if (txtCourseArea.Text.Trim() != "" && txtCourseNumber.Text.Trim() == "")
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
                    " left outer join approval_date on approval_date.FK_Course = course.id Where course_area like '%" +
                    txtCourseArea.Text.ToUpper().Trim() + "%';");

            }

            else if (txtCourseArea.Text.Trim() == "" && txtCourseNumber.Text.Trim() != "")
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
                    " left outer join approval_date on approval_date.FK_Course = course.id Where number like '%" + txtCourseNumber.Text.Trim() + "%';");

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
                    " left outer join approval_date on approval_date.FK_Course = course.id Where course_area like '%" +
                    txtCourseArea.Text.ToUpper().Trim() + "%' and number like '%" + txtCourseNumber.Text.Trim() + "%';");          
            }

            
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            update updateDialogue = new update();
            updateDialogue.ShowDialog();

            if(updateDialogue.DialogResult == true)
            {
                populateData();
                txtErrorBlock.Text = "Course has been updated.";
                btnRemove.IsEnabled = false;
                btnUpdate.IsEnabled = false;
            }
        }

        private async void searchQuery(string query)
        {
            
            SQLiteConnection conn = new SQLiteConnection(MainPage.defaultConnection);
            conn.Open();
            SQLiteCommand command = conn.CreateCommand();

            command.CommandText = query;

            try
            {
                int i = 1;
                SQLiteDataReader sdr = command.ExecuteReader();

                while (sdr.Read())
                {

                    txtErrorBlock.Text = "Populating Course: " + i.ToString();
                    await Task.Delay(TimeSpan.FromMilliseconds(1)); 
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
                        DateTime dummy = Convert.ToDateTime(sdr.GetString(1));
                        temp.Approval_Date = dummy.ToShortDateString();
                    }
                    catch
                    {
                        temp.Approval_Date = "";
                    }

                    //course area
                    try
                    {
                        temp.Course_Area = sdr.GetString(2).Trim();
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
                        temp.Course_Number = sdr.GetString(3).Trim();
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
                        temp.Title = "";
                    }
                    //department
                    try
                    {
                        temp.Department = sdr.GetString(5);
                    }
                    catch
                    {
                        temp.Department = "";
                    }
                    //instructor
                    try
                    {
                        temp.Instructor = sdr.GetString(6) + ", " + sdr.GetString(7);
                    }
                    catch
                    {
                        temp.Instructor = "";
                    }
                    //academic org
                    try
                    {
                        temp.Academic_Org = sdr.GetString(8);
                    }
                    catch
                    {
                        temp.Academic_Org = "";
                    }
                    //instruction mode
                    try
                    {
                        temp.Instruction_Mode = sdr.GetString(9);
                    }
                    catch
                    {
                        temp.Instruction_Mode = "";
                    }
                    //color code
                    try
                    {
                        temp.Grandfather_Color_Code = sdr.GetString(10);
                    }
                    catch
                    {
                        temp.Grandfather_Color_Code = "";
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
                        temp.Student_Verification_Method = "";
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
                        temp.Notes = "";
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
                    i++;
                }

                sdr.Close();

                conn.Close();

                queryResults.ItemsSource = courseData;
            }

            catch {

                txtErrorBlock.Text = "Invalid input.";
            }
            
        }
    }
}
