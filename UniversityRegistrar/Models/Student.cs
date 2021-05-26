using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityRegistrar.Models
{
  public class Student
  {
    public int StudentId { get; set; }
    public string Name { get; set; }

    [Display(Name = "Enrollment Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    public int? DepartmentId { get; set; }
    public virtual Department Department { get; set; }

    public virtual ICollection<CourseStudent> JoinEntities { get; }

    public Student()
    {
      this.JoinEntities = new HashSet<CourseStudent>();
    }
  }
}