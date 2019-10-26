﻿using System.Collections;
using System.Collections.Generic;
using WAES.Diff.Service.Domain.Entities;

namespace WAES.Diff.Service.Domain.Interfaces
{
    public interface IDiffCalculator
    {
        List<DiffDetail> GetComputedDiffs(BitArray left, BitArray right);
    }
}
