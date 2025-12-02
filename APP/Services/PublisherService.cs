using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class PublisherService : Service<Publisher>, IService<PublisherRequest, PublisherResponse>
    {
        public PublisherService(DbContext db) : base(db)
        {
        }

        public CommandResponse Create(PublisherRequest request)
        {
            if (Query().Any(p => p.Name == request.Name.Trim()))
                return Error("Publisher with the same name exists!");
            var entity = new Publisher
            {
                Name = request.Name.Trim()
            };
            Create(entity);
            return Success("Publisher created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(p => p.Id == id);
            if (entity is null)
                return Error("Publisher not found!");
            Delete(entity);
            return Success("Publisher deleted successfully.", entity.Id);
        }

        public PublisherRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(p => p.Id == id);
            if (entity is null)
                return null;
            return new PublisherRequest
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public PublisherResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(p => p.Id == id);
            if (entity is null)
                return null;
            return new PublisherResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Name = entity.Name
            };
        }

        public List<PublisherResponse> List()
        {
            return Query().Select(p => new PublisherResponse
            {
                Id = p.Id,
                Guid = p.Guid,
                Name = p.Name
            }).ToList();
        }

        public CommandResponse Update(PublisherRequest request)
        {
            if (Query().Any(p => p.Id != request.Id && p.Name == request.Name.Trim()))
                return Error("Publisher with the same name exists!");
            var entity = Query(false).SingleOrDefault(p => p.Id == request.Id);
            if (entity is null)
                return Error("Publisher not found!");
            entity.Name = request.Name.Trim();
            Update(entity);
            return Success("Publisher updated successfully.", entity.Id);
        }
    }
}
