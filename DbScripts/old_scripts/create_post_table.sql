CREATE TABLE dbo.Posts
(
	Id int not null identity primary key,
    Title varchar(50) NULL,
    Content varchar(max) NULL,
	Category varchar(50) NOT NULL,
	PostStatus varchar(50) NOT NULL
);