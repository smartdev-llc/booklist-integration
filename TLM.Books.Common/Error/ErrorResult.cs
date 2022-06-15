using System.ComponentModel.DataAnnotations.Schema;

namespace TLM.Books.Common.Error;

[NotMapped]
public class ErrorResult
{
    public ErrorResult() => this.ErrorValues = new List<string>();

    public string ErrorCode { get; set; }

    public string ErrorMessage { get; set; }

    public List<string> ErrorValues { get; set; }

    public override string ToString() => this.ErrorValues != null && this.ErrorValues.Count > 0 ? "[" + this.ErrorCode + ": " + this.ErrorMessage + " (" + string.Join<string>(',', (IEnumerable<string>) this.ErrorValues) + ")]" : "[" + this.ErrorCode + ": " + this.ErrorMessage + "]";
}