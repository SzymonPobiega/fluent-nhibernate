using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using Prefix=FluentNHibernate.Mapping.Prefix;

namespace FluentNHibernate.Automapping
{
    public class AutoMapProperty : IAutoMapper
    {
        private readonly BaseAccessStrategy[] accessStrategies = 
            new BaseAccessStrategy[] {
                new CamelCaseFieldAccessStrategy(), 
                new PropertyAccessStrategy()};

        private readonly Prefix[] prefixes = new[]
        {
            Prefix.m, 
            Prefix.mUnderscore, 
            Prefix.None, 
            Prefix.Underscore
        };

        private readonly IConventionFinder conventionFinder;
        private readonly AutoMappingExpressions expressions;

        public AutoMapProperty(IConventionFinder conventionFinder, AutoMappingExpressions expressions)
        {
            this.conventionFinder = conventionFinder;
            this.expressions = expressions;
        }

        private bool CanInferAccessType(Member property)
        {
            return InferAccessType(property) != null;
        }

        private string InferAccessType(Member property)
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

        public bool MapsProperty(Member property)
        {            
            return CanInferAccessType(property) && 
                HasExplicitTypeConvention(property) || IsMappableToColumnType(property);
        }

        private bool HasExplicitTypeConvention(Member property)
        {
            // todo: clean this up!
            //        What it's doing is finding if there are any IUserType conventions
            //        that would be applied to this property, if there are then we should
            //        definitely automap it. The nasty part is that right now we don't have
            //        a model, so we're having to create a fake one so the convention will
            //        apply to it.
            var conventions = conventionFinder
                .Find<IPropertyConvention>()
                .Where(c =>
                {
                    if (!typeof(IUserTypeConvention).IsAssignableFrom(c.GetType()))
                        return false;

                    var criteria = new ConcreteAcceptanceCriteria<IPropertyInspector>();
                    var acceptance = c as IConventionAcceptance<IPropertyInspector>;
                    
                    if (acceptance != null)
                        acceptance.Accept(criteria);

                    return criteria.Matches(new PropertyInspector(new PropertyMapping
                    {
                        Type = new TypeReference(property.PropertyType),
                        Member = property
                    }));
                });

            return conventions.FirstOrDefault() != null;
        }

        private static bool IsMappableToColumnType(Member property)
        {
            return property.PropertyType.Namespace == "System"
                    || property.PropertyType.FullName == "System.Drawing.Bitmap"
                    || property.PropertyType.IsEnum;
        }

        public void Map(ClassMappingBase classMap, Member property)
        {
            classMap.AddProperty(GetPropertyMapping(classMap.Type, property, classMap as ComponentMapping));
        }

        private PropertyMapping GetPropertyMapping(Type type, Member property, ComponentMapping component)
        {
            var mapping = new PropertyMapping
            {
                ContainingEntityType = type,
                Member = property
            };

            var columnName = property.Name;
            
            if (component != null)
                columnName = expressions.GetComponentColumnPrefix(component.Member) + columnName;

            mapping.AddDefaultColumn(new ColumnMapping { Name = columnName });
            mapping.Access = InferAccessType(property);
            

            if (!mapping.IsSpecified("Name"))
                mapping.Name = mapping.Member.Name;

            if (!mapping.IsSpecified("Type"))
                mapping.SetDefaultValue("Type", GetDefaultType(property));

            return mapping;
        }

        private TypeReference GetDefaultType(Member property)
        {
            var type = new TypeReference(property.PropertyType);

            if (property.PropertyType.IsEnum())
                type = new TypeReference(typeof(GenericEnumMapper<>).MakeGenericType(property.PropertyType));

            if (property.PropertyType.IsNullable() && property.PropertyType.IsEnum())
                type = new TypeReference(typeof(GenericEnumMapper<>).MakeGenericType(property.PropertyType.GetGenericArguments()[0]));

            return type;
        }

    }
}