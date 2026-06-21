using System.ComponentModel.DataAnnotations;

namespace DeepRestAPI.Models
{
    public class DtoOrder
    {
        public int orderId { get; set; }
        [MaxLength(100)]
        public string OrderName { get; set; }

        public DateTime CreatedDate { get; set; }


        public ICollection<DtoOrederItem> items { get; set; } = new List<DtoOrederItem>();
    }

}
