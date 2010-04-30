using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public class AutoMapComponent : AutoMapFeature, IAutoMapper<IHasMappedComponents>
    {
        private readonly AutoMappingExpressions expressions;
        private readonly AutoMapper mapper;

        public AutoMapComponent(AutoMappingExpressions expressions, AutoMapper mapper)
        {
            this.expressions = expressions;
            this.mapper = mapper;
        }

        public bool MapsProperty(Member property)
        {
            return CanInferAccessType(property) &&
                expressions.IsComponentType(property.PropertyType);
        }

        public void Map(IHasMappedComponents classMap, Member property)
        {
            var mapping = new ComponentMapping(ComponentType.Component)
            {
                Name = property.Name,
                Member = property,
                ContainingEntityType = classMap.Type,
                Type = property.PropertyType,
                Access = InferAccessType(property)
            };
            
            mapper.FlagAsMapped(property.PropertyType);
            mapper.MergeMap(property.PropertyType, mapping, new List<string>());

            classMap.AddComponent(mapping);
        }
    }
}