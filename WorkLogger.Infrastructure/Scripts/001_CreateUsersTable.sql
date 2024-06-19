CREATE TABLE Users
(
    Id INT PRIMARY KEY,
    Name NVARCHAR(MAX),
    Surname NVARCHAR(MAX),
    UserName NVARCHAR(MAX),
    Role INT,
    PasswordHash VARBINARY(MAX),
    PasswordSalt VARBINARY(MAX)
)