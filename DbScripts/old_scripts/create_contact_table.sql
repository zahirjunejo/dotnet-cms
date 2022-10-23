CREATE TABLE dbo.Contacts
(
	Id int not null identity primary key,
    FullName varchar(50) NOT NULL,
    LastName varchar(25) NOT NULL,
	PhoneNumber varchar(50) NOT NULL,
	Email varchar(75) NOT NULL,
	Content varchar(255) NOT NULL
);