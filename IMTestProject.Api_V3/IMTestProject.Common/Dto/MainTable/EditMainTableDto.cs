using System.ComponentModel.DataAnnotations;

namespace IMTestProject.Common.Dto.MainTable
{
    public class EditMainTableDto
    {
        [Required]
        public string TableName { get; set; }

        [Required]
        public string TableCode { get; set; }

        [Required]
        public int  Id { get; set; }
    }
}