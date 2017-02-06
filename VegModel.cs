using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Xml;
using HowLeakyModels._Custom;
using HowLeakyModels.Accounts;
using HowLeakyModels._Parameters;
using HowLeakyModels.DHMCoreLib.ErrorHandling;

namespace HowLeakyModels.AppData
{
    [Table("VegSyncModels")]
    public class VegModel:CustomSyncModel
    {
        public VegModel()
        {
            //Id = Guid.NewGuid();
            //MaxAllowTotalCover = new ParameterModel(ParameterModelType.Float);
            //MaxRootDepth = new ParameterModel(ParameterModelType.Float);
            //PanDaysPlantToHarvest = new ParameterModel(ParameterModelType.Int);
            //PanHarvestIndex = new ParameterModel(ParameterModelType.Float);
            //PanMaxResidueCover = new ParameterModel(ParameterModelType.Float);
            //PanPlantDay = new ParameterModel(ParameterModelType.Int);
            //PanWaterUseEfficiency = new ParameterModel(ParameterModelType.Float);
            //PanDay = new ParameterModel(ParameterModelType.IntVector);
            //GreenCover = new ParameterModel(ParameterModelType.FloatVector);
            //RootDepth = new ParameterModel(ParameterModelType.FloatVector);
            //ResidueCover = new ParameterModel(ParameterModelType.FloatVector);
            //PanDayCount = new ParameterModel(ParameterModelType.Int);

            InitialiseVirtuals();
        }

        private void InitialiseVirtuals()
        {
            //Regions = new HashSet<GeoRegion>();
            EditHistory = new HashSet<ChangeRecord>();
            AdditionalPermissions = new HashSet<ApplicationUser>();
        }

        //public VegModel(VegetationData vegdata,ApplicationUser user)
        //{
        //    MaxAllowTotalCover.Initialise((float)vegdata.MaxAllowTotalCover,"",user.UserName);
        //    MaxRootDepth.Initialise((float)vegdata.MaxRootDepth, "", user.UserName);
        //    PanDaysPlantToHarvest.Initialise(vegdata.PanDaysPlantToHarvest, "", user.UserName);
        //    PanHarvestIndex.Initialise(vegdata.PanHarvestIndex, "", user.UserName);
        //    PanMaxResidueCover.Initialise((float)vegdata.PanMaxResidueCover, "", user.UserName);
        //    PanPlantDay.Initialise(vegdata.PanPlantDay, "", user.UserName);
        //    PanWaterUseEfficiency.Initialise((float)vegdata.PanWaterUseEfficiency, "", user.UserName);
        //    PanDay.Initialise(vegdata.JulianDays, "", user.UserName);
        //    GreenCover.Initialise(vegdata.CropCover, "", user.UserName);
        //    RootDepth.Initialise(vegdata.RootDepth, "", user.UserName);
        //    ResidueCover.Initialise(vegdata.ResidueCover, "", user.UserName);
        //    PanDayCount.Initialise(vegdata.DataCount, "", user.UserName);
        //}

        public VegModel(ApplicationDbContext.ApplicationDbContext db,Stream stream, string filename, ApplicationUser appUser) : this()
        {
            LoadFromXMLStream(db,stream,filename);
            Id = Guid.NewGuid();
            CreatedBy = appUser.UserName;
            CreatedDate = DateTime.UtcNow;
            ModifiedBy = CreatedBy;
            ModifiedDate = CreatedDate;
        }

        public virtual ParameterModel MaxAllowTotalCover { get; set; }
        public virtual ParameterModel MaxRootDepth { get; set; }
        public virtual ParameterModel PanDaysPlantToHarvest { get; set; }
        public virtual ParameterModel PanHarvestIndex { get; set; }
        public virtual ParameterModel PanMaxResidueCover { get; set; }
        public virtual ParameterModel PanPlantDay { get; set; }
        public virtual ParameterModel PanWaterUseEfficiency { get; set; }
        public virtual ParameterModel PanDay { get; set; }
        public virtual ParameterModel GreenCover { get; set; }
        public virtual ParameterModel RootDepth { get; set; }
        public virtual ParameterModel ResidueCover { get; set; }

        public virtual ParameterModel PanDayCount { get; set; }

        //public virtual ICollection<GeoRegion> Regions { get; set; }
        public virtual ICollection<ChangeRecord> EditHistory { get; set; }
        public virtual ICollection<ApplicationUser> AdditionalPermissions { get; set; }


        public bool LoadFromXMLStream(ApplicationDbContext.ApplicationDbContext db, Stream stream,string filename)
        {
            try
            {
                string name = filename.Replace(".soil", "");
                string source = "File-import:  " + filename;
                XmlDocument Doc = new XmlDocument();
                Doc.Load(stream);
                XmlElement Root = Doc.DocumentElement;
                if (Root.HasChildNodes)
                {
                    XmlNode header = Root.SelectSingleNode("VegetationType");
                    if (header != null)
                    {
                        Name = (header.Attributes != null && header.Attributes["text"] != null) ? header.Attributes["text"].Value : name;
                        Summary = (header.Attributes != null && header.Attributes["Description"] != null) ? header.Attributes["Description"].Value : "";
                        Comments = (header.Attributes != null && header.Attributes["Comments"] != null) ? header.Attributes["Comments"].Value : "";
                        Comments = Comments + "\n" + source;
                        XmlNode modeltypenode = header.SelectSingleNode("ModelType");
                        if (modeltypenode != null)
                        {
                            int ModelType = Int32.Parse(modeltypenode.Attributes["index"].Value);
                            if (ModelType == 1)//Cover Model
                            {

                                XmlNode matrixdata = header.SelectSingleNode("CropFactorMatrix");
                                var _days = new List<int>();
                                var _greencover = new List<float>();
                                var _residuecover = new List<float>();
                                var _rootdepth = new List<float>();
                                
                                if (matrixdata != null && matrixdata.HasChildNodes)
                                {
                                    foreach (XmlNode child in matrixdata.ChildNodes)
                                    {
                                        int jday = Int32.Parse(child.Attributes["x"].Value);
                                        float cropcov = float.Parse(child.Attributes["y"].Value);
                                        float residuecov = float.Parse(child.Attributes["z"].Value);
                                        float rootdepth = float.Parse(child.Attributes["a"].Value);


                                        _days.Add(jday);
                                        _greencover.Add(cropcov);
                                        _residuecover.Add(residuecov);
                                        _rootdepth.Add(rootdepth);

                                    }
                                }
                               
                                MaxAllowTotalCover=InitialiseFromXMLNode(db,header, "MaxAllowTotalCover",ParameterModelType.Float, source);
                                MaxRootDepth = InitialiseFromXMLNode(db, header, "MaxRootDepth", ParameterModelType.Float, source);
                                PanDaysPlantToHarvest = InitialiseFromXMLNode(db, header, "PanDaysPlantToHarvest", ParameterModelType.Int, source);
                                PanHarvestIndex = InitialiseFromXMLNode(db, header, "PanHarvestIndex", ParameterModelType.Float, source);
                                PanMaxResidueCover = InitialiseFromXMLNode(db, header, "PanMaxResidueCover", ParameterModelType.Float, source);
                                PanPlantDay = InitialiseFromXMLNode(db, header, "PanPlantDay", ParameterModelType.Int, source);
                                PanWaterUseEfficiency = InitialiseFromXMLNode(db, header, "PanWaterUseEfficiency", ParameterModelType.Float, source);
                                PanDay=Initialise(db, _days, "", source);
                                GreenCover=Initialise(db, _greencover, "", source);
                                RootDepth=Initialise(db, _rootdepth, "", source);
                                ResidueCover=Initialise(db, _residuecover, "", source);
                                PanDayCount=Initialise(db, _days.Count, "", source);
                                
                            }
                            else //probably LAI- not supported
                            {
                                return false;
                            }
                        }
                        else //probably LAI- not supported
                        {
                            return false;
                        }
                    }

                }

             
            }
            catch (Exception ex)
            {
                ErrorLogger.HandleError(ex, "", false);
            }

            return false;
        }

        internal void RemoveFromDatabase(ApplicationDbContext.ApplicationDbContext db)
        {
            if(MaxAllowTotalCover!=null)MaxAllowTotalCover.RemoveFromDatabase(db);
            if(MaxRootDepth!=null)MaxRootDepth.RemoveFromDatabase(db);
            if(PanDaysPlantToHarvest!=null)PanDaysPlantToHarvest.RemoveFromDatabase(db);
            if(PanHarvestIndex!=null)PanHarvestIndex.RemoveFromDatabase(db);
            if(PanMaxResidueCover!=null)PanMaxResidueCover.RemoveFromDatabase(db);
            if(PanPlantDay!=null)PanPlantDay.RemoveFromDatabase(db);
            if(PanWaterUseEfficiency!=null)PanWaterUseEfficiency.RemoveFromDatabase(db);
            if(PanDay!=null)PanDay.RemoveFromDatabase(db);
            if(GreenCover!=null)GreenCover.RemoveFromDatabase(db);
            if(RootDepth!=null)RootDepth.RemoveFromDatabase(db);
            if(ResidueCover!=null)ResidueCover.RemoveFromDatabase(db);
            if (PanDayCount != null) PanDayCount.RemoveFromDatabase(db);
            
            //Regions.Clear();
            AdditionalPermissions.Clear();
            if (EditHistory != null)
            {
                foreach (var record in EditHistory.ToList())
                {
                    record.RemoveFromDatabase(db);
                }
                EditHistory.Clear();
            }

        }
    }
}