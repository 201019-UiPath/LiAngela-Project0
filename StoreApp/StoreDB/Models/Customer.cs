using System.Collections.Generic;

namespace StoreDB.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string MailingAddress { get; set; }
        
        public List<Order> OrderHistory { get; set; }
    }
}