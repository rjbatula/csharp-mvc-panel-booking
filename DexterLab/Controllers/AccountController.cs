using DexterLab.Models.Data;
using DexterLab.Models.ViewModels;
using DexterLab.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
            //Check isValid is true
            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid Username or Password");
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
                ModelState.AddModelError("", "Password do not match.");
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
                    ModelState.AddModelError("", "Email Address " + model.EmailAddress + " is already registered.");
                    model.EmailAddress = "";
                    return View("CreateAccount", model);
                }

                //Create userDTO
                UserDTO userDTO = new UserDTO()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    PhoneNumber = model.PhoneNumber,
                    Department = model.Department,
                    Password = hashedPassword
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
            }

            //Create a tempdata message
            TempData["Success"] = "You have successfully registered your account. You can log in now.";

            //Redirect
            return Redirect("~/Account/login");
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
                    ModelState.AddModelError("", "Password do not match");
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
                    ModelState.AddModelError("", "Username is already taken");
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