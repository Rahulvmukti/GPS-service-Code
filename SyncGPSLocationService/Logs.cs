using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncGPSLocationService
{
    public static class Logs
    {
        public static void InsertLog(string message)
        {
            try
            {
                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"ServiceLog.log"), DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt") + " " + message + System.Environment.NewLine);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
