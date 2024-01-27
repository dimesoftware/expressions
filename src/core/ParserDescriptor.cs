using System.Collections.Generic;
using System.ComponentModel;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Equivalent of the <see cref="TypeDescriptor"/> but then for the <see cref="IParser"/>
    /// </summary>
    public class ParserDescriptor
    {
        private readonly List<IParser> _parsers = new();

        /// <summary>
        /// Adds the parser to the descriptor
        /// </summary>
        /// <param name="parser">The parser to add</param>
        public void AddParser(IParser parser)
            => _parsers.Add(parser);

        /// <summary>
        /// Gets the <see cref="IParser"/> that corresponds to the <paramref name="converter"/>
        /// </summary>
        /// <param name="converter">The type converter</param>
        /// <returns>The compatible parser that complements the type converter</returns>
        public IParser GetParser(TypeConverter converter)
        {
            switch (converter)
            {
                case DoubleConverter _:
                    return _parsers.FirstOrDefault(x => x is IParser<double>);

                case DateTimeConverter _:
                    return _parsers.FirstOrDefault(x => x is IParser<DateTime>);

                case DateOnlyConverter _:
                    return _parsers.FirstOrDefault(x => x is IParser<DateOnly>);

                case DecimalConverter _:
                    return _parsers.FirstOrDefault(x => x is IParser<decimal>);

                case NullableConverter _:
                    Type underlyingType = (converter as NullableConverter)?.UnderlyingType;

                    if (underlyingType == typeof(decimal))
                        return GetParser(new DecimalConverter());
                    else if (underlyingType == typeof(DateTime))
                        return GetParser(new DateTimeConverter());
                    else if (underlyingType == typeof(DateOnly))
                        return GetParser(new DateOnlyConverter());
                    else if (underlyingType == typeof(double))
                        return GetParser(new DoubleConverter());

                    break;
            }

            return null;
        }

        /// <summary>
        /// Gets or sets the index
        /// </summary>
        /// <param name="index">The index to get or set</param>
        /// <returns>An IParser that matches the index</returns>
        public IParser this[int index]
        {
            get => _parsers[index];
            set => _parsers[index] = value;
        }
    }
}