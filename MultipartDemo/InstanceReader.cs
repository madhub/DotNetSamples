using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MultipartDemo
{
    public class InstanceReader : IInstanceReader
    {
        private readonly string file;

        public InstanceReader(string file)
        {
            this.file = file;
        }

        public string GetId()
        {
            return file;
        }

        public Stream GetData()
        {
            return new FileForwardOnlyStream(file);
        }
    }
}
