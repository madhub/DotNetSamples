using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MultipartDemo
{
    /// <summary>
    /// Opens file only when read is requested.
    /// </summary>
    public class FileForwardOnlyStream : Stream
    {
        private  Stream stream;
        private readonly string fileName;
        internal Stream Stream => stream ?? (stream = System.IO.File.OpenRead(fileName));

        public FileForwardOnlyStream(string fileName)
        {

            this.fileName = fileName;
        }
        public override bool CanRead => Stream.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => stream.Length;

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }



        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return Stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
