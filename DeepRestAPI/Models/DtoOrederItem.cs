using System.ComponentModel.DataAnnotations;

namespace DeepRestAPI.Models
{
    public class DtoOrederItem
    {
        [Required]
        public int itemId {  get; set; }
        public string? ItemName { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int Quantity { get ; set; }

    }
}
