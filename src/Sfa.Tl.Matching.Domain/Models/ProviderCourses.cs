﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class ProviderCourses
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }
        public virtual Provider Provider { get; set; }
    }
}