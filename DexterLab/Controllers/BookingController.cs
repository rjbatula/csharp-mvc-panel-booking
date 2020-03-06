using DexterLab.Models.Data;
using DexterLab.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DexterLab.Controllers
{
    public class BookingController : Controller
    {
        // GET: Booking
        [ActionName("select-booking")]
        public ActionResult Index()
        {
            return View("Index");
        }

        //GET: /Booking/BookPhysicalDevice
        [ActionName("book-physical-device")]
        [HttpGet]
        public ActionResult BookPhysicalDevice()
        {
            return View("BookPhysicalDevice");
        }

        //POST: /Booking/BookPhysicalDevice
        [ActionName("book-physical-device")]
        [HttpPost]
        public ActionResult BookPhysicalDevice(BookingVM model)
        {
            
            //Check if Mode State is valid
            if (!ModelState.IsValid)
            {
                return View("BookPhysicalDevice", model);
            }


            using (Db db = new Db())
            {
                var startDate = DateTime.ParseExact(model.BookingDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                var endDate = DateTime.ParseExact(model.BookingEndDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime parseDate;
                var todayDate = DateTime.Today;
                if(startDate < todayDate || endDate < todayDate){
                    TempData["Failure"] = "You cannot book a date that already passed.";
                    return View("BookPhysicalDevice", model);
                }

                //START with DATE COMPARISON HERE//
                for (parseDate = startDate; parseDate <= endDate; parseDate = parseDate.AddDays(1))
                {
                    

                    //Check if date is unique
                    if (!(db.Bookings.Any(x => x.BookingDate.Equals(parseDate))))
                    {
                        int startCounter = 1;
                        int countDev = 0;
                        string email = User.Identity.Name;

                        if (model.DeviceName == "Palo Alto")
                        {

                            countDev = 1;
                        }
                        else
                        {
                            countDev = 2;
                        }

                        int startP;
                        int endP;
                        if (model.DeviceType == "Physical")
                        {
                            startP = startCounter;
                            endP = countDev;
                        }
                        else
                        {
                            startP = 0;
                            endP = 0;
                        }

                        
                        //If Physical Device is chosen
                        if (model.DeviceType.Equals("Physical"))
                        {
                            //Continue with the booking
                            BookingDTO bookingDTO = new BookingDTO()
                            {
                                DeviceName = model.DeviceName,
                                DeviceSerialNo = model.DeviceSerialNo,
                                DeviceSpace = countDev,
                                DeviceType = model.DeviceType,
                                BookingDate = parseDate,
                                BookingEndDate = parseDate,
                                PanelStart = startP,
                                PanelEnd = endP,
                                BookingPurpose = model.BookingPurpose,
                                ServerInstalled = false,
                                ModifiedBy = "",
                                CreatedBy = email,
                                IPAddress = "",
                                Username = "",
                                Password = ""

                            };

                            db.Bookings.Add(bookingDTO);
                            db.SaveChanges();

                            //Physical
                            //Check if Identity is equals to the Email Address

                            if (db.Users.Where(a => a.EmailAddress == email).Any())
                            {
                                using (MailMessage mm = new MailMessage())
                                {
                                    mm.From = new MailAddress("no-reply@global.ntt");
                                    mm.To.Add(email);
                                    mm.Subject = "Confirmation for Booking Panel - Physical Device";
                                    string body = "Hello,";
                                    body += "<br /><br />You have successfully booked Dexter's Lab Panel";
                                    body += "<br /> Booking Date: " + model.BookingDate + " to " + model.BookingEndDate;
                                    body += "<br /><br />Panel Booked: Panel " + startCounter + " to Panel " + countDev;
                                    body += "<br /><br />Purpose of Booking: " + model.BookingPurpose;
                                    body += "<br /><br />Regards, ";
                                    body += "<br />NTT Dexter Lab";
                                    mm.Body = body;
                                    mm.IsBodyHtml = true;

                                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                    {
                                        smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                                        smtp.EnableSsl = true;
                                        smtp.Send(mm);
                                    }

                                    //Create a tempdata message
                                    TempData["Success"] = MvcHtmlString.Create("You have successfully booked your physical device in our panel." + "<br /><br />" + "Edit your booking with your credentials to remotely connect to server. Please check your email for confirmation.");
                                }
                            }
                        }
                        else
                        {
                            //Continue with the booking
                            BookingDTO bookingDTO = new BookingDTO()
                            {
                                DeviceName = model.DeviceName,
                                DeviceSerialNo = model.DeviceSerialNo,
                                DeviceSpace = countDev,
                                DeviceType = model.DeviceType,
                                BookingDate = parseDate,
                                BookingEndDate = parseDate,
                                PanelStart = startP,
                                PanelEnd = endP,
                                BookingPurpose = model.BookingPurpose,
                                ServerInstalled = false,
                                ModifiedBy = "",
                                CreatedBy = email,
                                IPAddress = "40.122.27.77",
                                Username = "fwadmin",
                                Password = "cisco"

                            };

                            db.Bookings.Add(bookingDTO);
                            db.SaveChanges();

                            //Virtualize
                            //Check if Identity is equals to the Email Address

                            if (db.Users.Where(a => a.EmailAddress == email).Any())
                            {
                                using (MailMessage mm = new MailMessage())
                                {
                                    mm.From = new MailAddress("no-reply@global.ntt");
                                    mm.To.Add(email);
                                    mm.Subject = "Confirmation for Booking Panel - Virtualize Device";
                                    string body = "Hello,";
                                    body += "<br /><br />You have successfully booked Dexter's Lab Panel";
                                    body += "<br /> Booking Date: " + model.BookingDate + " to " + model.BookingEndDate;
                                    body += "<br /><br />Purpose of Booking: " + model.BookingPurpose;
                                    body += "<br /><br />You can now go to 'My Bookings' and click the 'Connect via SSH' button to automatically connect";
                                    body += "<br /><br />If you decided to connect through your own, these are the credentials valid only until the end of your booking date:";
                                    body += "<br /><br />IP Address: 40.122.27.77";
                                    body += "<br /><br />Username: fwadmin";
                                    body += "<br /><br />Password: cisco";
                                    body += "<br /><br />Regards, ";
                                    body += "<br />NTT Dexter Lab";
                                    mm.Body = body;
                                    mm.IsBodyHtml = true;

                                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                    {
                                        smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                                        smtp.EnableSsl = true;
                                        smtp.Send(mm);
                                    }
                                    
                                    TempData["Success"] = MvcHtmlString.Create("You have successfully booked your virtual device in our panel.<br /><br /> Connect via SSH using the button at 'MyBookings'. <br /><br />" +
                                        "If you wish to connect on your own SSH device, here is the IP Address: <br /><br /> " +
                                        "40.122.27.77 <br /><br />" +
                                        "Check your email address to get the credentials provided by Dexter's Lab");

                                }
                            }
                        }
                        

                    }
                    else
                    {
                        int finalCounter = 0;
                        int elseDev = 0;
                        int xCounter = 1;

                        if (model.DeviceName == "Palo Alto")
                        {

                            elseDev = 1;
                        }
                        else
                        {
                            elseDev = 2;
                        }
                        //Check the panel
                        int maxPanel = 30;
                        int ds = elseDev;

                        //While loop for ds != 0
                        while (ds != 0)
                        {
                            if ((xCounter + (elseDev - 1)) > maxPanel)
                            {
                                TempData["Failure"] = "There are no sufficient space for booking any panel on this date.";
                                return View("BookPhysicalDevice", model);
                            }

                            //Check if initial panel no is != to PanelStart or PanelEnd
                            if ((!(db.Bookings.Where(x => x.BookingDate.Equals(parseDate)).Any(x => x.PanelEnd.Equals(xCounter)))) && (!(db.Bookings.Where(x => x.BookingDate.Equals(parseDate)).Any(x => x.PanelStart.Equals(xCounter)))))
                            {

                                ds--;
                            }
                            else
                            {
                                ds = elseDev; //Reset to OG
                                xCounter++; //Add one in Counter
                                            //if xCounter has reach maxPanel, tempdata error that all panels are book for the or not space are not sufficient for booking

                            }

                            finalCounter = xCounter;
                        }

                        int startP2;
                        int endP2;
                        if (model.DeviceType == "Physical")
                        {
                            startP2 = xCounter;
                            endP2 = xCounter + (elseDev - 1);
                        }
                        else
                        {
                            startP2 = 0;
                            endP2 = 0;
                        }

                        //Once device space reaches 0 and counter is equals to intial model.DeviceSpace
                        if (ds == 0)
                        {
                            string email = User.Identity.Name;

                            if (model.DeviceType.Equals("Physical"))
                            {
                                //Continue Booking for DTO
                                BookingDTO booking2DTO = new BookingDTO()
                                {
                                    DeviceName = model.DeviceName,
                                    DeviceSerialNo = model.DeviceSerialNo,
                                    DeviceSpace = elseDev,
                                    DeviceType = model.DeviceType,
                                    BookingDate = parseDate,
                                    BookingEndDate = parseDate,
                                    PanelStart = startP2,
                                    PanelEnd = endP2,
                                    BookingPurpose = model.BookingPurpose,
                                    ServerInstalled = false,
                                    ModifiedBy = "",
                                    CreatedBy = email,
                                    IPAddress = "",
                                    Username = "",
                                    Password = ""
                                };
                                db.Bookings.Add(booking2DTO);
                                db.SaveChanges();
                                //StartPanel = xCounter End Panel = xCounter plus devicespace
                                //Check if Identity is equals to the Email Address

                                if (db.Users.Where(a => a.EmailAddress == email).Any())
                                {
                                    int finalCount = xCounter + (elseDev - 1);
                                    using (MailMessage mm = new MailMessage())
                                    {
                                        mm.From = new MailAddress("no-reply@global.ntt");
                                        mm.To.Add(email);
                                        mm.Subject = "Confirmation for Booking Panel";
                                        string body = "Hello,";
                                        body += "<br /><br />You have successfully booked Dexter's Lab Panel";
                                        body += "<br /> Booking Date: " + model.BookingDate + " to " + model.BookingEndDate;
                                        if (xCounter == finalCount)
                                        {
                                            body += "<br /><br />Panel Booked: Panel " + finalCount;
                                        }
                                        else
                                        {
                                            body += "<br /><br />Panel Booked: Panel " + xCounter + " to Panel " + finalCount;
                                        }
                                        body += "<br /><br />Purpose of Booking: " + model.BookingPurpose;
                                        body += "<br /><br />Regards, ";
                                        body += "<br />NTT Dexter Lab";
                                        mm.Body = body;
                                        mm.IsBodyHtml = true;

                                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                        {
                                            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                                            smtp.EnableSsl = true;
                                            smtp.Send(mm);
                                        }

                                        //Create a tempdata message
                                        TempData["Success"] = MvcHtmlString.Create("You have successfully booked your physical device in our panel." + "<br /><br />" + "Edit your booking with your credentials to remotely connect to server. Please check your email for confirmation.");
                                    }
                                }
                            }
                            else
                            {
                                //Continue Booking for DTO
                                BookingDTO booking2DTO = new BookingDTO()
                                {
                                    DeviceName = model.DeviceName,
                                    DeviceSerialNo = model.DeviceSerialNo,
                                    DeviceSpace = elseDev,
                                    DeviceType = model.DeviceType,
                                    BookingDate = parseDate,
                                    BookingEndDate = parseDate,
                                    PanelStart = startP2,
                                    PanelEnd = endP2,
                                    BookingPurpose = model.BookingPurpose,
                                    ServerInstalled = false,
                                    ModifiedBy = "",
                                    CreatedBy = email,
                                    IPAddress = "40.122.27.77",
                                    Username = "fwadmin",
                                    Password = "cisco"
                                };
                                db.Bookings.Add(booking2DTO);
                                db.SaveChanges();
                                //StartPanel = xCounter End Panel = xCounter plus devicespace
                                //Check if Identity is equals to the Email Address

                                if (db.Users.Where(a => a.EmailAddress == email).Any())
                                {
                                    int finalCount = xCounter + (elseDev - 1);
                                    using (MailMessage mm = new MailMessage())
                                    {
                                        mm.From = new MailAddress("no-reply@global.ntt");
                                        mm.To.Add(email);
                                        mm.Subject = "Confirmation for Booking Panel - Virtualize Device";
                                        string body = "Hello,";
                                        body += "<br /><br />You have successfully booked Dexter's Lab Panel";
                                        body += "<br /> Booking Date: " + model.BookingDate + " to " + model.BookingEndDate;
                                        body += "<br /><br />Purpose of Booking: " + model.BookingPurpose;
                                        body += "<br /><br />You can now go to 'My Bookings' and click the 'Connect via SSH' button to automatically connect";
                                        body += "<br /><br />If you decided to connect through your own, these are the credentials valid only until the end of your booking date:";
                                        body += "<br /><br />IP Address: 40.122.27.77";
                                        body += "<br /><br />Username: fwadmin";
                                        body += "<br /><br />Password: cisco";
                                        body += "<br /><br />Regards, ";
                                        body += "<br />NTT Dexter Lab";
                                        mm.Body = body;
                                        mm.IsBodyHtml = true;

                                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                        {
                                            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                                            smtp.EnableSsl = true;
                                            smtp.Send(mm);
                                        }

                                        //Create a tempdata message
                                        TempData["Success"] = MvcHtmlString.Create("You have successfully booked your virtual device in our panel.<br /><br /> Connect via SSH using the button at 'MyBookings'. <br /><br />" +
                                        "If you wish to connect on your own SSH device, here is the IP Address: <br /><br /> " +
                                        "40.122.27.77 <br /><br />" +
                                        "Check your email address to get the credentials provided by Dexter's Lab");
                                    }
                                }
                            }
                            
                        }
                    }
                
                }
            }

            //Redirect
            return RedirectToAction("booking-success", "Booking");
        }

        //GET: /Booking/MyBooking
        [ActionName("my-bookings")]
        public ActionResult MyBookings()
        {


            using (Db db = new Db())
            {
                //Check if user is login
                string email = User.Identity.Name;
                List<MyBookingsVM> myBooking = new List<MyBookingsVM>();

                var booking = db.Bookings.Select(x => new MyBookingsVM
                {
                    Id = x.Id,
                    DeviceName = x.DeviceName,
                    DeviceSerialNo = x.DeviceSerialNo,
                    DeviceSpace = x.DeviceSpace,
                    DeviceType = x.DeviceType,
                    PanelStart = x.PanelStart,
                    PanelEnd = x.PanelEnd,
                    BookingDate = x.BookingDate,
                    BookingEndDate = x.BookingEndDate,
                    BookingPurpose = x.BookingPurpose,
                    ServerInstalled = x.ServerInstalled,
                    ModifiedBy = x.ModifiedBy,
                    CreatedBy = x.CreatedBy,
                    IPAddress = x.IPAddress,
                    Username = x.Username,
                    Password = x.Password

                }).ToList().Where(x => x.CreatedBy.Equals(email));

                //Init List for BookingVM
                //List<MyBookingsVM> bookings = db.Bookings.Where(x => x.CreatedBy.Equals(email)).Any();
                return View("MyBookings", booking);

            }
            //Check is identity has record in booking table
            //yes, show table
            //no, show "You have no current booking."
        }

        //GET: /Booking/EditBooking/id
        [ActionName("edit-booking")]
        [HttpGet]
        public ActionResult EditBooking(int id)
        {
            int bookingId = id;
            string email = User.Identity.Name;

            //Declare MyBookings VM
            EditBookingsVM model;

            using (Db db = new Db())
            {


                //Get user
                BookingDTO dto = db.Bookings.FirstOrDefault(x => x.Id.Equals(bookingId));

                if (dto == null)
                {
                    TempData["Failure"] = "The booking does not exist";
                    return View("MyBookings");
                }

                model = new EditBookingsVM();
                if (dto.CreatedBy == email)
                {
                    model.Id = dto.Id;
                    model.DeviceSerialNo = dto.DeviceSerialNo;
                    model.BookingPurpose = dto.BookingPurpose;
                    model.ServerInstalled = dto.ServerInstalled;
                    model.IPAddress = dto.IPAddress;
                    model.Username = dto.Username;
                    model.Password = "";
                    model.ConfirmPassword = "";


                }
                else
                {
                    TempData["Failure"] = "Invalid Panel Booking Edit Request";
                    return RedirectToAction("my-bookings");
                }


                return View("EditBooking", model);
            }


        }

        //POST: /Booking/EditBooking/id
        [ActionName("edit-booking")]
        [HttpPost]
        public ActionResult EditBooking(EditBookingsVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditBooking", model);
            }

            //Check if password is not empty
            if (!string.IsNullOrEmpty(model.Password))
            {
                //Check if password and confirm password matches
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    TempData["Failure"] = "Passwords do not match";
                    return View("EditBooking", model);
                }
            }

            string email = User.Identity.Name;

            using (Db db = new Db())
            {
                if (db.Bookings.Any(x => x.Id.Equals(model.Id)))
                {
                    BookingDTO dto = db.Bookings.Find(model.Id);
                    dto.DeviceSerialNo = model.DeviceSerialNo;
                    dto.BookingPurpose = model.BookingPurpose;
                    dto.ServerInstalled = model.ServerInstalled;
                    dto.ModifiedBy = email;
                    if (!string.IsNullOrEmpty(model.IPAddress))
                    {
                        dto.IPAddress = model.IPAddress;
                    }
                    if (!string.IsNullOrEmpty(model.Username))
                    {
                        dto.Username = model.Username;
                    }
    
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        if (model.Password.Equals(model.ConfirmPassword))
                        {
                            dto.Password = model.Password;
                        }
                    }
                    
                    //if (!string.IsNullOrEmpty(model.Password))
                    //{
                    //    CustomPasswordHasher hash = new CustomPasswordHasher();
                    //    string hashedPassword = hash.HashPassword(model.Password);

                    //    if (model.Password.Equals(model.ConfirmPassword))
                    //    {
                    //        dto.Password = hashedPassword;
                    //    }
                    //}

                    db.SaveChanges();
                }
                else
                {
                    TempData["Failure"] = "Invalid Edit Panel Booking Request";
                    return View("EditBooking", model);
                }
            }
            TempData["Success"] = "You have successfully edited your booking";
            return View("EditBooking");
        }

        //GET: /Booking/DeleteBooking/id
        [ActionName("delete-booking")]
        [HttpGet]
        public ActionResult DeleteBooking(int id)
        {
            using (Db db = new Db())
            {
                BookingDTO dto = db.Bookings.Find(id);

                db.Bookings.Remove(dto);

                db.SaveChanges();
            }

            TempData["Success"] = "You have successfully remove your booking";
            return RedirectToAction("my-bookings");
        }

        //GET: /Booking/SpinVirtual/id
        [ActionName("spin-virtual")]
        [HttpGet]
        public ActionResult SpinVirtual(int id)
        {
            string ipAdd, userName, pass;
            using (Db db = new Db())
            {
                BookingDTO dto = db.Bookings.Find(id);
                ipAdd = dto.IPAddress;
                userName = dto.Username;
                pass = dto.Password;
            }


            string VM = @"c:\temp\virtual.bat";
            string REDIRECT = @"c:\temp\redirect.bat";
            string Del = @"c:\temp\delete.bat";

            if (!System.IO.File.Exists(REDIRECT))
            {
                // Create a file to write to.
                using (StreamWriter sw = System.IO.File.CreateText(REDIRECT))
                {
                    sw.WriteLine("cd C:/temp");
                    sw.WriteLine("virtual.bat");

                }
            }
                if (!System.IO.File.Exists(VM))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = System.IO.File.CreateText(VM))
                    {
                        sw.WriteLine("@echo off");
                        //sw.WriteLine("cmdkey /generic:" + '"' + "192.168.15.41" + '"' + " /user:" + '"' + "SGMAIL" + Regex.Escape("\r") + "alphjoshua.batula" + '"' + " /pass:" + '"' + "P@ssw0rd" + '"');
                        //sw.WriteLine("mstsc /f /v:" + '"' + "192.168.15.41" + '"');
                        //sw.WriteLine("cmdkey /delete:" + '"' + "192.168.15.41" + '"');
                        sw.WriteLine("cmdkey /generic:" + '"' + ipAdd + '"' + " /user:" + '"' + userName + '"' + " /pass:" + '"' + pass + '"');
                        sw.WriteLine("mstsc /f /v:" + '"' + ipAdd + '"');
                        sw.WriteLine("cmdkey /delete:" + '"' + ipAdd + '"');

                    }

                }
            if (!System.IO.File.Exists(Del))
            {
                // Create a file to write to.
                using (StreamWriter sw = System.IO.File.CreateText(Del))
                {
                    sw.WriteLine("@echo off");
                    sw.WriteLine("del virtual.bat");
                    sw.WriteLine("del redirect.bat");
                    sw.WriteLine("del delete.bat");

                }

            }

            string command = "/C cd C:/temp/ & @echo off & redirect.bat & delete.bat";
                System.Diagnostics.Process.Start("cmd.exe", command);

            TempData["Success"] = "Connecting remotely to Virtual Machine...";
            return RedirectToAction("my-bookings");


        }

        //GET: /Booking/SpinSSH/id
        [ActionName("spin-SSH")]
        [HttpGet]
        public ActionResult SpinSSH(int id)
        {
            string ipAdd, userName, pass;
            using (Db db = new Db())
            {
                BookingDTO dto = db.Bookings.Find(id);
                ipAdd = dto.IPAddress;
                userName = dto.Username;
                pass = dto.Password;
            }

            string com = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\PuTTY (64-bit)";
            string com2 = @"putty.exe -ssh "+userName+"@"+ipAdd+" -pw " +pass;
            string command = "/C cd " + com + " & @echo off & " + com2;
            System.Diagnostics.Process.Start("cmd.exe", command);

            TempData["Success"] = "Connecting virtually via SSH...";
            return RedirectToAction("my-bookings");
        }

        //-------------------------------------Static Success/Failure Page--------------------------------

        //GET: /Booking/BookingSuccess
        [ActionName("booking-success")]
        public ActionResult BookingSuccess()
        {
            return View("BookingSuccess");
        }

    }

   
    
}