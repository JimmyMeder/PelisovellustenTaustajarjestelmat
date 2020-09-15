using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;


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