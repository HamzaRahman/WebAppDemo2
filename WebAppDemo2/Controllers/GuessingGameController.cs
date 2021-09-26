using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppDemo2.Models;

namespace WebAppDemo2.Controllers
{
    public class GuessingGameController : Controller
    {
        IHttpContextAccessor _httpContextAccessor;
        public GuessingGameController(IHttpContextAccessor _httpContextAccessor)
        {
            this._httpContextAccessor = _httpContextAccessor;
        }
        [Route("/GuessingGame")]
        public ActionResult Index()
        {
            // When the page is loaded for 
            //the first time, the controller should generate a random number between 1 and 100, that it will
            //save in a session so it remembers it for the page even if it is refreshed.
            Random rand = new Random();
            string randomNumber = rand.Next(1, 100).ToString();

            HttpContext.Session.SetString("Number", randomNumber);
            HttpContext.Session.SetString("Near", "");
            HttpContext.Session.SetString("checkMatch", "");

            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("Attempts", "0", cookieOptions);

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/GuessingGame")]
        public ActionResult Index(GuessingGame model)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(1);
            //If it is the correct one, you should get a message congratulating you on 
            //your success, and a new number should be generated.
            if (model.Number == Convert.ToInt32(HttpContext.Session.GetString("Number")))
            {
                Response.Cookies.Delete("Attemps");
                Response.Cookies.Append("Attemps", "0", cookieOptions);

                HttpContext.Session.SetString("Near", "");
                HttpContext.Session.SetString("checkMatch", "Congratulations ! you win ... please try again");

                // if result success than  again number generate 1 to 100 
                Random rand = new Random();
                string randomNumber = rand.Next(1, 100).ToString();
                HttpContext.Session.SetString("Number", randomNumber);

            }
            else
            {
                int c = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Cookies["Attemps"]) + 1;
                Response.Cookies.Delete("Attemps");

                Response.Cookies.Append("Attemps", c.ToString(), cookieOptions);

                int Difference = 0;
                string msg = "";
                if (model.Number > Convert.ToInt32(HttpContext.Session.GetString("Number")))
                {
                    Difference = model.Number - int.Parse(HttpContext.Session.GetString("Number"));
                    msg = " Number Is Too High." + " Difference: " + Difference.ToString();
                    HttpContext.Session.SetString("checkMatch", "");
                }
                else
                {
                    Difference = Convert.ToInt32(HttpContext.Session.GetString("Number")) - model.Number;
                    msg = " Number Is Too Low." + " Difference: " + Difference.ToString();
                    HttpContext.Session.SetString("checkMatch", "");
                }
                HttpContext.Session.SetString("Near", msg);

            }


            return View();
        }
    }
}
