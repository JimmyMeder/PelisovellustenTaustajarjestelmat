using System.ComponentModel.DataAnnotations;

namespace Assignment2
{
    public class NewItem
    {

        [StringLength(5)]
        public string Name { get; set; }

        [Range(1, 99, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Level { get; set; }
        
        [EnumDataType(typeof(ItemType))]
        public ItemType type { get; set; }
    }
}
