namespace ServiceLayer.Utilities.ExceptionHandling
{
    public static class ErrorCodes
    {
        public static int EmailOrUsernameAlreadyTaken = 10;
        public static int UserIsAlreadyAdmin = 11;
        public static int UserNotFound = 12;
        public static int TokenNotFound = 13;
        public static int AuthorizationDenied = 14;
        public static int IncorrectPassword = 15;
    }
}
