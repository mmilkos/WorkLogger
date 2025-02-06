ALTER TABLE Teams
    ADD COLUMN CompanyId INT;

ALTER TABLE Teams
    ADD CONSTRAINT FK_Teams_Companies FOREIGN KEY (CompanyId) REFERENCES Companies(Id);