using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.Resources.Studio.HostedServices
{
    public class LoadResourcesHostedService : IHostedService, IDisposable
    {
        private readonly IGameResources _gameResources;

        public LoadResourcesHostedService(IGameResources gameResources, IConfiguration configuration)
        {
            GameResourcesConstants.Paths.WorkingDirectoryPath = configuration.GetValue<string>("WorkingDirectoryPath");
            _gameResources = gameResources;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _gameResources.Load(typeof(DefineLoader),
                typeof(TextLoader),
                typeof(MoverLoader),
                typeof(ItemLoader),
                typeof(DialogLoader),
                typeof(ShopLoader),
                typeof(JobLoader),
                typeof(SkillLoader),
                typeof(ExpTableLoader),
                typeof(PenalityLoader)
                );
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}
