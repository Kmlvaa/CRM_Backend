﻿namespace CRM.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; } 
        public required string CompanyName { get; set; } 
    }
}
