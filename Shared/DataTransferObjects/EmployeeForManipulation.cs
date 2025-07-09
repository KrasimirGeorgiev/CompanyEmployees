using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record EmployeeForManipulation
    {
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 chararacters.")]
        public string? Name { get; init; }

        [Required(ErrorMessage = "Age is a required field")]
        [Range(18, int.MaxValue, ErrorMessage = "Age must be at least 18 years old.")]
        public int Age { get; init; }

        [Required(ErrorMessage = "Position is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
        public string? Position { get; init; }
    }
}