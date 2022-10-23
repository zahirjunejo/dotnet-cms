using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace uvrp.Models
{
    public class DirectoriesAddViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Organization Name")]
        public string OrganizationName { get; set; }
        [Required]
        public string Industry { get; set; }
        public HttpPostedFileBase Logo { get; set; }
        public string Website { get; set; }

        [DisplayName("Contact Address")]
        public string ContactAddress { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email")]
        public string EmailAddress { get; set; }

        [DisplayName("Point Of Contact")]
        public string PointOfContact { get; set; }

        [DisplayName("Is email private?")]
        public bool IsEmailPrivate { get; set; }

        [DisplayName("Is phone private?")]
        public bool IsPhonePrivate { get; set; }

        [DisplayName("Is address private?")]
        public bool IsAddressPrivate { get; set; }
    }
}