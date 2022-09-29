using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class Stylist
    {
        [Key]
        public int StylistId { get; set; }
        public string StylistName { get; set; }
        public string SalonName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Background { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }        

        //[ForeignKey("StylistSpecialtyId")]
        //public int StylistSpecialtyId { get; set; }

        //public virtual StylistSpecialty StylistSpecialty { get; set; }
    }
}
