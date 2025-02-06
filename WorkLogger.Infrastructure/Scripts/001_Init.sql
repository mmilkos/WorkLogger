CREATE TABLE Companies (
                           Id SERIAL PRIMARY KEY,
                           Name VARCHAR(255)
);

CREATE TABLE Users (
                       Id SERIAL PRIMARY KEY,
                       CompanyId INT,
                       Name VARCHAR(255),
                       Surname VARCHAR(255),
                       UserName VARCHAR(255),
                       Role INT,
                       PasswordHash BYTEA, -- Zamiast VARBINARY
                       PasswordSalt BYTEA, -- Zamiast VARBINARY
                       FOREIGN KEY (CompanyId) REFERENCES Companies(Id) ON DELETE CASCADE
);

CREATE TABLE UserTasks (
                           Id SERIAL PRIMARY KEY,
                           AssignedUserId INT NULL,
                           AuthorId INT NULL,
                           Name VARCHAR(255),
                           Description TEXT, -- Zamiast NVARCHAR(MAX)
                           LoggedHours FLOAT,
                           FOREIGN KEY (AssignedUserId) REFERENCES Users(Id) ON DELETE NO ACTION,
                           FOREIGN KEY (AuthorId) REFERENCES Users(Id) ON DELETE NO ACTION
);