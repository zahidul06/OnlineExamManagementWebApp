﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using OnlineExamManagementWebApp.Models;

namespace OnlineExamManagementWebApp.ViewModels {
    public class AssignTrainerViewModel {

        public int CourseId { get; set; }

        public string TrainerName{ get; set; }

        public bool IsLead { get; set; }
    }
}