using System.Collections.Generic;

namespace System.Linq.Expressions.Internals
{
    /// <summary>
    /// Fluent switch case builder
    /// </summary>
    internal static class Switch
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        internal static SwitchBuilder<TElement>.CaseBuilder On<TElement>(TElement element)
            => new SwitchBuilder<TElement>(element).Start();

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        internal class SwitchBuilder<TElement>
        {
            private readonly TElement _element;
            private TElement _firstCase;

            /// <summary>
            /// Initializes a new instance of the <see cref="SwitchBuilder{TElement}"/> class
            /// </summary>
            /// <param name="element"></param>
            internal SwitchBuilder(TElement element)
            {
                _element = element;
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            internal CaseBuilder Start()
                => new CaseBuilder { Switch = this };

            /// <summary>
            ///
            /// </summary>
            /// <param name="element"></param>
            /// <returns></returns>
            private ThenBuilder Case(TElement element)
            {
                _firstCase = element;
                return new ThenBuilder { Switch = this };
            }

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="TResult"></typeparam>
            /// <param name="result"></param>
            /// <returns></returns>
            private SwitchBuilder<TElement, TResult>.CaseBuilder Then<TResult>(TResult result)
                => new SwitchBuilder<TElement, TResult>(_element, _firstCase, result).Start();

            /// <summary>
            ///
            /// </summary>
            internal class CaseBuilder
            {
                /// <summary>
                ///
                /// </summary>
                internal SwitchBuilder<TElement> Switch { get; set; }

                /// <summary>
                ///
                /// </summary>
                /// <param name="element"></param>
                /// <returns></returns>
                internal ThenBuilder Case(TElement element)
                    => Switch.Case(element);
            }

            /// <summary>
            ///
            /// </summary>
            internal class ThenBuilder
            {
                /// <summary>
                ///
                /// </summary>
                internal SwitchBuilder<TElement> Switch { get; set; }

                /// <summary>
                ///
                /// </summary>
                /// <typeparam name="TResult"></typeparam>
                /// <param name="result"></param>
                /// <returns></returns>
                internal SwitchBuilder<TElement, TResult>.CaseBuilder Then<TResult>(TResult result)
                    => Switch.Then(result);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        internal class SwitchBuilder<TElement, TResult>
        {
            private readonly TElement _element;
            private TElement _currentCase;
            private readonly IDictionary<TElement, TResult> _map = new Dictionary<TElement, TResult>();

            /// <summary>
            /// Initializes a new instance of the <see cref="SwitchBuilder{TElement}"/> class
            /// </summary>
            /// <param name="element"></param>
            /// <param name="firstCase"></param>
            /// <param name="firstResult"></param>
            internal SwitchBuilder(TElement element, TElement firstCase, TResult firstResult)
            {
                _element = element;
                _map.Add(firstCase, firstResult);
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            internal CaseBuilder Start()
                => new CaseBuilder { Switch = this };

            /// <summary>
            ///
            /// </summary>
            /// <param name="element"></param>
            /// <returns></returns>
            private ThenBuilder Case(TElement element)
            {
                _currentCase = element;
                return new ThenBuilder { Switch = this };
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="result"></param>
            /// <returns></returns>
            private CaseBuilder Then(TResult result)
            {
                _map.Add(_currentCase, result);
                return new CaseBuilder { Switch = this };
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="defaultResult"></param>
            /// <returns></returns>
            private TResult Default(TResult defaultResult)
                => _map.TryGetValue(_element, out TResult result) ? result : defaultResult;

            /// <summary>
            ///
            /// </summary>
            internal class CaseBuilder
            {
                /// <summary>
                ///
                /// </summary>
                internal SwitchBuilder<TElement, TResult> Switch { get; set; }

                /// <summary>
                ///
                /// </summary>
                /// <param name="element"></param>
                /// <returns></returns>
                internal ThenBuilder Case(TElement element)
                    => Switch.Case(element);

                /// <summary>
                ///
                /// </summary>
                /// <param name="defaultResult"></param>
                /// <returns></returns>
                internal TResult Default(TResult defaultResult)
                    => Switch.Default(defaultResult);
            }

            /// <summary>
            ///
            /// </summary>
            internal class ThenBuilder
            {
                /// <summary>
                ///
                /// </summary>
                internal SwitchBuilder<TElement, TResult> Switch { get; set; }

                /// <summary>
                ///
                /// </summary>
                /// <param name="result"></param>
                /// <returns></returns>
                internal CaseBuilder Then(TResult result)
                    => Switch.Then(result);
            }
        }
    }
}