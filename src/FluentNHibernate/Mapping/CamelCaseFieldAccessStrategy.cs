using System.Reflection;

namespace FluentNHibernate.Mapping
{
    public class CamelCaseFieldAccessStrategy : BaseAccessStrategy
    {        
        private const string InvalidPrefixCamelCaseFieldM = "m is not a valid prefix for a CamelCase Field.";
        private const string InvalidPrefixCamelCaseFieldMUnderscore = "m_ is not a valid prefix for a CamelCase Field.";

        public override bool ValidatePrefix(Prefix prefix, out string errorMessage)
        {
            if(prefix == Prefix.m)
            {
                errorMessage = InvalidPrefixCamelCaseFieldM;
                return false;
            }
            if (prefix == Prefix.mUnderscore)
            {
                errorMessage = InvalidPrefixCamelCaseFieldMUnderscore;
                return false;
            }
            errorMessage = null;
            return true;
        }

        public override bool Matches(Prefix prefix, Member property)
        {
            string propertyName = property.Name;
            string fieldName = string.Concat(prefix.Value, char.ToLower(propertyName[0]), propertyName.Substring(1));
            var field = property.DeclaringType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return field != null;
        }

        public override string BuildValue(Prefix prefix)
        {
            return "field.camelcase" + prefix.Representation;
        }
    }
}