using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Streams.Resources
{
    public class ResourceReaderStream : Stream
    {
        private readonly Stream bufferedStream;
        private readonly byte[] keyData;
        private readonly int maxSize = 1024;
        private bool hasKeyFound = false;
        private bool unknownKey = true;
        private byte[] data;
        private int position = 0;
        private int readCount;

        public ResourceReaderStream(Stream stream, string key)
        {
            bufferedStream = new BufferedStream(stream, maxSize);
            keyData = Encoding.ASCII.GetBytes(key);
            data = new byte[maxSize];
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if(position == 0)
                ReadNextBlock(data, offset, data.Length);

            if(!hasKeyFound) 
                SeekValue(offset);
            
            if(hasKeyFound) 
                return ReadFieldValue(buffer, offset, count);

            return 0;
        }

        private int ReadFieldValue(byte[] buffer, int offset, int count)
        {
            int i = 0;
            while (position < data.Length && buffer.Length > i)
            {
                if (data[position] == 0 && position < data.Length - 2 
                    && data[position + 1] == 1 && data[position - 1] != 0)
                {
                    hasKeyFound = false;
                    break;
                }
                if (data[position] == 0)
                    position++;

                buffer[i++] = data[position++];

                if (position == 1024 && readCount == 1024)
                    ReadNextBlock(data, offset, data.Length);
                else if(position > readCount)
                    throw new EndOfStreamException();
            }

            return i;
        }

        private void SeekValue(int offset)
        {
            int i = 0;
            while(position < readCount)
            {
                i = SeekKey(i);

                if (i == keyData.Length)
                {
                    if (data[position + 1] != 0 || data[position + 2] != 1 
                        || data[position + 3] == data.Length)
                        throw new EndOfStreamException();
                    position += 3;
                    hasKeyFound= true;
                    unknownKey = false;
                    break;
                }

                position++;

                if(position == readCount && readCount == 1024)
                    ReadNextBlock(data, offset, data.Length);
                else if(position == readCount && unknownKey)
                    throw new EndOfStreamException();
            }
        }

        private int SeekKey(int i)
        {
            if (data[position] == keyData[i])
            {
                if (data[position] == 0 && data[position + 1] == 0) position++;
                else if (data[position] == 0 && data[position + 1] != 0) i = -1;
                i++;
            }
            else if (i != 0)
                i = 0;

            return i;
        }

        private void ReadNextBlock(byte[] data, int offset, int length)
        {
            readCount = bufferedStream.Read(data, offset, length);
            position = 0;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead => bufferedStream.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position 
        { 
            get => throw new NotSupportedException(); 
            set => throw new NotSupportedException(); 
        }
    }
}
