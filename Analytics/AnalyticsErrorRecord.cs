using HowLeakyModels.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HowLeakyModels.Analytics
{
    [Table("ErrorRecords")]
    public class AnalyticsErrorRecord
    {
        public AnalyticsErrorRecord()
        {
            AspNetUser = null;
            timeStamp = DateTime.UtcNow;
            deviceType = "";
            connectionType = "";
            appVersionNumber = "";
            iOSVersionNumber = "";
            deviceID = "";
            exceptionName = "";
            exceptionReason = "";
            userInfo = "";
            stackSymbols = "";
            stackReturnAddresses = "";
            developerMsg = "";
            timeZoneOffset = 0;
        }

        [Required]
        public Guid Id { get; set; }

        public virtual ApplicationUser AspNetUser { get; set; }

        public DateTime timeStamp { get; set; }
        public string deviceType { get; set; }
        public string connectionType { get; set; }
        public string appVersionNumber { get; set; }
        public string iOSVersionNumber { get; set; }
        public string deviceID { get; set; }
        public string exceptionName { get; set; }
        public string exceptionReason { get; set; }
        public string userInfo { get; set; }
        public string stackSymbols { get; set; }
        public string stackReturnAddresses { get; set; }
        public string developerMsg { get; set; }

        public float timeZoneOffset { get; set; }
    }
}