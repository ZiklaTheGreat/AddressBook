using System;
using System.Diagnostics;
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
using AddressBook.EditorWpfApp;
using Microsoft.Win32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeDirectory
{
    public partial class MainWindow : Window
    {
        private EmployeeList? employeeList = null;
        private bool bolZmeneny = false;
        //private SearchResult? sResult = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewClick(object sender, RoutedEventArgs e)
        {
            if (bolZmeneny)
            {
                var result = MessageBox.Show("Súbor bol zmenený, želáte si ho uložiť?", "Uložiť súbor",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

                if (result == MessageBoxResult.Yes)
                {
                    SaveClick(sender, e);
                }

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }

                bolZmeneny = false;
            }
            Application.Current.Shutdown();
            Process.Start("AddressBook.EditorWpfApp.exe");
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "JSON files (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                employeeList?.SaveToJson(new FileInfo(selectedFilePath));
            }
        }

        private void OpenClick(object sender, RoutedEventArgs e)
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
                    MessageBox.Show(caption: "Error", messageBoxText: "Vybrali ste prázdny súbor!");
                    return;
                }
                else
                {
                    DataGridEmployees.ItemsSource = employeeList;
                    UpdatePocet();
                }
            }
        }

        private void EndClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            var employeeWindow = new AddEmployeeWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var result = employeeWindow.ShowDialog();

            if (result == true)
            {
                Employee em = new                (
                    name: employeeWindow.EmployeeName ?? "",
                    position: employeeWindow.Position,
                    phone: employeeWindow.Phone,
                    email: employeeWindow.Mail,
                    room: employeeWindow.Room,
                    workPlace: employeeWindow.Workplace,
                    mainWorkplace: employeeWindow.MainWorkplace
                );
                if (employeeList == null)
                {
                    employeeList = [];
                    DataGridEmployees.ItemsSource = employeeList;

                }
                employeeList.Add(em);
                UpdatePocet();
                bolZmeneny = true;
            }
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            if (DataGridEmployees.SelectedItems.Count == 1)
            {
                Employee? employee = DataGridEmployees.SelectedItems[0] as Employee;

                var employeeWindow = new AddEmployeeWindow();

                if (employee != null)
                {
                    employeeWindow.SetEmployee(employee);
                }

                employeeWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                var result = employeeWindow.ShowDialog();

                if (result == true)
                {
                    var index = 0;
                    if (employeeList != null && employee != null)
                    {
                        index = employeeList.IndexOf(employee);
                    }

                    Employee updatedEmployee = new                    (
                        name: employeeWindow.EmployeeName ?? "",
                        position: employeeWindow.Position,
                        phone: employeeWindow.Phone,
                        email: employeeWindow.Mail,
                        room: employeeWindow.Room,
                        workPlace: employeeWindow.Workplace,
                        mainWorkplace: employeeWindow.MainWorkplace
                    );

                    DataGridEmployees.SelectedItem = null;
                    if (employeeList != null)
                    {
                        employeeList.RemoveAt(index);
                        employeeList.Insert(index, updatedEmployee);
                    }
                    bolZmeneny = true;
                    DisableButtons();
                }
            }
        }

        private void RemoveEmployeeClick(object sender, RoutedEventArgs e)
        {
            if (DataGridEmployees.SelectedItems.Count == 1)
            {
                var result = MessageBox.Show("Naozaj chcete odstrániť zamestnanca?", "Odstrániť zamestnanca",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (result == MessageBoxResult.Yes)
                {
                    Employee? employee = (DataGridEmployees.SelectedItems[0] as Employee);
                    DataGridEmployees.SelectedItem = null;
                    int index = employee != null && employeeList != null ? employeeList.IndexOf(employee) : 0;
                    employeeList?.RemoveAt(index);
                    UpdatePocet();
                    bolZmeneny = true;
                    DisableButtons();
                }

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            var employeeWindow = new AboutWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var result = employeeWindow.ShowDialog();

            if (result == true)
            {
                return;
            }
        }

        private void UpdatePocet()
        {
            PocetNajdenychText.Text = "Počet: " + employeeList?.Count;
            if (employeeList?.Count > 0)
            {
                SearchButton.IsEnabled = true;
            }
            else
            {
                SearchButton.IsEnabled = false;
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            var searchWindow = employeeList != null ? new SearchWindow(employeeList) : new SearchWindow([]);

            searchWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            searchWindow.ShowDialog();
        }

        private void DisableButtons()
        {
            RemoveButton.IsEnabled = false;
            EditButton.IsEnabled = false;
            MenuRemove.IsEnabled = false;
            MenuEdit.IsEnabled = false;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RemoveButton.IsEnabled = true;
            EditButton.IsEnabled = true;
            MenuEdit.IsEnabled = true;
            MenuRemove.IsEnabled = true;
        }
    }
}