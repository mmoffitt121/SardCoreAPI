 CREATE TABLE IF NOT EXISTS Worlds (
	Id          INT                  NOT NULL AUTO_INCREMENT,
	OwnerId     VARCHAR (255)        NOT NULL,
    Location    VARCHAR (100)        NOT NULL UNIQUE,
    Name        VARCHAR (255)        NOT NULL UNIQUE,
    Summary     VARCHAR (3000),
    CreatedDate DATETIME,
    PRIMARY KEY (Id),
    FOREIGN KEY (OwnerId) REFERENCES aspnetusers (Id)
 );