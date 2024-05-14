using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Range(1, 100)]
    [Required]
    public required int Age { get; set; }
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }
    [Required]
    [StringLength(100)]
    public required string Address { get; set; }
    [Required]
    [StringLength(100)]
    public required int PhoneNumber { get; set; }
}