using System;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public class AutoMapManyToOne : AutoMapFeature, IAutoMapper<IHasMappedReferences>
    {
        private readonly AutoMappingExpressions expressions;

        public AutoMapManyToOne(AutoMappingExpressions expressions)
        {
            this.expressions = expressions;
        }

        
        public bool MapsProperty(Member property)
        {
            return CanInferAccessType(property) && IsEntityType(property);
        }

        private static bool IsEntityType(Member p)
        {
            return
                p.PropertyType.Namespace != "System" && // ignore clr types (won't be entities)
                    p.PropertyType.Namespace != "System.Collections.Generic" &&
                        p.PropertyType.Namespace != "Iesi.Collections.Generic" &&
                            !p.PropertyType.IsEnum;
        }


        public void Map(IHasMappedReferences classMap, Member property)
        {
            var manyToOne = CreateMapping(property, classMap as ComponentMapping);
            classMap.AddReference(manyToOne);
        }

        private ManyToOneMapping CreateMapping(Member property, ComponentMapping component)
        {
            var mapping = new ManyToOneMapping { Member = property };

            mapping.Access = InferAccessType(property);
            mapping.SetDefaultValue(x => x.Name, property.Name);
            mapping.SetDefaultValue(x => x.Class, new TypeReference(property.PropertyType));

            var columnName = property.Name;

            if (component != null)
                columnName = expressions.GetComponentColumnPrefix(component.Member) + columnName;

            mapping.AddDefaultColumn(new ColumnMapping { Name = columnName + "_id" });


            return mapping;
        }
    }
}
