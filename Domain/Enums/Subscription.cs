
namespace Domain.Enums
{
    public class Subscription
    {
        public enum SubscriptionStatus
        {
            Active = 1,
            Expired = 2,
            Canceled = 3,
        }
        public enum SubscriptionPlan
        {
            Basic = 1,
            Standard = 2,
            Premieum = 3,
        }
    }
}
