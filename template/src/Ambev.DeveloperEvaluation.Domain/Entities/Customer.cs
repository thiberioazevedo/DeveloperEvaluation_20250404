using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Customer : BaseEntity
{
    public Customer(){
    }

    public Customer(Guid id, string name, string email, int number)
    {
        Id = id;
        Name = name;
        Email = email;
        Number = number;
    }

    public string Name { get; set; }
    public string Email { get; set; }
    public virtual ICollection<Sale> SaleCollection { get; set; }

    public bool SetName(string name)
    {
        Name = name;

        return true;
    }

    public ValidationResultDetail Validate()
    {
        var validator = new CustomerValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail(result);
    }
}