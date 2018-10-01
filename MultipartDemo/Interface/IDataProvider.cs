using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultipartDemo
{
    public interface IDataProvider
    {
        List<IInstanceReader> GetInstances();
    }
}
