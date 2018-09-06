﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OnlineExamManagementWebApp.BLL;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.ViewModels;

namespace OnlineExamManagementWebApp.Controllers {
    public class CourseController : Controller {
        private readonly CourseManager _courseManager = new CourseManager();
        private readonly TrainerManager _trainerManager = new TrainerManager();

        public ActionResult Entry() {
            return View(GetCourseEntryViewModel());
        }

        private CourseEntryViewModel GetCourseEntryViewModel() {
            var courseEntryViewModel = new CourseEntryViewModel {
                Organizations = _courseManager.GetAllOrganizations(),
                Tags = new SelectList(_courseManager.GetAllTags())
            };
            return courseEntryViewModel;
        }

        [HttpPost]
        public ActionResult Entry(CourseEntryViewModel viewModel) {
            if (ModelState.IsValid) {
                var course = new Course {
                    OrganizationId = viewModel.OrganizationId,
                    Name = viewModel.Name,
                    Code = viewModel.Code,
                    Duration = viewModel.Duration,
                    Credit = viewModel.Credit,
                    Outline = viewModel.Outline,
                    Tags = _courseManager.GetReleventTags(viewModel.SelectedTags)
                };
                if (!_courseManager.IsCourseSaved(course))
                    return RedirectToAction("Error");
                
                TempData["Course"] = course;
                ModelState.Clear();

                return RedirectToAction("Information");
            }

            return RedirectToAction("Error");
        }

        public ActionResult Information() {
            var course = (Course) TempData["Course"];
            course.Organization = _courseManager.GetOrganizationById(course.OrganizationId);

            var courseBasicInfoVm = new CourseBasicInfoViewModel {
                OrganizationCode = course.Organization.Code,
                Name = course.Name,
                Code = course.Code,
                Duration = course.Duration,
                Credit = course.Credit,
                Outline = course.Outline,                
            };

            return View(courseBasicInfoVm);
        }

        // Get all trainers using select2 ajax call
        public JsonResult GetTrainers(string searchTerm) {
            var trainerList = _trainerManager.GetAllTrainers();

            if (searchTerm != null) {
                trainerList = trainerList.Where(t => t.Name.Contains(searchTerm)).ToList();
            }

            var modifiedList = trainerList.Select(x => new { id=x.Id, text=x.Name });

            return Json(modifiedList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Error() {
            return View("Error");
        }
    }
}