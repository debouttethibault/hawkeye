using System.ComponentModel.DataAnnotations;

namespace Hawkeye.Central.Services.Models.Request
{
    public class UavRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
