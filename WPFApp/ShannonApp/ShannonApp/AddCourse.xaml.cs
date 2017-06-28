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

        public AddCourse()
        {
            InitializeComponent();

            //open a connection to the database

            SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\hhuffman\\Desktop\\Database\\Shannon.db");
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

            list.Sort();

            oCollection = new ObservableCollection<string>(list);

            sdr.Close();
        }


        private void insertCourseArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (insertCourseArea.SelectedValue.ToString() != "<Create New>")
            {
                return;
            }

            else if (insertCourseArea.SelectedValue.ToString() == "<Create New>")
            {
                newCourseArea inputCourseDialog = new newCourseArea();
                if (inputCourseDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\hhuffman\\Desktop\\Database\\Shannon.db");
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
            }
        }

        private void insertDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (insertDepartment.SelectedValue.ToString() != "<Create New>")
            {
                return;
            }

            else if (insertDepartment.SelectedValue.ToString() == "<Create New>")
            {

                newDepartment inputDepartmentDialog = new newDepartment();

                if (inputDepartmentDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\hhuffman\\Desktop\\Database\\Shannon.db");
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

            }
        }

        private void insertInstructionMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (insertInstructionMode.SelectedValue.ToString() != "<Create New>")
            {
                return;
            }

            else if (insertInstructionMode.SelectedValue.ToString() == "<Create New>")
            {

                newInstructionMode inputInstructionDialog = new newInstructionMode();

                if (inputInstructionDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\hhuffman\\Desktop\\Database\\Shannon.db");
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
            }

        }
        private void insertInstructor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void insertAcademicOrg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (insertAcademicOrg.SelectedValue.ToString() != "<Create New>")
            {
                return;
            }

            else if (insertAcademicOrg.SelectedValue.ToString() == "<Create New>")
            {

                newInstructionMode inputAcademicDialog = new newInstructionMode();

                if (inputAcademicDialog.ShowDialog() == true)
                {
                    //connect to the database
                    SQLiteConnection conn = new SQLiteConnection("Data Source=C:\\Users\\hhuffman\\Desktop\\Database\\Shannon.db");
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
            }
        }
    }
}

