namespace TLM.Books.Common;

public class Helpers
{
    public static string GetExceptionMessage(Exception ex) => "Message: " + ex.Message + ", InnerMessage: " + ex.InnerException?.Message;

}