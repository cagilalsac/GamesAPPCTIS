using System.ComponentModel;

namespace APP.Models
{
    public class FavoriteItem
    {
        public int UserId { get; set; }
        public int GameId { get; set; }

        [DisplayName("Game")]
        public string GameName { get; set; }
    }
}
