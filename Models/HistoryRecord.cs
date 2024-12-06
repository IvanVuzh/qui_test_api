using System.ComponentModel.DataAnnotations;

namespace qui_test_api.Models
{
    public class HistoryRecord
    {
        [Key]
        public Guid Id { get; set; }
        public string UserIdentifier { get; set; } = null!;
        public string CityName { get; set; } = null!;
        public DateTime QueryDate { get; set; }
        public string WeatherData { get; set; } = null!;
    }
}
