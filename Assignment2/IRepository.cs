using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Assignment2
{
    public interface IRepository
    {
        Task<Player> Get(Guid id);
        Task<Player> GetPlayerWithName(string name);
        Task<Player[]> GetPlayersWithXscore(int x);
        Task<Player[]> GetPlayersWithTag(string x);
        Task<Player[]> GetPlayersWithItemProperty(int level);
        Task<Player[]> GetPlayersWithXItems(int itemAmount);
        Task<UpdateResult> UpdatePlayerName(Guid playerId, string name);
        Task<UpdateResult> IncrementPlayerScore(Guid playerId, int score);
        Task<UpdateResult> AddItemToPlayer(Guid playerId, Item item);
        Task<Player> AddItemAndIncrementPlayer(Guid playerId, Guid itemId);
        Task<List<Player>> GetTopTenPlayers();
        Task<LevelCount> GetMostCommonLevel();

        Task<Player[]> GetAll();
        Task<Player> Create(Player player);
        Task<Player> Modify(Guid id, ModifiedPlayer player);
        Task<Player> Delete(Guid id);


        Task<Item> CreateItem(Guid playerId, Item item);
        Task<Item> GetItem(Guid playerId, Guid itemId);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> UpdateItem(Guid playerId, Item item);
        Task<Item> DeleteItem(Guid playerId, Item item);
    }
}