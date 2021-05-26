using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityRegistrar.Controllers
{
  public class StudentsController : Controller
  {
    private readonly UniversityRegistrarContext _db;

    public StudentsController(UniversityRegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Students.OrderBy(student => student.Name).ToList());
    }

    public ActionResult Create()
    {
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Student student, int CourseId)
    {
      if (CourseId != 0)
      {
        _db.CourseStudent.Add(new CourseStudent() { CourseId = CourseId, StudentId = student.StudentId });
      }
      _db.Students.Add(student);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Student selectedStudent = _db.Students
        .Include(student => student.JoinEntities)
        .ThenInclude(join => join.Course)
        .Include(student => student.Department)
        .FirstOrDefault(student => student.StudentId == id);
      return View(selectedStudent);
    }

    public ActionResult Edit(int id)
    {
      Student selectedStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Name");
      return View(selectedStudent);
    }

    [HttpPost]
    public ActionResult Edit(Student student, int[] courseIds)
    {
      foreach (int courseId in courseIds)
      {
        CourseStudent joinEntry = _db.CourseStudent.Where(entry => entry.StudentId == student.StudentId && entry.CourseId == courseId).Single();
        _db.CourseStudent.Remove(joinEntry);
      }

      _db.Entry(student).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new {id = student.StudentId});
    }

    public ActionResult AddCourse(int id)
    {
      Student selectedStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
      return View(selectedStudent);
    }

    [HttpPost]
    public ActionResult AddCourse(Student student, int CourseId)
    {
      CourseStudent joinTableEntry = null;
      try
      {
        joinTableEntry = _db.CourseStudent.Where(entry => entry.StudentId == student.StudentId && entry.CourseId == CourseId).Single();
      }
      catch
      {
        // Console.WriteLine("Doesn't exist in CourseStudent table");
      }


      if (CourseId != 0 && joinTableEntry == null)
      {
        _db.CourseStudent.Add(new CourseStudent() { CourseId = CourseId, StudentId = student.StudentId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Student selectedStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      return View(selectedStudent);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Student selectedStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      _db.Students.Remove(selectedStudent);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteCourse(int joinId)
    {
      CourseStudent joinEntry = _db.CourseStudent.FirstOrDefault(entry => entry.CourseStudentId == joinId);
      _db.CourseStudent.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddDepartment(int id)
    {
      Student selectedStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Name");
      return View(selectedStudent);
    }

    [HttpPost]
    public ActionResult AddDepartment(Student student)
    {
      _db.Entry(student).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteDepartment(int id)
    {
      Student selectedStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      selectedStudent.DepartmentId = null;
      _db.Entry(selectedStudent).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}