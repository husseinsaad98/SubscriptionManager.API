using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int SubscriptionTypeId { get; set; }
        [ForeignKey("SubscriptionTypeId")]

        public SubscriptionPlan SubscriptionType { get; set; }

        public int SubscriptionStatusId { get; set; }
        [ForeignKey("SubscriptionStatusId")]
        public SubscriptionStatus SubscriptionStatus { get; set; }
    }
}
