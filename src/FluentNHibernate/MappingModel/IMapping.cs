using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel
{
    public interface IHasMappedProperties : IMapping
    {
        IEnumerable<PropertyMapping> Properties { get; }
        void AddProperty(PropertyMapping property);
    }
    public interface IHasMappedCollections : IMapping
    {
        IEnumerable<ICollectionMapping> Collections { get; }
        void AddCollection(ICollectionMapping collection);
    }
    public interface IHasMappedReferences : IMapping
    {
        IEnumerable<ManyToOneMapping> References { get; }
        void AddReference(ManyToOneMapping manyToOne);
    }
    public interface IHasMappedComponents : IMapping
    {
        IEnumerable<IComponentMapping> Components { get; }
        void AddComponent(IComponentMapping component);
    }
    public interface IHasMappedOneToOnes : IMapping
    {
        IEnumerable<OneToOneMapping> OneToOnes { get; }
        void AddOneToOne(OneToOneMapping mapping);
    }
    public interface IHasMappedAnys : IMapping
    {
        IEnumerable<AnyMapping> Anys { get; }
        void AddAny(AnyMapping mapping);

    }
    public interface IHasMappedFilters : IMapping
    {
        IEnumerable<FilterMapping> Filters { get; }
        void AddFilter(FilterMapping mapping);
    }
    public interface IMapping
    {
        Type Type { get; set; }
    }
    public interface IHasMappedAllMembers :
        IHasMappedProperties,
        IHasMappedCollections,
        IHasMappedReferences,
        IHasMappedComponents,
        IHasMappedOneToOnes,
        IHasMappedAnys,
        IHasMappedFilters
    {
    }
    public interface IClassMapping : IHasMappedAllMembers        
    {
        IIdentityMapping Id { get; set; }
        VersionMapping Version { get; set; }
    }
}