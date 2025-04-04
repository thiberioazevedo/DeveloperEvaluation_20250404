using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Common.Validation;

public class ValidationResultDetail
{
    public bool IsValid { get; set; }
    public IEnumerable<ValidationErrorDetail> ValidationErrorDetailEnumerable
    { 
        get {
            return ValidationFailureList.Select(o => (ValidationErrorDetail)o);
        } 
    }
    public IList<ValidationFailure> ValidationFailureList { get; set; } = [];

    public ValidationResultDetail()
    {
        
    }

    public ValidationResultDetail(ValidationResult validationResult)
    {
        IsValid = validationResult.IsValid;
        ValidationFailureList = validationResult.Errors;
    }
}
