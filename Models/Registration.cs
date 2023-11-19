using System.ComponentModel.DataAnnotations;

namespace lr10.Models
{
    public class Registration
    {
        [Required(ErrorMessage = "Please enter your name")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Please select a preferred consultation date")]
        [FutureDate(ErrorMessage = "Consultation date must be in the future")]
        [NotOnWeekend(ErrorMessage = "Consultation cannot be on a weekend")]
        public required DateTime ConsultationDate { get; set; }

        [Required(ErrorMessage = "Please select a product for consultation")]
        [OnlyOnMonday("ConsultationDate", ErrorMessage = "Only on Monday")]
        public required string SelectedProduct { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            DateTime date;
            if (DateTime.TryParse(value.ToString(), out date)) return date > DateTime.Now;
            return false;
        }
    }

    public class NotOnWeekendAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            DateTime date;
            if (DateTime.TryParse(value.ToString(), out date)) return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
            return false;
        }
    }

    public class OnlyOnMonday : ValidationAttribute
    {
        private readonly string _otherProperty;

        public OnlyOnMonday(string otherProperty)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_otherProperty);

            if (property == null) throw new ArgumentException("Property with this name not found");

            var date = property.GetValue(validationContext.ObjectInstance, null);

            if (date is DateTime consultationDate && value is string selectedProduct && selectedProduct == "Basics")
            {
                if (consultationDate.DayOfWeek == DayOfWeek.Monday)
                {
                    return new ValidationResult("Consultation regarding 'Basics' cannot be scheduled on Monday");
                }
            }

            return ValidationResult.Success;
        }
    }
}
