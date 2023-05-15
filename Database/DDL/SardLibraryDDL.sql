/*** Time ***/

CREATE TABLE IF NOT EXISTS Eras (
	Id       INT           NOT NULL AUTO_INCREMENT,
	Stage    INT           NOT NULL,
	Eon      INT           NOT NULL,
	Epoch    INT           NOT NULL,
	Age      INT           NOT NULL,
	Era      INT           NOT NULL,
	PRIMARY KEY (Id)
);

/*** Document Types ***/

CREATE TABLE IF NOT EXISTS DocumentTypes (
	Id        INT            NOT NULL AUTO_INCREMENT,
	Name      VARCHAR (1000) NOT NULL,
	Diagetic  BIT            NOT NULL,
	PRIMARY KEY (Id)
);

/*** Document ***/

CREATE TABLE IF NOT EXISTS Documents (
	Id             INT              NOT NULL AUTO_INCREMENT,
	Title          VARCHAR (1000)   NOT NULL,
	Subtitle       VARCHAR (1000),
    DocumentTypeId INT,
	Summary        VARCHAR (6000),
	Content        MEDIUMTEXT,
	PRIMARY KEY (Id),
    FOREIGN KEY (DocumentTypeId) REFERENCES DocumentTypes (Id)
);

/*** Document Information ***/

CREATE TABLE IF NOT EXISTS Publishers (
	Id            INT           NOT NULL AUTO_INCREMENT,
	PublisherName INT           NOT NULL,
	PRIMARY KEY (Id)
);

/*** Maps ***/

CREATE TABLE IF NOT EXISTS MapLayers (
	Id         INT            NOT NULL AUTO_INCREMENT,
	Name       VARCHAR(1000)  NOT NULL,
	LayerDate  BIGINT,
	LayerEraId INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (LayerEraId) REFERENCES Eras (Id)
);

CREATE TABLE IF NOT EXISTS MapTiles (
	Z       FLOAT          NOT NULL,
	Y       FLOAT          NOT NULL,
	X       FLOAT          NOT NULL,
	LayerId INT            NOT NULL,
	Tile  MEDIUMBLOB NOT NULL,
	PRIMARY KEY (Z, Y, X, LayerId),
	FOREIGN KEY (LayerId) REFERENCES MapLayers (Id)
);

/*** Location Types ***/

CREATE TABLE IF NOT EXISTS CelestialObjectTypes (
	Id                INT            NOT NULL AUTO_INCREMENT,
	Name              VARCHAR(1000)  NOT NULL,
    PRIMARY KEY (Id)
);

/*** Locations ***/

CREATE TABLE IF NOT EXISTS Manifolds (
	Id         INT NOT NULL AUTO_INCREMENT,
	Number     INT NOT NULL UNIQUE,
	Name       VARCHAR(1000),
	DocumentId INT,
	PRIMARY KEY (Id),
    FOREIGN KEY (DocumentId) REFERENCES Documents (Id)
);

CREATE TABLE IF NOT EXISTS CelestialSystems (
	Id         INT            NOT NULL AUTO_INCREMENT,
	Name       VARCHAR(1000)  NOT NULL,
	ManifoldId INT            NOT NULL,
	DocumentId INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (ManifoldId) REFERENCES Manifolds (Id),
    FOREIGN KEY (DocumentId) REFERENCES Documents (Id)
);

CREATE TABLE IF NOT EXISTS CelestialObjects (
	Id                  INT            NOT NULL AUTO_INCREMENT,
	Name                VARCHAR(1000)  NOT NULL,
	CelestialSystemId   INT,
	DocumentId         INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (CelestialSystemId) REFERENCES CelestialSystems (Id),
    FOREIGN KEY (DocumentId) REFERENCES Documents (Id)
);

CREATE TABLE IF NOT EXISTS Continents (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	CelestialObjectId  INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	Aquatic            INT,
	DocumentId         INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (CelestialObjectId) REFERENCES CelestialObjects (Id),
    FOREIGN KEY (DocumentId) REFERENCES Documents (Id)
);

CREATE TABLE IF NOT EXISTS Subcontinents (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	ContinentId        INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	DocumentId         INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (ContinentId) REFERENCES Continents (Id),
    FOREIGN KEY (DocumentId) REFERENCES Documents (Id)
);

CREATE TABLE IF NOT EXISTS Regions (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	SubContinentId     INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	DocumentId         INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (SubContinentId) REFERENCES SubContinents (Id),
    FOREIGN KEY (DocumentId) REFERENCES Documents (Id)
);

CREATE TABLE IF NOT EXISTS Subregions (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	RegionId           INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	DocumentId         INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (RegionId) REFERENCES Regions (Id),
    FOREIGN KEY (DocumentId) REFERENCES Documents (Id)
);

CREATE TABLE IF NOT EXISTS Areas (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	SubregionId        INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	DocumentId         INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (SubregionId) REFERENCES Subregions (Id),
    FOREIGN KEY (DocumentId) REFERENCES Documents (Id)
);

CREATE TABLE IF NOT EXISTS LocationTypes (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	PRIMARY KEY (Id)
);

INSERT IGNORE INTO LocationTypes (Id, Name) VALUES
	(1, 'City'),
	(2, 'Town'),
	(3, 'Village'),
	(4, 'Hamlet'),
	(5, 'Fortress'),
	(6, 'River');


CREATE TABLE IF NOT EXISTS Locations (
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

CREATE TABLE IF NOT EXISTS Races (
	Id       INT            NOT NULL AUTO_INCREMENT,
	RaceName VARCHAR(1000)  NOT NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS Titles (
	Id         INT             NOT NULL AUTO_INCREMENT,
	TitleName  VARCHAR(1000)  NOT NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS People (
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
