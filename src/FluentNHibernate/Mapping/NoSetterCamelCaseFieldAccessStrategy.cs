namespace FluentNHibernate.Mapping
{
    public class NoSetterCamelCaseFieldAccessStrategy : CamelCaseFieldAccessStrategy
    {
        public override string BuildValue(Prefix prefix)
        {
            return "nosetter.camelcase" + prefix.Representation;
        }
    }
}