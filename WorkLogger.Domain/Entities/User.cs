using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using WorkLogger.Domain.Enums;

namespace WorkLogger.Domain.Entities;

[Table("users")]
public class User : BaseEntity
{
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string UserName { get; private set; }
    public Roles Role { get; private set; }
    public string PasswordHash { get; private set; }
    public int? TeamId { get; private set; }
    public Team Team { get; private set; }
    
    public class Builder
    {
        private int _companyId;
        private string _name;
        private string _surname;
        private string _userName;
        private Roles _role;
        private string _password;
        private int? _teamId;

        public Builder WithUserCredentials(string name, string surname, string userName, string password)
        {
            _name = name;
            _surname = surname;
            _userName = userName;
            _password = password;
            return this;
        }

        public Builder WithCompanyInfo(int companyId, int? teamId, Roles role)
        {
            _companyId = companyId;
            _teamId = teamId;
            _role = role;
            
            return this;
        }
        

        public User Build()
        {
            var passwordHasher = new PasswordHasher<User>();
            
            if (_companyId < 0 || string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_surname) || string.IsNullOrEmpty(_userName) ||
                _role == null)
            {
                throw new ArgumentException("Not all mandatory fields are filled");
            }
            
            var user = new User
            {
                CompanyId = _companyId,
                Name = _name,
                Surname = _surname,
                UserName = _userName,
                Role = _role,
                TeamId = _teamId
            };

            user.PasswordHash = passwordHasher.HashPassword(user, _password);

            return user;
        }
    }

    public void SetTeam(int? teamId)
    {
        TeamId = teamId;
    }
}