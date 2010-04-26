namespace FluentNHibernate.Mapping
{
    public abstract class BaseAccessStrategy
    {
        public virtual bool ValidatePrefix(Prefix prefix, out string errorMessage)
        {
            errorMessage = null;
            return true;
        }

        public abstract bool Matches(Prefix prefix, Member property);
        public abstract string BuildValue(Prefix prefix);
    }
}