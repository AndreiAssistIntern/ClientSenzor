using ClientSenzori.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClientSenzori.Client.Model
{
    public class Resetings 
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("Senzors")]
        public int SenzorsId { get; set; }
        public virtual Senzors Senzors { get; set; }


        public DateTime ResetDate{get; set;}

        public int BateryLevel { get; set; }

        public bool HasChecked { get; set; }
    }
}