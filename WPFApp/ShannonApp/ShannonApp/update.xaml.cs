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
        private ObservableCollection<string> verificationList;
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
            updateCourseArea.SelectedItem = MainPage.selectedCourse.courseArea;

            updateCourseNumber.Text = MainPage.selectedCourse.Course_Number;
            
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

            updatedCourse = new Course();

            updatedCourse.courseArea = updateCourseArea.Text;

            updatedCourse.Course_Number = updateCourseNumber.Text;
        }
    }
}
