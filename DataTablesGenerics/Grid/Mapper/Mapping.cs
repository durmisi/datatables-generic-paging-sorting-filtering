using System;
using System.Linq.Expressions;

namespace DataTablesGenerics.Grid.Mapper
{
    public class Mapping
    {
        public Mapping(Type @from, Type to)
        {
            From = from;
            To = to;
        }

        public Type From { get; set; }
        public Type To { get; set; }
        public Expression Value { get; set; }
    }
}