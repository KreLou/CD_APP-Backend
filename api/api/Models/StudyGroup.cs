﻿using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class StudyGroup
    {
        [Required]
        public int ID { get; set; }

        public string LongName { get; set; }
        public string ShortName { get; set; }
        public bool Active { get; set; }
        public StudyCourse StudyCourse { get; set; }
    }
}
