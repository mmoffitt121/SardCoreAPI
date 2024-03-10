CREATE TABLE IF NOT EXISTS DataPointTypes (
	Id      INT               NOT NULL AUTO_INCREMENT,
    Name    VARCHAR (1000),
    Summary VARCHAR (3000),
    PRIMARY KEY (Id)
 );
 
SET @preparedStatement = (SELECT IF(
  (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE
      (table_name = 'DataPointTypes')
      AND (table_schema = @Location)
      AND (column_name = 'Settings')
  ) > 0,
  "SELECT 1",
  "ALTER TABLE DataPointTypes ADD Settings TEXT;"
));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
 
 CREATE TABLE IF NOT EXISTS DataPointTypeParameter (
	Id                       INT               NOT NULL AUTO_INCREMENT,
    Name                     VARCHAR (1000),
    Summary                  VARCHAR (3000),
    DataPointTypeId          INT               NOT NULL,
    TypeValue                CHAR (3)          NOT NULL,
    Sequence                 INT               NOT NULL,
    DataPointTypeReferenceId INT,
    PRIMARY KEY (Id),
    FOREIGN KEY (DataPointTypeId) REFERENCES DataPointTypes (Id),
    FOREIGN KEY (DataPointTypeReferenceId) REFERENCES DataPointTypes (Id)
 );
 
 SET @preparedStatement = (SELECT IF(
  (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE
      (table_name = 'DataPointTypeParameter')
      AND (table_schema = @Location)
      AND (column_name = 'Settings')
  ) > 0,
  "SELECT 1",
  "ALTER TABLE DataPointTypeParameter ADD Settings TEXT;"
));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
 
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
    -> �tim� for time
 */
 
 CREATE TABLE IF NOT EXISTS DataPoints (
	Id      INT               NOT NULL AUTO_INCREMENT,
    Name    VARCHAR (1000),
    TypeId  INT               NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (TypeId) REFERENCES DataPointTypes (Id)
 );
 
 CREATE TABLE IF NOT EXISTS DataPointParameterInt (
	DataPointId               INT    NOT NULL,
    DataPointTypeParameterId  INT    NOT NULL,
    Value                     BIGINT,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );
 
  CREATE TABLE IF NOT EXISTS DataPointParameterDouble (
	DataPointId               INT    NOT NULL,
    DataPointTypeParameterId  INT    NOT NULL,
    Value                     DOUBLE,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );
 
  CREATE TABLE IF NOT EXISTS DataPointParameterString (
	DataPointId               INT            NOT NULL,
    DataPointTypeParameterId  INT            NOT NULL,
    Value                     VARCHAR (1000),
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );
 
  CREATE TABLE IF NOT EXISTS DataPointParameterSummary (
	DataPointId               INT  NOT NULL,
    DataPointTypeParameterId  INT  NOT NULL,
    Value                     TEXT,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );
 
 CREATE TABLE IF NOT EXISTS DataPointParameterDocument (
	DataPointId               INT        NOT NULL,
    DataPointTypeParameterId  INT        NOT NULL,
    Value                     LONGTEXT,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );
 
  CREATE TABLE IF NOT EXISTS DataPointParameterDataPoint (
	DataPointId               INT NOT NULL,
    DataPointTypeParameterId  INT NOT NULL,
    Value                     INT,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId),
    FOREIGN KEY (Value) REFERENCES DataPoints (Id)
 );
 
 CREATE TABLE IF NOT EXISTS DataPointParameterBoolean (
	DataPointId               INT NOT NULL,
    DataPointTypeParameterId  INT NOT NULL,
    Value                     BIT,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
 );
 
  CREATE TABLE IF NOT EXISTS DataPointParameterTime (
	DataPointId               INT        NOT NULL,
    DataPointTypeParameterId  INT        NOT NULL,
    Value                     DECIMAL(65, 0),
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
    ZoomProminenceMin    INT,
    ZoomProminenceMax    INT,
    UsesIcon             BIT,
    UsesLabel            BIT,
    IconURL              VARCHAR(3000),
    LabelFontSize        INT,
    LabelFontColor       CHAR(6),
	PRIMARY KEY (Id),
    FOREIGN KEY (ParentTypeId) REFERENCES LocationTypes (Id)
);

CREATE TABLE IF NOT EXISTS Locations (
	Id                INT           NOT NULL AUTO_INCREMENT,
	name              VARCHAR(1000) NOT NULL,
	LocationTypeId    INT,
    LayerId           INT,
	Longitude         FLOAT,
	Latitude          FLOAT,
    ParentId          INT,
    IconPath          VARCHAR(10000),
    ZoomProminenceMin INT,
    ZoomProminenceMax INT,
    IconURL           VARCHAR(3000),
    LabelFontSize     INT,
    LabelFontColor    CHAR(6),
	PRIMARY KEY (Id),
	FOREIGN KEY (LocationTypeId) REFERENCES LocationTypes (Id),
    FOREIGN KEY (ParentId) REFERENCES Locations (Id),
    FOREIGN KEY (LayerId) REFERENCES MapLayers (Id)
);

CREATE TABLE IF NOT EXISTS Regions (
    Id                INT            NOT NULL AUTO_INCREMENT,
    LocationId        INT            NOT NULL,
    Name              VARCHAR(1000)  NOT NULL,
    Shape             LONGTEXT       NOT NULL,
    ShowByDefault     BIT            NOT NULL,
    Color             VARCHAR(100)   NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (LocationId) REFERENCES Locations (Id)
);

CREATE TABLE IF NOT EXISTS DataPointLocations (
	LocationId      INT   NOT NULL,
    DataPointId     INT   NOT NULL,
    PRIMARY KEY (LocationId, DataPointId),
    FOREIGN KEY (LocationId) REFERENCES Locations (Id),
    FOREIGN KEY (DataPointId) REFERENCES DataPoints (Id)
);

CREATE TABLE IF NOT EXISTS Themes (
	Id								INT NOT NULL AUTO_INCREMENT,
	Name							VARCHAR(255),
    IsDefault                       BIT,
	PrimaryColor					VARCHAR(30),
	PrimaryColorSelected			VARCHAR(30),
	InvertedTextColor				VARCHAR(30),
	InvertedTextColorDisabled		VARCHAR(30),
	TextColor						VARCHAR(30),
	TextColorDisabled				VARCHAR(30),
	SecondaryTextColor				VARCHAR(30),
	TertiaryTextColor				VARCHAR(30),
	PrimaryAccentColor				VARCHAR(30),
	PrimaryAccentColorDisabled		VARCHAR(30),
    SecondaryAccentColor            VARCHAR(30),
    SecondaryAccentColorDisabled    VARCHAR(30),
    SecondaryAccentColorSelected    VARCHAR(30),
	BackgroundColor					VARCHAR(30),
	SecondaryBackgroundColor		VARCHAR(30),
	FieldOverlayColor				VARCHAR(30),
	FieldOverlayColorDark			VARCHAR(30),
	DestructiveActionColor			VARCHAR(30),
	PrimaryFont						VARCHAR(30),
	FontWeightbold					VARCHAR(30),
	DataPointValueFontSize			VARCHAR(30),
    PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS Featured (
	Id                INT NOT NULL,
    DataPointId       INT,
    PRIMARY KEY (Id),
    FOREIGN KEY (DataPointId) REFERENCES DataPoints (Id)
);

SET @preparedStatement = (SELECT IF(
  (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE
      (table_name = 'Locations')
      AND (table_schema = @Location)
      AND (column_name = 'IconSize')
  ) > 0,
  "SELECT 1",
  "ALTER TABLE Locations ADD IconSize INT;"
));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;

SET @preparedStatement = (SELECT IF(
  (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE
      (table_name = 'LocationTypes')
      AND (table_schema = @Location)
      AND (column_name = 'IconSize')
  ) > 0,
  "SELECT 1",
  "ALTER TABLE LocationTypes ADD IconSize INT;"
));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;

/** Units **/

CREATE TABLE IF NOT EXISTS Measurables (
	Id                         INT            NOT NULL AUTO_INCREMENT,
    Name                       VARCHAR(1000)  NOT NULL,
    Summary                    VARCHAR(3000),
    UnitType                   INT            NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS Units (
	Id                         INT            NOT NULL AUTO_INCREMENT,
    Name                       VARCHAR(1000)  NOT NULL,
    Summary                    VARCHAR(3000),
	AmountPerParent            DOUBLE,
    MeasurableId               INT            NOT NULL,
    Symbol                     VARCHAR(100)   NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (MeasurableId) REFERENCES Measurables (Id)
);

CREATE TABLE IF NOT EXISTS DataPointParameterUnit (
    DataPointId               INT  NOT NULL,
    DataPointTypeParameterId  INT  NOT NULL,
    Value                     DOUBLE,
    PRIMARY KEY (DataPointId, DataPointTypeParameterId)
);

CREATE TABLE IF NOT EXISTS PersistentZoomLevels (
	Zoom        INT            NOT NULL,
	MapLayerId  INT            NOT NULL,
	PRIMARY KEY (Zoom, MapLayerId),
    FOREIGN KEY (MapLayerId) REFERENCES MapLayers (Id)
);

/** Calendars **/

CREATE TABLE IF NOT EXISTS Calendars (
	Id                 INT            NOT NULL AUTO_INCREMENT,
    CalendarObject     MEDIUMTEXT     NOT NULL,
    PRIMARY KEY (Id)
);

/** Settings **/

CREATE TABLE IF NOT EXISTS SettingJSON (
	Id                  VARCHAR(500) NOT NULL,
    Setting             MEDIUMTEXT,
    PRIMARY KEY (Id)
);

/** Security **/

CREATE TABLE IF NOT EXISTS Roles (
	Id       VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS UserRoles (
	UserId VARCHAR(255) CHARACTER SET utf8mb4 NOT NULL,
    RoleId VARCHAR(255) NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (RoleId) REFERENCES Roles (Id)
);

CREATE TABLE IF NOT EXISTS RolePermissions (
	RoleId VARCHAR(255) NOT NULL,
    Permission VARCHAR(512) NOT NULL,
    PRIMARY KEY (Permission, RoleId),
    FOREIGN KEY (RoleId) REFERENCES Roles (Id)
);
