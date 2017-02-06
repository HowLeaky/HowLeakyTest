using HowLeakyModels.AppData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HowLeakyModels.Accounts
{
    [Table("ChangeRecords")]
    public class ChangeRecord
    {
        private SoilModel _soil;
        private VegModel _veg;
        private ApplicationUser _user;
        private DateTime _date;
        private string _action;

        public ChangeRecord()
        {
            _soil = null;
            _veg = null;
            _user = null;
            _action = null;
        }

        public ChangeRecord(SoilModel soil, ApplicationUser user, DateTime date, string action)
            : this()
        {
            _soil = soil;

            _user = user;
            _date = date;
            _action = action;
        }

        public ChangeRecord(VegModel veg, ApplicationUser user, DateTime date, string action)
            : this()
        {
            _veg = veg;
            _user = user;
            _date = date;
            _action = action;
        }


        ~ChangeRecord()
        {
            _soil = null;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        public virtual SoilModel soil { get { return _soil; } }

        public virtual VegModel veg { get { return _veg; } }

        [Display(Name = "User")]
        public virtual ApplicationUser user { get { return _user; } }
        [Display(Name = "Date")]
        public DateTime date { get { return _date; } }
        [Display(Name = "Action")]
        public string action { get { return _action; } }


        internal void RemoveFromDatabase(ApplicationDbContext.ApplicationDbContext db)
        {
            _soil = null;
            _veg = null;
            _user = null;
            db.Set<ChangeRecord>().Remove(this);
        }
    }
}