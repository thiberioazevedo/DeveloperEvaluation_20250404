using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Domain.Entities;


public class Branch : BaseEntity
{
    public Branch()
    {

    }
    public Branch(Guid id, string name, int number)
    {
        Id = id;
        Name = name;
        Number = number;
    }

    public string Name { get; set; }
    public virtual ICollection<Sale> SaleCollection { get; set; }

    public override ValidationResult GetValidationResult()
    {
        return new BranchValidator().Validate(this);
    }

    public bool SetName(string name)
    {
        Name = name;

        return true;
    }
}