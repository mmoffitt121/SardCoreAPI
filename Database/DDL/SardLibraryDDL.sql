 CREATE TABLE IF NOT EXISTS DataPointTypes (
	Id      INT               NOT NULL AUTO_INCREMENT,
    Name    VARCHAR (1000),
    Summary VARCHAR (3000),
    PRIMARY KEY (Id)
 );
 
 CREATE TABLE IF NOT EXISTS DataPointTypeParameter (
	Id               INT               NOT NULL AUTO_INCREMENT,
    Name             VARCHAR (1000),
    Summary          VARCHAR (3000),
    DataPointTypeId  INT               NOT NULL,
    TypeValue        CHAR (3)          NOT NULL,
    Sequence         INT               NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (DataPointTypeId) REFERENCES DataPointTypes (Id)
 );
 
  /*
 The TypeValue field contains a character value representing what data type this is.
    -> ‘int’ for integer
    -> ‘dub’ for double
    -> ‘str’ for string
    -> ‘sum’ for summary
    -> ‘doc’ for document
    -> ‘img’ for image
    -> ‘dat’ for data point
    -> ‘bit’ for boolean
 */
 
 CREATE TABLE IF NOT EXISTS DataPoints (
	Id      INT               NOT NULL AUTO_INCREMENT,
    Name    VARCHAR (1000),
    TypeId  INT               NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (TypeId) REFERENCES DataPointTypes (Id)
 );
 
 CREATE TABLE IF NOT EXISTS DataPointParameterInt (
	DataPointId               INT NOT NULL,
    DataPointTypeParameterId  INT NOT NULL,
    Value                     INT NOT NULL,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );
 
  CREATE TABLE IF NOT EXISTS DataPointParameterDouble (
	DataPointId               INT    NOT NULL,
    DataPointTypeParameterId  INT    NOT NULL,
    Value                     DOUBLE NOT NULL,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );
 
  CREATE TABLE IF NOT EXISTS DataPointParameterString (
	DataPointId               INT            NOT NULL,
    DataPointTypeParameterId  INT            NOT NULL,
    Value                     VARCHAR (1000) NOT NULL,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );
 
  CREATE TABLE IF NOT EXISTS DataPointParameterSummary (
	DataPointId               INT  NOT NULL,
    DataPointTypeParameterId  INT  NOT NULL,
    Value                     TEXT NOT NULL,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );
 
 CREATE TABLE IF NOT EXISTS DataPointParameterDocument (
	DataPointId               INT        NOT NULL,
    DataPointTypeParameterId  INT        NOT NULL,
    Value                     LONGTEXT   NOT NULL,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );
 
  CREATE TABLE IF NOT EXISTS DataPointParameterDataPoint (
	DataPointId               INT NOT NULL,
    DataPointTypeParameterId  INT NOT NULL,
    Value                     INT NOT NULL,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId),
    FOREIGN KEY (Value) REFERENCES DataPoints (Id)
 );
 
 CREATE TABLE IF NOT EXISTS DataPointParameterBoolean (
	DataPointId               INT NOT NULL,
    DataPointTypeParameterId  INT NOT NULL,
    Value                     BIT NOT NULL,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );

/*** Maps ***/

CREATE TABLE IF NOT EXISTS Map (
	Id                         INT            NOT NULL AUTO_INCREMENT,
    Name                       VARCHAR(1000)  NOT NULL,
    AreaZoomProminence         INT,
    SubregionZoomProminence    INT,
    RegionZoomProminence       INT,
    SubcontinentZoomProminence INT,
    ContinentZoomProminence    INT,
    IconPath                   VARCHAR(10000),
    PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS MapLayers (
	Id         INT            NOT NULL AUTO_INCREMENT,
	Name       VARCHAR(1000)  NOT NULL,
    MapId      INT            NOT NULL,
	PRIMARY KEY (Id),
    FOREIGN KEY (MapId) REFERENCES Map (Id)
);

CREATE TABLE IF NOT EXISTS MapTiles (
	Z        FLOAT          NOT NULL,
	Y        FLOAT          NOT NULL,
	X        FLOAT          NOT NULL,
	LayerId  INT            NOT NULL,
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
	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS CelestialSystems (
	Id         INT            NOT NULL AUTO_INCREMENT,
	Name       VARCHAR(1000)  NOT NULL,
	ManifoldId INT            NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (ManifoldId) REFERENCES Manifolds (Id)
);

CREATE TABLE IF NOT EXISTS CelestialObjects (
	Id                  INT            NOT NULL AUTO_INCREMENT,
	Name                VARCHAR(1000)  NOT NULL,
	CelestialSystemId   INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (CelestialSystemId) REFERENCES CelestialSystems (Id)
);

CREATE TABLE IF NOT EXISTS Continents (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	CelestialObjectId  INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	Oceanic            INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (CelestialObjectId) REFERENCES CelestialObjects (Id)
);

CREATE TABLE IF NOT EXISTS Subcontinents (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	ContinentId        INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (ContinentId) REFERENCES Continents (Id)
);

CREATE TABLE IF NOT EXISTS Regions (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	SubContinentId     INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (SubContinentId) REFERENCES SubContinents (Id)
);

CREATE TABLE IF NOT EXISTS Subregions (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	RegionId           INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (RegionId) REFERENCES Regions (Id)
);

CREATE TABLE IF NOT EXISTS Areas (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
	SubregionId        INT,
	Longitude          FLOAT,
	Latitude           FLOAT,
	PRIMARY KEY (Id),
	FOREIGN KEY (SubregionId) REFERENCES Subregions (Id)
);

CREATE TABLE IF NOT EXISTS LocationTypes (
	Id                 INT           NOT NULL AUTO_INCREMENT,
	Name               VARCHAR(1000) NOT NULL,
    IconPath           VARCHAR(10000),
    ZoomProminence     INT,
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
    IconPath       VARCHAR(10000),
    ZoomProminence INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (AreaId) REFERENCES Areas (Id),
	FOREIGN KEY (LocationTypeId) REFERENCES LocationTypes (Id)
);

