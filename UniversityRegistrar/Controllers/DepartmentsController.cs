using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System.Linq;

namespace UniversityRegistrar.Controllers
{
  public class DepartmentsController : Controller
  {
    private readonly UniversityRegistrarContext _db;

    public DepartmentsController(UniversityRegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Department> model = _db.Departments.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Department department)
    {
      _db.Departments.Add(department);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var selectedDepartment = _db.Departments
          .Include(department => department.Students)
          .Include(department => department.Courses)
          .FirstOrDefault(department => department.DepartmentId == id);
      return View(selectedDepartment);
    }

    public ActionResult Edit(int id)
    {
      var selectedDepartment = _db.Departments
        .Include(department => department.Students)
        .Include(department => department.Courses)
        .FirstOrDefault(department => department.DepartmentId == id);
      return View(selectedDepartment);
    }

    [HttpPost]
    public ActionResult Edit(Department department, int[] courseIds, int[] studentIds)
    {
      foreach (int studentId in studentIds)
      {
        Student studentEntry = _db.Students.Where(entry => entry.StudentId == studentId).Single();
        studentEntry.DepartmentId = null;
        _db.Entry(studentEntry).State = EntityState.Modified;
      }

      foreach (int courseId in courseIds)
      {
        Course courseEntry = _db.Courses.Where(entry => entry.CourseId == courseId).Single();
        courseEntry.DepartmentId = null;
        _db.Entry(courseEntry).State = EntityState.Modified;
      }

      _db.Entry(department).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new {id = department.DepartmentId});
    }

    public ActionResult Delete(int id)
    {
      var selectedDepartment = _db.Departments.FirstOrDefault(department => department.DepartmentId == id);
      return View(selectedDepartment);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var selectedDepartment = _db.Departments.FirstOrDefault(department => department.DepartmentId == id);
      _db.Departments.Remove(selectedDepartment);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteCourse(int departmentId, int courseId)
    {
      Course selectedCourse = _db.Courses.FirstOrDefault(course => course.CourseId == courseId);
      selectedCourse.DepartmentId = null;
      _db.Entry(selectedCourse).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new {id = departmentId});
    }
  }
}