CREATE TABLE Teams (
                           Id INT PRIMARY KEY IDENTITY,
                           Name NVARCHAR(255)
);

ALTER TABLE Users ADD TeamId INT NULL;

ALTER TABLE Users ADD FOREIGN KEY (TeamId) REFERENCES Teams(Id);