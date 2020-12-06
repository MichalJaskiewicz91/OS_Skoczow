using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace SKOEKO.Services
{
    public class SaveToFile
    {
        string unit = "m3";
        public StringWriter SaveToCSV(SqlDataReader reader)
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"ID\"\"Data\",\"Max Wartosc\",\"Min Wartosc\",\"Srednia Wartosc\",\"Ilosc\",\"Jednostka\"");
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
        public DataTable SaveToExcel(SqlDataReader reader)
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
    }
}