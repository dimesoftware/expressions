namespace System.Linq.Expressions.Internals
{
    internal class Switcher<T>
    {
        public Decision Case(Predicate<T> condition, Action<T> branch)
            => new Decision { Condition = condition, Branch = branch };

        public Decision Default(Action<T> branch)
            => new Decision { Condition = x => true, Branch = branch };

        public void Switch(T instance, params Decision[] cases)
        {
            foreach (var @case in cases)
            {
                if (!@case.Condition(instance))
                    continue;

                @case.Branch(instance);
                break;
            }
        }

        public class Decision
        {
            public Predicate<T> Condition { get; set; }
            public Action<T> Branch { get; set; }
        }
    }
}