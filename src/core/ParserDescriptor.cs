using System.Collections.Generic;
using System.ComponentModel;

namespace System.Linq.Expressions
{
    /// <summary>
    ///
    /// </summary>
    public class ParserDescriptor
    {
        private readonly List<IParser> _parsers = new List<IParser>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="parser"></param>
        public void AddParser(IParser parser)
            => _parsers.Add(parser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converter"></param>
        /// <returns></returns>
        public IParser GetParser(TypeConverter converter)
        {
            switch (converter)
            {
                case DoubleConverter _:
                    return _parsers.FirstOrDefault(x => x is IParser<double>);

                case DateTimeConverter _:
                    return _parsers.FirstOrDefault(x => x is IParser<DateTime>);

                case DecimalConverter _:
                    return _parsers.FirstOrDefault(x => x is IParser<decimal>);

                case NullableConverter _:
                    Type underlyingType = (converter as NullableConverter)?.UnderlyingType;

                    if (underlyingType == typeof(decimal))
                        return GetParser(new DecimalConverter());
                    else if (underlyingType == typeof(DateTime))
                        return GetParser(new DateTimeConverter());
                    else if (underlyingType == typeof(double))
                        return GetParser(new DoubleConverter());

                    break;
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IParser this[int index]
        {
            get => _parsers[index];
            set => _parsers[index] = value;
        }
    }
}