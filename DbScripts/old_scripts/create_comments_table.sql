CREATE TABLE dbo.Comments
(
	Id int not null identity primary key,
	Commentator varchar(100),
	EmailAddress varchar(100),
	Content varchar(250),
	PostId int
);