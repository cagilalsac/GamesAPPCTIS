using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class GameRequest : Request
    {
        [Required, StringLength(150)]
        public string Title { get; set; }

        public decimal Price { get; set; }

        [DisplayName("Release Date")]
        public DateTime? ReleaseDate { get; set; }

        [DisplayName("Top Seller")]
        public bool IsTopSeller { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Publisher")]
        public int? PublisherId { get; set; }

        [DisplayName("Tags")]
        public List<int> TagIds { get; set; } = new List<int>();
    }
}
