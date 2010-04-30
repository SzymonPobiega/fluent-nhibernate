using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping
{    
    public class AutoMapComponentCollection : AutoMapFeature, IAutoMapper<IHasMappedCollections>
    {
        readonly AutoMappingExpressions expressions;
        readonly AutoKeyMapper keys;
        readonly AutoMapper mapper;

        public AutoMapComponentCollection(AutoMappingExpressions expressions, AutoMapper mapper)
        {
            this.expressions = expressions;
            this.mapper = mapper;
            keys = new AutoKeyMapper(expressions);
        }

        public bool MapsProperty(Member property)
        {
            if (!property.PropertyType.IsGenericType)
                return false;

            var childType = property.PropertyType.GetGenericArguments()[0];

            return CanInferAccessType(property) &&
                IsCollection(property.PropertyType) &&
                    expressions.IsComponentType(childType);            
        }

        static bool IsCollection(Type propertyType)
        {
            bool isEnumerable = propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
            bool isDerivedFromEnumerable = propertyType.ClosesInterface(typeof(IEnumerable<>));
            return isEnumerable || isDerivedFromEnumerable;
        }

        public void Map(IHasMappedCollections classMap, Member property)
        {
            if (property.DeclaringType != classMap.Type)
                return;

            var elementType = property.PropertyType.GetGenericArguments()[0];
            var mapping = new ListMapping();

            mapping.ContainingEntityType = classMap.Type;
            mapping.Member = property;
            mapping.TableName = elementType.Name;
            mapping.SetDefaultValue(x => x.Name, property.Name);
            mapping.Access = InferAccessType(property);

            SetIndex(classMap, mapping, elementType);
            SetCompositeElement(classMap, mapping, elementType);
            keys.SetKey(property, classMap, mapping);

            classMap.AddCollection(mapping);            
        }

        static void SetIndex(IMapping classMap, IIndexedCollectionMapping mapping, Type elementType)
        {
            var indexMapping = new IndexMapping
            {
                ContainingEntityType = classMap.Type,                
            };
            var columnMapping = new ColumnMapping
            {
                Name = elementType.Name + "Index"
            };
            indexMapping.AddColumn(columnMapping);

            mapping.Index = indexMapping;
        }

        private void SetCompositeElement(IMapping classMap, ICollectionMapping mapping, Type elementType)
        {
            var compositeElement = new CompositeElementMapping
            {
                Class = new TypeReference(elementType),
                ContainingEntityType = classMap.Type,                                
            };

            mapper.FlagAsMapped(elementType);
            mapper.MapEverythingInClass(compositeElement, elementType, new List<string>());

            mapping.SetDefaultValue(x => x.CompositeElement, compositeElement);
        }
    }
}