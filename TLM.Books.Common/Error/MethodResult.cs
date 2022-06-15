using Microsoft.AspNetCore.Mvc;

namespace TLM.Books.Common.Error;

public class MethodResult<T> : VoidMethodResult
{
    public T Result { get; set; }

    public void AddResultFromErrorList(IEnumerable<ErrorResult> errorMessages)
    {
        foreach (ErrorResult errorMessage in errorMessages)
            this.AddErrorMessage(errorMessage);
    }

    public override IActionResult GetActionResult()
    {
        ObjectResult actionResult = new ObjectResult((object) this);
        if (!this.StatusCode.HasValue)
        {
            actionResult.StatusCode = !this.IsOK ? new int?(500) : new int?((object) this.Result != null ? 200 : 204);
            return (IActionResult) actionResult;
        }
        actionResult.StatusCode = this.StatusCode;
        return (IActionResult) actionResult;
    }
}