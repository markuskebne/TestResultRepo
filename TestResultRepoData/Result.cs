namespace TestResultRepoData
{
    public enum Result
    {
        Passed, 
        Failed,
        Inconclusive,
        Skipped,
        Error,
        Unknown
    }

    public static class ResultExtensions
    {
        /// <summary>
        /// Convert a string into enum Status
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Result ToStatus(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return Result.Unknown;
            }

            str = str.Trim().ToLower();

            switch (str)
            {
                case "skipped":
                case "ignored":
                case "not-run":
                case "notrun":
                case "notexecuted":
                case "not-executed":
                    return Result.Skipped;

                case "pass":
                case "passed":
                case "success":
                    return Result.Passed;

                case "warning":
                case "bad":
                case "pending":
                case "inconclusive":
                case "notrunnable":
                case "disconnected":
                case "passedbutrunaborted":
                    return Result.Inconclusive;

                case "fail":
                case "failed":
                case "failure":
                case "invalid":
                    return Result.Failed;

                case "error":
                case "aborted":
                case "timeout":
                    return Result.Error;

                default:
                    return Result.Unknown;
            }
        }

        /// <summary>
        /// Convert a Status into a string
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ToString(this Result status)
        {
            return status.ToString().ToLower();
        }
    }
}
