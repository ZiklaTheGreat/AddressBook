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
using AddressBook.CommonLibrary;

namespace AddressBook.EditorWpfApp
{
    public partial class AddEmployeeWindow : Window
    {
        public string? EmployeeName
        {
            get { return NameTextBox.Text; }
            set {; }
        }
        public string Position
        {
            get { return PositionTextBox.Text; }
            set {; }
        }
        public string Phone
        {
            get { return PhoneTextBox.Text; }
            set {; }
        }
        public string Mail
        {
            get { return MailTextBox.Text; }
            set {; }
        }
        public string Room
        {
            get { return RoomTextBox.Text; }
            set {; }
        }
        public string Workplace
        {
            get { return WorkplaceTextBox.Text; }
            set {; }
        }
        public string MainWorkplace
        {
            get { return MainWorkplaceTextBox.Text; }
            set {; }
        }

        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        private void AddEmployeeOK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public void SetEmployee(Employee employee)
        {
            NameTextBox.Text = employee.Name;
            PositionTextBox.Text = employee.Position;
            PhoneTextBox.Text = employee.Phone;
            MailTextBox.Text = employee.Email;
            WorkplaceTextBox.Text = employee.WorkPlace;
            MainWorkplaceTextBox.Text = employee.MainWorkplace;
        }
    }
}
