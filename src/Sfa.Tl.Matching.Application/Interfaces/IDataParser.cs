﻿using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDataParser<out T> where T : class
    {
        T Parse(string[] cells);

        IEnumerable<T> ParseToMany(string[] cells);
    }
}