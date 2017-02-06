//Copyright 2014, DHM Environmental Software Engineering Pty. Ltd
//The Copyright of this file belongs to DHM Environmental Software Engineering Pty. Ltd. (hereafter known as DHM) and selected clients of DHM.
//No content of this file may be reproduced, modified or used in software development without the express written permission from DHM.
//Where permission has been granted to use or modify this file, the full copyright information must remain unchanged at the top of the file.
//Where permission has been granted to modify this file, changes must be clearly identified through adding comments and annotations to the source-code,
//and a description of the changes (including who has made the changes), must be included after this copyright information.

using HowLeakyModels.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace HowLeakyModels.News
{
   [Table("Posts")]
    public class NewsItem
    {
        public NewsItem()
        {
            InitialiseVirtuals();
        }

        private void InitialiseVirtuals()
        {
            this.PostComments = new HashSet<NewsItemComment>();
            this.PostTags = new HashSet<NewsItemTag>();
        }

        [Required]
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Date Time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime DateTime { get; set; }

        //[Required]
        [Display(Name = "Published")]
        public bool Published { get; set; }


        [Required]
        [AllowHtml]
        [Display(Name = "Body")]
        public string Body { get; set; }

        //[Required]
        [Display(Name = "Image Filename")]
        public string ImageFileName { get; set; }

      
        [Display(Name = "User")]
        public virtual ApplicationUser AspNetUser { get; set; }

        [Display(Name = "Post Comments")]
        public virtual ICollection<NewsItemComment> PostComments { get; set; }

        [Display(Name = "Post Tags")]
        public virtual ICollection<NewsItemTag> PostTags { get; set; }

    }
}