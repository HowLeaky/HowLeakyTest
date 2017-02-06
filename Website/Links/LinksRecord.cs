using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace HowLeakyModels.Links
{
     [Table("LinksRecord")]
    public class LinksRecord
    {
        public LinksRecord() 
        {

        }

        [Required]
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "URL")]
        public string URLLink { get; set; }

        [Display(Name = "Image Filename")]
        public string ImageURI { get; set; }

        [AllowHtml]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}