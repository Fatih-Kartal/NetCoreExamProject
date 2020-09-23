﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreExamProject.Models
{
    public class Post
    {
        public string Title { get; set; }
        public string Content { get; set; }
        [Key]
        public string Link { get; set; }
    }
}
