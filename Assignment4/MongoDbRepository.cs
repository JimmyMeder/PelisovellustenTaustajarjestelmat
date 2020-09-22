

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
}