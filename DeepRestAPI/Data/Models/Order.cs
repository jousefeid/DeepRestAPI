using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DeepRestAPI.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        public virtual ICollection<OrderItem>? orderItems { get; set; }

    }
}
