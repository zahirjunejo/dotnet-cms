CREATE TABLE dbo.WorkForce
(
	Id int not null identity primary key,
	PriorityNumber int,
	Title varchar(100),
	DateCreated date,
	DownloadFileName varchar(100)
);