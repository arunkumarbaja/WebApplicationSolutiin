using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class City
    {
        [Key]
        public Guid CityId { get; set; }
        
        [Required(ErrorMessage ="City name cannot be empty")]
        public string? CityName { get; set; }
    }
}