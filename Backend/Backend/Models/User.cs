using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Models;
public class User
{

    public User(int age, int phoneNumber, string name, string address)
    {
        Age = age;
        PhoneNumber = phoneNumber;
        Name = name;
        Address = address;
    }

    public User()
    {

    }
    public required int Age { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required int PhoneNumber { get; set; }
}