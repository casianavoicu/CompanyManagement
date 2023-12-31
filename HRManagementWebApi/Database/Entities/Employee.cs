﻿using System.ComponentModel.DataAnnotations;

namespace HRManagementWebApi.Database.Entities
{
    public sealed class Employee
    {
        [Key]
        public int? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public decimal Salary { get; set; }

        public string? Email { get; set; }
    }
}