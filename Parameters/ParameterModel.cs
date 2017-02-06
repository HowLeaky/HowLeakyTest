using HowLeakyModels.DHMCoreLib.ErrorHandling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml;

namespace HowLeakyModels._Parameters
{
    public enum ParameterModelType { Int,Float,FloatVector,Bool,IntVector}


    [Table("ParameterModels")]
    public class ParameterModel
    {
        public ParameterModel()
        {           
            Data=new HashSet<ParameterModelElement>();
        }

        public ParameterModel( ParameterModelType type)
            : this()
        {
            ParameterType = type;
        }

        public ParameterModel(ApplicationDbContext.ApplicationDbContext db, ParameterModelType type, int defaultvalue)
            : this()
        {
            ParameterType = type;
            AddNewItem(db,defaultvalue.ToString(),  "");
        }

        public ParameterModel(ApplicationDbContext.ApplicationDbContext db, ParameterModelType type, float defaultvalue)
            : this()
        {
            ParameterType = type;
            AddNewItem(db, defaultvalue.ToString("G"), "");
        }

        public ParameterModel(ApplicationDbContext.ApplicationDbContext db, ParameterModelType type, bool defaultvalue)
            : this()
        {
            ParameterType = type;
            AddNewItem(db, defaultvalue.ToString(), "");
        }

        public ParameterModel(ApplicationDbContext.ApplicationDbContext db, ParameterModelType type, IEnumerable<float> defaultvalues)
            : this()
        {
            ParameterType = type;
            AddNewItem(db, String.Join(",", defaultvalues.Select(p => p.ToString("G")).ToArray()),  "");
        }

        public ParameterModel(ApplicationDbContext.ApplicationDbContext db, ParameterModelType type, IEnumerable<int> defaultvalues)
            : this()
        {
            ParameterType = type;
            AddNewItem(db, String.Join(",", defaultvalues.Select(p => p.ToString()).ToArray()), "");
        }

        public ParameterModel(ApplicationDbContext.ApplicationDbContext db, ParameterModelType type, string value):this()
        {
            ParameterType = type;
            Id = Guid.NewGuid();
            AddNewItem(db, value, "");
        }

        
        [Key]
        public Guid Id { get; set; }
        public ParameterModelType ParameterType { get; set; }

        public string Comments { get; set; }


        public string LastChange { get; set; }

        public virtual ICollection<ParameterModelElement> Data { get; set; }

        [NotMapped]
        [JsonIgnore]
        public ParameterModelElement Current
        {
            get { return Data.OrderByDescending(x => x.DateTimeStamp).FirstOrDefault(); }
        }

        public void AddNewItem(ApplicationDbContext.ApplicationDbContext db, string valuestring, /*string comment,*/ string user)
        {
            var data=new ParameterModelElement(this,valuestring, /*comment,*/user, DateTime.UtcNow);
            data.Id = Guid.NewGuid();
            db.ParameterModelElements.Add(data);
            Data.Add(data);

        }

        public string getValueString()
        {
            if (Current != null)
                return Current.ValueString;
            return "";
        }

        public int getIntValue()
        {
            if (Current != null)
                return Current.IntValue;
            return 0;
        }

        public int getIntValue(int i)
        {
            if (ParameterType == ParameterModelType.IntVector)
            {
                
                if (Current != null&&i<Current.IntVector.Count())
                    return Current.IntVector[i];
            }
            return 0;
        }

        public float getFloatValue(int i)
        {
            if (ParameterType == ParameterModelType.FloatVector)
            {

                if (Current != null && i < Current.FloatVector.Count())
                    return Current.FloatVector[i];
            }
            return 0;
        }



        public bool IsEqualTo(ParameterModel source)
        {
            if (source.Data.Count != Data.Count)
                return false;
            
            var count = Data.Count;
            for (int i = 0; i < count; ++i)
            {
                if (Data.ElementAt(i).IsEqualTo(source.Data.ElementAt(i))==false)
                {
                    return false;
                }
            }

            return true;
        }

        

        internal void Initialise(ApplicationDbContext.ApplicationDbContext db, float value,string comment, string source)
        {
            AddNewItem(db,value.ToString("G"),  source);
            Comments = comment;
        }
        internal void Initialise(ApplicationDbContext.ApplicationDbContext db, int value, string comment, string source)
        {
            AddNewItem(db, value.ToString(),  source);
            Comments = comment;
            
        }

        internal void Initialise(ApplicationDbContext.ApplicationDbContext db, List<float> values, string comment, string source)
        {
            AddNewItem(db, String.Join(",", values.Select(p => p.ToString("G")).ToArray()), source);
            Comments = comment;
        }

        internal void Initialise(ApplicationDbContext.ApplicationDbContext db, List<int> values, string comment, string source)
        {
            AddNewItem(db, String.Join(",", values.Select(p => p.ToString()).ToArray()), source);
            Comments = comment;
        }

       

        internal void InitialiseFromXMLNode(ApplicationDbContext.ApplicationDbContext db, XmlNode parentnode, string tag, string source)
        {
            try
            {
                if (parentnode != null && String.IsNullOrEmpty(tag) == false)
                {
                    XmlNode node = parentnode.SelectSingleNode(tag);
                    if (node != null)
                    {
                        var comment = (node.Attributes!=null && node.Attributes["Comments"] != null) ? node.Attributes["Comments"].InnerText : "";
                        if (ParameterType == ParameterModelType.Float)
                        {
                            float temp;
                            if (float.TryParse(node.InnerText, out temp))
                            {
                                
                                Initialise(db, temp, comment, source);
                            }
                         
                        }
                        else if (ParameterType == ParameterModelType.Int)
                        {
                            int temp;
                            if (Int32.TryParse(node.InnerText, out temp))
                            {
                                Initialise(db, temp, comment, source);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.HandleError(ex, "", true);
            }          
        }

        internal void RemoveFromDatabase(ApplicationDbContext.ApplicationDbContext db)
        {
            
            foreach (var item in Data.ToList())
            {
                item.RemoveFromDatabase(db);
            }
            Data = null;
            db.Set<ParameterModel>().Remove(this);
        }
    }
}