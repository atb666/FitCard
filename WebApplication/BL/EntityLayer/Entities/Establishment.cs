using System;

namespace BL
{
    public class Establishment
    {
        public long? Id { get; internal set; }
        public string CompanyName { get; set; }
        public string TradingName { get; set; }
        public string Cnpj { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public string Phone { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public EstablishmentCategory Category { get; set; }
        public EstablishmentStatus Status { get; set; }
    }
}