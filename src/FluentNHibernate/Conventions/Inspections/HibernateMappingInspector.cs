using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class HibernateMappingInspector : IHibernateMappingInspector
    {
        private readonly HibernateMapping mapping;

        public HibernateMappingInspector(HibernateMapping mapping)
        {
            this.mapping = mapping;
        }

        public Type EntityType
        {
            get { return mapping.Classes.First().Type; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Classes.First().Name; }
        }

        public bool IsSet(Attr property)
        {
            return mapping.HasUserDefinedValue(property);
        }

        public string Catalog
        {
            get { return mapping.Catalog; }
        }

        public Access DefaultAccess
        {
            get { return Access.FromString(mapping.DefaultAccess); }
        }

        public Cascade DefaultCascade
        {
            get { return Cascade.FromString(mapping.DefaultCascade); }
        }

        public bool DefaultLazy
        {
            get { return mapping.DefaultLazy; }
        }

        public bool AutoImport
        {
            get { return mapping.AutoImport; }
        }

        public string Schema
        {
            get { return mapping.Schema; }
        }
    }
}