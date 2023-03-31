CREATE TABLE Angorainnes (
	Id         INT    NOT NULL,
	Stage      INT    NOT NULL,
	Eon        INT    NOT NULL,
	Epoch      INT    NOT NULL,
	Age        INT    NOT NULL,
	Era        INT    NOT NULL,
	PRIMARY KEY (Id)
)

CREATE TABLE Dates (
    Id         INT NOT NULL,
	Angorainne INT NOT NULL,
	Moyorainne INT,
	Limitainne INT,
	PRIMARY KEY (Id)
)

CREATE TABLE Races (
	Id       INT           NOT NULL,
	RaceName VARCHAR(MAX)  NOT NULL,
	PRIMARY KEY (Id)
)

CREATE TABLE Titles (
	Id         INT           NOT NULL,
	TitleName  VARCHAR(MAX)  NOT NULL,
	PRIMARY KEY (Id)
)

CREATE TABLE People (
	Id         INT           NOT NULL,
	FirstName  VARCHAR(MAX),
	LastName   VARCHAR(MAX),
	RaceId     INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (RaceId) REFERENCES Races (Id)
)

CREATE TABLE Documents (
	Id          INT           NOT NULL,
	Title       VARCHAR(MAX)  NOT NULL,
	Subtitle    VARCHAR(MAX),
	AuthorId    INT,
	PublisherId VARCHAR(MAX),
	PRIMARY KEY (Id),
	FOREIGN KEY (AuthorId) REFERENCES People(Id)
)

