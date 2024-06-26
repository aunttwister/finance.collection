﻿using IntrinsicValue.Calculation;
using IntrinsicValue.Calculation.DataSets;
using MediatR;

namespace Parallelize.It
{
    public interface IRunTasksAsyncService<T1> where T1 : BaseIntrinsicModelCommand
    {
        public Task<T1> RunScrapersAsync(T1 request);
    }
}