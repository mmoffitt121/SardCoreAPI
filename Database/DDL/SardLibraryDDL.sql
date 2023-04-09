/*** Time ***/

CREATE TABLE Eras (
	Id       INT           NOT NULL IDENTITY(1,1),
	Stage    INT           NOT NULL,
	Eon      INT           NOT NULL,
	Epoch    INT           NOT NULL,
	Age      INT           NOT NULL,
	Era      INT           NOT NULL,
	PRIMARY KEY (Id)
)

/*** Maps ***/

CREATE TABLE MapLayers (
	Id         INT           NOT NULL IDENTITY(1,1),
	Name       VARCHAR(MAX)  NOT NULL,
	LayerDate  BIGINT,
	LayerEraId INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (LayerEraId) REFERENCES Eras (Id)
)

CREATE TABLE MapTiles (
	Z       FLOAT          NOT NULL,
	Y       FLOAT          NOT NULL,
	X       FLOAT          NOT NULL,
	LayerId INT            NOT NULL,
	Tile  VARBINARY(MAX) NOT NULL,
	PRIMARY KEY (Z, Y, X, LayerId),
	FOREIGN KEY (LayerId) REFERENCES MapLayers (Id)
)

/*** Location Types ***/

CREATE TABLE CelestialObjectTypes (
	Id                INT           NOT NULL IDENTITY(1,1),
	Name              VARCHAR(MAX)  NOT NULL,
)

/*** Locations ***/

CREATE TABLE Manifolds (
	Id      INT NOT NULL IDENTITY(1,1),
	Number  INT NOT NULL UNIQUE,
	Name    VARCHAR(MAX),
	PRIMARY KEY (Id)
)

CREATE TABLE CelestialSystems (
	Id         INT           NOT NULL IDENTITY(1,1),
	Name       VARCHAR(MAX)  NOT NULL,
	ManifoldId INT           NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (ManifoldId) REFERENCES Manifolds (Id)
)

CREATE TABLE CelestialObjects (
	Id                  INT           NOT NULL IDENTITY(1,1),
	Name                VARCHAR(MAX)  NOT NULL,
	CelestialSystemId   INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (CelestialSystemId) REFERENCES CelestialSystems (Id)
)

CREATE TABLE Continents (
	Id                 INT          NOT NULL IDENTITY(1,1),
	Name               VARCHAR(MAX) NOT NULL,
	CelestialObjectId  INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (CelestialObjectId) REFERENCES CelestialObjects (Id)
)

CREATE TABLE Subcontinents (
	Id                 INT          NOT NULL IDENTITY(1,1),
	Name               VARCHAR(MAX) NOT NULL,
	ContinentId        INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (ContinentId) REFERENCES Continents (Id)
)

CREATE TABLE Regions (
	Id                 INT          NOT NULL IDENTITY(1,1),
	Name               VARCHAR(MAX) NOT NULL,
	SubContinentId     INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (SubContinentId) REFERENCES SubContinents (Id)
)

CREATE TABLE Subregions (
	Id                 INT          NOT NULL IDENTITY(1,1),
	Name               VARCHAR(MAX) NOT NULL,
	RegionId           INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (RegionId) REFERENCES Regions (Id)
)

CREATE TABLE Areas (
	Id                 INT          NOT NULL IDENTITY(1,1),
	Name               VARCHAR(MAX) NOT NULL,
	SubregionId        INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (SubregionId) REFERENCES Subregions (Id)
)

CREATE TABLE LocationTypes (
	Id                 INT          NOT NULL IDENTITY(1,1),
	Name               VARCHAR(MAX) NOT NULL,
	PRIMARY KEY (Id)
)

INSERT INTO LocationTypes (Name) VALUES
	('City'),
	('Town'),
	('Village'),
	('Hamlet'),
	('Fortress'),
	('River')


CREATE TABLE Locations (
	Id             INT           NOT NULL IDENTITY(1,1),
	LocationName   INT           NOT NULL,
	AreaId         INT,
	LocationTypeId INT,
	Longitude      FLOAT,
	Latitude       FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (AreaId) REFERENCES Areas (Id),
	FOREIGN KEY (LocationTypeId) REFERENCES LocationTypes (Id)
)

/*** Characters ***/

CREATE TABLE Races (
	Id       INT           NOT NULL IDENTITY(1,1),
	RaceName VARCHAR(MAX)  NOT NULL,
	PRIMARY KEY (Id)
)

CREATE TABLE Titles (
	Id         INT           NOT NULL IDENTITY(1,1),
	TitleName  VARCHAR(MAX)  NOT NULL,
	PRIMARY KEY (Id)
)

CREATE TABLE People (
	Id         INT           NOT NULL IDENTITY(1,1),
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
	Id        INT           NOT NULL IDENTITY(1,1),
	Name      VARCHAR (MAX) NOT NULL,
	Diagetic  BIT           NOT NULL,
	PRIMARY KEY (Id)
)

/*** Document ***/

CREATE TABLE Publishers (
	Id            INT           NOT NULL IDENTITY(1,1),
	PublisherName INT           NOT NULL,
	LocationId    INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (LocationId) REFERENCES Locations(Id)
)

CREATE TABLE Documents (
	Id          INT            NOT NULL IDENTITY(1,1),
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

/*** Document Associations ***/

CREATE TABLE DocumentLocations (
	Id         INT     NOT NULL IDENTITY(1,1),
	DocumentId INT     NOT NULL,
	LocationId INT     NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (DocumentId) REFERENCES Documents (Id),
	FOREIGN KEY (LocationId) REFERENCES Locations (Id)
)

CREATE TABLE DocumentAreas (
	Id         INT     NOT NULL IDENTITY(1,1),
	DocumentId INT     NOT NULL,
	AreaId     INT     NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (DocumentId) REFERENCES Documents (Id),
	FOREIGN KEY (AreaId) REFERENCES Areas (Id)
)

CREATE TABLE DocumentPeople (
	Id         INT     NOT NULL IDENTITY(1,1),
	DocumentId INT     NOT NULL,
	PersonId   INT     NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (DocumentId) REFERENCES Documents (Id),
	FOREIGN KEY (PersonId) REFERENCES People (Id)
)

CREATE TABLE DocumentPublishers (
	Id          INT     NOT NULL IDENTITY(1,1),
	DocumentId  INT     NOT NULL,
	PublisherId INT     NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (DocumentId) REFERENCES Documents (Id),
	FOREIGN KEY (PublisherId) REFERENCES Publishers (Id)
)

CREATE TABLE DocumentRaces (
	Id          INT     NOT NULL IDENTITY(1,1),
	DocumentId  INT     NOT NULL,
	RaceId      INT     NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (DocumentId) REFERENCES Documents (Id),
	FOREIGN KEY (RaceId) REFERENCES Races (Id)
)
