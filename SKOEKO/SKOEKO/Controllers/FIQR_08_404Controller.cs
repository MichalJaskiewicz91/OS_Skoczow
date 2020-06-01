using SKOEKO.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.IO;

namespace SKOEKO.Controllers
{
    public class FIQR_08_404Controller : Controller
    {
        // GET: FIQR_08_404
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FIQR_08_404_searchDay()
        {
            return View();
        }
        public ActionResult FIQR_08_404_searchHour()
        {
            return View();
        }
        public ActionResult FIQR_08_404_saveSearchDay()
        {
            return View();
        }
        public ActionResult FIQR_08_404_saveSearchHour()
        {
            return View();
        }

         [HttpPost]
        public ActionResult FIQR_08_404_resultDay(Search find)
        {

            var od = find.from;
            var doo = find.to;
            var timeSt = find.timeStart;
            var timeEn = find.timeEnd;

            DateTime dataOd = DateTime.Parse(od);   // Dodanie dnia do daty
            DateTime parsedDataOdDay = dataOd.AddDays(+1);
            String reparsedDataOd = parsedDataOdDay.ToString("yyyy-M-d");


            DateTime dataEnd = DateTime.Parse(doo);     // Dodanie dnia do daty
            DateTime parsedDataEndDay = dataEnd.AddDays(+1);
            String reparsedDataEnd = parsedDataEndDay.ToString("yyyy-M-d");



            String odDateTime = reparsedDataOd + " " + timeSt;
            String endDateTime = reparsedDataEnd + " " + timeEn;

            String stm = "SELECT * FROM [dbo].[FIQR_08_404_Doba] WHERE Data > '" + odDateTime + "' AND Data < '" + endDateTime + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            ViewBag.reader = reader;
            return View();

        }
        [HttpPost]
        public ActionResult FIQR_08_404_resultHour(Search find)
        {

            var od = find.from;
            var doo = find.to;
            var timeSt = find.timeStart;
            var timeEn = find.timeEnd;


            DateTime timeOd = DateTime.Parse(timeSt);   // Dodanie dnia do daty
            DateTime parsedTimeOdDay = timeOd.AddHours(+1);
            String reparsedTimeOd = parsedTimeOdDay.ToString("HH:mm:ss");


            String endDateTime = doo + " " + timeEn;
            DateTime dateTimeEnd = DateTime.Parse(endDateTime);   // Dodanie dnia do daty
            DateTime parsedTimeEndDay = dateTimeEnd.AddHours(+1);
            String reparsedTimeEnd = parsedTimeEndDay.ToString("yyyy-MM-dd HH:mm:ss");

            String odDateTime = od + " " + reparsedTimeOd;
            

            String stm = "SELECT * FROM [dbo].[FIQR_08_404] WHERE Data > '" + odDateTime + "' AND Data < '" + reparsedTimeEnd + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            ViewBag.reader = reader;
            return View();

        }

        [HttpPost]
        public ActionResult FIQR_08_404_saveResultDay(Search find)
        {

            var od = find.from;
            var doo = find.to;
            var timeSt = find.timeStart;
            var timeEn = find.timeEnd;

            DateTime dataOd = DateTime.Parse(od);   // Dodanie dnia do daty
            DateTime parsedDataOdDay = dataOd.AddDays(+1);
            String reparsedDataOd = parsedDataOdDay.ToString("yyyy-M-d");
            

            DateTime dataEnd = DateTime.Parse(doo);     // Dodanie dnia do daty
            DateTime parsedDataEndDay = dataEnd.AddDays(+1);
            String reparsedDataEnd = parsedDataEndDay.ToString("yyyy-M-d");



            String odDateTime = reparsedDataOd + " " + timeSt;
            String endDateTime = reparsedDataEnd + " " + timeEn;


            String stm = "SELECT * FROM [dbo].[FIQR_08_404_Doba] WHERE Data > '" + odDateTime + "' AND Data < '" + endDateTime + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            ViewBag.reader = reader;
            return View();

        }
        [HttpPost]
        public ActionResult FIQR_08_404_saveResultHour(Search find)
        {

            var od = find.from;
            var doo = find.to;
            var timeSt = find.timeStart;
            var timeEn = find.timeEnd;


            DateTime timeOd = DateTime.Parse(timeSt);   // Dodanie dnia do daty
            DateTime parsedTimeOdDay = timeOd.AddHours(+1);
            String reparsedTimeOd = parsedTimeOdDay.ToString("HH:mm:ss");


            String endDateTime = doo + " " + timeEn;
            DateTime dateTimeEnd = DateTime.Parse(endDateTime);   // Dodanie dnia do daty
            DateTime parsedTimeEndDay = dateTimeEnd.AddHours(+1);
            String reparsedTimeEnd = parsedTimeEndDay.ToString("yyyy-MM-dd HH:mm:ss");

            String odDateTime = od + " " + reparsedTimeOd;

            String stm = "SELECT * FROM [dbo].[FIQR_08_404] WHERE Data > '" + odDateTime + "' AND Data < '" + reparsedTimeEnd + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            ViewBag.reader = reader;
            return View();

        }
    }
}