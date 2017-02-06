using HowLeakyModels._Parameters;
using HowLeakyModels.DHMCoreLib.ErrorHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;


namespace HowLeakyModels._Custom
{
[Table("SyncModels")]
    public abstract class CustomSyncModel
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }//Going to use strings here - as User Account could be deleted, and we would like to preserve who did this.

        [Required]
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; } //Going to use strings here - as User Account could be deleted, and we would like to preserve who did this.

        [Required]
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Comments { get; set; }
        public bool Published { get; set; }


        protected float ExtractFloatFromNode(XmlNode header, string key)
        {
            try
            {
                if (header != null && String.IsNullOrEmpty(key) == false)
                {
                    XmlNode node = header.SelectSingleNode(key);
                    if (node != null)
                    {
                        float temp;
                        if (float.TryParse(node.InnerText, out temp))
                            return temp;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.HandleError(ex, "", true);
            }
            return 0;

        }

        protected int ExtractIntFromNode(XmlNode header, string key)
        {
            try
            {
                if (header != null && String.IsNullOrEmpty(key) == false)
                {
                    XmlNode node = header.SelectSingleNode(key);
                    if (node != null)
                    {
                        int temp;
                        if (Int32.TryParse(node.InnerText, out temp))
                            return temp;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.HandleError(ex, "", true);
            }
            return 0;

        }

        protected ParameterModel Initialise(ApplicationDbContext.ApplicationDbContext db, List<int> intarray, string comment, string source)
        {
            var newparameter = new ParameterModel(ParameterModelType.IntVector) { Id = Guid.NewGuid() };
            if (intarray != null)
            {
                newparameter.Initialise(db, intarray, comment, source);
            }
            return newparameter;
        }

        protected ParameterModel Initialise(ApplicationDbContext.ApplicationDbContext db, List<float> floatarray, string comment, string source)
        {
            var newparameter = new ParameterModel(ParameterModelType.IntVector) { Id = Guid.NewGuid() };
            if (floatarray != null)
            {
                newparameter.Initialise(db, floatarray, comment, source);
            }
            return newparameter;
        }

        protected ParameterModel Initialise(ApplicationDbContext.ApplicationDbContext db, int intvalue, string comment, string source)
        {
            var newparameter = new ParameterModel(ParameterModelType.Int) { Id = Guid.NewGuid() };
            newparameter.Initialise(db, intvalue, comment, source);
            return newparameter;
        }

        protected ParameterModel InitialiseFromXMLNode(ApplicationDbContext.ApplicationDbContext db, XmlNode header, string comment, ParameterModelType type, string source)
        {
            var newparameter = new ParameterModel(type) { Id = Guid.NewGuid() };
            newparameter.InitialiseFromXMLNode(db, header, comment, source);
            return newparameter;
        }
    }
}