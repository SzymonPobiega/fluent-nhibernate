namespace FluentNHibernate.Mapping
{
    public class PropertyAccessStrategy : BaseAccessStrategy
    {
        public override bool Matches(Prefix prefix, Member property)
        {
            return property.CanWrite;
        }
        public override string BuildValue(Prefix prefix)
        {
            return "property";
        }
    }
}