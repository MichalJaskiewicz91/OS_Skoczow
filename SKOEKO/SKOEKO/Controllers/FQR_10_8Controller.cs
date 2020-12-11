using SKOEKO.Models;
using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.IO;
using SKOEKO.Services;
using ClosedXML.Excel;
using System.Data;
using SKOEKO.Helpers;

namespace SKOEKO.Controllers
{
    public class FQR_10_8Controller : Controller
    {
        private static string quantityDay = "FQR_10_8_Raport_Dobowy";
        private static string quantityHour = "FQR_10_8_Raport_Godzinowy";
        private static string quantityMonth = "FQR_10_8_Raport_Miesięczny";
        ParseData parseData = new ParseData();
        StringWriter stringWriter = new StringWriter();
        DataTable dataTable = new DataTable();
        private DateTime monthYear;
        private DateTime nextMonthYear;
        string dateToRaport;
        private int month;
        private int year;
        private int nextMonth;
        private int nextYear;
        private int passedYearToQuery;




        public ActionResult FQR_10_8_searchDay()
        {
            return View();
        }
        public ActionResult FQR_10_8_searchHour()
        {
            return View();
        }
        public ActionResult FQR_10_8_saveSearchDay()
        {
            return View();
        }
        public ActionResult FQR_10_8_saveSearchHour()
        {
            return View();
        }
        public ActionResult FQR_10_8_searchMonth()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FQR_10_8_resultDay(Search find)
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

            String stm = "SELECT * FROM [dbo].[FQR_10_8_Doba] WHERE Data > '" + odDateTime + "' AND Data < '" + endDateTime + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            ViewBag.reader = reader;
            return View();

        }
        [HttpPost]
        public ActionResult FQR_10_8_resultHour(Search find)
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


            String stm = "SELECT * FROM [dbo].[FQR_10_8] WHERE Data > '" + odDateTime + "' AND Data < '" + reparsedTimeEnd + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            ViewBag.reader = reader;
            return View();

        }

        [HttpPost]
        public void FQR_10_8_saveResultDay(Search find, string sumbit)
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

            // Create date to raport
            dateToRaport = quantityDay + "_" + od + "-" + doo;

            String odDateTime = reparsedDataOd + " " + timeSt;
            String endDateTime = reparsedDataEnd + " " + timeEn;


            String stm = "SELECT * FROM [dbo].[FQR_10_8_Doba] WHERE Data > '" + odDateTime + "' AND Data < '" + endDateTime + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");

            // Database for debugging
            //SqlConnection conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Citect;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            // Distinguish whether save to CSV or Excel
            if (sumbit == "Zapisz do CSV")
            {
                stringWriter = parseData.ParseDayDataCSV(reader);

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=" + dateToRaport + ".csv");
                Response.ContentType = "text/csv";

                Response.Write(stringWriter.ToString());
                Response.End();
            }
            else
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dataTable = parseData.ParseDayDataExcel(reader);

                    wb.Worksheets.Add(dataTable, "Report");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.speadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + dateToRaport + ".xlsx");
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
        [HttpPost]
        public void FQR_10_8_saveResultHour(Search find, string sumbit)
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

            // Create date and time to raport
            dateToRaport = quantityHour + "_" + od + "_" + timeSt + "-" + doo + "_" + timeEn;

            String odDateTime = od + " " + reparsedTimeOd;

            String stm = "SELECT * FROM [dbo].[FQR_10_8] WHERE Data > '" + odDateTime + "' AND Data < '" + reparsedTimeEnd + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            // Distinguish whether save to CSV or Excel
            if (sumbit == "Zapisz do CSV")
            {
                stringWriter = parseData.ParseHourDataCSV(reader);

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=" + dateToRaport + ".csv");
                Response.ContentType = "text/csv";

                Response.Write(stringWriter.ToString());
                Response.End();
            }
            else
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dataTable = parseData.ParseHourDataExcel(reader);

                    wb.Worksheets.Add(dataTable, "Report");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.speadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + dateToRaport + ".xlsx");
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

        }
        [HttpPost]
        public ActionResult FQR_10_8_resultMonth(Search find, string sumbit)
        {

            // parse the data
            monthYear = DateTime.Parse(find.MonthYear);

            // take out current month and year
            month = monthYear.Month;
            year = monthYear.Year;

            // next month and year
            nextMonthYear = monthYear.AddMonths(1);

            // take out next month and year
            nextMonth = nextMonthYear.Month;
            nextYear = nextMonthYear.Year;

            // Ccheck whether years are equal
            if (year == nextYear)
            {
                passedYearToQuery = year;
            }
            else
            {
                passedYearToQuery = nextYear;
            }


            // Create date to raport
            Months monthEnum = GetValues.GetEnumValue<Months>(month);
            dateToRaport = quantityMonth + "_" + monthEnum.ToString() + "_" + year;


            String stm = "SELECT * " +
                "FROM [dbo].[FQR_10_8_Doba]" +
                "WHERE (MONTH(Data) = '" + month + "' AND DAY(DATA) <> 1 AND YEAR(Data) = '" + year + "') " +
                "OR (MONTH(Data) = '" + nextMonth + "' AND DAY(DATA) = 1 AND YEAR(Data) = '" + passedYearToQuery + "')" +
                "ORDER BY Data ASC";

            //// Database for debugging
            //SqlConnection conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Citect;Integrated Security=True");

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            // Distinguish whether show the data or save to the excel
            if (sumbit == "Zapisz do Excel")
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dataTable = parseData.ParseMonthDataExcel(reader);

                    wb.Worksheets.Add(dataTable, "Report");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.speadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + dateToRaport + ".xlsx");
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                return null;
            }

            dataTable = parseData.ParseMonthDataExcel(reader);
            return View(dataTable);
        }
    }
}