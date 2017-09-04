using ClientSenzori.Client.Model;
using ClientSenzori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSenzori.Client.ModelView
{
    public class HomeClientModelView
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }

        public List<Resetings> reseting { get; set; }
        public List<Senzors> senzors { get; set; }
        public Dictionary<string,List<string>> SenzorInfo { get; set; }
        
        public string SenzorName { get; set; }

        public HomeClientModelView()
        {
            SenzorInfo = new Dictionary<string, List<string>>();
            senzors = new List<Senzors>();
            reseting = new List<Resetings>();
        }

    }
}