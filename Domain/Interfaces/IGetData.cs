using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IGetData<T> where T: class
    {
        T Get(string key);
        IEnumerable<T> GetList();
    }
}
