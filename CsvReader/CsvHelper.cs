using CsvReader.Attributes;
using CsvReader.Enums;
using CsvReader.Exceptions;
using CsvReader.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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

        public static IEnumerable<T> ImportCsv<T>(string filePath, CsvReadOptions options)
        {
            var lines = File.ReadAllLines(filePath);
            if (lines == null || lines.Length == 0)
                throw new CsvReaderException("File is empty.");

            var propertyInfo = typeof(T).GetProperties();

            var headers = lines[0].SplitString().ToList();

            Dictionary<string, int> indexList = CreateMatch(propertyInfo, headers, options);

            for (int i = 1; i < lines.Length; i++)
            {
                var csvProp = lines[i].SplitString().ToList();
                var item = Activator.CreateInstance<T>();

                foreach (var prop in propertyInfo)
                {
                    var type = prop.PropertyType;
                    var index = indexList[prop.Name];

                    if (index >= 0)
                    {
                        var valueOfItem = csvProp[index];

                        var value = Convert.ChangeType(valueOfItem, type);
                        prop.SetValue(item, value, null);
                    }
                }
                yield return item;
            }
        }

        private static Dictionary<string, int> CreateMatch(
            PropertyInfo[] propertyInfo, 
            List<string> headers,
            CsvReadOptions options=CsvReadOptions.Full)
        {
            try
            {
                var result = new Dictionary<string, int>();
                foreach (var prop in propertyInfo)
                {
                    var columnsName = prop.GetColumnNameOrNull();

                    var index = headers.IndexOf(columnsName??prop.Name);

                    if(options==CsvReadOptions.Full && index==-1)
                        throw new CsvReaderException("The number of the columns don't match with number of the properties. Try selecting partial mode");

                    result[prop.Name] = index;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new CsvReaderException("The columns in file don't match with those in properties.", ex);
            }
            
        }

        public static void ExportCsv<T>(List<T> items, StreamWriter sw)
        {
            var fileText = new StringBuilder(); 
            var lineBuilder = new StringBuilder(); 
            var properties=typeof(T).GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                var name = (properties[i].GetColumnNameOrNull() ?? properties[i].Name).QuoteStringWithComma();
                lineBuilder.Append(name);
                if(i<properties.Length-1)
                    lineBuilder.Append(',');
            }
            
            fileText.Append(lineBuilder.ToString());
            fileText.Append(Environment.NewLine);
            lineBuilder.Clear();

            foreach (var item in items)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item, null).ToString().QuoteStringWithComma();
                    lineBuilder.Append(value);
                    if(i<properties.Length-1)
                        lineBuilder.Append(',');
                }
                
                fileText.Append(lineBuilder);
                fileText.Append(Environment.NewLine);
                lineBuilder.Clear();
            }

            //var file=File.Create(filePath);
            sw.Write(fileText.ToString());
        }

    }
}
