using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace APP.Services
{
    public class GameService : Service<Game>, IService<GameRequest, GameResponse>
    {
        public GameService(DbContext db) : base(db)
        {
            //CultureInfo = new CultureInfo("tr-TR"); // for Turkish culture
        }

        protected override IQueryable<Game> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(g => g.GameTags).ThenInclude(gt => gt.Tag)
                .Include(g => g.Publisher).OrderByDescending(g => g.ReleaseDate)
                .ThenBy(g => g.Title);
        }

        public CommandResponse Create(GameRequest request)
        {
            if (Query().Any(g => g.Title == request.Title.Trim()))
                return Error("Game with same title exists!");
            var entity = new Game
            {
                IsTopSeller = request.IsTopSeller,
                Price = request.Price,
                PublisherId = request.PublisherId ?? 0,
                ReleaseDate = request.ReleaseDate,
                Title = request.Title.Trim()
            };
            Create(entity);
            return Success("Game created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(g => g.Id == id);
            if (entity is null)
                return Error("Game not found!");
            Delete(entity.GameTags);
            Delete(entity);
            return Success("Game deleted successfully.", entity.Id);
        }

        public GameRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(g => g.Id == id);
            if (entity is null)
                return null;
            return new GameRequest
            {
                Id = entity.Id,
                IsTopSeller = entity.IsTopSeller,
                Price = entity.Price,
                PublisherId = entity.PublisherId,
                ReleaseDate = entity.ReleaseDate,
                Title = entity.Title
            };
        }

        public GameResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(g => g.Id == id);
            if (entity is null)
                return null;
            return new GameResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                IsTopSeller = entity.IsTopSeller,
                Price = entity.Price,
                PublisherId = entity.PublisherId,
                ReleaseDate = entity.ReleaseDate,
                Title = entity.Title,

                IsTopSellerF = entity.IsTopSeller ? "Top Seller" : string.Empty,
                PriceF = entity.Price.ToString("C2"),

                Publisher = entity.Publisher.Name,
                PublisherResponse = new PublisherResponse
                {
                    Guid = entity.Publisher.Guid,
                    Id = entity.Publisher.Id,
                    Name = entity.Publisher.Name
                },

                Tags = string.Join("<br>", entity.GameTags.OrderBy(gt => gt.Tag.Name).Select(gt => gt.Tag.Name)),
                TagsResponse = entity.GameTags.OrderBy(gt => gt.Tag.Name).Select(gt => new TagResponse
                {
                    Guid = gt.Tag.Guid,
                    Id = gt.Tag.Id,
                    Name = gt.Tag.Name
                }).ToList(),

                ReleaseDateF = entity.ReleaseDate.HasValue ? entity.ReleaseDate.Value.ToString("MM/dd/yyyy") : string.Empty
            };
        }

        public List<GameResponse> List()
        {
            return Query().Select(g => new GameResponse
            {
                Id = g.Id,
                Guid = g.Guid,
                IsTopSeller = g.IsTopSeller,
                Price = g.Price,
                PublisherId = g.PublisherId,
                ReleaseDate = g.ReleaseDate,
                Title = g.Title,

                IsTopSellerF = g.IsTopSeller ? "Top Seller" : string.Empty,
                PriceF = g.Price.ToString("C2"),

                Publisher = g.Publisher.Name,
                PublisherResponse = new PublisherResponse
                {
                    Guid = g.Publisher.Guid,
                    Id = g.Publisher.Id,
                    Name = g.Publisher.Name
                },

                Tags = string.Join("<br>", g.GameTags.OrderBy(gt => gt.Tag.Name).Select(gt => gt.Tag.Name)),
                TagsResponse = g.GameTags.OrderBy(gt => gt.Tag.Name).Select(gt => new TagResponse
                {
                    Guid = gt.Tag.Guid,
                    Id = gt.Tag.Id,
                    Name = gt.Tag.Name
                }).ToList(),
                ReleaseDateF = g.ReleaseDate.HasValue ? g.ReleaseDate.Value.ToString("MM/dd/yyyy") : string.Empty
            }).ToList();
        }

        public CommandResponse Update(GameRequest request)
        {
            if (Query().Any(g => g.Id != request.Id && g.Title == request.Title.Trim()))
                return Error("Game with same title exists!");
            var entity = Query(false).SingleOrDefault(g => g.Id == request.Id);
            if (entity is null)
                return Error("Game not found!");
            Delete(entity.GameTags);
            entity.IsTopSeller = request.IsTopSeller;
            entity.Price = request.Price;
            entity.PublisherId = request.PublisherId ?? 0;
            entity.ReleaseDate = request.ReleaseDate;
            entity.Title = request.Title.Trim();
            Update(entity);
            return Success("Game updated successfully.", entity.Id);
        }
    }
}
