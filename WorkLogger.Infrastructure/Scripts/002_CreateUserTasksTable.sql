CREATE TABLE UserTasks
(
    Id INT PRIMARY KEY,
    AssignedUserId INT,
    AuthorId INT,
    Name NVARCHAR(MAX),
    Description NVARCHAR(MAX),
    LoggedHours FLOAT,

    FOREIGN KEY (AssignedUserId) REFERENCES Users(Id),
    FOREIGN KEY (AuthorId) REFERENCES Users(Id)
)