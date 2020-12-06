﻿using SKOEKO.Models;
using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.IO;
using SKOEKO.Services;
using ClosedXML.Excel;
using System.Data;

namespace SKOEKO.Controllers
{
    public class FQR_4_1Controller : Controller
    {
        private static string quantityDay = "FQR_4_1_Raport_Dobowy";
        private static string quantityHour = "FQR_4_1_Raport_Godzinowy";
        SaveToFile saveToFile = new SaveToFile();
        StringWriter stringWriter = new StringWriter();
        DataTable dataTable = new DataTable();
        string dateToRaport;


        public ActionResult FQR_4_1_searchDay()
        {
            return View();
        }
        public ActionResult FQR_4_1_searchHour()
        {
            return View();
        }
        public ActionResult FQR_4_1_saveSearchDay()
        {
            return View();
        }
        public ActionResult FQR_4_1_saveSearchHour()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FQR_4_1_resultDay(Search find)
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

            String stm = "SELECT * FROM [dbo].[FQR_4_1_Doba] WHERE Data > '" + odDateTime + "' AND Data < '" + endDateTime + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            ViewBag.reader = reader;
            return View();

        }
        [HttpPost]
        public ActionResult FQR_4_1_resultHour(Search find)
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
            

            String stm = "SELECT * FROM [dbo].[FQR_4_1] WHERE Data > '" + odDateTime + "' AND Data < '" + reparsedTimeEnd + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            ViewBag.reader = reader;
            return View();

        }

        [HttpPost]
        public void FQR_4_1_saveResultDay(Search find, string sumbit)
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
            dateToRaport = quantityDay+"_"+ od + "-" + doo;

            String odDateTime = reparsedDataOd + " " + timeSt;
            String endDateTime = reparsedDataEnd + " " + timeEn;


            String stm = "SELECT * FROM [dbo].[FQR_4_1_Doba] WHERE Data > '" + odDateTime + "' AND Data < '" + endDateTime + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");

            // Database for debugging
            //SqlConnection conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Citect;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            // Distinguish whether save to CSV or Excel
            if (sumbit == "Zapisz do CSV")
            {
                stringWriter = saveToFile.SaveToCSV(reader);

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename="+dateToRaport+".csv");
                Response.ContentType = "text/csv";

                Response.Write(stringWriter.ToString());
                Response.End();
            }
            else
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    dataTable = saveToFile.SaveToExcel(reader);

                    wb.Worksheets.Add(dataTable, "Customers");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.speadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename="+dateToRaport+".xlsx");
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
        public void FQR_4_1_saveResultHour(Search find, string sumbit)
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
            dateToRaport = quantityHour + "_" + od + "_" + timeSt + "-" + doo + timeEn;

            String odDateTime = od + " " + reparsedTimeOd;

            String stm = "SELECT * FROM [dbo].[FQR_4_1] WHERE Data > '" + odDateTime + "' AND Data < '" + reparsedTimeEnd + "'ORDER BY Data ASC";

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Citect;Integrated Security=true");
            conn.Open();
            SqlCommand cmd = new SqlCommand(stm, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            // Distinguish whether save to CSV or Excel
            if (sumbit == "Zapisz do CSV")
            {
                stringWriter = saveToFile.SaveToCSV(reader);

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
                    dataTable = saveToFile.SaveToExcel(reader);

                    wb.Worksheets.Add(dataTable, "Customers");
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

    }
}