using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public class BaseEntity : IComparable<BaseEntity>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Number { get; internal set; }
    public BaseEntity()
    {
    }
    public void SetNumber(int number)
    {
        Number = number;
    }
    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }

    public int CompareTo(BaseEntity? other)
    {
        if (other == null)
        {
            return 1;
        }

        return other!.Id.CompareTo(Id);
    }

    public virtual ValidationResult GetValidationResult() {
        throw new NotImplementedException();
    }

    public ValidationResultDetail Validate()
    {
        var result = GetValidationResult();

        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            ValidationFailureList = result.Errors
        };
    }
}

public class EntityChanged
{
    public Guid Key { get; set; }
    public string Name { get; set; }
    public IList<PropertieChanged> Properties { get; set; } = [];
    public EOperationPropertieChanged OperationPropertieChanged { get; set; }
}
public class PropertieChanged
{
    public string Name { get; set; }
    public object Value { get; set; }
}

public enum EOperationPropertieChanged { 
    Created,
    Updated,
    Deleted
}