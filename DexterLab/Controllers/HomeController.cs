using DexterLab.Models.Data;
using DexterLab.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DexterLab.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(DateTime? startDate)
        {
            

            using (Db db = new Db())
            {
                if(startDate != null)
                {
                    var model = db.Bookings.Select(x => new GenericBookingVM
                    {
                        Id = x.Id,
                        DeviceName = x.DeviceName,
                        PanelStart = x.PanelStart,
                        PanelEnd = x.PanelEnd,
                        BookingDate = x.BookingDate,
                        ServerInstalled = x.ServerInstalled

                    }).ToList().Where(x => x.BookingDate == startDate);
                    TempData["date"] = startDate.Value.ToString("MMM dd yyyy");
                    return View("Index", model);
                    
                }
                else
                {
                    var model = db.Bookings.Select(x => new GenericBookingVM
                    {
                        Id = x.Id,
                        DeviceName = x.DeviceName,
                        PanelStart = x.PanelStart,
                        PanelEnd = x.PanelEnd,
                        BookingDate = x.BookingDate,
                        ServerInstalled = x.ServerInstalled

                    }).ToList().Where(x => x.BookingDate.Equals(DateTime.Today.Date));
                    TempData["date"] = DateTime.Today.Date.ToString("MMM dd yyyy");
                    return View("Index", model);
                }
                
               
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}