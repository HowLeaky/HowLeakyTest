using HowLeakyModels.Accounts;
using HowLeakyModels.AppData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HowLeakyModels.Analysis
{
    //public enum AnalysisType
    //{
    //    HowOftenDoesItRain,
    //    HowOftenIsItWarm,
    //    HowOftenIsItCool,
    //    HowOftenIsSoilWetMM,
    //    HowOftenIsSoilWetPC,
    //    HowWetIsTheSoilMM,
    //    HowWetIsTheSoilPC

    //}

    //public enum HowOftenEqualityType
    //{
    //    MoreThan,
    //    LessThan
    //}
    public class AnalysisModel
    {
        public AnalysisModel ()
        {
            
        }

        public Guid Id { get; set; }
        //public AnalysisType AnalysisType { get; set; }
        //public HowOftenEqualityType? HoEqualityType { get; set; }
        //public int? ThresholdRain { get; set; }
        //public int? ThresholdMaxTemp { get; set; }
        //public int? ThresholdMinTemp { get; set; }
        //public int? ThresholdSoilWater_mm { get; set; }
        //public int? ThresholdSoilWater_pc { get; set; }
        //public int? AccumulationPeriod { get; set; }
        //public int? StartDay { get; set; }
        //public int? EndDay { get; set; }
        //public int? StartMonth { get; set; }
        //public int? EndMonth { get; set; }

        
        public virtual ApplicationUser Owner { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public virtual SoilModel Soil { get; set; }
        public virtual VegModel Veg { get; set; }
        public virtual ClimateDataModel ClimateData { get; set;}



        internal void RemoveFromDatabase(ApplicationDbContext.ApplicationDbContext db)
        {
            Soil = null;
            Veg = null;
            ClimateData = null;
            Owner = null;
        }
    }
}