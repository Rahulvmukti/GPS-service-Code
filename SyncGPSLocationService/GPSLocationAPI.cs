using exam.Models;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace SyncGPSLocationService
{
    public class GPSLocationAPI
    {
        private string ConnStr = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();

        public void SaveGPSVehicleDetails()
        {
            try
            {
                VehicleData apiResponse = GetVehicleDataFromApi();
                if (apiResponse != null && apiResponse.Root != null && apiResponse.Root.VehicleData != null)
                {
                    List<VehicleDataItem> vehicleDataList = apiResponse.Root.VehicleData;
                    SaveDataToSqlServer(vehicleDataList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public VehicleData GetVehicleDataFromApi()
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gpstrackURL"].ToString());
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var tokenno = ConfigurationManager.AppSettings["tokenno"].ToString();
                    client.DefaultRequestHeaders.Add("Authorization", tokenno);
                    HttpResponseMessage response = client.GetAsync("api/v1/positions").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string res = response.Content.ReadAsStringAsync().Result;
                        VehicleData oresponse = JsonConvert.DeserializeObject<VehicleData>(res);
                        stopWatch.Stop();
                        long tm = stopWatch.ElapsedMilliseconds;
                        return oresponse;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SaveDataToSqlServer(List<VehicleDataItem> vehicleDataList)
        {
            try
            {
                //Stopwatch stopWatch = new Stopwatch();
                //stopWatch.Start();
                //long tm2;
                string connectionString = ConnStr;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (var vehicleData in vehicleDataList)
                    {
                        //Stopwatch stopWatch2 = new Stopwatch();
                        //stopWatch2.Start();
                        InsertIntoSql(connection, vehicleData);
                        //stopWatch2.Stop();
                        //tm2 = stopWatch2.ElapsedMilliseconds;

                    }
                }
                //stopWatch.Stop();
                //long tm = stopWatch.ElapsedMilliseconds;
                Console.WriteLine("Data inserted into SQL Server successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving data to SQL Server: {ex.Message}");
            }
        }

        private void InsertIntoSql(SqlConnection connection, VehicleDataItem item)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand("InsertVehicleGpsData", connection))
                {
                    string DatetimeDt = "";
                    string GPSActualTimeDt = "";
                    DateTime.TryParseExact(item.Datetime, "dd-MM-yyyy HH:mm:ss",
                        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime odt);
                    DateTime.TryParseExact(item.GPSActualTime, "dd-MM-yyyy HH:mm:ss",
                        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime odt2);

                    if (odt == DateTime.MinValue)
                    {
                        odt = new DateTime(1900, 1, 1);
                    }
                    if (odt2 == DateTime.MinValue)
                    {
                        odt2 = new DateTime(1900, 1, 1);
                    }
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // Assuming your stored procedure parameters match the properties of the VehicleDataItem class
                    sqlCommand.Parameters.AddWithValue("@Company", item.Company);
                    sqlCommand.Parameters.AddWithValue("@Branch", item.Branch);
                    sqlCommand.Parameters.AddWithValue("@Vehicle_No", item.Vehicle_No);
                    sqlCommand.Parameters.AddWithValue("@Vehicle_Name", item.Vehicle_Name);
                    sqlCommand.Parameters.AddWithValue("@Vehicletype", item.Vehicletype);
                    sqlCommand.Parameters.AddWithValue("@Driver_First_Name", item.Driver_First_Name);
                    sqlCommand.Parameters.AddWithValue("@Driver_Middle_Name", item.Driver_Middle_Name);
                    sqlCommand.Parameters.AddWithValue("@Driver_Last_Name", item.Driver_Last_Name);
                    sqlCommand.Parameters.AddWithValue("@Imeino", item.Imeino);
                    sqlCommand.Parameters.AddWithValue("@DeviceModel", item.DeviceModel);
                    sqlCommand.Parameters.AddWithValue("@Location", item.Location);
                    sqlCommand.Parameters.AddWithValue("@POI", item.POI);
                    sqlCommand.Parameters.AddWithValue("@Datetime", odt);
                    sqlCommand.Parameters.AddWithValue("@GPSActualTime", odt2);
                    sqlCommand.Parameters.AddWithValue("@Latitude", item.Latitude.Replace("-", "0"));
                    sqlCommand.Parameters.AddWithValue("@Longitude", item.Longitude.Replace("-", "0"));
                    sqlCommand.Parameters.AddWithValue("@Status", item.Status);
                    sqlCommand.Parameters.AddWithValue("@Speed", item.Speed);
                    sqlCommand.Parameters.AddWithValue("@GPS", item.GPS);
                    sqlCommand.Parameters.AddWithValue("@Angle", item.Angle);
                    sqlCommand.Parameters.AddWithValue("@IGN", item.IGN.Replace("-", "0"));
                    sqlCommand.Parameters.AddWithValue("@Power", item.Power.Replace("-", "0"));
                    sqlCommand.Parameters.AddWithValue("@Door1", item.Door1);
                    sqlCommand.Parameters.AddWithValue("@Door2", item.Door2);
                    sqlCommand.Parameters.AddWithValue("@Door3", item.Door3);
                    sqlCommand.Parameters.AddWithValue("@Door4", item.Door4);
                    sqlCommand.Parameters.AddWithValue("@AC", item.AC);
                    sqlCommand.Parameters.AddWithValue("@Temperature", item.Temperature);
                    sqlCommand.Parameters.AddWithValue("@Fuel", item.Fuel);
                    sqlCommand.Parameters.AddWithValue("@SOS", item.SOS);
                    sqlCommand.Parameters.AddWithValue("@Distance", item.Distance.Replace("-", "0"));
                    sqlCommand.Parameters.AddWithValue("@Odometer", item.Odometer.Replace("-", "0"));

                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data into SQL Server: {ex.Message}");
                throw;
            }
        }
    }
}
