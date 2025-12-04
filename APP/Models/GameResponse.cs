using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class GameResponse : Response
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        [DisplayName("Release Date")]
        public DateTime? ReleaseDate { get; set; }

        [DisplayName("Top Seller")]
        public bool IsTopSeller { get; set; }
        
        public int PublisherId { get; set; }

        [DisplayName("Price")]
        public string PriceF { get; set; }

        [DisplayName("Release Date")]
        public string ReleaseDateF { get; set; }

        [DisplayName("Top Seller")]
        public string IsTopSellerF { get; set; }

        public string Publisher { get; set; }

        public PublisherResponse PublisherResponse { get; set; }

        public string Tags { get; set; }

        public List<TagResponse> TagsResponse { get; set; }
    }
}
