alter table dbo.Posts
	add EventStartDate datetime,
	    EventEndDate datetime,
	    Recurring bit,
	    EventLocation varchar(150),
	    ImageUpload varchar(100),
	    Website varchar(100),
	    Organization varchar(100),
	    EventContact varchar(100),
	    Email varchar(100),
	    Phone varchar(100);