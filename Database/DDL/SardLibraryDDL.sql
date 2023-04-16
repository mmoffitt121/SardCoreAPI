/*** Time ***/

CREATE TABLE Eras (
	Id       INT           NOT NULL AUTO_INCREMENT,
	Stage    INT           NOT NULL,
	Eon      INT           NOT NULL,
	Epoch    INT           NOT NULL,
	Age      INT           NOT NULL,
	Era      INT           NOT NULL,
	PRIMARY KEY (Id)
);

/*** Document ***/

CREATE TABLE Documents (
	Id          INT              NOT NULL AUTO_INCREMENT,
	Title       VARCHAR (1000)   NOT NULL,
	Subtitle    VARCHAR (1000),
	Summary     VARCHAR (6000),
	Content     MEDIUMTEXT,
	PRIMARY KEY (Id)
);

/*** Maps ***/

CREATE TABLE MapLayers (
	Id         INT            NOT NULL AUTO_INCREMENT,
	Name       VARCHAR(1000)  NOT NULL,
	LayerDate  BIGINT,
	LayerEraId INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (LayerEraId) REFERENCES Eras (Id)
);

CREATE TABLE MapTiles (
	Z       FLOAT          NOT NULL,
	Y       FLOAT          NOT NULL,
	X       FLOAT          NOT NULL,
	LayerId INT            NOT NULL,
	Tile  MEDIUMBLOB NOT NULL,
	PRIMARY KEY (Z, Y, X, LayerId),
	FOREIGN KEY (LayerId) REFERENCES MapLayers (Id)
);

/*** Location Types ***/

CREATE TABLE CelestialObjectTypes (
	Id                INT            NOT NULL AUTO_INCREMENT,
	Name              VARCHAR(1000)  NOT NULL,
    PRIMARY KEY (Id)
);

/*** Locations ***/

CREATE TABLE Manifolds (
	Id      INT NOT NULL AUTO_INCREMENT,
	Number  INT NOT NULL UNIQUE,
	Name    VARCHAR(1000),
	PRIMARY KEY (Id)
);

CREATE TABLE CelestialSystems (
	Id         INT            NOT NULL AUTO_INCREMENT,
	Name       VARCHAR(1000)  NOT NULL,
	ManifoldId INT            NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (ManifoldId) REFERENCES Manifolds (Id)
);

CREATE TABLE CelestialObjects (
	Id                  INT            NOT NULL AUTO_INCREMENT,
	Name                VARCHAR(1000)  NOT NULL,
	CelestialSystemId   INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (CelestialSystemId) REFERENCES CelestialSystems (Id)
);

CREATE TABLE Continents (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	CelestialObjectId  INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	Aquatic            INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (CelestialObjectId) REFERENCES CelestialObjects (Id)
);

CREATE TABLE Subcontinents (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	ContinentId        INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (ContinentId) REFERENCES Continents (Id)
);

CREATE TABLE Regions (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	SubContinentId     INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (SubContinentId) REFERENCES SubContinents (Id)
);

CREATE TABLE Subregions (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	RegionId           INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (RegionId) REFERENCES Regions (Id)
);

CREATE TABLE Areas (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	SubregionId        INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (SubregionId) REFERENCES Subregions (Id)
);

CREATE TABLE LocationTypes (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	PRIMARY KEY (Id)
);

INSERT INTO LocationTypes (Name) VALUES
	('City'),
	('Town'),
	('Village'),
	('Hamlet'),
	('Fortress'),
	('River');


CREATE TABLE Locations (
	Id             INT           NOT NULL AUTO_INCREMENT,
	LocationName   VARCHAR(1000) NOT NULL,
	AreaId         INT,
	LocationTypeId INT,
	Longitude      FLOAT,
	Latitude       FLOAT,
    DocumentId     INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (AreaId) REFERENCES Areas (Id),
	FOREIGN KEY (LocationTypeId) REFERENCES LocationTypes (Id),
    FOREIGN KEY (DocumentId) REFERENCES Documents (Id)
);

/*** Characters ***/

CREATE TABLE Races (
	Id       INT            NOT NULL AUTO_INCREMENT,
	RaceName VARCHAR(1000)  NOT NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE Titles (
	Id         INT             NOT NULL AUTO_INCREMENT,
	TitleName  VARCHAR(1000)  NOT NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE People (
	Id         INT             NOT NULL AUTO_INCREMENT,
	FirstName  VARCHAR(1000),
	LastName   VARCHAR(1000),
	RaceId     INT,
	BirthEraId INT,
	BirthDate  BIGINT,
	TitleId    INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (RaceId) REFERENCES Races (Id),
	FOREIGN KEY (BirthEraId) REFERENCES Eras (Id),
	FOREIGN KEY (TitleId) REFERENCES Titles (Id)
);

/*** Document Types ***/

CREATE TABLE DocumentTypes (
	Id        INT            NOT NULL AUTO_INCREMENT,
	Name      VARCHAR (1000) NOT NULL,
	Diagetic  BIT            NOT NULL,
	PRIMARY KEY (Id)
);

/*** Document Information ***/

CREATE TABLE Publishers (
	Id            INT           NOT NULL AUTO_INCREMENT,
	PublisherName INT           NOT NULL,
	LocationId    INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (LocationId) REFERENCES Locations(Id)
);
