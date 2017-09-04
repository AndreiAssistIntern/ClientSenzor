using ClientSenzori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSenzori.Client.Security
{
    public class Secure
    {
        public bool check(string header)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                byte[] head = Convert.FromBase64String(header);
                string auth = Convert.ToBase64String(head);
                var client = db.client.FirstOrDefault(c =>c.Email == auth);
            }

                return false;
        }
    }
}