CREATE TABLE IF NOT EXISTS Roles (
  Id VARCHAR(128) NOT NULL,
  Name VARCHAR(256) NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS Users (
  Id VARCHAR(128) NOT NULL,
  Email VARCHAR(512) DEFAULT NULL UNIQUE,
  EmailConfirmed TINYINT(1) NOT NULL,
  PasswordHash LONGTEXT,
  SecurityStamp LONGTEXT,
  PhoneNumber LONGTEXT,
  PhoneNumberConfirmed TINYINT(1) NOT NULL,
  TwoFactorEnabled TINYINT(1) NOT NULL,
  LockoutEndDateUtc datetime DEFAULT NULL,
  LockoutEnabled TINYINT(1) NOT NULL,
  AccessFailedCount INT(11) NOT NULL,
  UserName VARCHAR(512) NOT NULL UNIQUE,
  PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS UserClaims (
  Id INT(11) NOT NULL AUTO_INCREMENT,
  UserId VARCHAR(128) NOT NULL,
  ClaimType LONGTEXT,
  ClaimValue LONGTEXT,
  PRIMARY KEY (Id),
  UNIQUE KEY Id (Id),
  KEY UserId (UserId),
  CONSTRAINT ApplicationUser_Claims FOREIGN KEY (UserId) REFERENCES users (Id) ON DELETE CASCADE ON UPDATE NO ACTION
);

CREATE TABLE IF NOT EXISTS UserLogins (
  LoginProvider VARCHAR(128) NOT NULL,
  ProviderKey VARCHAR(128) NOT NULL,
  UserId VARCHAR(128) NOT NULL,
  PRIMARY KEY (LoginProvider,ProviderKey,UserId),
  KEY ApplicationUser_Logins (UserId),
  CONSTRAINT ApplicationUser_Logins FOREIGN KEY (UserId) REFERENCES users (Id) ON DELETE CASCADE ON UPDATE NO ACTION
);

CREATE TABLE IF NOT EXISTS UserRoles (
  UserId VARCHAR(128) NOT NULL,
  RoleId VARCHAR(128) NOT NULL,
  PRIMARY KEY (UserId,RoleId),
  KEY IdentityRole_Users (RoleId),
  CONSTRAINT ApplicationUser_Roles FOREIGN KEY (UserId) REFERENCES users (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT IdentityRole_Users FOREIGN KEY (RoleId) REFERENCES roles (Id) ON DELETE CASCADE ON UPDATE NO ACTION
) ;