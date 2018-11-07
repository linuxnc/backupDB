using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UploadAndDelete
{
    class Program
    {
        static void Main(string[] args)
        {
            string uploadFile = ConfigurationManager.AppSettings["uploadFile"];
            FileInfo fileInfo = new FileInfo(uploadFile);
            string ftpFile = string.Format("{0:yyyy-MM-dd hh-mm-ss}", System.DateTime.Now) + "-" + fileInfo.Name;
            string url = ConfigurationManager.AppSettings["ftpPath"] + ftpFile;

            string myuser = ConfigurationManager.AppSettings["ftpUser"];
            string mypass = ConfigurationManager.AppSettings["ftpPass"];

            //System.Security.SecureString mypass = new System.Security.SecureString();
            //mypass.AppendChar('T');
            //mypass.AppendChar('e');
            //mypass.AppendChar('s');
            //mypass.AppendChar('t');
            //mypass.AppendChar('1');
            //mypass.AppendChar('2');
            //mypass.AppendChar('3');
            //mypass.AppendChar('4');
            //mypass.AppendChar('!');
            //mypass.MakeReadOnly();
            
            if (!EventLog.SourceExists("UploadSublotDB"))
            {
                EventLog.CreateEventSource("UploadSublotDB", "sublotlog");
            }

            if (!File.Exists(uploadFile))
            {
                EventLog.WriteEntry("UploadSublotDB", uploadFile + " is not exist!", System.Diagnostics.EventLogEntryType.Warning);
                return;
            }

            WebClient webClient = new WebClient();
              
            webClient.Credentials = new NetworkCredential(myuser, mypass);
            //Console.WriteLine(uploadFile+" is uploading ... ... ");

            try
            {
                EventLog.WriteEntry("UploadSublotDB", uploadFile + " is uploading.");

                webClient.UploadFile(new Uri(url), uploadFile);

                EventLog.WriteEntry("UploadSublotDB", uploadFile +" is uploaded complete");
                File.Move(uploadFile, uploadFile.Replace(fileInfo.Name, ftpFile));
                
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("UploadSublotDB", "Upload fail.", System.Diagnostics.EventLogEntryType.Warning);
                EventLog.WriteEntry("UploadSublotDB", e.Message, System.Diagnostics.EventLogEntryType.Warning);
            }
            finally
            {
            }
             
        } 
    }
}
