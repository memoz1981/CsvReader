using CsvReader.Attributes;
using CsvReader.Exceptions;
using CsvReader.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CsvReader
{
    public static class CsvHelper
    {
        public static IEnumerable<T> ImportCsv<T>(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            if (lines == null || lines.Length == 0)
                throw new CsvReaderException("File is empty.");

            var propertyInfo = typeof(T).GetProperties();

            var headers = lines[0].SplitString().ToList();

            if (headers.Count != propertyInfo.Count())
            {
                throw new CsvReaderException("The number of the columns don't match with number of the properties. Try selecting partial mode");
            }

            Dictionary<string, int> indexList = CreateMatch(propertyInfo, headers);

            for (int i = 1; i < lines.Length; i++)
            {
                var csvProp = lines[i].SplitString().ToList();
                var item = Activator.CreateInstance<T>();

                foreach (var prop in propertyInfo)
                {
                    var type = prop.PropertyType;
                    var index = indexList[prop.Name];
                    var valueOfItem = csvProp[index];

                    var value = Convert.ChangeType(valueOfItem, type);
                    prop.SetValue(item, value, null);
                }
                yield return item;  
            }
        }

        private static Dictionary<string, int> CreateMatch(
            PropertyInfo[] propertyInfo, 
            List<string> headers)
        {
            try
            {
                var result = new Dictionary<string, int>();
                foreach (var prop in propertyInfo)
                {
                    var columnsName = prop.GetColumnNameOrNull();

                    var index = headers.IndexOf(columnsName??prop.Name);
                    result[prop.Name] = index;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new CsvReaderException("The columns in file don't match with those in properties.", ex);
            }
            
        }


    }
}
