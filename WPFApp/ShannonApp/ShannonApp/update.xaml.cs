using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShannonApp
{
    /// <summary>
    /// Interaction logic for update.xaml
    /// </summary>
    public partial class update : Window
    {
        public static Course updatedCourse;

        private ObservableCollection<string> courseAreaList;
        private ObservableCollection<string> departmentList;
        private ObservableCollection<string> modeList;
        private ObservableCollection<string> instructorList;
        private ObservableCollection<string> academicOrgList;

        public update()
        {
            InitializeComponent();

            //populate all of the comboboxes

            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\WPF_Project\\Database\\Shannon.db");
            conn.Open();
            SQLiteCommand command = conn.CreateCommand();

            //populate the course area list
            populateList(ref command, "SELECT course_area FROM COURSE_AREA;", ref courseAreaList);

            //populate the department list
            populateList(ref command, "SELECT Department from Department;", ref departmentList);

            //populate the instruction mode list
            populateList(ref command, "SELECT Instruction_Mode from Instruction_Mode;", ref modeList);

            //populate academic org list
            populateList(ref command, "SELECT Academic_ORG from Academic_ORG;", ref academicOrgList);

            //populate instructor list
            populateInstructors(ref command, "SELECT Instructor_First_Name, Instructor_Last_Name from Instructor;", ref instructorList);

            conn.Close();

            //populate the combo box with the related data
            updateCourseArea.ItemsSource = courseAreaList;

            updateDepartment.ItemsSource = departmentList;

            updateInstructionMode.ItemsSource = modeList;

            updateAcademicOrg.ItemsSource = academicOrgList;

            updateInstructor.ItemsSource = instructorList;

            //set the data to match the selected input
            updateCourseArea.SelectedItem = MainPage.selectedCourse.Course_Area;

            updateCourseNumber.Text = MainPage.selectedCourse.Course_Number;

            updateTitle.Text = MainPage.selectedCourse.Title;

            updateDepartment.SelectedItem = MainPage.selectedCourse.Department;

            updateInstructionMode.SelectedItem = MainPage.selectedCourse.Instruction_Mode;

            updateInstructor.SelectedItem = MainPage.selectedCourse.Instructor;

            updateCourseId.Text = MainPage.selectedCourse.Course_Id.ToString();

            updateAcademicOrg.SelectedItem = MainPage.selectedCourse.Academic_Org;

            updateStudentVerificationMethod.Text = MainPage.selectedCourse.Student_Verification_Method;

            updateNotes.Text = MainPage.selectedCourse.Notes;

            chkBoxApproved.IsChecked = MainPage.selectedCourse.Approved;

            updateDate.Text = MainPage.selectedCourse.Approval_Date; 
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            btnDialogOk.Focus();
        }

        private void populateList(ref SQLiteCommand command, string selectStatement, ref ObservableCollection<string> oCollection)
        {
            command.CommandText = selectStatement;
            SQLiteDataReader sdr = command.ExecuteReader();

            List<string> list = new List<string>();

            while (sdr.Read())
            {
                if (!list.Contains(sdr.GetString(0)))
                {
                    list.Add(sdr.GetString(0));
                }
            }

            if (!list.Contains("<Create New>"))
                list.Add("<Create New>");

            if (!list.Contains(""))
            {
                list.Add("");
            }

            list.Sort();

            oCollection = new ObservableCollection<string>(list);

            sdr.Close();

        }

        private void populateInstructors(ref SQLiteCommand command, string selectStatement, ref ObservableCollection<string> oCollection)
        {
            command.CommandText = selectStatement;
            SQLiteDataReader sdr = command.ExecuteReader();

            List<string> list = new List<string>();

            while (sdr.Read())
            {
                if (!list.Contains(sdr.GetString(1) + ", " + sdr.GetString(0)))
                {
                    list.Add(sdr.GetString(1) + ", " + sdr.GetString(0));
                }
            }

            if (!list.Contains("<Create New>"))
                list.Add("<Create New>");

            if (!list.Contains(""))
            {
                list.Add("");
            }

            list.Sort();

            oCollection = new ObservableCollection<string>(list);

            sdr.Close();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            //capture data and do error checking
            //connect to the database
            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\WPF_Project\\Database\\Shannon.db");
            conn.Open();
            SQLiteCommand command = conn.CreateCommand();

            //get the foreign keys for each combo box field

            Dictionary<string, string> inputFields = new Dictionary<string, string>();

            inputFields.Clear();

            if (updateCourseArea.Text == "" || updateCourseArea.Text == "<Create New>")
            {
                txtUpdateResponse.Text = "Please select a valid Course Area.";
                return;
            }

            else
            {
                //get the foreign key
                command.CommandText = "Select ID from course_area where course_area = '" + updateCourseArea.Text + "';";
                SQLiteDataReader sdr = command.ExecuteReader();

                int id = -1;

                while (sdr.Read())
                {
                    id = sdr.GetInt32(0);
                }

                if (id != -1)
                    inputFields.Add("courseArea", id.ToString());

                sdr.Close();
            }

            if (updateCourseNumber.Text == "")
            {
                txtUpdateResponse.Text = "Please enter a valid Course Number.";
                return;
            }
            else
            {
                inputFields.Add("courseNumber", "'" + updateCourseNumber.Text + "'");
            }

            if (updateTitle.Text == "")
            {
                inputFields.Add("title", "NULL");
            }

            else
            {
                inputFields.Add("title", "'" + updateTitle.Text + "'");
            }

            if (updateDepartment.Text == "")
            {
                inputFields.Add("department", "NULL");
            }

            else if (updateDepartment.Text == "<Create New>")
            {
                txtUpdateResponse.Text = "Please select a valid department.";
            }

            else
            {
                //get the foreign key of the department
                command.CommandText = "Select ID from department where department = '" + updateDepartment.Text + "';";
                SQLiteDataReader sdr = command.ExecuteReader();

                int id = -1;

                while (sdr.Read())
                {
                    id = sdr.GetInt32(0);
                }

                if (id != -1)
                    inputFields.Add("department", id.ToString());

                sdr.Close();
            }


            if (updateInstructionMode.Text == "")
            {
                inputFields.Add("instructionMode", "NULL");
            }

            else if (updateInstructionMode.Text == "<Create New>")
            {
                txtUpdateResponse.Text = "Please select a valid instruction mode.";
            }

            else
            {

                command.CommandText = "Select ID from instruction_mode where instruction_mode = '" + updateInstructionMode.Text + "';";
                SQLiteDataReader sdr = command.ExecuteReader();

                int id = -1;

                while (sdr.Read())
                {
                    id = sdr.GetInt32(0);
                }

                if (id != -1)
                    inputFields.Add("instructionMode", id.ToString());

                sdr.Close();
            }

            if (updateInstructor.Text == "")
            {
                inputFields.Add("instructor", "NULL");
            }

            else if (updateInstructor.Text == "<Create New>")
            {
                txtUpdateResponse.Text = "Please select a valid instructor.";
            }

            else
            {
                string[] names = Regex.Split(updateInstructor.Text, ", ");

                command.CommandText = "Select ID from instructor where instructor_first_name = '" +
                    names[1] + "' and instructor_last_name = '" + names[0] + "';";
                SQLiteDataReader sdr = command.ExecuteReader();

                int id = -1;

                while (sdr.Read())
                {
                    id = sdr.GetInt32(0);
                }

                if (id != -1)
                    inputFields.Add("instructor", id.ToString());

                sdr.Close();

            }

            if (updateCourseId.Text == "")
            {
                inputFields.Add("courseId", "NULL");
            }

            else
            {
                int dummy;
                if (System.Int32.TryParse(updateCourseId.Text, out dummy))
                {
                    inputFields.Add("courseId", "'" + updateCourseId.Text + "'");
                }

                else
                {
                    txtUpdateResponse.Text = "The Course ID field must be an integer or left blank.";
                    return;
                }

            }

            if (updateAcademicOrg.Text == "")
            {
                inputFields.Add("academicOrg", "NULL");
            }

            else if (updateAcademicOrg.Text == "<Create New>")
            {
                txtUpdateResponse.Text = "Please select a valid academic org.";
            }

            else
            {
                command.CommandText = "Select ID from academic_org where academic_org = '" + updateAcademicOrg.Text + "';";
                SQLiteDataReader sdr = command.ExecuteReader();

                int id = -1;

                while (sdr.Read())
                {
                    id = sdr.GetInt32(0);
                }

                if (id != -1)
                    inputFields.Add("academicOrg", id.ToString());

                sdr.Close();
            }

            if (updateStudentVerificationMethod.Text == "")
            {
                inputFields.Add("studentVerif", "NULL");
            }

            else
            {
                inputFields.Add("studentVerif", "'" + updateStudentVerificationMethod.Text + "'");
            }

            if (updateNotes.Text == "")
            {
                inputFields.Add("notes", "NULL");
            }

            else
            {
                inputFields.Add("notes", "'" + updateNotes.Text + "'");
            }

            if (chkBoxApproved.IsChecked == true)
            {
                inputFields.Add("approved", "True");

                //check if there is a date associated with the course
                command.CommandText = "Select ID, approval_date from approval_date where fk_course = "  + MainPage.selectedCourse.ID + ";";
                SQLiteDataReader sdr = command.ExecuteReader();

                int id = -1;
                string tempDate = "";

                while (sdr.Read())
                {
                    id = sdr.GetInt32(0);
                    try { tempDate = Convert.ToDateTime(sdr.GetString(1)).ToShortDateString(); }
                    catch { tempDate = ""; }
                }
                sdr.Close();

                DateTime tester;

                if (id == -1)
                {
                    
                    //check if there is a valid date
                    if (updateDate.Text != "")
                    {
                        
                        if (System.DateTime.TryParse(updateDate.Text, out tester))
                        {
                            command.CommandText = "Insert into approval_date (approval_date, fk_course) Values('" + tester.ToShortDateString() + "', " + MainPage.selectedCourse.ID +");";
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        command.CommandText = "Insert into approval_date (approval_date, fk_course) Values(NULL," + MainPage.selectedCourse.ID +");";
                        command.ExecuteNonQuery();
                    }
                }

                else
                {
                    //update the existing date
                    if(updateDate.Text != "")
                    {
                        if(System.DateTime.TryParse(updateDate.Text, out tester))
                        {
                            command.CommandText = "Update approval_date set approval_date = '" + tester.ToShortDateString() + "' where fk_course =" + MainPage.selectedCourse.ID + ";";
                            command.ExecuteNonQuery();
                        }
                        
                    }
                    else
                    {
                        command.CommandText = "Update approval_date set approval_date = NULL where fk_course =" + MainPage.selectedCourse.ID + ";";
                        command.ExecuteNonQuery();
                    }
                    
                }                    
            }

            else
            {
                inputFields.Add("approved", "False");
            }

            //insert the course with the corresponding properties        
            command.CommandText = "UPDATE course " +
                "set number =" + inputFields["courseNumber"] + "," +
                "fk_course_area =" + inputFields["courseArea"] + "," +
                "course_id =" + inputFields["courseId"] + ", " +
                "student_verification_method =" + inputFields["studentVerif"] + ", " +
                "notes =" + inputFields["notes"] + ", " +
                "fk_department =" + inputFields["department"] + ", " +
                "fk_instruction_mode =" + inputFields["instructionMode"] + ", " +
                "fk_instructor =" + inputFields["instructor"] + ", " +
                "fk_academic_org = " + inputFields["academicOrg"] + ", " +
                "title =" + inputFields["title"] + ", " +
                "approved = '" + inputFields["approved"] + "' " +
                "where course.id = " + MainPage.selectedCourse.ID.ToString() +";";

            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                txtUpdateResponse.Text = "Error updating database, check if input is valid.";
                return;
            }

            conn.Close();
            DialogResult = true;
        }

        //Error checking similar to adding a new course
        private void updateCourseArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (updateCourseArea.SelectedValue.ToString() != "<Create New>")
            {
                return;
            }

            else if (updateCourseArea.SelectedValue.ToString() == "<Create New>")
            {
                newCourseArea inputCourseDialog = new newCourseArea();
                if (inputCourseDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\WPF_Project\\Database\\Shannon.db");
                    conn.Open();
                    SQLiteCommand command = conn.CreateCommand();

                    //insert the new course area

                    ////Inserting data
                    command.CommandText = "INSERT INTO Course_Area (Course_Area) VALUES('" + inputCourseDialog.Answer + "');";
                    command.ExecuteNonQuery();

                    //refresh the course area list
                    populateList(ref command, "SELECT course_area FROM COURSE_AREA;", ref courseAreaList);

                    this.updateCourseArea.ItemsSource = courseAreaList;

                    this.updateCourseArea.SelectedItem = inputCourseDialog.Answer;

                    conn.Close();
                }

                else
                {
                    this.updateCourseArea.SelectedItem = MainPage.selectedCourse.Course_Area;
                }
            }
        }

        private void updateDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (updateDepartment.SelectedValue.ToString() != "<Create New>")
            {
                return;
            }

            else if (updateDepartment.SelectedValue.ToString() == "<Create New>")
            {

                newDepartment inputDepartmentDialog = new newDepartment();

                if (inputDepartmentDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\WPF_Project\\Database\\Shannon.db");
                    conn.Open();
                    SQLiteCommand command = conn.CreateCommand();

                    ////Inserting data
                    command.CommandText = "INSERT INTO Department (Department) VALUES('" + inputDepartmentDialog.Answer + "');";
                    command.ExecuteNonQuery();

                    //refresh the list
                    populateList(ref command, "SELECT Department FROM Department;", ref departmentList);

                    this.updateDepartment.ItemsSource = departmentList;

                    this.updateDepartment.SelectedItem = inputDepartmentDialog.Answer;

                    conn.Close();
                }

                else
                {
                    this.updateDepartment.SelectedItem = MainPage.selectedCourse.Department;
                }

            }
        }

        private void updateInstructionMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (updateInstructionMode.SelectedValue.ToString() != "<Create New>")
            {
                return;
            }

            else if (updateInstructionMode.SelectedValue.ToString() == "<Create New>")
            {

                newInstructionMode inputInstructionDialog = new newInstructionMode();

                if (inputInstructionDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\WPF_Project\\Database\\Shannon.db");
                    conn.Open();
                    SQLiteCommand command = conn.CreateCommand();

                    ////Inserting data
                    command.CommandText = "INSERT INTO Instruction_Mode (Instruction_Mode) VALUES('" + inputInstructionDialog.Answer + "');";
                    command.ExecuteNonQuery();

                    //refresh the list
                    populateList(ref command, "SELECT Instruction_Mode FROM Instruction_Mode;", ref modeList);

                    this.updateInstructionMode.ItemsSource = modeList;

                    this.updateInstructionMode.SelectedItem = inputInstructionDialog.Answer;

                    conn.Close();
                }

                else
                {
                    this.updateInstructionMode.SelectedItem = MainPage.selectedCourse.Instruction_Mode;
                }
            }
        }

        private void updateInstructor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (updateInstructor.SelectedValue.ToString() != "<Create New>")
            {
                return;
            }
            else if (updateInstructor.SelectedValue.ToString() == "<Create New>")
            {
                newInstructor inputInstructorDialog = new newInstructor();

                if (inputInstructorDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\WPF_Project\\Database\\Shannon.db");
                    conn.Open();
                    SQLiteCommand command = conn.CreateCommand();

                    ////Inserting data
                    command.CommandText = "INSERT INTO instructor (Instructor_First_Name, Instructor_Last_Name) VALUES('" + inputInstructorDialog.firstAnswer + "' ,'" + inputInstructorDialog.lastAnswer + "');";
                    command.ExecuteNonQuery();

                    //refresh the list
                    populateInstructors(ref command, "SELECT Instructor_First_Name, Instructor_Last_Name from Instructor;", ref instructorList);

                    this.updateInstructor.ItemsSource = instructorList;

                    this.updateInstructor.SelectedItem = inputInstructorDialog.lastAnswer + ", " + inputInstructorDialog.firstAnswer;

                    conn.Close();
                }

                else
                {
                    updateInstructor.SelectedItem = MainPage.selectedCourse.Instructor;
                }

            }
        }
        private void updateAcademicOrg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (updateAcademicOrg.SelectedValue.ToString() != "<Create New>")
            {
                return;
            }

            else if (updateAcademicOrg.SelectedValue.ToString() == "<Create New>")
            {

                newInstructionMode inputAcademicDialog = new newInstructionMode();

                if (inputAcademicDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\WPF_Project\\Database\\Shannon.db");
                    conn.Open();
                    SQLiteCommand command = conn.CreateCommand();

                    ////Inserting data
                    command.CommandText = "INSERT INTO Academic_ORG (Academic_ORG) VALUES('" + inputAcademicDialog.Answer + "');";
                    command.ExecuteNonQuery();

                    //refresh the list
                    populateList(ref command, "SELECT Academic_ORG FROM Academic_ORG;", ref academicOrgList);

                    this.updateAcademicOrg.ItemsSource = academicOrgList;

                    this.updateAcademicOrg.SelectedItem = inputAcademicDialog.Answer;

                    conn.Close();
                }

                else
                {
                    this.updateAcademicOrg.SelectedItem = MainPage.selectedCourse.Academic_Org;
                }
            }
        }

        private void chkBoxApproved_Checked(object sender, RoutedEventArgs e)
        {
            updateDate.IsEnabled = true;
        }

        private void chkBoxApproved_Unchecked(object sender, RoutedEventArgs e)
        {
            updateDate.IsEnabled = false;
        }
    }
}
