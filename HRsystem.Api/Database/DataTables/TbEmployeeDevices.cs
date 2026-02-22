using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables
{
    [Table("Tb_Employee_Devices_Track")]
    public class TbEmployeeDevicesTrack
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Unique device identifier from the mobile app.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string DeviceUid { get; set; } = string.Empty;


        [StringLength(200)]
        public string? FcmToken { get; set; }  = default!;

        /// <summary>
        /// Device platform: 1 = Android, 2 = iOS
        /// </summary>
        [Required]
        public int Platform { get; set; }

        /// <summary>
        /// Operating system version (e.g., "13" for Android, "16.5" for iOS)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string OsVersion { get; set; } = string.Empty;

        /// <summary>
        /// Device manufacturer (e.g., "Samsung", "Apple")
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Manufacturer { get; set; } = string.Empty;

        /// <summary>
        /// Device model (e.g., "Galaxy S23", "iPhone 14 Pro")
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Device brand (e.g., "Samsung", "Apple")
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Brand { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if device is physical (true) or emulator (false)
        /// </summary>
        [Required]
        public bool IsPhysicalDevice { get; set; } = true;

        /// <summary>
        /// Unique device fingerprint for additional verification
        /// (Android fingerprint or iOS device identifier hash)
        /// </summary>
        [Required]
        [StringLength(500)]
        public string DeviceFingerprint { get; set; } = string.Empty;

        /// <summary>
        /// Mobile app version at time of registration (optional)
        /// </summary>
        [StringLength(50)]
        public string? AppVersion { get; set; }

        /// <summary>
        /// When the device was first registered
        /// </summary>
        [Required]
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last time the device was used for attendance
        /// </summary>
        [Required]
        public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates if device is currently active/approved
        /// </summary>
        [Required]
        public bool IsActiveDevice { get; set; } = true;

        /// <summary>
        /// User ID who deactivated/reset the device (if applicable)
        /// </summary>
        public int? ResetByUserId { get; set; }

        /// <summary>
        /// Date when device was deactivated/reset (if applicable)
        /// </summary>
        public DateTime? ResetByUserDate { get; set; }

        // Navigation property (optional)
        // [ForeignKey(nameof(EmployeeId))]
        // public virtual TbEmployee? Employee { get; set; }
    }
}