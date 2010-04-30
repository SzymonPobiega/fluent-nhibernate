using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Automapping
{
    public interface IAutoMappingRule
    {
        bool MapsProperty(IMapping map, Member property);
        void Map(IMapping map, Member property);
    }
}