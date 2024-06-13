using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AddressBook.CommonLibrary
{
    public class EmployeeList : ObservableCollection<Employee>
    {
        public static EmployeeList? LoadFromJson(FileInfo jsonFile)
        {
            if (jsonFile == null || !jsonFile.Exists)
            {
                Console.Error.WriteLine("Nepodarilo sa otvoriť vstupný súbor!");
                return null;
            }

            string jsonData = File.ReadAllText(jsonFile.FullName);
            
            if (jsonData == "")
            {
                Console.Error.WriteLine("Prázdny súbor!");
                return null;
            }

            EmployeeList? employees = JsonConvert.DeserializeObject<EmployeeList>(jsonData);

            return employees;
        }
        public void SaveToJson(FileInfo jsonFile)
        {
            ArgumentNullException.ThrowIfNull(jsonFile);

            try
            {
                string employees = JsonConvert.SerializeObject(this);
                File.WriteAllText(jsonFile.FullName, employees);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving employees to JSON: " + ex.Message);
            }
        }
        public IEnumerable<string> GetPositions()
        {
            List<string> positions = [];
            positions = this.Select(employee => employee.Position).Distinct().ToList();

            return positions.Order();
        }
        public IEnumerable<string> GetMainWorkplaces()
        {
            List<string> mainWorkplaces = [];
            mainWorkplaces = this.Select(employee => employee.MainWorkplace ?? "").Distinct().ToList();

            return mainWorkplaces.Order();
        }

        public SearchResult Search(string? mainWorkplace = null, string? position = null, string? name = null)
        {
            List<Employee> filteredEmployees = [];

            foreach (var employee in this.Items)
            {
                if (!string.IsNullOrEmpty(mainWorkplace))
                {
                    if (employee.MainWorkplace != mainWorkplace)
                    {
                        continue;
                    }
                }

                if (!string.IsNullOrEmpty(position))
                {
                    if (employee.Position != position)
                    {
                        continue;
                    }
                }

                if (!string.IsNullOrEmpty(name))
                {
                    if (!employee.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                }

                filteredEmployees.Add(employee);
            }

            filteredEmployees = filteredEmployees.Distinct().ToList();

            return new SearchResult([.. filteredEmployees]);
        }
    }
}
