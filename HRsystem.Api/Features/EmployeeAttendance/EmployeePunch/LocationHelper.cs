namespace HRsystem.Api.Features.EmployeeAttendance.EmployeePunch
{
    public static class LocationHelper
    {
        public static bool IsWithinAllowedRadius(double employeeLat, double employeeLng, double locationLat, double locationLng, double allowedRadiusMeters)
        {
            double R = 6371000; // نصف قطر الأرض بالمتر
            double dLat = ToRadians(locationLat - employeeLat);
            double dLon = ToRadians(locationLng - employeeLng);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(employeeLat)) * Math.Cos(ToRadians(locationLat)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c; // المسافة بالمتر

            return distance <= allowedRadiusMeters;
        }

        private static double ToRadians(double deg) => deg * (Math.PI / 180);
    }

}
