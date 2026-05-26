using System.ComponentModel.DataAnnotations;

namespace PrimaryVets.Models
{
    public class PetRegisterViewModel
    {

        public string Name { get; set; } = string.Empty;
        public string PetName { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;

        public string Species { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int? Age { get; set; }
        public double? BodyTemperature { get; set; }
        public double? Weight { get; set; }
        public string PetBehavior { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string SelectedTests { get; set; } = string.Empty;
        public string Treatment { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        // Page 2
        public List<string> Reports { get; set; } = new();
        public string Remarks { get; set; } = string.Empty;
    }
}