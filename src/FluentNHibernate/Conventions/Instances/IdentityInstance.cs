using System;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Instances
{
    public class IdentityInstance : IdentityInspector, IIdentityInstance
    {
        readonly ColumnInstanceHelper c;
        readonly IdMapping mapping;
        bool nextBool = true;

        public IdentityInstance(IdMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
            c = new ColumnInstanceHelper(mapping);
        }

        public void Column(string columnName)
        {
            if (mapping.Columns.UserDefined.Count() > 0)
                return;

            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : originalColumn; // TODO: Fix

            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }

        public new void UnsavedValue(string unsavedValue)
        {
            if (!mapping.HasUserDefinedValue(Attr.UnsavedValue))
                mapping.UnsavedValue = unsavedValue;
        }

        public void CustomType(string type)
        {
            if (!mapping.HasUserDefinedValue(Attr.Type))
                mapping.Type = new TypeReference(type);
        }

        public void CustomType(Type type)
        {
            if (!mapping.HasUserDefinedValue(Attr.Type))
                mapping.Type = new TypeReference(type);
        }

        public void CustomType<T>()
        {
            CustomType(typeof(T));
        }

        public new IAccessInstance Access
        {
            get
            {
                return new AccessInstance(value =>
                {
                    if (!mapping.HasUserDefinedValue(Attr.Access))
                        mapping.Access = value;
                });
            }
        }

        public IGeneratorInstance GeneratedBy
        {
            get
            {
                if (!mapping.HasUserDefinedValue(Attr.Generator))
                    mapping.Generator = new GeneratorMapping();
                
                return new GeneratorInstance(mapping.Generator, mapping.Type.GetUnderlyingSystemType());
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IIdentityInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public new void Length(int length)
        {
            if (c.ThisOrColumnHasUserDefinedValue(Attr.Length))
                return;

            c.SetOnEachColumn(x => x.Length = length);
        }

        public new void Precision(int precision)
        {
            if (c.ThisOrColumnHasUserDefinedValue(Attr.Precision))
                return;

            c.SetOnEachColumn(x => x.Precision = precision);
        }

        public new void Scale(int scale)
        {
            if (c.ThisOrColumnHasUserDefinedValue(Attr.Scale))
                return;

            c.SetOnEachColumn(x => x.Scale = scale);
        }

        public new void Nullable()
        {
            if (!c.ThisOrColumnHasUserDefinedValue(Attr.NotNull))
                c.SetOnEachColumn(x => x.NotNull = !nextBool);

            nextBool = true;
        }

        public new void Unique()
        {
            if (!c.ThisOrColumnHasUserDefinedValue(Attr.Unique))
                c.SetOnEachColumn(x => x.Unique = nextBool);

            nextBool = true;
        }

        public new void UniqueKey(string columns)
        {
            if (c.ThisOrColumnHasUserDefinedValue(Attr.UniqueKey))
                return;

            c.SetOnEachColumn(x => x.UniqueKey = columns);
        }

        public void CustomSqlType(string sqlType)
        {
            if (c.ThisOrColumnHasUserDefinedValue(Attr.SqlType))
                return;

            c.SetOnEachColumn(x => x.SqlType = sqlType);
        }

        public new void Index(string index)
        {
            if (c.ThisOrColumnHasUserDefinedValue(Attr.Index))
                return;

            c.SetOnEachColumn(x => x.Index = index);
        }

        public new void Check(string constraint)
        {
            if (c.ThisOrColumnHasUserDefinedValue(Attr.Check))
                return;

            c.SetOnEachColumn(x => x.Check = constraint);
        }

        public new void Default(object value)
        {
            if (c.ThisOrColumnHasUserDefinedValue(Attr.Default))
                return;

            c.SetOnEachColumn(x => x.Default = value.ToString());
        }
    }
}