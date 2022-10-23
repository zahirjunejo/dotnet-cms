CREATE TABLE dbo.Directory
(
	Id int not null identity primary key,
    OrganizationName varchar(100),
	Industry varchar(50),
	Logo varchar(50),
	Website varchar(100),
	ContactAddress varchar(250),
	PhoneNumber varchar(100),
	PointOfContact varchar(100),
	EmailAddress varchar(100)
);