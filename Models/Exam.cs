using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreExamProject.Models
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        public Post Post { get; set; }
        public List<Question> Questions { get; set; }
    }
}
