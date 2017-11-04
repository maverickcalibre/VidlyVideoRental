using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VidlyVideoRental.Models;

namespace VidlyVideoRental.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter customer's name.")]
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        //public MembershipType MembershipType { get; set; }        //Define separate Dto class for membership to completely decouple from domain model

        //[Display(Name = "Membership Type")]
        public byte MembershipTypeId { get; set; }

        //[Display(Name = "Date of Birth")]
        //[Min18YearsIfAMember]
        public DateTime? Birthdate { get; set; }
    }
}