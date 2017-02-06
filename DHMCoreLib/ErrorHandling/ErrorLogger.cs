//Copyright 2014, DHM Environmental Software Engineering Pty. Ltd
//The Copyright of this file belongs to DHM Environmental Software Engineering Pty. Ltd. (hereafter known as DHM) and selected clients of DHM.
//No content of this file may be reproduced, modified or used in software development without the express written permission from DHM.
//Where permission has been granted to use or modify this file, the full copyright information must remain unchanged at the top of the file.
//Where permission has been granted to modify this file, changes must be clearly identified through adding comments and annotations to the source-code,
//and a description of the changes (including who has made the changes), must be included after this copyright information.

using HowLeakyModels.Accounts;
using HowLeakyModels.Analytics;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HowLeakyModels.DHMCoreLib.ErrorHandling

{
    public class ErrorLogger
    {
        static readonly object newlock = new object();
        public static void HandleError(Exception ex, String title, bool ShowToUser)
        {
            try
            {
                lock (newlock)
                {
                    String LogText = GenerateMessage(ex, title);
                    System.Diagnostics.Debug.WriteLine(LogText);

                    ApplicationDbContext.ApplicationDbContext db = new ApplicationDbContext.ApplicationDbContext();

                    ApplicationUser user = null;
                    String username = HttpContext.Current.User.Identity.Name;
                    if (String.IsNullOrEmpty(username) == false)
                        user = db.Users.SingleOrDefault(u => u.UserName == username);

                    AnalyticsErrorRecord record = new AnalyticsErrorRecord();
                    record.AspNetUser=user;
                    record.timeStamp=DateTime.UtcNow;
                    record.deviceType="Website";
                    record.connectionType="Online";
                    record.appVersionNumber="N.A.";
                    record.iOSVersionNumber="N.A.";
                    record.deviceID="N.A.";
                    record.exceptionName=ex.Message;
                    record.exceptionReason=ex.ToString();
                    record.userInfo="";
                    record.stackSymbols=ex.StackTrace;
                    record.stackReturnAddresses="";
                    record.developerMsg = title;
                    //db.ErrorLogs.Add(record);
                    db.SaveChanges();

                    // SaveToLogFile(LogText);
                    //if (ShowToUser)
                    //    MessageBox.Show(LogText, "An error has occurred", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
            catch (Exception ex2)
            {
                //SaveToLogFile(ex2.ToString());
                //MessageBox.Show(ex2.ToString(), "Error logger crashed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private static string GenerateMessage(Exception ex, string title)
        {
            try
            {
                String msg = "";
                String versioninfo = GetVersionInfo();

                if (String.IsNullOrEmpty(title) == false)
                    msg = String.Format("Title: {0}\r\n", title);
                if (ex != null)
                {
                    msg = String.Format("{0}Message: {1}\r\n", msg, ex.Message);
                    msg = String.Format("{0}Version: {1}\r\n", msg, versioninfo);
                    msg = String.Format("{0}Source: {1}\r\n", msg, ex.Source);
                    msg = String.Format("{0}Target Site: {1}\r\n", msg, ex.TargetSite);
                    msg = String.Format("{0}StackTrace: {1}\r\n", msg, ex.StackTrace);
                    msg = String.Format("{0}Inner Exception: {1}\r\n", msg, ex.InnerException);
                    msg = String.Format("{0}Data: {1}\r\n", msg, ex.Data);
                }
                msg = String.Format("{0}Date/Time: {1}\r\n", msg, DateTime.Now);
                return msg;
            }
            catch (Exception ex2)
            {
                return ex2.ToString();
            }
           // return "";
        }

        private static string GetVersionInfo()
        {
            try
            {
                Assembly assem = Assembly.GetEntryAssembly();
                if (assem != null)
                {
                    AssemblyName assemname = assem.GetName();
                    if (assemname != null)
                    {
                        //Version version = assemname.Version;
                        return String.Format("{0} V{1}", assemname.Name, assemname.Version.ToString());
                    }
                }
            }
            catch (Exception ex2)
            {
                return ex2.ToString();
            }
            return "No Version Information Found";
        }

        private static void SaveToLogFile(String LogText)
        {
            try
            {
                
                    String basedirectory = HttpContext.Current.Server.MapPath(String.Format("~/App_Data"));
				
					FileStream fs = new FileStream(basedirectory + "/ErrorLog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
					StreamWriter s = new StreamWriter(fs);
					s.Close();
					fs.Close();
					FileStream fs1 = new FileStream(basedirectory + "/ErrorLog.txt", FileMode.Append, FileAccess.Write);
					StreamWriter s1 = new StreamWriter(fs1);
					s1.Write(LogText);
					s1.Write("===========================================================================================\r\n");
					s1.Close();
					fs1.Close();
				
            }
            catch
            {
                //MessageBox.Show("An error has occurred and then whoops... the error logger also crashed when trying to save the log file.", "Error logger Problem", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
    }
}
