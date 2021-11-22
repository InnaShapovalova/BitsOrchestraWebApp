using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Married { get; set; }
        public string Phone { get; set; }
        public decimal? Salary { get; set; }

        public Person(string name, DateTime? dateOfBirth, bool? married, string phone, decimal? salary)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Married = married;
            Phone = phone;
            Salary = salary;
        }

        public Person()
        {}
    }
}
