
using System.ComponentModel.DataAnnotations;

namespace Assignment2
{
    public class NewPlayer
    {
        [StringLength(5)]
        public string Name { get; set; }
    }
}
