using AddressBook.CommonLibrary;
using System;
using System.CommandLine;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace AddressBook.ViewerConsoleApp
{
    internal class Program
    {
        /// <summary>
        /// Display text to output.
        /// </summary>
        /// <param name="input"> input file.</param>
        /// <param name="name"> name to search.</param>
        /// <param name="mainWorkplace"> workplace to search.</param>
        /// <param name="position"> position to search.</param>
        /// <param name="output"> output file.</param>
        private static void Main(string input, string name, string mainWorkplace, string position, string output)
        {
            if (input == null)
            {
                Console.WriteLine("Nebol zadany vstupny subor");
                return;
            }

            EmployeeList? employees = EmployeeList.LoadFromJson(new FileInfo(input));
            if (employees == null)
            {
                Console.WriteLine("Chyba pri nacitavani zo subora");
                return;
            }

            var s = employees.Search(mainWorkplace: mainWorkplace, position: position, name: name);
            if (s.Employees.Length == 0)
            {
                Console.WriteLine("Nikto nebol nájdený");
            }
            for (int i = 0; i < s.Employees.Length; i++)
            {
                s.Employees[i].Vypis(i + 1);
            }

            if (output != null)
            {
                s.SaveToCsv(new FileInfo(output));
            }
        }
    }
}
