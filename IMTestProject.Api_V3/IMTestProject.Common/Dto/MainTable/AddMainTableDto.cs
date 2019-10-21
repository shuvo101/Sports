using System.ComponentModel.DataAnnotations;

namespace IMTestProject.Common.Dto.MainTable
{
    public class AddMainTableDto
    {
        [Required]
        public string TableName { get; set; }

        [Required]
        public string TableCode { get; set; }
    }
}