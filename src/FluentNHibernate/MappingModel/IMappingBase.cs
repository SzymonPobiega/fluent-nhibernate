using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    public interface IMappingBase
    {
        void AcceptVisitor(IMappingModelVisitor visitor);
        bool HasUserDefinedValue(Attr property);
    }
}