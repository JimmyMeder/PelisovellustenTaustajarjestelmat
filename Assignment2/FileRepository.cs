using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using MongoDB.Driver;


namespace Assignment2
{

    class FileRepository : IRepository
    {
        public async Task<Player> Get(Guid id)
        {
            string stringPlayers = await File.ReadAllTextAsync("game-dev.txt");
            Players players = JsonConvert.DeserializeObject<Players>(stringPlayers);
            foreach (var item in players.playersList)
            {
                if (item.Id == id)
                    return item;
            }
            return null;
        }

        public Task<Player> GetPlayerWithName(string name)
        {
            return null;
        }

        public async Task<Player[]> GetPlayersWithXscore(int x)
        {
            return null;
        }
        public async Task<Player[]> GetPlayersWithTag(string x)
        {
            return null;
        }

        public async Task<Player[]> GetPlayersWithItemProperty(int level)
        {
            return null;
        }

        public async Task<Player[]> GetPlayersWithXItems(int itemAmount)
        {
            return null;
        }
        public async Task<UpdateResult> UpdatePlayerName(Guid playerId, string name)
        {
            return null;
        }
        public async Task<UpdateResult> IncrementPlayerScore(Guid playerId, int score)
        {
            return null;
        }
        public async Task<UpdateResult> AddItemToPlayer(Guid playerId, Item item)
        {
            return null;
        }
        public async Task<Player> AddItemAndIncrementPlayer(Guid playerId, Guid itemId)
        {
            return null;
        }
        public async Task<List<Player>> GetTopTenPlayers()
        {
            return null;
        }
        public async Task<LevelCount> GetMostCommonLevel()
        {
            return null;
        }

        public string UpdatePlayerName(string oldName, string name)
        {
            return null;
        }
        public async Task<Player[]> GetAll()
        {
            string stringPlayers = await File.ReadAllTextAsync("game-dev.txt");
            Players players = JsonConvert.DeserializeObject<Players>(stringPlayers);
            return players.playersList.ToArray();
        }
        public async Task<Player> Create(Player player)
        {
            string stringPlayers = await File.ReadAllTextAsync("game-dev.txt");
            Players players = JsonConvert.DeserializeObject<Players>(stringPlayers);
            players.AddPlayer(player);
            File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));
            return null;
        }
        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            string stringPlayers = await File.ReadAllTextAsync("game-dev.txt");
            Players players = JsonConvert.DeserializeObject<Players>(stringPlayers);

            for (int i = 0; i < players.playersList.Count; i++)
            {
                if (players.playersList[i].Id == id)
                {
                    players.playersList[i].Score = player.Score;
                }
            }
            File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));

            return null;
        }
        public async Task<Player> Delete(Guid id)
        {
            string stringPlayers = await File.ReadAllTextAsync("game-dev.txt");
            Players players = JsonConvert.DeserializeObject<Players>(stringPlayers);

            for (int i = 0; i < players.playersList.Count; i++)
            {
                if (players.playersList[i].Id == id)
                {
                    players.playersList.RemoveAt(i);
                }
            }
            File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));

            return null;
        }

        public Task<Item> CreateItem(Guid playerId, Item item)
        {
            throw new NotImplementedException();
        }

        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            throw new NotImplementedException();
        }

        public Task<Item[]> GetAllItems(Guid playerId)
        {
            throw new NotImplementedException();
        }

        public Task<Item> UpdateItem(Guid playerId, Item item)
        {
            throw new NotImplementedException();
        }

        public Task<Item> DeleteItem(Guid playerId, Item item)
        {
            throw new NotImplementedException();
        }
    }

    public class Players
    {
        public List<Player> playersList = new List<Player>();


        public void AddPlayer(Player player)
        {
            playersList.Add(player);
        }
    }

}