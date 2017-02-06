using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Encoding = System.Text.Encoding;
using System.Globalization;
using HowLeakyModels.Accounts;

namespace HowLeakyModels.AppData
{
    public class ClimateDataModel
    {
        public ClimateDataModel()
        {
            
        }

        public ClimateDataModel(ApplicationDbContext.ApplicationDbContext db, System.IO.Stream stream, string filename, ApplicationUser appUser)
        {
            LoadMetaData(db, stream, filename);
            Id = Guid.NewGuid();
            FileName = filename;
            ImportedBy = appUser.UserName;
            ImportedDate = DateTime.UtcNow;           
        }
        
        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public String Name { get; set; }
       
        public String StationCode { get; set; }

        public string Country { get; set; }
        public String State { get; set; }
       
        public float Latitude { get; set; }
       
        public float Longitude { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }

        public string ImportedBy { get; set; }

        public DateTime?ImportedDate { get; set; }

        public string Comments { get; set; }

        public string FileName { get; set; }

        private void LoadMetaData(ApplicationDbContext.ApplicationDbContext db, System.IO.Stream stream, string filename)
        {
            Name = filename.Replace(".p51", "");

            stream.Position = 0;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line;
                bool foundheader = false;
                bool readfirstdata = false;
                string lastline;
                while ((line = reader.ReadLine()) != null)
                {
                    if (foundheader == false)
                    {
                        if (line.Contains("date") && line.Contains("jday"))
                        {
                            foundheader = true;
                        }
                        else
                        {
                            var items = line.Split(' ');
                            if (items.Count() > 8)
                            {
                                float lat;
                                float lon;
                                if (float.TryParse(items[0], out lat))
                                {
                                    Latitude = lat;
                                }
                                if (float.TryParse(items[1], out lon))
                                {
                                    Latitude = lon;
                                }
                                Comments = line;
                            }
                        }
                    }
                    else
                    {
                        var items = line.Split('\t');
                        if (!readfirstdata)
                        {
                            if (items.Count() == 8)
                            {

                                var date = TryParseDate(items[0]);
                                if (date != null)
                                {
                                    StartDate = date;
                                    readfirstdata = true;
                                }

                            }
                        }
                        else
                        {
                            if (items.Count() == 8)
                            {

                                var date = TryParseDate(items[0]);
                                if (date != null)
                                {
                                    EndDate = date;
                                }

                            }
                        }
                    }

                    lastline = line;
                }
            }
        }

        private DateTime? TryParseDate(string text)
        {
            if (text.Length == 8)
            {
                DateTime dateTime;
                if (DateTime.TryParseExact(text, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out dateTime))
                {
                    return dateTime;

                }
            }
            return null;
        }
    }

}