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
    -> �int� for integer
    -> �dub� for double
    -> �str� for string
    -> �sum� for summary
    -> �doc� for document
    -> �img� for image
    -> �dat� for data point
    -> �bit� for boolean
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
    Summary                    VARCHAR(3000),
    Loops                      BIT,
    DefaultZ                   FLOAT,
    DefaultX                   FLOAT,
    DefaultY                   FLOAT,
    MinZoom                    INT,
    MaxZoom                    INT,
    IsDefault                  BIT,
    IconURL                    VARCHAR(3000),
    PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS MapLayers (
	Id          INT            NOT NULL AUTO_INCREMENT,
	Name        VARCHAR(1000)  NOT NULL,
    Summary     VARCHAR(3000),
    MapId       INT            NOT NULL,
    IsBaseLayer BIT,
    IsIconLayer BIT,
    IconURL     VARCHAR(3000),
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

/*** Locations ***/

CREATE TABLE IF NOT EXISTS LocationTypes (
	Id                   INT           NOT NULL AUTO_INCREMENT,
	Name                 VARCHAR(1000) NOT NULL,
    Summary              VARCHAR(3000),
    ParentTypeId         INT,
    AnyTypeParent        BIT,
    IconPath             VARCHAR(3000),
    ZoomProminence       INT,
	PRIMARY KEY (Id),
    FOREIGN KEY (ParentTypeId) REFERENCES LocationTypes (Id)
);

CREATE TABLE IF NOT EXISTS Locations (
	Id             INT           NOT NULL AUTO_INCREMENT,
	LocationName   VARCHAR(1000) NOT NULL,
	AreaId         INT,
	LocationTypeId INT,
	Longitude      FLOAT,
	Latitude       FLOAT,
    ParentId       INT,
    IconPath       VARCHAR(10000),
    ZoomProminence INT,
	PRIMARY KEY (Id),
	FOREIGN KEY (AreaId) REFERENCES Areas (Id),
	FOREIGN KEY (LocationTypeId) REFERENCES LocationTypes (Id),
    FOREIGN KEY (ParentId) REFERENCES Locations (Id)
);

