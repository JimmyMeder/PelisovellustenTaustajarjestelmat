using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;


namespace Assignment2
{
    public enum ItemType
    {
        Sword,
        Potion,
        Shield
    }
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Range(1, 99, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Level { get; set; }

        [EnumDataType(typeof(ItemType))]
        public ItemType type { get; set; }

        [NotPast]
        public DateTime CreationTime { get; set; }
    }

        public class NotPast : ValidationAttribute
    {
        public DateTime CreationTime { get; }

        public string GetErrorMessage() => $"Date from the past";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var today = DateTime.UtcNow;

            if (today >= CreationTime)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}