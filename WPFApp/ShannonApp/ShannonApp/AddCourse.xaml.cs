﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShannonApp
{
    /// <summary>
    /// Interaction logic for AddCourse.xaml
    /// </summary>
    public partial class AddCourse : Page
    {
        private ObservableCollection<string> courseAreaList;
        private ObservableCollection<string> departmentList;
        private ObservableCollection<string> modeList;
        private ObservableCollection<string> instructorList;
        private ObservableCollection<string> academicOrgList;
        private ObservableCollection<string> verificationList;
        private Dictionary<string, string> inputFields;

        public AddCourse()
        {
            InitializeComponent();
            //open a connection to the database

            SQLiteConnection conn = new SQLiteConnection(MainPage.defaultConnection);
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
            insertCourseArea.ItemsSource = courseAreaList;

            insertDepartment.ItemsSource = departmentList;

            insertInstructionMode.ItemsSource = modeList;

            insertAcademicOrg.ItemsSource = academicOrgList;

            insertInstructor.ItemsSource = instructorList;

            insertDate.SelectedDate = System.DateTime.Now;

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


        private void insertCourseArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (insertCourseArea.SelectedValue.ToString().Trim() != "<Create New>")
            {
                return;
            }

            else if (insertCourseArea.SelectedValue.ToString().Trim() == "<Create New>")
            {
                newCourseArea inputCourseDialog = new newCourseArea();
                if (inputCourseDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection(MainPage.defaultConnection);
                    conn.Open();
                    SQLiteCommand command = conn.CreateCommand();

                    //insert the new course area

                    ////Inserting data
                    command.CommandText = "INSERT INTO Course_Area (Course_Area) VALUES('" + inputCourseDialog.Answer + "');";
                    command.ExecuteNonQuery();

                    //refresh the course area list
                    populateList(ref command, "SELECT course_area FROM COURSE_AREA;", ref courseAreaList);

                    this.insertCourseArea.ItemsSource = courseAreaList;

                    this.insertCourseArea.SelectedItem = inputCourseDialog.Answer;

                    conn.Close();
                }

                else
                {
                    this.insertCourseArea.SelectedItem = "";
                }
            }
        }

        private void insertDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (insertDepartment.SelectedValue.ToString().Trim() != "<Create New>")
            {
                return;
            }

            else if (insertDepartment.SelectedValue.ToString().Trim() == "<Create New>")
            {

                newDepartment inputDepartmentDialog = new newDepartment();

                if (inputDepartmentDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection(MainPage.defaultConnection);
                    conn.Open();
                    SQLiteCommand command = conn.CreateCommand();

                    ////Inserting data
                    command.CommandText = "INSERT INTO Department (Department) VALUES('" + inputDepartmentDialog.Answer + "');";
                    command.ExecuteNonQuery();

                    //refresh the list
                    populateList(ref command, "SELECT Department FROM Department;", ref departmentList);

                    this.insertDepartment.ItemsSource = departmentList;

                    this.insertDepartment.SelectedItem = inputDepartmentDialog.Answer;

                    conn.Close();
                }

                else
                {
                    this.insertDepartment.SelectedItem = "";
                }

            }
        }

        private void insertInstructionMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (insertInstructionMode.SelectedValue.ToString().Trim() != "<Create New>")
            {
                return;
            }

            else if (insertInstructionMode.SelectedValue.ToString().Trim() == "<Create New>")
            {

                newInstructionMode inputInstructionDialog = new newInstructionMode();

                if (inputInstructionDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection(MainPage.defaultConnection);
                    conn.Open();
                    SQLiteCommand command = conn.CreateCommand();

                    ////Inserting data
                    command.CommandText = "INSERT INTO Instruction_Mode (Instruction_Mode) VALUES('" + inputInstructionDialog.Answer + "');";
                    command.ExecuteNonQuery();

                    //refresh the list
                    populateList(ref command, "SELECT Instruction_Mode FROM Instruction_Mode;", ref modeList);

                    this.insertInstructionMode.ItemsSource = modeList;

                    this.insertInstructionMode.SelectedItem = inputInstructionDialog.Answer;

                    conn.Close();
                }

                else
                {
                    this.insertInstructionMode.SelectedItem = "";
                }
            }

        }
        private void insertInstructor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (insertInstructor.SelectedValue.ToString().Trim() != "<Create New>")
            {
                return;
            }
            else if (insertInstructor.SelectedValue.ToString().Trim() == "<Create New>")
            {
                newInstructor inputInstructorDialog = new newInstructor();

                if (inputInstructorDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection(MainPage.defaultConnection);
                    conn.Open();
                    SQLiteCommand command = conn.CreateCommand();

                    ////Inserting data
                    command.CommandText = "INSERT INTO instructor (Instructor_First_Name, Instructor_Last_Name) VALUES('" + inputInstructorDialog.firstAnswer + "' ,'" + inputInstructorDialog.lastAnswer + "');";
                    command.ExecuteNonQuery();

                    //refresh the list
                    populateInstructors(ref command, "SELECT Instructor_First_Name, Instructor_Last_Name from Instructor;", ref instructorList);

                    this.insertInstructor.ItemsSource = instructorList;

                    this.insertInstructor.SelectedItem = inputInstructorDialog.lastAnswer + ", " + inputInstructorDialog.firstAnswer;

                    conn.Close();
                }

                else
                {
                    insertInstructor.SelectedItem = "";
                }


            }
        }

        private void insertAcademicOrg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (insertAcademicOrg.SelectedValue.ToString().Trim() != "<Create New>")
            {
                return;
            }

            else if (insertAcademicOrg.SelectedValue.ToString().Trim() == "<Create New>")
            {

                newInstructionMode inputAcademicDialog = new newInstructionMode();

                if (inputAcademicDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection(MainPage.defaultConnection);
                    conn.Open();
                    SQLiteCommand command = conn.CreateCommand();

                    ////Inserting data
                    command.CommandText = "INSERT INTO Academic_ORG (Academic_ORG) VALUES('" + inputAcademicDialog.Answer + "');";
                    command.ExecuteNonQuery();

                    //refresh the list
                    populateList(ref command, "SELECT Academic_ORG FROM Academic_ORG;", ref academicOrgList);

                    this.insertAcademicOrg.ItemsSource = academicOrgList;

                    this.insertAcademicOrg.SelectedItem = inputAcademicDialog.Answer;

                    conn.Close();
                }

                else
                {
                    this.insertAcademicOrg.SelectedItem = "";
                }
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            txtInsertResponse.Text = "";

            SQLiteConnection conn = new SQLiteConnection(MainPage.defaultConnection);

            conn.Open();
            SQLiteCommand command = conn.CreateCommand();

            inputFields = new Dictionary<string, string>();

            inputFields.Clear();

            if (insertCourseArea.Text.Trim() == "" || insertCourseArea.Text.Trim() == "<Create New>")
            {
                txtInsertResponse.Text = "Please select a valid Course Area.";
                return;
            }

            else
            {
                //get the foreign key
                command.CommandText = "Select ID from course_area where course_area = '" + insertCourseArea.Text.Trim() + "';";
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

            if (insertCourseNumber.Text.Trim() == "")
            {
                txtInsertResponse.Text = "Please enter a valid Course Number.";
                return;
            }

            else
            {
                //check whether or not the specified course already exists 
                command.CommandText = "SELECT ID from course where fk_course_area = " + inputFields["courseArea"] + " and number = '" + insertCourseNumber.Text.Trim() + "';";
                SQLiteDataReader sdr = command.ExecuteReader();

                int id = -1;

                while (sdr.Read())
                {
                    id = sdr.GetInt32(0);
                }

                sdr.Close();

                if (id != -1)
                {
                    txtInsertResponse.Text = "Course already exists.";
                    return;
                }

                inputFields.Add("courseNumber", "'" + insertCourseNumber.Text.Trim() + "'");
            }

            if (insertTitle.Text.Trim() == "")
            {
                inputFields.Add("title", "NULL");
            }

            else
            {
                inputFields.Add("title", "'" + insertTitle.Text.Trim() + "'");
            }

            if (insertDepartment.Text.Trim() == "")
            {
                inputFields.Add("department", "NULL");
            }

            else if (insertDepartment.Text.Trim() == "<Create New>")
            {
                txtInsertResponse.Text = "Please select a valid department.";
            }

            else
            {
                //get the foreign key of the department
                command.CommandText = "Select ID from department where department = '" + insertDepartment.Text.Trim() + "';";
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


            if (insertInstructionMode.Text.Trim() == "")
            {
                inputFields.Add("instructionMode", "NULL");
            }

            else if (insertInstructionMode.Text.Trim() == "<Create New>")
            {
                txtInsertResponse.Text = "Please select a valid instruction mode.";
            }

            else
            {

                command.CommandText = "Select ID from instruction_mode where instruction_mode = '" + insertInstructionMode.Text.Trim() + "';";
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

            if (insertInstructor.Text.Trim() == "")
            {
                inputFields.Add("instructor", "NULL");
            }

            else if (insertInstructor.Text.Trim() == "<Create New>")
            {
                txtInsertResponse.Text = "Please select a valid instructor.";
            }

            else
            {
                string[] names = Regex.Split(insertInstructor.Text.Trim(), ", ");

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

            if (insertCourseId.Text.Trim() == "")
            {
                inputFields.Add("courseId", "NULL");
            }

            else
            {
                int dummy;
                if (System.Int32.TryParse(insertCourseId.Text.Trim(), out dummy))
                {
                    inputFields.Add("courseId", "'" + insertCourseId.Text.Trim() + "'");
                }

                else
                {
                    txtInsertResponse.Text = "The Course ID field must be an integer or left blank.";
                    return;
                }

            }

            if (insertAcademicOrg.Text.Trim() == "")
            {
                inputFields.Add("academicOrg", "NULL");
            }

            else if (insertAcademicOrg.Text.Trim() == "<Create New>")
            {
                txtInsertResponse.Text = "Please select a valid academic org.";
            }

            else
            {
                command.CommandText = "Select ID from academic_org where academic_org = '" + insertAcademicOrg.Text.Trim() + "';";
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

            if (insertStudentVerificationMethod.Text.Trim() == "")
            {
                inputFields.Add("studentVerif", "NULL");
            }

            else
            {
                inputFields.Add("studentVerif", "'" + insertStudentVerificationMethod.Text.Trim() + "'");
            }

            if (insertNotes.Text.Trim() == "")
            {
                inputFields.Add("notes", "NULL");
            }

            else
            {
                inputFields.Add("notes", "'" + insertNotes.Text.Trim() + "'");
            }

            if (chkBoxApproved.IsChecked == true)
            {
                inputFields.Add("approved", "True");

                if (insertDate.Text.Trim() == "")
                {
                    inputFields.Add("approvalDate", "NULL");
                }

                else
                {
                    inputFields.Add("approvalDate", "'" + insertDate.Text.Trim() + "'");
                }
            }

            else
            {
                inputFields.Add("approved", "False");
            }

            //insert the course with the corresponding properties
            command.CommandText = "INSERT INTO course(number, fk_course_area, course_id, student_verification_method, notes, fk_department," +
                "fk_instruction_mode, fk_instructor, fk_academic_org, title, approved) " +
                "values (" + inputFields["courseNumber"] + ","
                + inputFields["courseArea"] + ", " + inputFields["courseId"] + ", " + inputFields["studentVerif"] + ", " + inputFields["notes"] +
                ", " + inputFields["department"] + ", " + inputFields["instructionMode"] + "," + inputFields["instructor"] + "," + inputFields["academicOrg"] + "," +
                inputFields["title"] + ",'" + inputFields["approved"] + "');";

            try
            {
                command.ExecuteNonQuery();
                if (chkBoxApproved.IsChecked == true)
                {
                    getId("Select id from course order by 1 desc limit 1;", "ID", ref command);
                    command.CommandText = "INSERT into approval_date(approval_date, fk_course) values('" + inputFields["approvalDate"] + "'," + inputFields["ID"] + ");";
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                txtInsertResponse.Text = "Error inserting data into database.";
            }


            conn.Close();

            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Uri("MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void chkBoxApproved_Checked(object sender, RoutedEventArgs e)
        {
            insertDate.IsEnabled = true;
        }

        private void chkBoxApproved_Unchecked(object sender, RoutedEventArgs e)
        {
            insertDate.IsEnabled = false;
        }

        private bool getId(string selectStatement, string field, ref SQLiteCommand command)
        {
            command.CommandText = selectStatement;
            SQLiteDataReader sdr = command.ExecuteReader();

            int id = -1;

            while (sdr.Read())
            {
                id = sdr.GetInt32(0);
            }

            if (id != -1)
            {
                inputFields.Add(field, id.ToString());
                sdr.Close();
                return true;
            }


            return false;

        }
    }
}