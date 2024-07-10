using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkLogger.Domain.Enums;

namespace WorkLogger.Domain.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string UserName { get; private set; }
    public Roles Role { get; private set; }
    public byte[] PasswordHash { get; private set; }
    public byte[] PasswordSalt { get; private set; }
    public int? TeamId { get; private set; }
    
    public class Builder
    {
        private int _companyId;
        private string _name;
        private string _surname;
        private string _userName;
        private Roles _role;
        private byte[] _passwordHash;
        private byte[] _passwordSalt;
        private int? _teamId;

        public Builder WithUserCredentials(string name, string surname, string userName)
        {
            _name = name;
            _surname = surname;
            _userName = userName;
            return this;
        }

        public Builder WithCompanyInfo(int companyId, int? teamId, Roles role)
        {
            _companyId = companyId;
            _teamId = teamId;
            _role = role;
            
            return this;
        }

        public Builder WithPassword(byte[] passwordHash, byte[] passwordSalt)
        {
            _passwordHash = passwordHash;
            _passwordSalt = passwordSalt;
            return this;
        }

        public User Build()
        {
            if (_companyId < 0 || string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_surname) || string.IsNullOrEmpty(_userName) ||
                _role == null ||
                _passwordHash == null || _passwordSalt == null)
            {
                throw new ArgumentException("Not all mandatory fields are filled");
            }

            return new User
            {
                CompanyId = _companyId,
                Name = _name,
                Surname = _surname,
                UserName = _userName,
                Role = _role,
                PasswordHash = _passwordHash,
                PasswordSalt = _passwordSalt,
                TeamId = _teamId
            };
        }
    }
}