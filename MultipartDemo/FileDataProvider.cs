using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultipartDemo
{
    public class FileDataProvider : IDataProvider
    {
        public static List<string> files = new List<string> {"File.txt", "price-difference.png", "File.json" };
        public List<IInstanceReader> GetInstances()
        {
            List<IInstanceReader> readers = new List<IInstanceReader>();
            foreach (var item in files)
            {
                readers.Add(new InstanceReader(item));
            }
            return readers;
        }
    }
}
