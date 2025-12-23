using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace HRsystem.Api.Features.EmployeeDevices
{

    #region CheckEmployeeDevice
    public record CheckEmployeeDeviceQuery() : IRequest<bool>;

    public class CheckEmployeeDeviceHandler
        : IRequestHandler<CheckEmployeeDeviceQuery, bool>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public CheckEmployeeDeviceHandler(
            DBContextHRsystem db,
            ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(CheckEmployeeDeviceQuery request, CancellationToken ct)
        {
            return await _db.TbEmployeeDevices
                .AsNoTracking()
                .AnyAsync(d =>
                    d.EmployeeId == _currentUser.EmployeeID &&
                    d.DeviceUuid == _currentUser.DeviceId &&
                    d.IsActiveDevice,
                    ct);
        }
    }

    #endregion

    #region AddDevice
    public record AddEmployeeDeviceCommand(
           string DeviceUuid,
           string? NativeOsId,
           string? ModelName,
           string OsType,
           string? OsVersion,
           string? Manufacturer,
           bool IsPhysical
       ) : IRequest<bool>;

    public class AddEmployeeDeviceHandler
        : IRequestHandler<AddEmployeeDeviceCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public AddEmployeeDeviceHandler(
            DBContextHRsystem db,
            ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(AddEmployeeDeviceCommand request, CancellationToken ct)
        {
            // ❌ Device already exists (globally)
            var deviceExists = await _db.TbEmployeeDevices
                .AnyAsync(d =>
                    d.DeviceUuid == request.DeviceUuid &&
                    d.IsActiveDevice,
                    ct);

            if (deviceExists)
                return false;

            var entity = new TbEmployeeDevices
            {
                EmployeeId = (int)_currentUser.EmployeeID,
                DeviceUuid = request.DeviceUuid,
                NativeOsId = request.NativeOsId,
                ModelName = request.ModelName,
                OsType = request.OsType,
                OsVersion = request.OsVersion,
                Manufacturer = request.Manufacturer,
                IsPhysical = request.IsPhysical,
                RegisteredAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow,
                IsActiveDevice = true
            };

            _db.TbEmployeeDevices.Add(entity);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }

    public class AddEmployeeDeviceCommandValidator
        : AbstractValidator<AddEmployeeDeviceCommand>
    {
        public AddEmployeeDeviceCommandValidator()
        {
            RuleFor(x => x.DeviceUuid)
                .NotEmpty()
                .WithMessage("Device UUID is required")
                .MaximumLength(100);

            RuleFor(x => x.OsType)
                .NotEmpty()
                .WithMessage("OS type is required")
                .MaximumLength(20);

            RuleFor(x => x.ModelName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.ModelName));

            RuleFor(x => x.NativeOsId)
                .MaximumLength(255)
                .When(x => !string.IsNullOrWhiteSpace(x.NativeOsId));

            RuleFor(x => x.OsVersion)
                .MaximumLength(20)
                .When(x => !string.IsNullOrWhiteSpace(x.OsVersion));

            RuleFor(x => x.Manufacturer)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Manufacturer));
        }
    }

    #endregion

    #region Reset

    public record ResetEmployeeDeviceCommand(int EmployeeId)
      : IRequest<bool>;
    public class ResetEmployeeDeviceHandler
        : IRequestHandler<ResetEmployeeDeviceCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public ResetEmployeeDeviceHandler(
            DBContextHRsystem db,
            ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            ResetEmployeeDeviceCommand request,
            CancellationToken ct)
        {
            //var device = await _db.TbEmployeeDevices
            //    .FirstOrDefaultAsync(d =>
            //        d.EmployeeId == request.EmployeeId &&
            //        d.IsActiveDevice, ct);

            int rowsAffected = await _db.TbEmployeeDevices
        .Where(d => d.EmployeeId == request.EmployeeId && d.IsActiveDevice)
        .ExecuteUpdateAsync(setters => setters
            .SetProperty(d => d.IsActiveDevice, false)
            .SetProperty(d => d.ResetByUserId, _currentUser.UserId)
            .SetProperty(d => d.ResetByUserDate, DateTime.UtcNow),
            ct);

            if (rowsAffected == 0)
                return false;

           

          //  await _db.SaveChangesAsync(ct);
            return true;
        }
    }



    #endregion


    #region ListEmployeeDevices

    public class EmployeeDeviceDto
    {
        public int DeviceId { get; set; }
        public string DeviceUuid { get; set; } = string.Empty;
        public string? ModelName { get; set; }
        public string OsType { get; set; } = string.Empty;
        public string? OsVersion { get; set; }
        public string? Manufacturer { get; set; }
        public bool IsPhysical { get; set; }
        public bool IsActiveDevice { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime LastActiveAt { get; set; }
    }

    public record ListEmployeeDevicesQuery(int EmployeeId) : IRequest<List<EmployeeDeviceDto>>;

    public class ListEmployeeDevicesHandler : IRequestHandler<ListEmployeeDevicesQuery, List<EmployeeDeviceDto>>
    {
        private readonly DBContextHRsystem _db;

        public ListEmployeeDevicesHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<List<EmployeeDeviceDto>> Handle(ListEmployeeDevicesQuery request, CancellationToken ct)
        {
            return await _db.TbEmployeeDevices
                .AsNoTracking()
                .Where(d => d.EmployeeId == request.EmployeeId)
                .OrderByDescending(d => d.IsActiveDevice)
                .ThenByDescending(d => d.LastActiveAt)
                .Select(d => new EmployeeDeviceDto
                {
                    DeviceId = d.DeviceId,
                    DeviceUuid = d.DeviceUuid,
                    ModelName = d.ModelName,
                    OsType = d.OsType,
                    OsVersion = d.OsVersion,
                    Manufacturer = d.Manufacturer,
                    IsPhysical = d.IsPhysical,
                    IsActiveDevice = d.IsActiveDevice,
                    RegisteredAt = d.RegisteredAt,
                    LastActiveAt = d.LastActiveAt
                })
                .ToListAsync(ct);
        }
    }


    #endregion


}


