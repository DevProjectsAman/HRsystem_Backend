using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Employee
{
    // Request
    public record DeleteEmployeeCommand(long EmployeeId) : IRequest<bool>;

    // Handler
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly DBContextHRsystem _db;

        public DeleteEmployeeHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _db.TbEmployees
                .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, cancellationToken);

            if (employee == null)
                return false; // Employee not found

            _db.TbEmployees.Remove(employee);
            await _db.SaveChangesAsync(cancellationToken);

            return true; // Successfully deleted
        }
    }
}

