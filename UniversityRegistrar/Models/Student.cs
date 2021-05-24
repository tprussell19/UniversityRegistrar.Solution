using System;

namespace UniversityRegistrarContext.Models
{
  public class Student
  {
    public int StudentId {get; set;}
    public string Name {get; set;}
    public DateTime EnrollmentDate {get; set;} = DateTime.Now;

    public virtual ICollection<CourseStudent> JoinEntities {get;}

    public Student()
    {
      this.JoinEntities = new HashSet<CourseStudent>();
    }
  }
}