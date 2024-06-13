using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.CommonLibrary
{
    public class SearchResult(Employee[] employees)
    {
        public Employee[] Employees { get { return employees; } }

        public void SaveToCsv(FileInfo csvFile, string delimiter = "\t")
        {
            if (csvFile == null || !csvFile.Exists)
            {
                throw new ArgumentException("CSV file path is invalid or the file does not exist.");
            }

            using var streamWriter = new StreamWriter(csvFile.FullName, false);
            streamWriter.WriteLine("Name" + delimiter + "MainWorkplace" + delimiter + "Workplace" + delimiter +
                                   "Room" + delimiter + "Phone" + delimiter + "Email" + delimiter + "Position");

            foreach (var employee in Employees)
            {
                streamWriter.WriteLine($"{employee.Name}{delimiter}{employee.MainWorkplace}{delimiter}{employee.WorkPlace}" +
                                       $"{delimiter}{employee.Room}{delimiter}{employee.Phone}{delimiter}{employee.Email}{delimiter}{employee.Position}");
            }
        }
    }
}
