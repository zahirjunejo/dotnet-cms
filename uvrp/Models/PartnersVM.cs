using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace uvrp.Models
{
    public class PartnersVM
    {
        public int Id { get; set; }
        public HttpPostedFileBase Logo { get; set; }
        [DisplayName("Name")]
        public string name { get; set; }

        [DisplayName("Description")]
        public string description { get; set; }
        
        [DisplayName("Company home page")]
        public string CompanyHomePage { get; set; }
    }
}