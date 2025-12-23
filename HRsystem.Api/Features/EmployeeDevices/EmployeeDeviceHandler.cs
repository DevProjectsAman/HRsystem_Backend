using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDevices
{
    #region Enums
    public enum DevicePlatform
    {
        Android = 1,
        iOS = 2
    }
    #endregion

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

            var device = await _db.TbEmployeeDevicesTrack
        .FirstOrDefaultAsync(d =>
             d.EmployeeId == _currentUser.EmployeeID &&
             d.DeviceUid == _currentUser.DeviceId &&
             d.IsActiveDevice);

            if (device == null)
                return false;
            // Primary check: fingerprint must match
            //if (device.DeviceFingerprint != fingerprint)
            //{
            //    // Log potential security breach
            //    await _auditService.LogAsync(new SecurityEvent
            //    {
            //        Type = "DeviceFingerprintMismatch",
            //        EmployeeId = device.EmployeeId,
            //        Severity = "High"
            //    });

              
           

            // Update last active
            device.LastActiveAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return true;
        }
    }
    #endregion

    #region AddDevice
    public record AddEmployeeDeviceCommand(
        string DeviceId,
        DevicePlatform Platform,
        string OsVersion,
        string Manufacturer,
        string Model,
        string Brand,
        bool IsPhysicalDevice,
        string DeviceFingerprint,
        string? AppVersion = null
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
            var deviceExists = await _db.TbEmployeeDevicesTrack
                .AnyAsync(d =>
                    d.DeviceUid == request.DeviceId &&
                    d.IsActiveDevice,
                    ct);

            if (deviceExists)
                return false;

            var entity = new TbEmployeeDevicesTrack
            {
                EmployeeId = (int)_currentUser.EmployeeID,
                DeviceUid = request.DeviceId,
                Platform = (int)request.Platform,
                OsVersion = request.OsVersion,
                Manufacturer = request.Manufacturer,
                Model = request.Model,
                Brand = request.Brand,
                IsPhysicalDevice = request.IsPhysicalDevice,
                DeviceFingerprint = request.DeviceFingerprint,
                AppVersion = request.AppVersion,
                RegisteredAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow,
                IsActiveDevice = true
            };

            _db.TbEmployeeDevicesTrack.Add(entity);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }

    public class AddEmployeeDeviceCommandValidator
        : AbstractValidator<AddEmployeeDeviceCommand>
    {
        public AddEmployeeDeviceCommandValidator()
        {
            RuleFor(x => x.DeviceId)
                .NotEmpty()
                .WithMessage("Device ID is required")
                .MaximumLength(255);

            RuleFor(x => x.Platform)
                .IsInEnum()
                .WithMessage("Platform must be Android or iOS");

            RuleFor(x => x.OsVersion)
                .NotEmpty()
                .WithMessage("OS version is required")
                .MaximumLength(20);

            RuleFor(x => x.Manufacturer)
                .NotEmpty()
                .WithMessage("Manufacturer is required")
                .MaximumLength(100);

            RuleFor(x => x.Model)
                .NotEmpty()
                .WithMessage("Model is required")
                .MaximumLength(100);

            RuleFor(x => x.Brand)
                .NotEmpty()
                .WithMessage("Brand is required")
                .MaximumLength(100);

            RuleFor(x => x.DeviceFingerprint)
                .NotEmpty()
                .WithMessage("Device fingerprint is required")
                .MaximumLength(500);

            RuleFor(x => x.AppVersion)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.AppVersion));
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
            int rowsAffected = await _db.TbEmployeeDevicesTrack
                .Where(d => d.EmployeeId == request.EmployeeId && d.IsActiveDevice)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(d => d.IsActiveDevice, false)
                    .SetProperty(d => d.ResetByUserId, _currentUser.UserId)
                    .SetProperty(d => d.ResetByUserDate, DateTime.UtcNow),
                    ct);

            return rowsAffected > 0;
        }
    }
    #endregion

    #region ListEmployeeDevices
    public class EmployeeDeviceDto
    {
        public int Id { get; set; }
        public string DeviceId { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string OsVersion { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public bool IsPhysicalDevice { get; set; }
        public string DeviceFingerprint { get; set; } = string.Empty;
        public string? AppVersion { get; set; }
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
            return await _db.TbEmployeeDevicesTrack
                .AsNoTracking()
                .Where(d => d.EmployeeId == request.EmployeeId)
                .OrderByDescending(d => d.IsActiveDevice)
                .ThenByDescending(d => d.LastActiveAt)
                .Select(d => new EmployeeDeviceDto
                {
                    Id = d.Id,
                    DeviceId = d.DeviceUid,
                    Platform = ((DevicePlatform)d.Platform).ToString(),
                    OsVersion = d.OsVersion,
                    Manufacturer = d.Manufacturer,
                    Model = d.Model,
                    Brand = d.Brand,
                    IsPhysicalDevice = d.IsPhysicalDevice,
                    DeviceFingerprint = d.DeviceFingerprint,
                    AppVersion = d.AppVersion,
                    IsActiveDevice = d.IsActiveDevice,
                    RegisteredAt = d.RegisteredAt,
                    LastActiveAt = d.LastActiveAt
                })
                .ToListAsync(ct);
        }
    }
    #endregion
}
