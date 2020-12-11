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
        /// <summary>
        /// A method that parse data for daily raport exported to CSV
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public StringWriter ParseDayDataCSV(SqlDataReader reader)
        {
            string unit = "m3";
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
            string unit = "m3";
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
            string unit = "m3";
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
            string unit = "m3";
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
        /// <summary>
        /// A method that parse DIR data for daily raport exported to CSV
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public StringWriter ParseDayDataDirCSV(SqlDataReader reader)
        {
            string unit = "g/l";
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"ID\",\"Data\",\"Max Wartosc\",\"Min Wartosc\",\"Jednostka\"");
            //Read the reader
            while (reader.Read())
            {
                // Parse the data
                DateTime data = reader.GetDateTime(2);
                string Max = reader.GetString(0);
                string Min = reader.GetString(1);
                int Id = reader.GetInt32(3);

                DateTime parsedData = data.AddDays(-1);
                String reparsedData = parsedData.ToString("yyyy-MM-dd");
                float parsedMax = float.Parse(Max);
                float parsedMin = float.Parse(Min);


                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", Id, reparsedData, parsedMax, parsedMin, unit));
            }
            return sw;
        }
        /// <summary>
        /// A method that parse DIR data for raport exported to excel
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DataTable ParseDayDataDirExcel(SqlDataReader reader)
        {
            string unit = "g/l";
            // Create datatable
            DataTable datatable = new DataTable();

            // Create columns
            DataColumn idColumn = new DataColumn("ID", typeof(Int32));
            DataColumn dataColumn = new DataColumn("Data", typeof(String));
            DataColumn maxColumn = new DataColumn("Max Wartość", typeof(float));
            DataColumn minColumn = new DataColumn("Min Wartość", typeof(float));
            DataColumn jednostkaColumn = new DataColumn("Jednostka", typeof(String));

            // Add columns do datatable
            datatable.Columns.Add(idColumn);
            datatable.Columns.Add(dataColumn);
            datatable.Columns.Add(maxColumn);
            datatable.Columns.Add(minColumn);
            datatable.Columns.Add(jednostkaColumn);

            // Add a row
            DataRow dataRow;

            // Read the reader
            while (reader.Read())
            {

                // Get the data
                DateTime data = reader.GetDateTime(2);
                string Max = reader.GetString(0);
                string Min = reader.GetString(1);
                int Id = reader.GetInt32(3);

                // Parse the data
                DateTime parsedData = data.AddDays(-1);
                String reparsedData = parsedData.ToString("yyyy-MM-dd");
                float parsedMax = float.Parse(Max);
                float parsedMin = float.Parse(Min);


                // Assign data to the row
                dataRow = datatable.NewRow();
                dataRow["ID"] = Id;
                dataRow["Data"] = reparsedData;
                dataRow["Max Wartość"] = parsedMax;
                dataRow["Min Wartość"] = parsedMin;
                dataRow["Jednostka"] = unit;

                // Add row to the datatable
                datatable.Rows.Add(dataRow);

            }
            return datatable;
        }
        /// <summary>
        /// A method that parse DIR data for hourly daily raport exported to CSV
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public StringWriter ParseHourDataDirCSV(SqlDataReader reader)
        {
            string unit = "g/l";
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"ID\",\"Data\",\"Max Wartosc\",\"Min Wartosc\",\"Jednostka\"");
            //Read the reader
            while (reader.Read())
            {
                // Parse the data
                DateTime data = reader.GetDateTime(2);
                string Max = reader.GetString(0);
                string Min = reader.GetString(1);
                int Id = reader.GetInt32(3);

                DateTime parsedData = data.AddHours(-1).AddMinutes(-15);
                DateTime firstParse = parsedData.AddHours(+1);
                String secondParse = parsedData.ToString("yyyy-MM-dd HH:mm:ss");
                String thirdParse = firstParse.ToString("HH:mm:ss");
                String all = secondParse + "-" + thirdParse;
                float parsedMax = float.Parse(Max);
                float parsedMin = float.Parse(Min);


                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", Id, all, parsedMax, parsedMin, unit));
            }
            return sw;
        }
        /// <summary>
        /// A method that parse DIR data for hourly raport exported to excel
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DataTable ParseHourDataDirExcel(SqlDataReader reader)
        {
            string unit = "g/l";
            // Create datatable
            DataTable datatable = new DataTable();

            // Create columns
            DataColumn idColumn = new DataColumn("ID", typeof(Int32));
            DataColumn dataColumn = new DataColumn("Data", typeof(String));
            DataColumn maxColumn = new DataColumn("Max Wartość", typeof(float));
            DataColumn minColumn = new DataColumn("Min Wartość", typeof(float));
            DataColumn jednostkaColumn = new DataColumn("Jednostka", typeof(String));

            // Add columns do datatable
            datatable.Columns.Add(idColumn);
            datatable.Columns.Add(dataColumn);
            datatable.Columns.Add(maxColumn);
            datatable.Columns.Add(minColumn);
            datatable.Columns.Add(jednostkaColumn);

            // Add a row
            DataRow dataRow;

            // Read the reader
            while (reader.Read())
            {

                // Get the data
                DateTime data = reader.GetDateTime(2);
                string Max = reader.GetString(0);
                string Min = reader.GetString(1);
                int Id = reader.GetInt32(3);

                // Parse the data
                DateTime parsedData = data.AddHours(-1).AddMinutes(-15);
                DateTime firstParse = parsedData.AddHours(+1);
                String secondParse = parsedData.ToString("yyyy-MM-dd HH:mm:ss");
                String thirdParse = firstParse.ToString("HH:mm:ss");
                String all = secondParse + "-" + thirdParse;
                float parsedMax = float.Parse(Max);
                float parsedMin = float.Parse(Min);


                // Assign data to the row
                dataRow = datatable.NewRow();
                dataRow["ID"] = Id;
                dataRow["Data"] = all;
                dataRow["Max Wartość"] = parsedMax;
                dataRow["Min Wartość"] = parsedMin;
                dataRow["Jednostka"] = unit;

                // Add row to the datatable
                datatable.Rows.Add(dataRow);

            }
            return datatable;
        }
        /// <summary>
        /// A method that parse data for daily raport exported to excel
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DataTable ParseMonthDataExcel(SqlDataReader reader)
        {
            // Averages
            float averageMax = 0;
            float averageMin = 0;
            float averageSr = 0;

            // Sums
            float sumMin = 0;
            float sumMax = 0;
            float sumSr = 0;
            float sumIlosc= 0;

            // Sonstiges :)
            string unit = "m3";
            int nrDnia = 1;

            // Create datatable
            DataTable datatable = new DataTable();

            // Create columns
            DataColumn idColumn = new DataColumn("ID", typeof(Int32));
            DataColumn nrDniaColumn = new DataColumn("Nr Dnia", typeof(int));
            DataColumn dataColumn = new DataColumn("Data", typeof(String));
            DataColumn maxColumn = new DataColumn("Max Wartość", typeof(float));
            DataColumn minColumn = new DataColumn("Min Wartość", typeof(float));
            DataColumn sredniaColumn = new DataColumn("Średnia Wartość", typeof(float));
            DataColumn iloscColumn = new DataColumn("Ilość", typeof(float));
            DataColumn jednostkaColumn = new DataColumn("Jednostka", typeof(String));

            // Add columns do datatable
            datatable.Columns.Add(idColumn);
            datatable.Columns.Add(nrDniaColumn);
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

                // Calculate average and sum
                sumMin += parsedMin;
                sumMax += parsedMax;
                sumSr += parsedSr;
                sumIlosc += parsedIlosc;
                // Assign data to the row
                dataRow = datatable.NewRow();
                dataRow["ID"] = Id;
                dataRow["Nr Dnia"] = nrDnia;
                dataRow["Data"] = reparsedData;
                dataRow["Max Wartość"] = parsedMax;
                dataRow["Min Wartość"] = parsedMin;
                dataRow["Średnia Wartość"] = parsedSr;
                dataRow["Ilość"] = parsedIlosc;
                dataRow["Jednostka"] = unit;

                // Add row to the datatable
                datatable.Rows.Add(dataRow);

                // Increase a number of the day
                nrDnia++;
            }

            // Calculate the averages
            averageMax = sumMax / (nrDnia - 1);
            averageMin = sumMin / (nrDnia - 1);
            averageSr = sumSr / (nrDnia - 1);

            // Add row that consist of sum and average
            dataRow = datatable.NewRow();
            dataRow["Data"] = "Średnia/Suma";
            dataRow["Max Wartość"] = averageMax;
            dataRow["Min Wartość"] = averageMin;
            dataRow["Średnia Wartość"] = averageSr;
            dataRow["Ilość"] = sumIlosc;
            dataRow["Jednostka"] = unit;

            // Add row to the datatable
            datatable.Rows.Add(dataRow);

            return datatable;
        }
    }
}