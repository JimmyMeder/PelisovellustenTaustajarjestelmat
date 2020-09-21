/*using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Assignment2.Controllers
{
    [ApiController]
    [Route("Player")]
    class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly IRepository _irepository;

        public PlayersController(ILogger<PlayersController> logger, IRepository irepository)
        {
            _logger = logger;
            _irepository = irepository;
        }
        [HttpGet]
        [Route("Get")]
        public async Task<Player> Get(string id)
        {
            return await _irepository.Get(id);
        }
        public async Task<Player[]> GetAll()
        {
            return await _irepository.GetAll();
        }
        public async Task<Player> Create(Player player)
        {

            return await _irepository.Create(player);
        }
        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
        {

            return await _irepository.Modify(id, player);
        }
        public async Task<Player> Delete(Guid id)
        {
            return await _irepository.Delete(id);
        }
    }
}*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Assignment2.Controllers
{
    [ApiController]
    [Route("player")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly IRepository _irepository;

        public PlayersController(ILogger<PlayersController> logger, IRepository irepository)
        {
            _logger = logger;
            _irepository = irepository;
        }

        [HttpGet]
        [Route("newPlayer/{name}")]
        public void new_new_player(string name)
        {
            NewPlayer newPlayer = new NewPlayer();
            newPlayer.Name = name;
            Create(newPlayer);
        }


        public Player Create(NewPlayer newPlayer)
        {
            _logger.LogInformation("Creating new player with name " + newPlayer.Name);
            Player player = new Player()
            {
                Id = Guid.NewGuid(),
                Name = newPlayer.Name
            };
            _logger.LogInformation("Name now: " + player.Name);
            _logger.LogInformation("ID now: " + player.Id);
            _irepository.Create(player);
            return player;
        }

        [NotImplExceptionFilter]
        [HttpGet]
        [Route("ListPlayers")]
        public Task<Player[]> GetAll()
        {
            Task<Player[]> list_players = _irepository.GetAll();
            return list_players;
        }

        [HttpPost]
        [Route("mod/{id:Guid}")]
        public async Task<Player> Modify(Guid id, [FromBody] ModifiedPlayer player)
        {
            await _irepository.Modify(id, player);
            return null;
        }

        [HttpPost]
        [Route("delete/{id:Guid}")]
        public async Task<Player> Delete(Guid id)
        {
            await _irepository.Delete(id);
            return null;
        }
    }
}