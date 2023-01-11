using Soccer.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Soccer.Models
{
    public class TeamViewModel : Team
    {
        [Display(Name = "Foto")]
        public IFormFile? ImageFile { get; set; }
    }
}
