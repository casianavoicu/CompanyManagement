using CompanyManagement.Common.Dto;
using System.ComponentModel.DataAnnotations;

namespace ITSuportManagementApi.Database.Entities
{
    public class Equipment
    {
        [Key]
        public int Id { get; set; }

        public EquipmentEnum EquipmentType { get; set; }

        public decimal EquipmentPrice { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}