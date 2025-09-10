using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;

namespace HRsystem.Api.Features.VacationType.CreateVacationType
{
    public record CreateVacationTypeCommand(
        string VacationName,
        string? Description,
        bool? IsPaid,
        bool? RequiresHrApproval
    ) : IRequest<TbVacationType>;

    public class Handler : IRequestHandler<CreateVacationTypeCommand, TbVacationType>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbVacationType> Handle(CreateVacationTypeCommand request, CancellationToken ct)
        {
            var entity = new TbVacationType
            {
                VacationName = request.VacationName,
                Description = request.Description,
                IsPaid = request.IsPaid,
                RequiresHrApproval = request.RequiresHrApproval
            };

            _db.TbVacationTypes.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity;
        }
    }
}
