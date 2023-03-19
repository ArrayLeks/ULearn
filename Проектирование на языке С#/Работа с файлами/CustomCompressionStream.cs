using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams.Compression
{
    public class CustomCompressionStream : Stream
    {
        private bool read;
        private Stream baseStream;
        private const int SIZE = 1024;
        private bool hasEndedStream = false;
        private Queue<byte> readQueue;
        private Queue<byte> decompressedData;
        
        public CustomCompressionStream(Stream baseStream, bool read)
        {
            this.read = read;  // Используйте этот флаг, чтобы понимать:
                               // ваш стрим открыт в режиме чтения или в режиме записи.
                               // Не нужно поддерживать и чтение и запись одновременно.
            this.baseStream = baseStream;
            if (read)
            {
                readQueue = new Queue<byte>();
                decompressedData = new Queue<byte>();
            }
        } 

        public override void Flush()
        {
            baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if(!read) throw new InvalidOperationException();

            if(readQueue.Count < 2) FillReadQueue(offset, SIZE);
            int i = 0 + offset;
            while(i < count)
            {
                DecompressData(readQueue);
                while(decompressedData.Count > 0)
                {
                    buffer[i++] = decompressedData.Dequeue();
                    if (i >= count + offset) break;
                }
                if (i != count && !hasEndedStream) FillReadQueue(offset, SIZE);
                else break;
            }

            return i - offset;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if(read) throw new InvalidOperationException();

            var compressData = CompressData(buffer
                .Skip(offset)
                .Take(buffer.Length - (offset * 2))
                .ToArray());
            baseStream.Write(compressData, 0, compressData.Length);
        }

        private void FillReadQueue(int offset, int count)
        {
            var readBuffer = new byte[1];
            int readCount = 0;
            while (readQueue.Count != count || readQueue.Count != SIZE)
            {
                readCount = baseStream.Read(readBuffer, 0, 1);
                if (readCount == 0) break;
                readQueue.Enqueue(readBuffer[0]);
                readCount = baseStream.Read(readBuffer, 0, 1);
                if (readCount == 0) throw new InvalidOperationException();
                readQueue.Enqueue(readBuffer[0]);
            }
            if (readQueue.Count < SIZE) hasEndedStream = true;
        }

        private byte[] CompressData(byte[] buffer)
        {
            var compressedData = new List<byte>();

            for (int i = 0, repeat = 1; i < buffer.Length; i++, repeat = 1)
            {
                while(i < buffer.Length - 1 && buffer[i] == buffer[i + 1])
                {
                    repeat++; i++;
                    if (repeat >= 255) break;
                }
                compressedData.AddRange(new[] { (byte)repeat, buffer[i] });
            }
            return compressedData.ToArray();
        }

        private void DecompressData(Queue<byte> buffer)
        {
            for (byte repeat = 1, symbol = 0; buffer.Count != 0;)
            {
                repeat = buffer.Dequeue();
                if(buffer.Count == 0)
                    throw new InvalidOperationException();
                symbol = buffer.Dequeue();
                while(repeat > 0)
                {
                    decompressedData.Enqueue(symbol);
                    repeat--;
                }
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead => read;

        public override bool CanSeek => read;

        public override bool CanWrite => !read;

        public override long Length => throw new NotSupportedException();

        public override long Position 
        {
            get => throw new NotSupportedException(); 
            set => throw new NotSupportedException(); 
        }
    }
}
