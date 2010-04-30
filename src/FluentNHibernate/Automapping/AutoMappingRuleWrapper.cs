using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Automapping
{
    public class AutoMappingRuleWrapper<T> : IAutoMappingRule       
    {
        readonly IAutoMapper<T> mapper;

        public AutoMappingRuleWrapper(IAutoMapper<T> mapper)
        {
            this.mapper = mapper;
        }

        public bool MapsProperty(IMapping map, Member property)
        {
            return map is T && mapper.MapsProperty(property);
        }
        public void Map(IMapping map, Member property)
        {
            mapper.Map((T)map, property);
        }
    }
}