using System;
using System.Linq;
using System.Collections.Generic;

namespace Tehtava2
{
    class Program
    {
        static void Main(string[] args)
        {
            Player[] players = new Player[1000000];
            PlayerForAnotherGame[] playersFromAnotherGame = new PlayerForAnotherGame[1000000];
            InstantiateId(ref players);

            for (int i = 0; i < playersFromAnotherGame.Length; i++)
            {
                playersFromAnotherGame[i] = new PlayerForAnotherGame();
                Random rnd = new Random();
                playersFromAnotherGame[i].Score = rnd.Next(1, 1000000);
            }
            //Tests for for tehtava2 onward
            players[0].Items = new List<Item>();
            players[0].Items.Add(new Item() { Id = Guid.NewGuid(), Level = 1 });
            players[0].Items.Add(new Item() { Id = Guid.NewGuid(), Level = 2 });
            players[0].Items.Add(new Item() { Id = Guid.NewGuid(), Level = 3 });
            players[0].Items.Add(new Item() { Id = Guid.NewGuid(), Level = 4 });
            players[0].Items.Add(new Item() { Id = Guid.NewGuid(), Level = 5 });
            players[0].Items.Add(new Item() { Id = Guid.NewGuid(), Level = 6 });
            players[0].Items.Add(new Item() { Id = Guid.NewGuid(), Level = 7 });
            players[0].Items.Add(new Item() { Id = Guid.NewGuid(), Level = 8 });
            players[0].Items.Add(new Item() { Id = Guid.NewGuid(), Level = 9 });

            //Tehtava 5
            Process test = PrintItem;
            ProcessEachItem(players[0], test);
            //Tehtava 6
            ProcessEachItem(players[0], Item => Console.WriteLine("Id: " + Item.Id + " Level: " + Item.Level));
            //Func<List<Item>, Player> testShit = (List<Item> temp) => {return null;};

            //Tests for tehtava 3 and 4
            /*Console.WriteLine(players[0].GetHighestLevelItem().Level);
            Item[] loopList = GetItemsVariations.GetItems(players[0]);
            Item[] linqList = GetItemsVariations.GetItemsWithLinq(players[0]);
            foreach (Item item in loopList)
            {
                Console.WriteLine("Loop: " + item.Level);
            }
            foreach (Item item in linqList)
            {
                Console.WriteLine("Linq: " + item.Level);
            }
            Console.WriteLine("\nSharp: " + players[0].getFirstItem().Level);
            Console.WriteLine("\nLinq: " + players[0].getFirstItemWithLinq().Level);*/

            //Tehtava 7 
            Game<Player> game1 = new Game<Player>(players.ToList());
            Player[] topPlayers = game1.GetTop10Players();
            foreach (Player p in topPlayers)
            {
                Console.WriteLine(p.Score);
            }

            Console.WriteLine("\nAnother Game:");
            Game<PlayerForAnotherGame> game2 = new Game<PlayerForAnotherGame>(playersFromAnotherGame.ToList());
            PlayerForAnotherGame[] topPlayersFromAnotherGame = game2.GetTop10Players();
            foreach (PlayerForAnotherGame p in topPlayersFromAnotherGame)
            {
                Console.WriteLine(p.Score);
            }
        }
        //Tehtava 1
        public static void InstantiateId(ref Player[] players)
        {
            Dictionary<Guid, int> duplicateCheck = new Dictionary<Guid, int>(); ;
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player();
                Guid generatedGuid = Guid.NewGuid();
                if (!duplicateCheck.ContainsKey(generatedGuid))
                {
                    players[i].Id = generatedGuid;
                    duplicateCheck.Add(generatedGuid, 1);

                    Random rnd = new Random();
                    players[i].Score = rnd.Next(1, 1000000);
                }
                else
                {
                    Console.WriteLine("Duplicate found!");
                    i--;
                }
            }
        }
        //Tehtava 5 and 6
        delegate void Process(Item item);
        public static void PrintItem(Item item)
        {
            Console.WriteLine("Id: " + item.Id + " Level: " + item.Level);
        }
        static void ProcessEachItem(Player player, Process process)
        {
            foreach (Item item in player.Items)
            {
                process(item);
            }
        }
    }
    //Tehtava2
    public static class PlayerExtensions
    {

        public static Item GetHighestLevelItem(this Player player)
        {
            Item highestItem = new Item();
            if (!player.Items.Any())
            {
                return highestItem;
            }
            else
            {
                highestItem = player.Items[0];
            }
            foreach (Item i in player.Items)
            {
                if (i.Level > highestItem.Level)
                {
                    highestItem = i;
                }
            }
            return highestItem;
        }
    }
    //Tehtava 3
    public static class GetItemsVariations
    {
        public static Item[] GetItems(this Player player)
        {
            Item[] returnable = new Item[player.Items.Count];
            for (int i = 0; i < player.Items.Count; i++)
            {
                returnable[i] = player.Items[i];
            }
            return returnable;
        }

        public static Item[] GetItemsWithLinq(this Player player)
        {
            return player.Items.ToArray();
        }

    }
    //Tehtava4
    public static class GetFirstItemVariations
    {
        public static Item getFirstItem(this Player player)
        {
            if (player.Items.Count > 0)
            {
                return player.Items[0];
            }
            else
            {
                return null;
            }
        }
        public static Item getFirstItemWithLinq(this Player player)
        {
            if (player.Items.Any())
            {
                return player.Items.First();
            }
            else
            {
                return null;
            }
        }
    }


    //Tehtava 7
    public class Game<T> where T : IPlayer
    {
        private List<T> _players;

        public Game(List<T> players)
        {
            _players = players;
        }

        public T[] GetTop10Players()
        {
            //List<T> SortedList = _players.OrderByDescending(_players => _players.Score).ToList();
            //return SortedList.Take(10).ToArray();

            return _players.OrderByDescending(_players => _players.Score).Take(10).ToArray();
        }
    }


}
