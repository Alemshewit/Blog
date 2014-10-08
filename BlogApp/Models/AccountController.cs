using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;//handles inputs and out puts
using System.Web.Security; //import namespace for using membership

namespace BlogApp.Models
{
    public class AccountController : Controller
    {
        //set up my data context
        Models.spAlemEntities db = new Models.spAlemEntities();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //step3. add an httppostfilebase parameter
        [HttpPost]
        public ActionResult Register(Models.Registration register, HttpPostedFileBase ImageUrl)
        {
            if (ImageUrl != null)
            {
                //save the image to our website
                //guid generates random characters, that we can use
                //to make sure the file name not repeated
                string filename = Guid.NewGuid().ToString().Substring(0, 6) + ImageUrl.FileName;
                //specify the path to save the file to 
                //Server.Mappath actually get the phsical location of the website on the server
                string path = Path.Combine(Server.MapPath("~/content/"), filename);
                //save the file
                ImageUrl.SaveAs(path);
                //update our registartion object, with the ImageUrl
                register.ImageUrl = "/content/" + filename;
            }
            //create our membership user 
            Membership.CreateUser(register.Username, register.Password);
            //create our Author object
            Models.Author author = new Models.Author();
            author.Name = register.Name;
            author.ImageURl = register.ImageUrl;
            author.Username = register.Username;

            //add the autor to the database
            db.Authors.Add(author);
            db.SaveChanges();

            //registration complete Log in the user
            FormsAuthentication.SetAuthCookie(register.Username, false);

            //kick the user to the create post section
            return RedirectToAction("Index", "Post");
        }

        public ActionResult Logout()
        {
            //to log out a user do this
            FormsAuthentication.SignOut();
            //kick the user to the login screen
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Login login)
        {
            //see if they are a valid user
            if(Membership.ValidateUser(login.Username, login.Password))
            {
                //credentials are gold, log them in
                FormsAuthentication.SetAuthCookie(login.Username, false);
                //kick the user to the create post page
                return RedirectToAction("Index", "Post");
            }
            else
            {
                //bad news bears, bad password or username
                ViewBag.ErrorMessage = "Invalid username and/or password";
                return View(login);
            }
        }
    }
}
