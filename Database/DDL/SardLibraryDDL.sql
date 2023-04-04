/*** Time ***/

CREATE TABLE Eras (
	Id       INT           NOT NULL,
	Stage    INT           NOT NULL,
	Eon      INT           NOT NULL,
	Epoch    INT           NOT NULL,
	Age      INT           NOT NULL,
	Era      INT           NOT NULL,
	PRIMARY KEY (Id)
)

/*** Maps ***/

CREATE TABLE MapTiles (
	Z FLOAT NOT NULL,
	Y FLOAT NOT NULL,
	X FLOAT NOT NULL,
	PRIMARY KEY (Z, Y, X)
)

/*** Location Types ***/

CREATE TABLE CelestialObjectTypes (
	Id                INT           NOT NULL,
	Name              VARCHAR(MAX)  NOT NULL,
)

/*** Locations ***/

CREATE TABLE Manifolds (
	Id      INT NOT NULL,
	Number  INT NOT NULL UNIQUE,
	Name    VARCHAR(MAX),
	PRIMARY KEY (Id)
)

CREATE TABLE CelestialSystems (
	Id         INT           NOT NULL,
	Name       VARCHAR(MAX)  NOT NULL,
	ManifoldId INT           NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (ManifoldId) REFERENCES Manifolds (Id)
)

CREATE TABLE CelestialObjects (
	Id                  INT           NOT NULL,
	Name                VARCHAR(MAX)  NOT NULL,
	CelestialSystemId INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (CelestialSystemId) REFERENCES CelestialSystem (Id)
)

CREATE TABLE Continents (
	Id                 INT          NOT NULL,
	Name               VARCHAR(MAX) NOT NULL,
	CelestialObjectId  INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (CelestialObjectId) REFERENCES CelestialObject (Id)
)

CREATE TABLE Subcontinents (
	Id                 INT          NOT NULL,
	Name               VARCHAR(MAX) NOT NULL,
	ContinentId        INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (ContinentId) REFERENCES Continent (Id)
)

CREATE TABLE Regions (
	Id                 INT          NOT NULL,
	Name               VARCHAR(MAX) NOT NULL,
	SubContinentId     INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (SubContinentId) REFERENCES SubContinent (Id)
)

CREATE TABLE Subregions (
	Id                 INT          NOT NULL,
	Name               VARCHAR(MAX) NOT NULL,
	RegionId           INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (RegionId) REFERENCES Region (Id)
)

CREATE TABLE Areas (
	Id                 INT          NOT NULL,
	Name               VARCHAR(MAX) NOT NULL,
	ContinentId        INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (ContinentId) REFERENCES Continent (Id)
)

CREATE TABLE Locations (
	Id           INT           NOT NULL,
	LocationName INT           NOT NULL,
	Longitude    FLOAT,
	Latitude     FLOAT,
	PRIMARY KEY (Id),
)

/*** Characters ***/

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
	BirthEraId INT,
	BirthDate  BIGINT,
	TitleId    INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (RaceId) REFERENCES Races (Id),
	FOREIGN KEY (BirthEraId) REFERENCES Eras (Id),
	FOREIGN KEY (TitleId) REFERENCES Titles (Id)
)

/*** Document Types ***/

CREATE TABLE DocumentTypes (
	Id        INT           NOT NULL,
	Name      VARCHAR (MAX) NOT NULL,
	Diagetic  BIT           NOT NULL,
	PRIMARY KEY (Id)
)

/*** Document ***/

CREATE TABLE Publishers (
	Id            INT           NOT NULL,
	PublisherName INT           NOT NULL,
	LocationId    INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (LocationId) REFERENCES Locations(Id)
)

CREATE TABLE Documents (
	Id          INT            NOT NULL,
	Title       VARCHAR (MAX)  NOT NULL,
	Subtitle    VARCHAR (MAX),
	AuthorId    INT,
	PublisherId INT,
	Summary     VARCHAR (MAX),
	Content     VARCHAR (MAX),
	PRIMARY KEY (Id),
	FOREIGN KEY (AuthorId) REFERENCES People(Id),
	FOREIGN KEY (PublisherId) REFERENCES Publishers(Id)
)
