using System;
using System.Collections.Generic;

namespace UniversityRegistrar.Models
{
  public class Course
  {
    public int CourseId { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Credits { get; set; }

    public virtual ICollection<CourseStudent> JoinEntities { get; }

    public Course()
    {
      this.JoinEntities = new HashSet<CourseStudent>();
    }
  }
}