using System.ComponentModel.DataAnnotations;

namespace MyAvanaApi.Models.ViewModels
{
    public class CheckoutRequest
    {
        [Required]
        public string SubscriptionId { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string CardOwnerFirstName { get; set; }
        [Required]
        public string CardOwnerLastName { get; set; }
        [Required]
        public long? ExpirationYear { get; set; }
        [CreditCard]
        public string CardNumber { get; set; }
        [Required]
        public long? ExpirationMonth { get; set; }
        [Required]
        public string CVV2 { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string Address { get;  set; }
    }
}
