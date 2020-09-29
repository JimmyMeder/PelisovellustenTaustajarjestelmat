

using System;
using System.Threading.Tasks;
using MongoDB.Bson;
//using game_server.Players;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Assignment2
{

    public class MongoDbRepository : IRepository
    {

        private readonly IMongoCollection<Player> _playerCollection;
        private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;

        public MongoDbRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("game");
            _playerCollection = database.GetCollection<Player>("players");

            _bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
        }

        public Task<Player> Get(Guid id)
        {
            var filter = Builders<Player>.Filter.Eq(player => player.Id, id);
            return _playerCollection.Find(filter).FirstAsync();
        }

        public Task<Player> GetPlayerWithName(string name)
        {
            var filter = Builders<Player>.Filter.Eq("Name", name);
            return _playerCollection.Find(filter).FirstAsync();
        }

        public async Task<Player[]> GetPlayersWithXscore(int x)
        {
            var filter = Builders<Player>.Filter.Gte("Score", x);
            List<Player> players = await _playerCollection.Find(filter).ToListAsync();

            return players.ToArray();
        }

        public async Task<Player[]> GetPlayersWithTag(string x)
        {
            var filter = Builders<Player>.Filter.Eq("Tag", x);
            List<Player> players = await _playerCollection.Find(filter).ToListAsync();

            return players.ToArray();
        }

        public async Task<Player[]> GetPlayersWithItemProperty(int level)
        {
            var filter = Builders<Player>.Filter.ElemMatch<Item>(p => p.items, Builders<Item>.Filter.Eq(i => i.Level, level));
            List<Player> players = await _playerCollection.Find(filter).ToListAsync();

            return players.ToArray(); ;
        }

        public async Task<Player[]> GetPlayersWithXItems(int itemAmount)
        {
            var filter = Builders<Player>.Filter.Size(p => p.items, itemAmount);
            List<Player> players = await _playerCollection.Find(filter).ToListAsync();

            return players.ToArray();
        }

        public async Task<UpdateResult> UpdatePlayerName(Guid playerId, string name)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            var update = Builders<Player>.Update.Set("Name", name);
            return await _playerCollection.UpdateOneAsync(filter, update);

        }
        public async Task<UpdateResult> IncrementPlayerScore(Guid playerId, int score)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            var update = Builders<Player>.Update.Inc("Score", score);
            return await _playerCollection.UpdateOneAsync(filter, update);
        }
        public async Task<UpdateResult> AddItemToPlayer(Guid playerId, Item item)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            var update = Builders<Player>.Update.Push("items", item);
            return await _playerCollection.UpdateOneAsync(filter, update);
        }

        public async Task<Player> AddItemAndIncrementPlayer(Guid playerId, Guid itemId)
        {
            var playerFilter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            var updateScore = Builders<Player>.Update.Inc("Score", 10);
            await _playerCollection.FindOneAndUpdateAsync(playerFilter, updateScore);
            var filterItem = Builders<Player>.Filter.ElemMatch<Item>(p => p.items, Builders<Item>.Filter.Eq(i => i.Id, itemId));
            return await _playerCollection.FindOneAndDeleteAsync(filterItem);
        }

        public async Task<List<Player>> GetTopTenPlayers()
        {
            /*
            List<Player> players = await _playerCollection.Find("").Sort(Builders<Player>.Sort.Descending("Score")).Limit(10).ToListAsync<Player>();
            return players.ToArray(); 
            */
            return await _playerCollection.Find("").Sort(Builders<Player>.Sort.Descending("Score")).Limit(10).ToListAsync<Player>();
        }

        public async Task<LevelCount> GetMostCommonLevel()
        {
            //_playerCollection.Aggregate().Project(p => p.Level).Group(l => l,p => new LevelCount {Id = p.Key,Count = p.Sum()})
            //var count = await _playerCollection.CountAsync(Builders<Player>.Filter.Eq(p => p.Level, 3));
            return await (Task<LevelCount>)_playerCollection.Aggregate().Project(p => p.Level).Group(l => l, p => new LevelCount { Id = p.Key, Count = p.Sum() }).SortByDescending(l => l.Count).Limit(1);
        }

        public async Task<Player[]> GetAll()
        {
            var players = await _playerCollection.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();
        }
        public async Task<Player> Create(Player player)
        {
            await _playerCollection.InsertOneAsync(player);
            return player;
        }
        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            Player returnPlayer = await _playerCollection.Find(filter).FirstAsync();
            returnPlayer.Score = player.Score;
            await _playerCollection.ReplaceOneAsync(filter, returnPlayer);
            return returnPlayer;
        }
        public async Task<Player> Delete(Guid id)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            return await _playerCollection.FindOneAndDeleteAsync(filter);
        }









        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Player player = await _playerCollection.Find(filter).FirstAsync();
            player.items.Add(item);
            await _playerCollection.ReplaceOneAsync(filter, player);
            return item;
        }

        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Player player = await _playerCollection.Find(filter).FirstAsync();
            for (int i = 0; i < player.items.Count; i++)
            {
                if (player.items[i].Id == itemId)
                    return player.items[i];
            }
            return null;
        }



        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Player player = await _playerCollection.Find(filter).FirstAsync();
            return player.items.ToArray();
        }

        public async Task<Item> UpdateItem(Guid playerId, Item item)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Player player = await _playerCollection.Find(filter).FirstAsync();
            for (int i = 0; i < player.items.Count; i++)
            {
                if (player.items[i].Id == item.Id)
                {
                    player.items[i] = item;
                }
            }
            await _playerCollection.ReplaceOneAsync(filter, player);
            return item;
        }

        public async Task<Item> DeleteItem(Guid playerId, Item item)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Player player = await _playerCollection.Find(filter).FirstAsync();
            for (int i = 0; i < player.items.Count; i++)
            {
                if (player.items[i].Id == item.Id)
                {
                    player.items.RemoveAt(i);
                }
            }
            await _playerCollection.ReplaceOneAsync(filter, player);
            return item;
        }
    }


    public class LevelCount
    {
        public int Id { get; set; }
        public int Count { get; set; }

    };

}