using ClientSenzori.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSenzori.Client.Model
{
    public class Clients
    {
        [Key]
        public int id { get; set; }

        public virtual ICollection<Senzors> senzors { get; set; }

        public virtual ICollection<Resetings> Reset { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PathImage { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}