using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class User
    {

        public User(int age, int phoneNumber, string name, string address)
        {
            Age = age;
            PhoneNumber = phoneNumber;
            Name = name;
            Address = address;
        }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
    }
}
