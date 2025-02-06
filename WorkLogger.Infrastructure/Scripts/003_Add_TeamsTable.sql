CREATE TABLE Teams (
                       Id SERIAL PRIMARY KEY,
                       Name VARCHAR(255)
);

ALTER TABLE Users ADD COLUMN TeamId INT NULL;

ALTER TABLE Users ADD CONSTRAINT FK_Users_Teams FOREIGN KEY (TeamId) REFERENCES Teams(Id);