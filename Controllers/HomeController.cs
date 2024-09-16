using DataAccessLayer.Concrete;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;
using System.Runtime.ConstrainedExecution;

namespace CustomsTracking_.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }

        public IActionResult Index(string xrayFilter,string driverNameFilter, DateTime TransactionFilter, string VehicleTypeFilter)
        {
            //Filtreleme işlemi yapılması gerekiyor . Repositories klasörünü filtreleme için açtım(?)
            
            return View();

        } 
   
        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPage(LoginPage model)
        {
            var existingEntry = _context.LoginPages.FirstOrDefault(x => x.Plate == model.Plate);

            if (existingEntry != null)
            {
                ModelState.AddModelError("Plate", "Bu plakaya sahip bir araç zaten kayıtlı.");
                return View("LoginPage", model);


            }

            _context.LoginPages.Add(model);
            _context.SaveChanges();

            return RedirectToAction("RegistrationPage", new { plate = model.Plate });

        }

        public IActionResult LoginList()
        {
            return View(_context.LoginPages);
        }


        [HttpGet]
        public IActionResult RegistrationPage(string plate)
        {
            ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "VehicleTypeName");
            ViewBag.VehicleBrands = new SelectList(_context.VehicleBrands, "Id", "BrandName");
            ViewBag.VehicleModels = new SelectList(_context.VehicleModels, "Id", "ModelName");

            return View();
        }
        [HttpPost]
        public IActionResult RegistrationPage(RegistrationPage model)
        { 
            var existingEntry = _context.LoginPages.FirstOrDefault(x => x.Plate == model.Plate);

            if (existingEntry != null)
            {
                
                var registration = new RegistrationPage
                {
                    Plate = existingEntry.Plate,
                    DriverName = model.DriverName,
                    DriverSurname = model.DriverSurname,
                    DriverFatherName = model.DriverFatherName,
                    NationalId = model.NationalId,
                    Passport = model.Passport,
                    VehicleTypeId = model.VehicleTypeId,
                    VehicleBrandId = model.VehicleBrandId,
                    VehicleModelId = model.VehicleModelId,
                    LoadPosition = model.LoadPosition,
                    LoadStatus = model.LoadPosition ? model.LoadStatus : "Veri Yok", 
                    TransactionDate = model.TransactionDate,
                };

             
                _context.RegistrationPages.Add(registration);
                _context.SaveChanges();

             
                return RedirectToAction("DispatchPage", new { plate = registration.Plate });
            }

            ModelState.AddModelError("Plate", "Bu plakaya sahip bir kayıt bulunamadı.");

            ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Id", "VehicleTypeName");
            ViewBag.VehicleBrands = new SelectList(_context.VehicleBrands, "Id", "BrandName");     
            ViewBag.VehicleModels = new SelectList(_context.VehicleModels, "Id", "ModelName");
            return View(model);
        }

        public IActionResult RegistrationList()
        {
            var registrations = _context.RegistrationPages.ToList();

            foreach (var registration in registrations)
            {
                if (registration.LoadPosition == false)
                {
                    registration.LoadStatus = "Veri Yok";
                }
            }

            return View(registrations);
        }

        [HttpGet]
        public IActionResult DispatchPage(string plate)
        {
            ViewBag.Plate = plate;
            ViewBag.DispatchReasons = new SelectList(_context.DispatchReasons, "Id", "XrayReason");
            return View();
          
        }
        [HttpPost]
        public IActionResult DispatchPage(DispatchPage model)
        {
            var existingEntry = _context.LoginPages.FirstOrDefault(x => x.Plate == model.Plate);

            if (existingEntry != null)
            {
                var dispatch = new DispatchPage
                {
                    Plate = existingEntry.Plate,
                    XrayStatus = model.XrayStatus,
                    DispatchReasonId = model.DispatchReasonId,
                    TransactionDate = model.TransactionDate

                };

                _context.DispatchPages.Add(dispatch);
                _context.SaveChanges();

                return RedirectToAction("ExitPage", new { plate = dispatch.Plate });
            }

            ModelState.AddModelError("Plate", "Bu plakaya sahip bir kayıt bulunamadı.");
            return View(model);
        }
        public IActionResult DispatchList()
        {
            return View(_context.DispatchPages);
        }

        [HttpGet]
        public IActionResult ExitPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ExitPage(ExitPage model)
        {
            var existingEntry = _context.LoginPages.FirstOrDefault(x => x.Plate == model.Plate);

            if (existingEntry != null)
            {
                var exit = new ExitPage
                {
                    Plate = existingEntry.Plate,
                    TransactionDate = model.TransactionDate
                };

                _context.ExitPages.Add(exit);
                _context.SaveChanges();


                _context.LoginPages.Remove(existingEntry);
               
            }

            return RedirectToAction("LoginPage", new { plate = model.Plate });

        }
    }
}
