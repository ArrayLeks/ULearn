using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.API
{
    public class APIObject : IDisposable
    {
        int field;

        public APIObject(int number)
        {
            field = number;
            MagicAPI.Allocate(number);
        }
        
        private bool disposedValue;

        ~APIObject()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                MagicAPI.Free(field);
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
