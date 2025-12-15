namespace HRsystem.Api.Services.LookupCashing
{


    public static class ActivityStatusCodes
    {
        public const string Pending = "PENDING";
        public const string ApprovedByManager = "APRV_MGR";
        public const string ApprovedByHR = "APRV_HR";
        public const string Rejected = "REJECTED";
        public const string RejectedByHR = "REJ_HR";
        public const string Cancelled = "CANCELLED";
        public const string Completed = "COMPLETED";
    }



    public sealed class ActivityStatusLookup
    {
        public int Id { get; init; }
        public string Code { get; init; } = default!;
        public string NameAr { get; init; } = default!;
        public string NameEn { get; init; } = default!;
        public bool IsFinal { get; init; }
    }

    public interface IActivityStatusLookupCache
    {
        int GetIdByCode(string code);
        string GetCodeById(int id);

        ActivityStatusLookup GetByCode(string code);
        ActivityStatusLookup GetById(int id);

        bool IsFinalStatus(string code);
        bool IsFinalStatus(int id);
    }

    public sealed class ActivityStatusLookupCache : IActivityStatusLookupCache
    {
        private readonly Dictionary<string, ActivityStatusLookup> _byCode;
        private readonly Dictionary<int, ActivityStatusLookup> _byId;

        public ActivityStatusLookupCache(IEnumerable<ActivityStatusLookup> lookups)
        {
            _byCode = lookups.ToDictionary(x => x.Code);
            _byId = lookups.ToDictionary(x => x.Id);
        }

        public int GetIdByCode(string code)
            => _byCode[code].Id;

        public string GetCodeById(int id)
            => _byId[id].Code;

        public ActivityStatusLookup GetByCode(string code)
            => _byCode[code];

        public ActivityStatusLookup GetById(int id)
            => _byId[id];

        public bool IsFinalStatus(string code)
            => _byCode[code].IsFinal;

        public bool IsFinalStatus(int id)
            => _byId[id].IsFinal;
    }

}
