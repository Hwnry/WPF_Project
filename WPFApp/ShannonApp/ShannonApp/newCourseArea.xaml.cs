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
using System.Windows.Shapes;

namespace ShannonApp
{
    /// <summary>
    /// Interaction logic for newCourseArea.xaml
    /// </summary>
    public partial class newCourseArea : Window
    {

        public newCourseArea()
        {
            InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtCourseArea.SelectAll();
            txtCourseArea.Focus();
        }

        public string Answer
        {
            get { return txtCourseArea.Text.ToUpper(); }
        }
    }
}
