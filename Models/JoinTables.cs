namespace HealthInsurance.Models
{
    public class JoinTables
    {
        public Users users { get; set; }
        public Subscriptions subscriptions { get; set; }
        public Testimonials testimonials { get; set; }
        public Beneficiaries beneficiaries { get; set; }
    }
}
