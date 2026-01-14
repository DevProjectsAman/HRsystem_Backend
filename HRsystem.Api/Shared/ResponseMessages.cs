namespace HRsystem.Api.Shared
{
    public class ResponseMessages
    {

        #region Done
        public const string Saved = "data has been saved successfully";
        public const string Deleted = "data has been Deleted successfully";
        public const string Updated = "data has been Updated successfully";
        public const string Aprroved = "data has been Approved successfully";
        public const string SucessRegister = "Succ. Registered";
        public const string SucessUpdated = "Succ. Updated";
        public const string SucessLogin = "Succ. Login";
        public const string DataReturnedSucc = "Data Returned Succ.essfully";

        public const string PublishedMessage = "Report has been Published successfully";

        #endregion




        #region Error
        public const string InvalidSaved = "error! data not saved";
        public const string InvalidDelete = "error! data not Deleted";
        public const string InvalidLogin = "invalid UserName Or Passwod ";
        public const string PhoneAleardyExist = "Phone Already Exist";
        public const string UserNameAleardyExist = "UserName Already Exist";
        public const string EmailAleardyExist = "Email Already Exist";
        public const string RoleInvalid = "Invalid Roles";
        public const string InvalidValidation = "Invalid Validation";
        public const string ReportCompleted = "ReportCompleted";

        public const string IDNotFound = "ID Not Found";
        public const string NoDataFound = "No Data Found";
        public const string NoItemsToPublish = "no items to publish";
        public const string AlreadyPublished = "Report Already published";

        public const string SponsorAleardyExist = "Sponsor Already Exist";
        public const string ReportTypeAleardyExist = "Report Type Already Exist";
        public const string SponsorReportTypeAleardyExist = " Sponsor Report Type Already Exist";

        public const string NotAllowed = "Not allowed to do this action";

        #endregion

    }
}
