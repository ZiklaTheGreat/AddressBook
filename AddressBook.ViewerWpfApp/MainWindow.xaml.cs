using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AddressBook.CommonLibrary;
using Microsoft.Win32;

namespace EmployeeDirectory
{
    public partial class MainWindow : Window
    {
        private EmployeeList? employeeList = null;

        public string EmployeeName
        {
            get { return NameTextBox.Text; }
            set { ; }
        }
        public IEnumerable<string>? Functions { get; set; }
        public IEnumerable<string>? Workplaces { get; set; }
        public SearchResult? SResult { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
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

        private void InputFile_Click(object sender, RoutedEventArgs e) // This method will be called when "Otvoriť..." is clicked
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "JSON files (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                employeeList = EmployeeList.LoadFromJson(new FileInfo(selectedFilePath));

                if (employeeList == null)
                {
                    MessageBox.Show(caption: "Error", messageBoxText: "Chyba pri otvarani suboru!");
                } else
                {
                    Functions = employeeList.GetPositions();
                    FunctionComboBox.ItemsSource = Functions;
                    Workplaces = employeeList.GetMainWorkplaces();
                    WorkplaceComboBox.ItemsSource = Workplaces;
                }
            }
        }

        private void OutputFile_Click(object sender, RoutedEventArgs e) // This method will be called when "Otvoriť..." is clicked
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