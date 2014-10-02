using System;
using System.Reflection;

namespace DataTablesGenerics.Grid.Mapper
{
    public class LambdaConventionPropertyContributor : SimplePropertyContributor
    {
        private readonly Func<PropertyInfo, PropertyInfo, bool> _convention;

        public LambdaConventionPropertyContributor(Func<PropertyInfo, PropertyInfo, bool> convention)
        {
            this._convention = convention;
        }

        protected override bool MatchProperty(PropertyInfo fromProperty, PropertyInfo toProperty)
        {
            return _convention(fromProperty, toProperty);
        }
    }
}