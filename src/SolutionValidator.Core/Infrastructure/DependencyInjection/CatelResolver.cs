using Catel.IoC;

namespace SolutionValidator.Core.Infrastructure.DependencyInjection
{
    public class CatelResolver : IResolver
    {
        public T Resolve<T>()
        {
            return ServiceLocator.Default.ResolveType<T>();
        }
    }
}
