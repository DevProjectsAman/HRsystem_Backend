namespace HRsystem.Api.Services.LookupCashing
{
    
    public static class ActivityCodes
    {
        public const string Attendance = "ATT";
        public const string VacationRequest = "REQ_VAC";
        public const string MissionRequest = "REQ_MISSION";
        public const string ExcuseRequest = "REQ_EXCUSE";
    }

    public sealed class ActivityTypeLookup
    {
        public int Id { get; init; }
        public string Code { get; init; } = default!;
        public string NameAr { get; init; } = default!;
        public string NameEn { get; init; } = default!;
    }
    public interface IActivityTypeLookupCache
    {
        int GetIdByCode(string code);
        string GetCodeById(int id);
        ActivityTypeLookup GetByCode(string code);
        ActivityTypeLookup GetById(int id);
    }
    public sealed class ActivityTypeLookupCache : IActivityTypeLookupCache
    {
        private readonly Dictionary<string, ActivityTypeLookup> _byCode;
        private readonly Dictionary<int, ActivityTypeLookup> _byId;

        public ActivityTypeLookupCache(IEnumerable<ActivityTypeLookup> lookups)
        {
            _byCode = lookups.ToDictionary(x => x.Code);
            _byId = lookups.ToDictionary(x => x.Id);
        }

        public int GetIdByCode(string code)
            => _byCode[code].Id;

        public string GetCodeById(int id)
            => _byId[id].Code;

        public ActivityTypeLookup GetByCode(string code)
            => _byCode[code];

        public ActivityTypeLookup GetById(int id)
            => _byId[id];
    }

}
