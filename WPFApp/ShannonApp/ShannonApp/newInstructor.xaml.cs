﻿using System;
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
    /// Interaction logic for newInstructor.xaml
    /// </summary>
    public partial class newInstructor : Window
    {
        public newInstructor()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtInstructorLastName.SelectAll();
            txtInstructorLastName.Focus();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {

            txtError.Text = ""; 

            if(txtInstructorFirstName.Text != "" && txtInstructorLastName.Text != "")
            {
                DialogResult = true;
            }

            else
            {
                txtError.Text = "Please enter first and last name.";
            }
        }

        public string lastAnswer
        {
            get { return txtInstructorLastName.Text.ToUpper(); }
        }

        public string firstAnswer
        {
            get { return txtInstructorFirstName.Text.ToUpper(); }
        }
    }
}
