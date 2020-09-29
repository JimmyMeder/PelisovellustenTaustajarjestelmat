using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;


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
        public void new_player(string name)
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




        [HttpGet]
        [Route("get/?minScore=x")]
        public async Task<Player[]> GetPlayers(int x)
        {
            return await _irepository.GetPlayersWithXscore(x);
        }

        [HttpGet]
        [Route("get/?name={name}")]
        public async Task<Player> GetPlayerWithName(string name)
        {
            return await _irepository.GetPlayerWithName(name);
        }

        [HttpGet]
        [Route("get/?tag={tag}")]
        public async Task<Player[]> GetPlayersWithTag(string tag)
        {
            return await _irepository.GetPlayersWithTag(tag);
        }
        [HttpGet]
        [Route("get/items/?level={level}")]
        public async Task<Player[]> GetPlayersWithItemLevel(int level)
        {
            return await _irepository.GetPlayersWithItemProperty(level);
        }

        [HttpGet]
        [Route("get/items/?amount={amount}")]
        public async Task<Player[]> GetPlayersWithItemAmount(int amount)
        {
            return await _irepository.GetPlayersWithXItems(amount);
        }

        [HttpGet]
        [Route("get/?playerId={playerId}")]
        public async Task<UpdateResult> UpdatePlayerName(Guid playerId, [FromBody] string newName)
        {
            return await _irepository.UpdatePlayerName(playerId, newName);
        }

        [HttpGet]
        [Route("increment/?playerId={playerId}")]
        public async Task<UpdateResult> IncrementPlayerScore(Guid playerId, [FromBody] int score)
        {
            return await _irepository.IncrementPlayerScore(playerId, score);
        }

        [HttpGet]
        [Route("addItem/?playerId={playerId}")]
        public async Task<UpdateResult> AddItemToPlayer(Guid playerId, [FromBody] Item item)
        {
            return await _irepository.AddItemToPlayer(playerId, item);
        }

        [HttpGet]
        [HttpDelete]
        [Route("{playerId:Guid}/items/{itemId:Guid}")]
        public async Task<Player> AddItemAndIncrementPlayer(Guid playerId, Guid itemId)
        {
            return await _irepository.AddItemAndIncrementPlayer(playerId, itemId);
        }

        [HttpGet]
        [Route("getTopTen")]
        public async Task<List<Player>> GetTopTenPlayers()
        {
            return await _irepository.GetTopTenPlayers();
        }

        [HttpGet]
        [Route("getMostCommon")]
        public async Task<LevelCount> GetMostCommonLevel()
        {
            return await _irepository.GetMostCommonLevel();
        }

    }
}