using ClientSenzori.Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClientSenzori.Models
{
    public class Senzors
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("Clients")]
        public int ClientId { get; set; }
        public virtual Clients Clients { get; set; }

        public virtual ICollection<Resetings> reseting {get;set;}

        public string Name { get; set; }

        public string Value { get; set; }

        public string ValueType { get; set; }

        public int BateryLevel { get; set; }

      
    }
}