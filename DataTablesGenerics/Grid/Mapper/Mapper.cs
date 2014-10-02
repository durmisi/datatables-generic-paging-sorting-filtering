using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataTablesGenerics.Grid.Mapper
{
    public class Mapper
    {
        private readonly IMemberBindingContributor[] _bindingContributors;
        private readonly ICollection<Mapping> _mappings = new List<Mapping>();

        public Mapper()
            : this(new SimplePropertyContributor(), new ProjectedBindingContributor())
        {

        }
        public Mapper(params IMemberBindingContributor[] bindingContributors)
        {
            _bindingContributors = bindingContributors;
        }

        public void AddMap<TFrom, TTo>(Expression<Func<TFrom, TTo>> expression)
        {
            var mapping = new Mapping(typeof(TFrom), typeof(TTo))
            {
                Value = expression
            };
            _mappings.Add(mapping);
        }

        public void AddMap<TFrom, TTo>()
        {
            var mapping = new Mapping(typeof(TFrom), typeof(TTo))
            {
                Value = GenerateAutomaticExpression<TFrom, TTo>()
            };
            _mappings.Add(mapping);
        }

        private Expression GenerateAutomaticExpression<TFrom, TTo>()
        {
            var parameter = Expression.Parameter(typeof(TFrom));
            var newExpression = Expression.New(typeof(TTo));
            var body = Expression.MemberInit(newExpression, GenerateBindings<TFrom, TTo>(parameter).ToList());
            return Expression.Lambda(body, parameter);
        }

        private IEnumerable<MemberBinding> GenerateBindings<TFrom, TTo>(ParameterExpression parameter)
        {
            return _bindingContributors
                    .SelectMany(memberBindingContributor => memberBindingContributor.GetBindings<TFrom, TTo>(parameter));
        }
        
        public TTo Map<TFrom, TTo>(TFrom sampleEntity)
        {
            var mapper = _mappings.FirstOrDefault(m => m.From == typeof(TFrom) && m.To == typeof(TTo));
            return ((Expression<Func<TFrom, TTo>>)mapper.Value).Compile()(sampleEntity);
        }

        public Expression<Func<TFrom, TTo>> GetProjection<TFrom, TTo>()
        {
            return (Expression<Func<TFrom, TTo>>)_mappings.FirstOrDefault(m => m.From == typeof(TFrom) && m.To == typeof(TTo)).Value;
        }
    }
}
