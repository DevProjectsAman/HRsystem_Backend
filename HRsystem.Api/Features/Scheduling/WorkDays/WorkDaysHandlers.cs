using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Scheduling.WorkDays
{
    public class WorkDaysDto
    {
        public int WorkDaysId { get; set; }
        public string WorkDaysDescription { get; set; }
        public List<string> WorkDaysNames { get; set; }
    }

    // CREATE
    public record CreateWorkDaysCommand(string Description, List<string> Names) : IRequest<int>;

    public class CreateWorkDaysHandler : IRequestHandler<CreateWorkDaysCommand, int>
    {
        private readonly DBContextHRsystem _db;
        public CreateWorkDaysHandler(DBContextHRsystem db) => _db = db;

        public async Task<int> Handle(CreateWorkDaysCommand request, CancellationToken ct)
        {
            var entity = new TbWorkDays
            {
                WorkDaysDescription = request.Description,
                WorkDaysNames = request.Names
            };

            _db.TbWorkDays.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity.WorkDaysId;
        }
    }

    // GET ALL
    public record GetAllWorkDaysQuery() : IRequest<List<WorkDaysDto>>;

    public class GetAllWorkDaysHandler : IRequestHandler<GetAllWorkDaysQuery, List<WorkDaysDto>>
    {
        private readonly DBContextHRsystem _db;
        public GetAllWorkDaysHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<WorkDaysDto>> Handle(GetAllWorkDaysQuery request, CancellationToken ct)
        {
            return await _db.TbWorkDays
                .Select(w => new WorkDaysDto
                {
                    WorkDaysId = w.WorkDaysId,
                    WorkDaysDescription = w.WorkDaysDescription,
                    WorkDaysNames = w.WorkDaysNames
                })
                .ToListAsync(ct);
        }
    }

    // GET BY ID
    public record GetWorkDaysByIdQuery(int Id) : IRequest<WorkDaysDto?>;

    public class GetWorkDaysByIdHandler : IRequestHandler<GetWorkDaysByIdQuery, WorkDaysDto?>
    {
        private readonly DBContextHRsystem _db;
        public GetWorkDaysByIdHandler(DBContextHRsystem db) => _db = db;

        public async Task<WorkDaysDto?> Handle(GetWorkDaysByIdQuery request, CancellationToken ct)
        {
            return await _db.TbWorkDays
                .Where(w => w.WorkDaysId == request.Id)
                .Select(w => new WorkDaysDto
                {
                    WorkDaysId = w.WorkDaysId,
                    WorkDaysDescription = w.WorkDaysDescription,
                    WorkDaysNames = w.WorkDaysNames
                })
                .FirstOrDefaultAsync(ct);
        }
    }

    // UPDATE
    public record UpdateWorkDaysCommand(int Id, string Description, List<string> Names) : IRequest<bool>;

    public class UpdateWorkDaysHandler : IRequestHandler<UpdateWorkDaysCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public UpdateWorkDaysHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(UpdateWorkDaysCommand request, CancellationToken ct)
        {
            var entity = await _db.TbWorkDays.FindAsync(new object[] { request.Id }, ct);
            if (entity == null) return false;

            entity.WorkDaysDescription = request.Description;
            entity.WorkDaysNames = request.Names;

            await _db.SaveChangesAsync(ct);
            return true;
        }
    }

    // DELETE
    public record DeleteWorkDaysCommand(int Id) : IRequest<bool>;

    public class DeleteWorkDaysHandler : IRequestHandler<DeleteWorkDaysCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public DeleteWorkDaysHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteWorkDaysCommand request, CancellationToken ct)
        {
            var entity = await _db.TbWorkDays.FindAsync(new object[] { request.Id }, ct);
            if (entity == null) return false;

            _db.TbWorkDays.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
