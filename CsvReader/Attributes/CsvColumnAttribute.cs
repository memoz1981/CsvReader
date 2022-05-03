using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvReader.Attributes
{
    public class CsvColumnAttribute : Attribute 
    {
        public string Name { get;}

        public CsvColumnAttribute(string name)
        {
            Name = name;
        }

        public string GetName() => Name; 
    }
}
