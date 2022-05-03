using System.Collections.Generic;
using System.Text;

namespace CsvReader.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitString(this string text)
        {
            bool isQuoteStarted = false;

            var builder = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '"')
                {
                    isQuoteStarted = !isQuoteStarted;
                }
                else if (text[i] == ',')
                {
                    if (!isQuoteStarted)
                    {
                        yield return builder.ToString();
                        builder.Clear();
                    }
                    else
                    {
                        builder.Append(text[i]);
                    }
                }
                else
                {
                    builder.Append(text[i]);
                }
            }
            yield return builder.ToString();
        }

        public static IEnumerable<string> SplitString(this string text, char separator=',', char combiner='"')
        {
            bool isQuoteStarted = false;

            var builder = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == combiner)
                {
                    isQuoteStarted = !isQuoteStarted;
                }
                else if (text[i] == separator)
                {
                    if (!isQuoteStarted)
                    {
                        yield return builder.ToString();
                        builder.Clear();
                    }
                    else
                    {
                        builder.Append(text[i]);
                    }
                }
                else
                {
                    builder.Append(text[i]);
                }
            }
            yield return builder.ToString();
        }

        public static string QuoteStringWithComma(this string text)
        {
            var builder= new StringBuilder();
            if(text.IndexOf(',')>0)
            {
                builder.Append('"');
                builder.Append(text);
                builder.Append('"');
                return builder.ToString();
            }
            return text;
        }

    }
}
