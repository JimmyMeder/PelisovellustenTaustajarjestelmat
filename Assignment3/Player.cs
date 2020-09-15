using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace Assignment2
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreationTime { get; set; }
    }

}