using DexterLab.Models.Data;
using DexterLab.Models.ViewModels;
using DexterLab.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Threading;

namespace DexterLab.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("/Account/login");
        }

        //GET: /account/login
        [ActionName("login")]
        [HttpGet]
        public ActionResult Login()
        {
            //Check user is not logged in
            string userEmail = User.Identity.Name;

            if (!string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("~/");
            }
            return View();
        }

        //POST: /account/login
        [ActionName("login")]
        [HttpPost]
        public ActionResult Login(LoginUserVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Set bool isValid for checker
            bool isValid = false;
            bool confirmBool = false;

            //Call DbSet
            using (Db db = new Db())
            {
                var user = db.Users.SingleOrDefault(x => x.EmailAddress.Equals(model.EmailAddress));

                if(user != null)
                {
                    //Fetch stored password by user
                    var samplePassword = user.Password;

                    //Verify Password
                    CustomPasswordHasher hash = new CustomPasswordHasher();
                    var prodPassword = hash.HashPassword(model.Password);

                    //Check if users account activated
                    if (user.EmailConfirm == confirmBool)
                    {
                        TempData["Failure"] = "Please check your email for account activation";
                        return View(model);
                    }

                    if (samplePassword.Equals(prodPassword))
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                }
            }
            //Check isValid is false
            if (!isValid)
            {
                TempData["Failure"] = "Invalid Username or Password";
                return View(model);
            }
            else
            {
                //Set a cookie or session for the user
                FormsAuthentication.SetAuthCookie(model.EmailAddress, model.RememberMe);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.EmailAddress, model.RememberMe));            }

        }

        //GET: /account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        //POST: /account/create-account
        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", model);
            }

            //Check if passwords match
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                TempData["Failure"] = "Passwords do not match";
                return View("CreateAccount", model);
            }

            //Hash the Password
            CustomPasswordHasher hash = new CustomPasswordHasher();
            string hashedPassword = hash.HashPassword(model.Password);

            using (Db db = new Db())
            {
                //Make sure username is unique
                if(db.Users.Any(x => x.EmailAddress.Equals(model.EmailAddress)))
                {
                    TempData["Failure"] = "Email Address " + model.EmailAddress + " is already registered.";
                    model.EmailAddress = "";
                    return View("CreateAccount", model);
                }
                //Create a activation GUID
                Guid activationCode = Guid.NewGuid();

                //Create userDTO
                UserDTO userDTO = new UserDTO()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    PhoneNumber = model.PhoneNumber,
                    Department = model.Department,
                    Password = hashedPassword,
                    EmailConfirm = false,
                    ActivationCode = activationCode.ToString(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };

                //Add userDTO
                db.Users.Add(userDTO);

                //Save DTO
                db.SaveChanges();

                //Add to userRoleDTO
                int userId = userDTO.Id;

                UserRoleDTO userRoleDTO = new UserRoleDTO()
                {
                    UserId = userId,
                    RoleId = 2
                };
                db.UserRoles.Add(userRoleDTO);

                db.SaveChanges();

                //Mail Message
                using (MailMessage mm = new MailMessage())
                {
                    mm.From = new MailAddress("cruxalphonse@gmail.com");
                    mm.To.Add(model.EmailAddress);
                    mm.Subject = "Account Activation For Dexter Lab";
                    string body = "Hello " + model.FirstName + " " + model.LastName + ",";
                    body += "<br /><br />Please click the following link to activate your account";
                    body += "<br /><a href = '" + string.Format("{0}://{1}/Account/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click here to activate your account.</a>";
                    body += "<br /><br />Thanks";
                    body += "<br />NTT Dexter Lab";
                    mm.Body = body;
                    mm.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                        smtp.EnableSsl = true;
                        smtp.Send(mm);
                    }
                }
            }

            //Create a tempdata message
            TempData["Success"] = "You have successfully registered your account. Check your email to activate your account before logging in.";

            //Redirect
            return Redirect("~/Account/login");
        }

        //GET: /Account/Activation
        public ActionResult Activation()
        {

            ViewBag.Message = "Invalid Activation code.";
            if (RouteData.Values["id"] != null)
            {
                Guid activeCode = new Guid(RouteData.Values["id"].ToString());
                using (Db db = new Db())
                {
                    var entity = db.Users.Where(x => x.ActivationCode == activeCode.ToString()).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.EmailConfirm = true;
                        entity.ActivationCode = "";
                    }
                    db.SaveChanges();
                    ViewBag.Message = "Account Successfully Activated";
                   
                }
            }
            else
            {
                return Redirect("~/Account/Login");
            }

            return View();
        }

        //GET: /account/logout
        [ActionName("logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Account/login");
        }

        //GET: /account/user-profile
        [ActionName("user-profile")]
        [HttpGet]
        [Authorize]
        public ActionResult UserProfile()
        {
            //Get Username
            string username = User.Identity.Name;

            //Declare model
            UserProfileVM model;

            //Using DbSet
            using (Db db = new Db())
            {
                //Get user
                UserDTO dto = db.Users.FirstOrDefault(x => x.EmailAddress.Equals(username));

                //Build model
                model = new UserProfileVM(dto);

            }

            //return View with model
            return View("UserProfile", model);
        }

        //POST: /account/user-profile
        [ActionName("user-profile")]
        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult UserProfile(UserProfileVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            //Check if password is not empty
            if (!string.IsNullOrEmpty(model.Password))
            {
                //Check if password and confirm password matches
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    TempData["Failure"] = "Passwords do not match";
                    return View("UserProfile", model);
                }
            }

            //using DbSet
            using (Db db = new Db())
            {
                //Get email address
                string emailAddress = User.Identity.Name;

                //Check if username is unique
                if(db.Users.Where(x => x.Id != model.Id).Any(x => x.EmailAddress == emailAddress))
                {
                    TempData["Failure"] = "Username is already taken";
                    model.EmailAddress = "";
                    return View("UserProfile", model);
                }

                //Edit DTO
                UserDTO dto = db.Users.Find(model.Id);

                dto.FirstName = model.FirstName;
                dto.LastName = model.LastName;
                dto.EmailAddress = model.EmailAddress;
                dto.PhoneNumber = model.PhoneNumber;
                dto.Department = model.Department;
                dto.ModifiedOn = DateTime.Now;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    CustomPasswordHasher hash = new CustomPasswordHasher();
                    string hashedPassword = hash.HashPassword(model.Password);

                    if (model.Password.Equals(model.ConfirmPassword))
                    {
                        dto.Password = hashedPassword;
                    }
                }

                //Save Changes
                db.SaveChanges();

            }

            //Set Temp Message
            TempData["Success"] = "You have successfully updated your profile";

            //Redirect
            return Redirect("~/Account/user-profile");


        }

        //GET: /account/reset-password
        [ActionName("reset-password")]
        [HttpGet]
        public ActionResult ResetPassword()
        {
        
            return View("ResetPassword");
        }

        //POST: /account/reset-password
        [ActionName("reset-password")]
        [HttpPost]
        public ActionResult ResetPassword(UserVM model)
        {

            using (Db db = new Db())
            {
                var user = db.Users.SingleOrDefault(x => x.EmailAddress.Equals(model.EmailAddress));

                if (user == null)
                {
                    TempData["Failure"] = "The email address entered is invalid";
                    return View("ResetPassword", model);
                }
                else
                {
                    //Create a activation GUID
                    Guid resetCode = Guid.NewGuid();

                    user.ResetCode = resetCode.ToString();
                    user.ModifiedOn = DateTime.Now;

                    //Save DTO
                    db.SaveChanges();

                    //Mail Message
                    using (MailMessage mm = new MailMessage())
                    {
                        mm.From = new MailAddress("cruxalphonse@gmail.com");
                        mm.To.Add(model.EmailAddress);
                        mm.Subject = "Password Recovery Request For Dexter Lab";
                        string body = "Hello " + model.FirstName + " " + model.LastName + ",";
                        body += "<br /><br />You recently requested to reset your password. Please click the following link to activate your account";
                        body += "<br /><a href = '" + string.Format("{0}://{1}/Account/Reset/{2}", Request.Url.Scheme, Request.Url.Authority, resetCode) + "'>Click here to reset your password.</a>";
                        body += "<br /><br />Thanks";
                        body += "<br />NTT Dexter Lab";
                        mm.Body = body;
                        mm.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                            smtp.EnableSsl = true;
                            smtp.Send(mm);
                        }
                    }
                }

                //Create a tempdata message
                TempData["Success"] = "Please check your email to reset your password";

                //Redirect
                return Redirect("~/Account/login");
            }
        }

        //GET: /Account/Reset/{id}
        [HttpGet]
        public ActionResult Reset()
        {
            return View();
        }
        //POST: /Account/Reset/{id}
        [HttpPost]
        public ActionResult Reset(ResetPasswordVM model)
        {

            if (RouteData.Values["id"] != null)
            {
                //Check model state
                if (!ModelState.IsValid)
                {
                    return View("Reset", model);
                }

                Guid resetCode = new Guid(RouteData.Values["id"].ToString());
                using (Db db = new Db())
                {
                    //Check if passwords match
                    if (!model.Password.Equals(model.ConfirmPassword))
                    {
                        TempData["Failure"] = "Password do not match";
                        return View("Reset", model);
                    }


                    //Hash the Password
                    CustomPasswordHasher hash = new CustomPasswordHasher();
                    string hashedPassword = hash.HashPassword(model.Password);

                    var entity = db.Users.Where(x => x.ResetCode == resetCode.ToString()).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.ResetCode = "";
                        entity.Password = hashedPassword;

                    }
                    db.SaveChanges();
                    ViewBag.Message = "Account Successfully Resetted";
                    return RedirectToAction("ResetAck");
                }
            }
            else
            {
                return Redirect("~/Account/Login");
            }
            
        }

        public ActionResult ResetAck()
        {
            ViewBag.Message = "Invalid Activation Code";
            return View();
        }
        



        //---------------------------------PARTIALS----------------------------------------------------------------------------------
        //User Navigation Partials
        public ActionResult UserNavPartial()
        {
            //Get email address
            string username = User.Identity.Name;

            //Declare the model
            UserNavPartialVM model;

            using (Db db = new Db())
            {
                //Get the user
                UserDTO dto = db.Users.FirstOrDefault(x => x.EmailAddress == username);

                //Build the model
                model = new UserNavPartialVM()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    EmailAddress = dto.EmailAddress
                };
            }

            //Return the partial view model
            return PartialView(model);
        }
    }
}