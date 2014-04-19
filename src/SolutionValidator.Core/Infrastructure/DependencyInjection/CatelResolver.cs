using Catel.IoC;

namespace SolutionValidator.Infrastructure.DependencyInjection
{
    public class CatelResolver : IResolver
    {
        public T Resolve<T>()
        {
            return ServiceLocator.Default.ResolveType<T>();
        }
    }
}
