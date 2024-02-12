namespace Domain.Models
{
    public class SubscriptionStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
