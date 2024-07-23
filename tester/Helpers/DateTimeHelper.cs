namespace tester.Helpers
{
    public static class DateTimeHelper
    {

        private static readonly TimeZoneInfo EasternAfricaTimeZone;

        static DateTimeHelper()
        {
            try
            {
                EasternAfricaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                throw new Exception("The time zone 'E. Africa Standard Time' could not be found on your system.");
            }
            catch (InvalidTimeZoneException)
            {
                throw new Exception("The time zone 'E. Africa Standard Time' is invalid.");
            }
        }

        public static DateTime GetCurrentEATTime()
        {
            try
            {
                var utcNow = DateTime.UtcNow;
                return TimeZoneInfo.ConvertTimeFromUtc(utcNow, EasternAfricaTimeZone);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while converting the current UTC time to EAT time.", ex);
            }
        }

        public static DateTime GetEATTimeFromUtc(DateTime utcTime)
        {
            try
            {
                if (utcTime.Kind != DateTimeKind.Utc)
                {
                    utcTime = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(utcTime, EasternAfricaTimeZone);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while converting UTC time to EAT time.", ex);
            }
        }

    }
}
