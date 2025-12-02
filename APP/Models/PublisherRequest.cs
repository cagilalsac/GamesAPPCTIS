using CORE.APP.Models;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class PublisherRequest : Request
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
    }
}
