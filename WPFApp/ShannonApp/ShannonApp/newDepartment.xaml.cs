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
    /// Interaction logic for newDepartment.xaml
    /// </summary>
    public partial class newDepartment : Window
    {
        public newDepartment()
        {
            InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true; 
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtDepartment.SelectAll();
            txtDepartment.Focus();
        }

        public string Answer
        {
            get { return txtDepartment.Text.ToUpper(); }
        }
    }
}
