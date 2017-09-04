using ClientSenzori.Client.Model;
using ClientSenzori.Models;
using ClientSenzori.ModelView;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientSenzori.Controllers
{
    public class SenzorsManageController : Controller
    {

        //to do
        public ActionResult ShowClients()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            }
            return View();
        }
        // GET: SenzorsManage
        public ActionResult Index()
        {
            AssignedSenzor senzorClients = new AssignedSenzor();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                senzorClients.EmailClients = db.client.Select(s => s.Email).ToList();
                var Names = db.senzors.Where(s => s.ClientId == 1).ToList();
                senzorClients.SenzorNames = Names.Select(s => s.Name).ToList();

            }

            return View(senzorClients);
        }

        public ActionResult AddSenzor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSenzor(SenzorAdd NewSenzor)
        {
            if (!ModelState.IsValid)
            {
                return View(NewSenzor);
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Clients client = new Clients();
                string clientEmail = "";
                var CheckSenzorName = db.senzors.Where(S => S.Name == NewSenzor.NameOfSenzor);
                if (CheckSenzorName.Count() > 0)
                {
                    ViewBag.Error = "Name exist";
                    return View();
                }
                try
                {
                    clientEmail = db.client.FirstOrDefault(c => c.Email == "ClientMaster@mail.com").FirstName;
                    client = db.client.First(c => c.Email == "ClientMaster@mail.com");
                }
                catch (Exception e)
                {
                    Clients MasterClient = new Clients();
                    MasterClient.Email = "ClientMaster@mail.com";
                    MasterClient.FirstName = "Master";
                    MasterClient.LastName = "Master";
                    MasterClient.Password = "Master";
                    client = db.client.Add(MasterClient);
                    db.SaveChanges();

                }

                Senzors senzor = new Senzors();
                senzor.Clients = client;
                senzor.ClientId = client.id;
                senzor.Name = NewSenzor.NameOfSenzor;
                senzor.ValueType = NewSenzor.ValueType;
                senzor.BateryLevel = 100;
                db.senzors.Add(senzor);
                db.SaveChanges();


            }
            return RedirectToAction("/Index");

        }

        [HttpPost]
        public ActionResult Index(AssignedSenzor assigned)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Clients client = new Clients();
                client = db.client.FirstOrDefault(c => c.Email == assigned.EmailClient);
                if (client != null)
                {
                    Senzors senzor = new Senzors();
                    senzor = db.senzors.First(s => s.Name == assigned.SenzorName);
                    if (senzor != null)
                    {
                        senzor.ClientId = client.id;
                        senzor.Clients = client;
                        db.Entry(senzor).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            return Index();
        }

        public ActionResult Edit()
        {
            AssignedSenzor senzorClients = new AssignedSenzor(true);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {



                var clients = db.client.ToList();

                foreach (var c in clients)
                {
                    Clients client = new Clients();
                    List<string> senzors = new List<string>();
                    try
                    {
                        senzors = db.senzors.Where(s => s.ClientId == c.id).Select(d => d.Name).ToList();
                        senzorClients.assignedSenzor.Add(c.Email, senzors);

                    }
                    catch (Exception e)
                    {
                        Senzors senzor = new Senzors();
                        senzor.Name = "No senzor assigned to the user";
                        senzors.Add(senzor.Name);
                        senzorClients.assignedSenzor.Add(c.Email, senzors);
                    }
                }

            }
            return View(senzorClients);
        }
        [HttpPost]
        public ActionResult Edit(AssignedSenzor senzorClients)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    Clients client = new Clients();
                    client = db.client.FirstOrDefault(c => c.Email == senzorClients.EmailClient);
                    Senzors senzor = new Senzors();
                    senzor = db.senzors.FirstOrDefault(s => s.Name == senzorClients.SenzorName);
                    senzor.ClientId = 1;
                    db.Entry(senzor).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.Error = "On error has occur while trying to remove the senzor";
                }
            }

            return Edit();
        }

        public ActionResult ShowRecords()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var clients = db.client.ToList();
                List<ShowRecords> showRecords = new List<ShowRecords>();
                List<Senzors> senzors = new List<Senzors>();
                //   List<Clients> clients = new List<Clients>();
                // List<Resetings> records = new List<Resetings>();
                foreach (var client in clients)
                {
                    senzors = db.senzors.Where(s => s.ClientId == client.id).ToList();
                    foreach (var senzor in senzors)
                    {
                        using (ApplicationDbContext db1 = new ApplicationDbContext())
                        {
                            var records = db1.resetings.Where(r => r.SenzorsId == senzor.id);
                            if (records.Count() > 0)
                            {
                                foreach (var record in records)
                                {

                                    ShowRecords showRecord = new ShowRecords();
                                    showRecord.FirstName = client.FirstName;
                                    showRecord.LastName = client.LastName;
                                    showRecord.Email = client.Email;
                                    showRecord.SenzorName = senzor.Name;
                                    showRecord.HasCheck = record.HasChecked;
                                    showRecord.CheckDate = record.ResetDate;
                                    showRecord.BateryLevel = senzor.BateryLevel;
                                    showRecords.Add(showRecord);

                                }
                            }
                            else
                            {
                                ShowRecords showRecord = new ShowRecords();
                                showRecord.FirstName = client.FirstName;
                                showRecord.LastName = client.LastName;
                                showRecord.Email = client.Email;
                                showRecord.SenzorName = senzor.Name;
                                showRecord.BateryLevel = senzor.BateryLevel;
                                showRecord.CheckDate = DateTime.Now;
                                showRecord.HasCheck = false;
                                if (senzor.ClientId == 1)
                                {
                                    showRecord.Status = "Not assigned yet";
                                }
                                else
                                {
                                    showRecord.Status = "New assigned senzor";
                                }
                                showRecords.Add(showRecord);
                            }
                        }

                    }
                }
                return View(showRecords);
            }


        }


        public ActionResult DeleteSenzor()
        {
            List<DeleteSenzor> deleteSenzor = new List<DeleteSenzor>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<Senzors> senzors = new List<Senzors>();

                try
                {
                    senzors = db.senzors.ToList();
                    foreach (var senzor in senzors)
                    {
                        var client = db.client.FirstOrDefault(c => c.id == senzor.ClientId);
                        int CountRecors = db.resetings.Where(r => r.SenzorsId == senzor.id).Count();
                        DeleteSenzor currentSenzor = new DeleteSenzor();
                        currentSenzor.Email = client.Email;
                        currentSenzor.FirstName = client.FirstName;
                        currentSenzor.LastName = client.LastName;
                        currentSenzor.SenzorName = senzor.Name;
                        currentSenzor.CountRecords = CountRecors;
                        deleteSenzor.Add(currentSenzor);
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Error = "Somethin went wrong" + e.Message;
                }

            }


            return View(deleteSenzor);
        }

        [HttpPost]
     
        public ActionResult DeleteSenzor(DeleteSenzor deleteSenzor)
        {

            return DeleteSenzor();
        }

    }
}