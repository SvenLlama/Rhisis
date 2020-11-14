using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Resources.Studio.Models;

namespace Rhisis.Resources.Studio.Controllers
{
    [ApiController]
    [Route("item-data")]
    public class ItemDataController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IGameResources _gameResources;

        public ItemDataController(ILogger<ItemDataController> logger, IGameResources gameResources)
        {
            _logger = logger;
            _gameResources = gameResources;
        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<ItemDataModel> Get()
        {
            return _gameResources.Items?.Values.Select(i => new ItemDataModel(i)) ?? Enumerable.Empty<ItemDataModel>();
        }
    }
}
