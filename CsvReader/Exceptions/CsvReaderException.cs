using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvReader.Exceptions
{
    public class CsvReaderException : Exception
    {
        public CsvReaderException() : base() { }

        public CsvReaderException(string message) : base(message) { }

        public CsvReaderException(string message, Exception innerException) : base(message, innerException) { }

    }
}
