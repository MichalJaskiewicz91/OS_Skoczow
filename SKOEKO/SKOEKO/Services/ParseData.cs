using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace SKOEKO.Services
{
    public class ParseData
    {
        string unit = "m3";

        /// <summary>
        /// A method that parse data for daily raport exported to CSV
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public StringWriter ParseDayDataCSV(SqlDataReader reader)
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"ID\",\"Data\",\"Max Wartosc\",\"Min Wartosc\",\"Srednia Wartosc\",\"Ilosc\",\"Jednostka\"");
            //Read the reader
            while (reader.Read())
            {
                // Parse the data
                DateTime data = reader.GetDateTime(4);
                string Max = reader.GetString(0);
                string Min = reader.GetString(1);
                string Sr = reader.GetString(2);
                string Ilosc = reader.GetString(3);
                int Id = reader.GetInt32(5);

                DateTime parsedData = data.AddDays(-1);
                String reparsedData = parsedData.ToString("yyyy-MM-dd");
                float parsedMax = float.Parse(Max);
                float parsedMin = float.Parse(Min);
                float parsedSr = float.Parse(Sr);
                float parsedIlosc = float.Parse(Ilosc);

                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\"", Id, reparsedData, parsedMax, parsedMin, parsedSr, parsedIlosc,unit));  
            }
            return sw;
        }

        /// <summary>
        /// A method that parse data for hourly raport exported to CSV
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public StringWriter ParseHourDataCSV(SqlDataReader reader)
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"ID\",\"Data\",\"Wartosc\",\"Jednostka\"");

            //Read the reader
            while (reader.Read())
            {
                // Get the data
                string Wartosc = reader.GetString(0);
                DateTime Data = reader.GetDateTime(1);
                int Id = reader.GetInt32(2);

                // Parse the data
                DateTime parsedData = Data.AddHours(-1).AddMinutes(-15);
                DateTime firstParse = parsedData.AddHours(+1);
                String secondParse = parsedData.ToString("yyyy-MM-dd HH:mm:ss");
                String thirdParse = firstParse.ToString("HH:mm:ss");
                String all = secondParse + "-" + thirdParse;
                float parsedWartosc = float.Parse(Wartosc);

                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",Id, all, parsedWartosc, unit));
            }
            return sw;
        }

        /// <summary>
        /// A method that parse data for daily raport exported to excel
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DataTable ParseDayDataExcel(SqlDataReader reader)
        {
            // Create datatable
            DataTable datatable = new DataTable();

            // Create columns
            DataColumn idColumn = new DataColumn("ID", typeof(Int32));
            DataColumn dataColumn = new DataColumn("Data", typeof(String));
            DataColumn maxColumn = new DataColumn("Max Wartość", typeof(float));
            DataColumn minColumn = new DataColumn("Min Wartość", typeof(float));
            DataColumn sredniaColumn = new DataColumn("Średnia Wartość", typeof(float));
            DataColumn iloscColumn = new DataColumn("Ilość", typeof(float));
            DataColumn jednostkaColumn = new DataColumn("Jednostka", typeof(String));

            // Add columns do datatable
            datatable.Columns.Add(idColumn);
            datatable.Columns.Add(dataColumn);
            datatable.Columns.Add(maxColumn);
            datatable.Columns.Add(minColumn);
            datatable.Columns.Add(sredniaColumn);
            datatable.Columns.Add(iloscColumn);
            datatable.Columns.Add(jednostkaColumn);

            // Add a row
            DataRow dataRow;

            // Read the reader
            while (reader.Read())
            {

                // Get the data
                DateTime data = reader.GetDateTime(4);
                string Max = reader.GetString(0);
                string Min = reader.GetString(1);
                string Sr = reader.GetString(2);
                string Ilosc = reader.GetString(3);
                int Id = reader.GetInt32(5);

                // Parse the data
                DateTime parsedData = data.AddDays(-1);
                String reparsedData = parsedData.ToString("yyyy-MM-dd");
                float parsedMax = float.Parse(Max);
                float parsedMin = float.Parse(Min);
                float parsedSr = float.Parse(Sr);
                float parsedIlosc = float.Parse(Ilosc);

                // Assign data to the row
                dataRow = datatable.NewRow();
                dataRow["ID"] = Id;
                dataRow["Data"] = reparsedData;
                dataRow["Max Wartość"] = parsedMax;
                dataRow["Min Wartość"] = parsedMin;
                dataRow["Średnia Wartość"] = parsedSr;
                dataRow["Ilość"] = parsedIlosc;
                dataRow["Jednostka"] = unit;

                // Add row to the datatable
                datatable.Rows.Add(dataRow);

            }
            return datatable;
        }
        /// <summary>
        /// A method that parse data for hourly raport exported to excel
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DataTable ParseHourDataExcel(SqlDataReader reader)
        {
            // Create datatable
            DataTable datatable = new DataTable();

            // Create columns
            DataColumn idColumn = new DataColumn("ID", typeof(Int32));
            DataColumn dataColumn = new DataColumn("Data", typeof(String));
            DataColumn wartoscColumn = new DataColumn("Wartość", typeof(float));
            DataColumn jednostkaColumn = new DataColumn("Jednostka", typeof(String));

            // Add columns do datatable
            datatable.Columns.Add(idColumn);
            datatable.Columns.Add(dataColumn);
            datatable.Columns.Add(wartoscColumn);
            datatable.Columns.Add(jednostkaColumn);

            // Add a row
            DataRow dataRow;

            // Read the reader
            while (reader.Read())
            {

                // Get the data
                string Wartosc = reader.GetString(0);
                DateTime Data = reader.GetDateTime(1);
                int Id = reader.GetInt32(2);

                // Parse the data
                DateTime parsedData = Data.AddHours(-1).AddMinutes(-15);
                DateTime firstParse = parsedData.AddHours(+1);
                String secondParse = parsedData.ToString("yyyy-MM-dd HH:mm:ss");
                String thirdParse = firstParse.ToString("HH:mm:ss");
                String all = secondParse + "-" + thirdParse;
                float parsedWartosc = float.Parse(Wartosc);

                // Assign data to the row
                dataRow = datatable.NewRow();
                dataRow["ID"] = Id;
                dataRow["Data"] = all;
                dataRow["Wartość"] = parsedWartosc;
                dataRow["Jednostka"] = unit;

                // Add row to the datatable
                datatable.Rows.Add(dataRow);

            }
            return datatable;
        }
    }
}