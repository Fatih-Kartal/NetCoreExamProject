using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreExamProject.Models
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        public string PostLink { get; set; }
        public string CreateDate { get; set; }

        [NotMapped]
        public Post Post { get; set; }
        public List<Question> Questions { get; set; }
    }
}
