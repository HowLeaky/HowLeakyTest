using HowLeakyModels.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HowLeakyModels.CMS
{
    public enum CmsType
    {
        Title,
        SubTitle,
        Text
    }

    public class Cms
    {
        public Cms()
        {
            
        }

        public Cms(string identifier)
        {
            Identifier = identifier;
            Title = "";
            SubTitle = "";
            Text = "";            
        }

        [Key]
        public string Identifier { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        [AllowHtml]
        public string Text { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public virtual ApplicationUser LastModifiedBy { get; set; }

        public string getCmsElement(CmsType type)
        {
            switch (type) 
            {
                case CmsType.Title:
                    return Title;
                case CmsType.SubTitle:
                    return SubTitle;
                case CmsType.Text:
                    return Text;
                default:
                    return "Not a valid CMS type";
            }
        }
    }
}