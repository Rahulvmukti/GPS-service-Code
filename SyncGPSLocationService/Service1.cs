using exam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SyncGPSLocationService
{
    public partial class Service1 : ServiceBase
    {
        Timer checkForTime;
        public Service1()
        {
            InitializeComponent();
            try
            {
                GPSLocationAPI oAPI = new GPSLocationAPI(); 
                SaveGPSVehicleDetails();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        void checkForTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                SaveGPSVehicleDetails();
            }
            catch (Exception ex)
            {
                Logs.InsertLog(ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Logs.InsertLog("Service Started"); 
                double interval60Minutes = Convert.ToInt32(ConfigurationManager.AppSettings["TimeInterval"]) * 1000; // milliseconds
                Logs.InsertLog("Time Interval is set to " + interval60Minutes.ToString()); 
                Timer checkForTime = new Timer(interval60Minutes);
                checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
                checkForTime.Enabled = true; 
                SaveGPSVehicleDetails();
            }
            catch (Exception ex)
            {
                Logs.InsertLog("Service OnStart Error: " + ex.Message);
            }
        }

        protected override void OnStop()
        {
            Logs.InsertLog("Service Stopped");
        }

        protected void SaveGPSVehicleDetails()
        {
            GPSLocationAPI oAPI = new GPSLocationAPI();
            try
            {
                        VehicleData oresp = oAPI.GetVehicleDataFromApi();
                        if (oresp != null && oresp.Root != null && oresp.Root.VehicleData != null)
                        {
                            List<VehicleDataItem> vehicleDataList = oresp.Root.VehicleData;
                    //foreach (var vehicleData in vehicleDataList)
                    //{ 
                    //    oAPI.SaveDataToSqlServer(vehicleDataList);
                    //}
                    oAPI.SaveDataToSqlServer(vehicleDataList);
                } 
            }
            catch (Exception ex)
            {
                Logs.InsertLog("SaveGPSVehicleDetails() Error: " + ex.Message);
            }
        }

    }
}
