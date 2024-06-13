using AddressBook.CommonLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace AddressBook.EditorWpfApp
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private readonly EmployeeList? employeeList = null; //EmployeeList.LoadFromJson(new FileInfo("C:\\Users\\Martin\\OneDrive - Žilinská univerzita v Žiline\\Pracovná plocha\\Uni\\2. ročník\\Letný Semester\\C#\\AddressBook\\employees.json"));

        public string? EmployeeName
        {
            get { return NameTextBox.Text; }
            set {; }
        }

        public IEnumerable<string> Functions
        {
            get { return employeeList != null ? employeeList.GetPositions() : new EmployeeList().GetPositions(); }
            set { ; }
        }

        public IEnumerable<string> Workplaces
        {
            get { return employeeList != null ? employeeList.GetMainWorkplaces() : new EmployeeList().GetMainWorkplaces(); }
            set { ; }
        }
        public SearchResult? SResult { get; set; }

        public SearchWindow(EmployeeList pEmployeeList)
        {
            InitializeComponent();
            employeeList = pEmployeeList;
            Functions = employeeList.GetPositions();
            FunctionComboBox.ItemsSource = Functions;
            Workplaces = employeeList.GetMainWorkplaces();
            WorkplaceComboBox.ItemsSource = Workplaces;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeeList == null)
            {
                MessageBox.Show(caption: "Error", messageBoxText: "Nebol zadany ziadny subor!");
            }
            else
            {
                SResult = employeeList.Search(mainWorkplace: WorkplaceComboBox.Text,
                    position: FunctionComboBox.Text,
                    name: NameTextBox.Text);
                PocetNajdenychText.Text = "Počet nájdených zamestnancov: " + SResult.Employees.Length;
            }

            employeeListBox.ItemsSource = SResult?.Employees;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = "";
            FunctionComboBox.Text = "";
            WorkplaceComboBox.Text = "";
            PocetNajdenychText.Text = "Počet nájdených zamestnancov: " + 0;
            employeeListBox.ItemsSource = null;
        }

        private void OutputFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "CSV files (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                SResult?.SaveToCsv(new FileInfo(selectedFilePath));
            }
        }
    }
}
