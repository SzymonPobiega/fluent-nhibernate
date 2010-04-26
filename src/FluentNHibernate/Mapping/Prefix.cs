namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Naming strategy prefix.
    /// </summary>
    public class Prefix
    {
        public static readonly Prefix None = new Prefix("", "");
        public static readonly Prefix Underscore = new Prefix("-underscore", "_");
        public static readonly Prefix m = new Prefix("-m", "m");
        public static readonly Prefix mUnderscore = new Prefix("-m-underscore", "m_");

        private readonly string representation;
        readonly string value;

        private Prefix(string representation, string value)
        {
            this.representation = representation;
            this.value = value;
        }

        public string Representation
        {
            get { return representation; }
        }

        public string Value
        {
            get { return value; }
        }
    }
}