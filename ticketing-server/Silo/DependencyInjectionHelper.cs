using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Silo.FakeData;

namespace Silo
{
    public static class DependencyInjectionHelper
    {
        public static void IocContainerRegistration(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IShowRepository, FakeShowRepository>();
        }
    }
}