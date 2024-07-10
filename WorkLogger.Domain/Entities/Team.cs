using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkLogger.Domain.Entities;

public class Team : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Name { get; private set; }
    public ICollection<User> TeamMembers { get; set; }

    public Team(int companyId, string name)
    {
        CompanyId = companyId;
        Name = name;
    }
}