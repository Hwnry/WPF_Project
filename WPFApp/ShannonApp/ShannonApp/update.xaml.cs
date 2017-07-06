using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Dictionary<string, string> inputFields;

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
            DialogResult = true;

            //capture all of the changes

            //update the database
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

     


    }
}
