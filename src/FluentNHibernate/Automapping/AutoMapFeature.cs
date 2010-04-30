using FluentNHibernate.Mapping;

namespace FluentNHibernate.Automapping
{
    public abstract class AutoMapFeature
    {
        readonly BaseAccessStrategy[] accessStrategies = 
            new BaseAccessStrategy[] {
                new CamelCaseFieldAccessStrategy(), 
                new PropertyAccessStrategy()};
        readonly Prefix[] prefixes = new[]
        {
            Prefix.m, 
            Prefix.mUnderscore, 
            Prefix.None, 
            Prefix.Underscore
        };

        protected bool CanInferAccessType(Member property)
        {
            return InferAccessType(property) != null;
        }

        protected string InferAccessType(Member property)
        {
            foreach (var accessStrategy in accessStrategies)
            {
                foreach (var prefix in prefixes)
                {
                    string message;
                    if (accessStrategy.ValidatePrefix(prefix, out message) &&
                        accessStrategy.Matches(prefix, property))
                    {
                        return accessStrategy.BuildValue(prefix);
                    }
                }
            }
            return null;
        }
    }
}