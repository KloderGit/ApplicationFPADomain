using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IAddData<T> where T : class
    {
        bool Add(T obj);
        bool AddRange(IEnumerable<T> array);
    }
}
