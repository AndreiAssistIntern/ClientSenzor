using ClientSenzori.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSenzori.ModelView
{
    public class AssignedSenzor
    {
        public string EmailClient { get; set; }

        public string SenzorName { get; set; }

        public List<string> EmailClients { get; set; }

        public List<string> SenzorNames { get; set; }

        public Dictionary<string, List<string>> assignedSenzor { get; set; }
       
        public AssignedSenzor(bool dictionary)
        {
            assignedSenzor = new Dictionary<string, List<string>>();
        }

        public AssignedSenzor()
        {
            
            EmailClients = new List<string>();
            SenzorNames = new List<string>();
        }
        public Dictionary<string, List<string>> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }
        private Dictionary<string, List<string>> attributes;
    }
}