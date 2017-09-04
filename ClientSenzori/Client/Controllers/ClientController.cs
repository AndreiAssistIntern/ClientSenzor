using ClientSenzori.Client.Model;
using ClientSenzori.Client.ModelView;
using ClientSenzori.Client.Security;
using ClientSenzori.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace ClientSenzori.Client.Controllers
{
    public class MyActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string[] except = { "Register", "Login" };
            string actionName = filterContext.ActionDescriptor.ActionName;
            if (except.Contains(actionName))
                return;
            string admin = System.Web.HttpContext.Current.User.Identity.Name;


            if (admin != "")
            {
                filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary
                               {{ "Controller", "Account" },
                                      { "Action", "Index" } });
            }

            if (filterContext.HttpContext.Session["UserID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                  new RouteValueDictionary
                   {{ "Controller", "Client" },
                                      { "Action", "Login" } });
            }

        }
    }

    [MyActionFilter]
    public class ClientController : Controller
    {
        public ActionResult Home()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string ClientEmail = Session["UserId"].ToString();
                HomeClientModelView home = new HomeClientModelView();

                Senzors senzor = new Senzors();
                try
                {
                    var client = db.client.FirstOrDefault(c => c.Email == ClientEmail);
                    home.Email = ClientEmail;
                    home.FirstName = client.FirstName;
                    home.LastName = client.LastName;
                    home.ImagePath = client.PathImage;
                    home.senzors = db.senzors.Where(s => s.ClientId == client.id).ToList();
                    if (home.senzors.Count() > 0)
                    {

                        string KeySenzor = "";
                        foreach (var reset in home.senzors)
                        {
                            List<string> SenzorInfo = new List<string>();
                            Resetings reseting = new Resetings();

                            KeySenzor = reset.Name;
                            //SenzorInfo.Add(reset.Name);
                            SenzorInfo.Add(reset.BateryLevel.ToString());

                            try
                            {
                                //  reseting = db.resetings.LastOrDefault(r => r.SenzorsId == reset.id);
                                var getall = db.resetings.GroupBy(r => r.SenzorsId).Select(p => p.FirstOrDefault(w => w.id == p.Max(m => m.id)))
     .OrderBy(p => p.SenzorsId).ToList();
                                reseting = getall.Where(s => s.SenzorsId == reset.id).FirstOrDefault();
                                SenzorInfo.Add(reseting.BateryLevel.ToString());
                                SenzorInfo.Add(reseting.HasChecked.ToString());


                            }
                            catch (Exception e)
                            {
                                SenzorInfo.Add("TBD");
                                SenzorInfo.Add("New Senzor");

                            }
                            home.SenzorInfo.Add(KeySenzor, SenzorInfo);

                        }

                    }
                    return View(home);
                }
                catch (Exception e)
                {
                    ViewBag.Error = "";
                }
                return View(home);
            }


        }

        [HttpPost]
        public ActionResult Home(HomeClientModelView SenorName)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string ClientEmail = Session["UserId"].ToString();
                Resetings reset = new Resetings();
                Senzors senzor = new Senzors();

                try
                {
                    senzor = db.senzors.FirstOrDefault(s => s.Name == SenorName.SenzorName);
                    reset.SenzorsId = senzor.id;
                    reset.BateryLevel = senzor.BateryLevel;
                    reset.ResetDate = DateTime.Now;
                    reset.HasChecked = true;
                    db.resetings.Add(reset);
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    ViewBag.Error = "Something went wrong finding the senzor";
                }


            }
            return Home();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel client)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Try again";
                return View();
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {

                    var CheckClient = db.client.FirstOrDefault(c => c.Email == client.Email && c.Password == client.Password);
                    FormsAuthentication.SetAuthCookie(CheckClient.Email, false);

                    Session["UserID"] = CheckClient.Email;

                    Response.Redirect("Home");

                }
                catch (Exception e)
                {
                    ViewBag.Error = "Bad credential for client";
                    return View();
                }
            }

            return View();
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModelView NewClient)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {

                    Clients client = new Clients();
                    client.FirstName = NewClient.FirstName;
                    client.LastName = NewClient.LastName;
                    client.Email = NewClient.Email;
                    client.Password = NewClient.Password;


                    if (NewClient.file != null)
                    {
                        string pic = System.IO.Path.GetFileName(NewClient.file.FileName);
                        client.PathImage = "~/Client/ClientContent/" + client.Email + pic;
                        string path = System.IO.Path.Combine(Server.MapPath("~Client/ClientContent"), client.Email + pic);
                        NewClient.file.SaveAs(path);
                        try
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                NewClient.file.InputStream.CopyTo(ms);
                                byte[] array = ms.GetBuffer();
                            }
                        }
                        catch (Exception e)
                        {
                            ViewBag.Error = "Problem on uploading the image";
                        }
                    }

                    client = db.client.Add(client);
                    db.SaveChanges();

                    var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));


                    if (!roleManager.RoleExists("Client"))
                    {
                        var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                        role.Name = "Client";
                        roleManager.Create(role);
                    }


                    return RedirectToAction("/Login");
                }

            }
            return View();
        }


        public ActionResult Edit()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string email = Session["UserId"].ToString();
                Clients client = new Clients();

                try
                {
                    client = db.client.FirstOrDefault(c => c.Email == email);
                    EditClient CurrentDataClient = new EditClient();
                    CurrentDataClient.ImagePath = client.PathImage;
                    CurrentDataClient.FirstName = client.FirstName;
                    CurrentDataClient.LastName = client.LastName;
                    return View(CurrentDataClient);

                }
                catch (Exception e)
                {
                    ViewBag.Error = "Some problems has occured";
                }
            }


            return View();
        }
        [HttpPost]
        public ActionResult Edit(EditClient ClientEdit)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                string email = Session["UserId"].ToString();
                Clients client = new Clients();
                try
                {
                    client = db.client.FirstOrDefault(c => c.Password == ClientEdit.CurrentPassword);


                    Clients UpdateClient = new Clients();
                    if (ClientEdit.file != null)
                    {
                        string pic = System.IO.Path.GetFileName(ClientEdit.file.FileName);
                        client.PathImage = "~/Client/ClientContent/" + email + pic;
                        string path = System.IO.Path.Combine(Server.MapPath("~/Client/ClientContent"), client.Email + pic);
                        ClientEdit.file.SaveAs(path);
                        try
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                ClientEdit.file.InputStream.CopyTo(ms);
                                byte[] array = ms.GetBuffer();
                            }
                        }
                        catch (Exception e)
                        {
                            ViewBag.Error += "Problem on uploading the image";
                        }
                    }



                    string fullPath = Request.MapPath(ClientEdit.ImagePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    client.Password = ClientEdit.Password;
                    client.LastName = ClientEdit.LastName;
                    client.FirstName = ClientEdit.FirstName;
                    db.Entry(client).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("/Home");
                }
                catch (Exception e)
                {
                    ViewBag.Error += "The current password is not valid";
                    return Edit();
                }
            }

        }



    }
}