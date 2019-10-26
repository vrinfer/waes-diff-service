﻿using System.Collections.Generic;
using WAES.Diff.Service.Common.Enums;

namespace WAES.Diff.Service.Web.Models.Responses
{
    public class DiffResponse
    {
        public DiffStatus Status { get; set; }
        public IEnumerable<Diff> Differences { get; set; }
    }
}