﻿using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;

namespace WAES.Diff.Service.Domain.Interfaces
{
    public interface IDiffService
    {
        Task<DiffResult> GetDiff(Guid id);
    }
}
