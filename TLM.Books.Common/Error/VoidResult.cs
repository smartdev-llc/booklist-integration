using Microsoft.AspNetCore.Mvc;

namespace TLM.Books.Common.Error;

public class VoidMethodResult
{
    private readonly List<ErrorResult> _errorMessages = new List<ErrorResult>();

    public void AddErrorMessage(ErrorResult errorResult) => this._errorMessages.Add(errorResult);

    public void AddErrorMessage(string errorCode, string errorMessage, string[] errorValues)
    {
        ErrorResult errorResult = new ErrorResult()
        {
            ErrorCode = errorCode,
            ErrorMessage = errorMessage
        };
        if (errorValues != null && (uint) errorValues.Length > 0U)
        {
            foreach (string errorValue in errorValues)
                errorResult.ErrorValues.Add(errorValue);
        }
        this.AddErrorMessage(errorResult);
    }

    public void AddErrorMessage(string exceptionErrorMessage, string exceptionStackTrace = "") => this.AddErrorMessage("ERR_COM_API_SERVER_ERROR", "API_ERROR", new string[0], exceptionErrorMessage, exceptionStackTrace);

    private void AddErrorMessage(
        string errorCode,
        string errorMessage,
        string[] errorValues,
        string exceptionErrorMessage,
        string exceptionStackTrace)
    {
        this._errorMessages.Add(new ErrorResult()
        {
            ErrorCode = errorCode,
            ErrorMessage = "Error: " + errorMessage + ", Exception Message: " + exceptionErrorMessage + ", Stack Trace: " + exceptionStackTrace,
            ErrorValues = new List<string>((IEnumerable<string>) errorValues)
        });
    }

    public IReadOnlyCollection<ErrorResult> ErrorMessages => (IReadOnlyCollection<ErrorResult>) this._errorMessages;

    public bool IsOK => this._errorMessages.Count == 0;

    public int? StatusCode { get; set; }

    public virtual IActionResult GetActionResult()
    {
        ObjectResult actionResult = new ObjectResult((object) this);
        if (!this.StatusCode.HasValue)
        {
            actionResult.StatusCode = new int?(500);
            return (IActionResult) actionResult;
        }
        actionResult.StatusCode = this.StatusCode;
        return (IActionResult) actionResult;
    }
}