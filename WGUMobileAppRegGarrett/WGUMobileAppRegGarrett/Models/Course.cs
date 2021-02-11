using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace WGUMobileAppRegGarrett.Models
{
    [Table("Courses")]
    class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public string Name { get; set; }  
    }
}
