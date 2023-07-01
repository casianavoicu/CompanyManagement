using CompanyManagement.Common.Dto;

namespace ITSuportManagementApi.Dto
{
    public class EquipmentDto
    {
        public EquipmentEnum EquipmentType { get; set; }

        public decimal EquipmentPrice { get; set; }

        public int EmployeeId { get; set; }

    }
}
