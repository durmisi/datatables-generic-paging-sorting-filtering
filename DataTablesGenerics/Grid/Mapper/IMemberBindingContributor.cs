using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataTablesGenerics.Grid.Mapper
{
    public interface IMemberBindingContributor
    {
        IEnumerable<MemberBinding> GetBindings<TFrom, TTo>(Expression parameter);
    }
}
