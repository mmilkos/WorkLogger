ALTER TABLE Teams
    ADD CompanyId INT;

ALTER TABLE Teams
    ADD FOREIGN KEY (CompanyId) REFERENCES Companies(Id);