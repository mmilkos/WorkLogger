CREATE TABLE Companies (
                           Id INT PRIMARY KEY IDENTITY,
                           Name NVARCHAR(255)
);

CREATE TABLE Users (
                       Id INT PRIMARY KEY IDENTITY,
                       CompanyId INT,
                       Name NVARCHAR(255),
                       Surname NVARCHAR(255),
                       UserName NVARCHAR(255),
                       Role INT,
                       PasswordHash VARBINARY(64),
                       PasswordSalt VARBINARY(16),
                       FOREIGN KEY (CompanyId) REFERENCES Companies(Id) ON DELETE CASCADE
);

CREATE TABLE UserTasks (
                           Id INT PRIMARY KEY IDENTITY,
                           AssignedUserId INT NULL,
                           AuthorId INT NULL,
                           Name NVARCHAR(255),
                           Description NVARCHAR(MAX),
                           LoggedHours FLOAT,
                           FOREIGN KEY (AssignedUserId) REFERENCES Users(Id) ON DELETE NO ACTION,
                           FOREIGN KEY (AuthorId) REFERENCES Users(Id) ON DELETE NO ACTION
);