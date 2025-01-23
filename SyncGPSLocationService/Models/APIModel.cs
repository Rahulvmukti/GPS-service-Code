using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exam.Models
{
    public class VehicleData
    {
        public Root Root { get; set; }
    }

    public class Root
    {
        public List<VehicleDataItem> VehicleData { get; set; }
    }

    public class VehicleDataItem
    {
        public string Company { get; set; }
        public string Branch { get; set; }
        public string Vehicle_No { get; set; }
        public string Vehicle_Name { get; set; }
        public string Vehicletype { get; set; }
        public string Driver_First_Name { get; set; }
        public string Driver_Middle_Name { get; set; }
        public string Driver_Last_Name { get; set; }
        public string Imeino { get; set; }
        public string DeviceModel { get; set; }
        public string Location { get; set; }
        public string POI { get; set; }
        public string Datetime { get; set; }
        public string GPSActualTime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Status { get; set; }
        public string Speed { get; set; }
        public string GPS { get; set; }
        public string Angle { get; set; }
        public string IGN { get; set; }
        public string Power { get; set; }
        public string Door1 { get; set; }
        public string Door2 { get; set; }
        public string Door3 { get; set; }
        public string Door4 { get; set; }
        public string AC { get; set; }
        public string Temperature { get; set; }
        public string Fuel { get; set; }
        public string SOS { get; set; }
        public string Distance { get; set; }
        public string Odometer { get; set; }
    }








}