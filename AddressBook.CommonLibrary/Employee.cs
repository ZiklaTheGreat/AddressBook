using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AddressBook.CommonLibrary
{
    public record Employee : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string? Room { get; set; }
        public string? MainWorkplace { get; set; }
        public string? WorkPlace { get; set; }

        public Employee(string name, string position, string? phone, string email, string? room, string? mainWorkplace, string? workPlace)
        {
            Name = name;
            Position = position;
            Phone = phone;
            Email = email;
            Room = room;
            MainWorkplace = mainWorkplace;
            WorkPlace = workPlace;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void Vypis(int index)
        {
            Console.WriteLine($"[{index}] {Name}");
            Console.WriteLine($"Pracovisko: {WorkPlace}");
            Console.WriteLine($"Miestnosť: {Room}");
            Console.WriteLine($"Telefón: {Phone}");
            Console.WriteLine($"E-mail: {Email}");
            Console.WriteLine($"Funkcia: {Position} \n");
        }

        public string csvLine()
        {
            var delimiter = "\t";
            return $"{Name}{delimiter}{MainWorkplace}{delimiter}{WorkPlace}" +
                   $"{delimiter}{Room}{delimiter}{Phone}{delimiter}{Email}{delimiter}{Position}";
        }
    }
}
