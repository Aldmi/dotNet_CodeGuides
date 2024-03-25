using System;
using System.Diagnostics;

namespace Using_DI.Services;

public interface IFooService
{
    public Guid ObjGuid { get; init; }

    public int GetCounterValue();
}