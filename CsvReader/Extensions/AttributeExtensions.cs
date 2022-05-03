using CsvReader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsvReader.Extensions
{
    public static class AttributeExtensions
    {
        public static string GetColumnNameOrNull(this PropertyInfo prop)
        {
            var attList = prop.GetCustomAttributes(true);

            foreach (var att in attList)
            {
                if (att is CsvColumnAttribute)
                {
                    var attCsv = att as CsvColumnAttribute;
                    return attCsv.GetName();
                }
            }
            return null; 
        }
    }
}
