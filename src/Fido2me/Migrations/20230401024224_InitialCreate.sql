CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `ApiResources` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Enabled` tinyint(1) NOT NULL,
    `Name` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `DisplayName` varchar(200) CHARACTER SET utf8mb4 NULL,
    `Description` varchar(1000) CHARACTER SET utf8mb4 NULL,
    `AllowedAccessTokenSigningAlgorithms` varchar(100) CHARACTER SET utf8mb4 NULL,
    `ShowInDiscoveryDocument` tinyint(1) NOT NULL,
    `RequireResourceIndicator` tinyint(1) NOT NULL,
    `Created` datetime(6) NOT NULL,
    `Updated` datetime(6) NULL,
    `LastAccessed` datetime(6) NULL,
    `NotEditable` tinyint(1) NOT NULL,
    CONSTRAINT `PK_ApiResources` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ApiScopes` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Enabled` tinyint(1) NOT NULL,
    `Name` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `DisplayName` varchar(200) CHARACTER SET utf8mb4 NULL,
    `Description` varchar(1000) CHARACTER SET utf8mb4 NULL,
    `Required` tinyint(1) NOT NULL,
    `Emphasize` tinyint(1) NOT NULL,
    `ShowInDiscoveryDocument` tinyint(1) NOT NULL,
    `Created` datetime(6) NOT NULL,
    `Updated` datetime(6) NOT NULL,
    `LastAccessed` datetime(6) NOT NULL,
    `NonEditable` tinyint(1) NOT NULL,
    CONSTRAINT `PK_ApiScopes` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Clients` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Enabled` tinyint(1) NOT NULL,
    `ClientId` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `ProtocolType` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `RequireClientSecret` tinyint(1) NOT NULL,
    `ClientName` varchar(2000) CHARACTER SET utf8mb4 NULL,
    `Description` varchar(1000) CHARACTER SET utf8mb4 NULL,
    `ClientUri` varchar(2000) CHARACTER SET utf8mb4 NULL,
    `LogoUri` varchar(2000) CHARACTER SET utf8mb4 NULL,
    `RequireConsent` tinyint(1) NOT NULL,
    `AllowRememberConsent` tinyint(1) NOT NULL,
    `AlwaysIncludeUserClaimsInIdToken` tinyint(1) NOT NULL,
    `RequirePkce` tinyint(1) NOT NULL,
    `AllowPlainTextPkce` tinyint(1) NOT NULL,
    `RequireRequestObject` tinyint(1) NOT NULL,
    `AllowAccessTokensViaBrowser` tinyint(1) NOT NULL,
    `FrontChannelLogoutUri` varchar(2000) CHARACTER SET utf8mb4 NULL,
    `FrontChannelLogoutSessionRequired` tinyint(1) NOT NULL,
    `BackChannelLogoutUri` varchar(2000) CHARACTER SET utf8mb4 NULL,
    `BackChannelLogoutSessionRequired` tinyint(1) NOT NULL,
    `AllowOfflineAccess` tinyint(1) NOT NULL,
    `IdentityTokenLifetime` int NOT NULL,
    `AllowedIdentityTokenSigningAlgorithms` varchar(100) CHARACTER SET utf8mb4 NULL,
    `AccessTokenLifetime` int NOT NULL,
    `AuthorizationCodeLifetime` int NOT NULL,
    `ConsentLifetime` int NULL,
    `AbsoluteRefreshTokenLifetime` int NOT NULL,
    `SlidingRefreshTokenLifetime` int NOT NULL,
    `RefreshTokenUsage` int NOT NULL,
    `UpdateAccessTokenClaimsOnRefresh` tinyint(1) NOT NULL,
    `RefreshTokenExpiration` int NOT NULL,
    `AccessTokenType` int NOT NULL,
    `EnableLocalLogin` tinyint(1) NOT NULL,
    `IncludeJwtId` tinyint(1) NOT NULL,
    `AlwaysSendClientClaims` tinyint(1) NOT NULL,
    `ClientClaimsPrefix` varchar(200) CHARACTER SET utf8mb4 NULL,
    `PairWiseSubjectSalt` varchar(200) CHARACTER SET utf8mb4 NULL,
    `UserSsoLifetime` int NULL,
    `UserCodeType` varchar(100) CHARACTER SET utf8mb4 NULL,
    `DeviceCodeLifetime` int NOT NULL,
    `CibaLifetime` int NULL,
    `PollingInterval` int NULL,
    `CoordinateLifetimeWithUserSession` tinyint(1) NULL,
    `Created` datetime(6) NOT NULL,
    `Updated` datetime(6) NOT NULL,
    `LastAccessed` datetime(6) NOT NULL,
    `NonEditable` tinyint(1) NOT NULL,
    CONSTRAINT `PK_Clients` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `DataProtectionKeys` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FriendlyName` longtext CHARACTER SET utf8mb4 NULL,
    `Xml` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_DataProtectionKeys` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `DeviceCodes` (
    `UserCode` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `DeviceCode` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `SubjectId` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `SessionId` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `ClientId` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(200) CHARACTER SET utf8mb4 NULL,
    `CreationTime` datetime(6) NOT NULL,
    `Expiration` datetime(6) NOT NULL,
    `Data` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_DeviceCodes` PRIMARY KEY (`UserCode`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `IdentityProviders` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Scheme` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `DisplayName` varchar(200) CHARACTER SET utf8mb4 NULL,
    `Enabled` tinyint(1) NOT NULL,
    `Type` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `Properties` longtext CHARACTER SET utf8mb4 NULL,
    `Created` datetime(6) NOT NULL,
    `Updated` datetime(6) NULL,
    `LastAccessed` datetime(6) NULL,
    `NonEditable` datetime(6) NOT NULL,
    CONSTRAINT `PK_IdentityProviders` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `IdentityResources` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Enabled` tinyint(1) NOT NULL,
    `Name` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `DisplayName` varchar(200) CHARACTER SET utf8mb4 NULL,
    `Description` varchar(2000) CHARACTER SET utf8mb4 NULL,
    `Required` tinyint(1) NOT NULL,
    `Emphasize` tinyint(1) NOT NULL,
    `ShowInDiscoveryDocument` tinyint(1) NOT NULL,
    `Created` datetime(6) NOT NULL,
    `Updated` datetime(6) NOT NULL,
    `NonEditable` tinyint(1) NOT NULL,
    CONSTRAINT `PK_IdentityResources` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Keys` (
    `Id` varchar(450) CHARACTER SET utf8mb4 NOT NULL,
    `Version` int NOT NULL,
    `Created` datetime(6) NOT NULL,
    `Use` varchar(450) CHARACTER SET utf8mb4 NULL,
    `Algorithm` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `IsX509Certificate` tinyint(1) NOT NULL,
    `DataProtected` tinyint(1) NOT NULL,
    `Data` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Keys` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `PersistedGrants` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Key` varchar(200) CHARACTER SET utf8mb4 NULL,
    `Type` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `SubjectId` varchar(200) CHARACTER SET utf8mb4 NULL,
    `SessionId` varchar(100) CHARACTER SET utf8mb4 NULL,
    `ClientId` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(200) CHARACTER SET utf8mb4 NULL,
    `CreationTime` datetime(6) NOT NULL,
    `Expiration` datetime(6) NULL,
    `ConsumedTime` datetime(6) NULL,
    `Data` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_PersistedGrants` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ServerSideSessions` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Key` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Scheme` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `SubjectId` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `SessionId` varchar(100) CHARACTER SET utf8mb4 NULL,
    `DisplayName` varchar(100) CHARACTER SET utf8mb4 NULL,
    `Created` datetime(6) NOT NULL,
    `Renewed` datetime(6) NOT NULL,
    `Expires` datetime(6) NOT NULL,
    `Data` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_ServerSideSessions` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ApiResourceClaims` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ApiResourceId` bigint NOT NULL,
    `Type` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_ApiResourceClaims` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ApiResourceProperties` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ApiResourceId` bigint NOT NULL,
    `Key` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value` varchar(2000) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_ApiResourceProperties` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ApiResourceScopes` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Scope` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `ApiResourceId` bigint NOT NULL,
    CONSTRAINT `PK_ApiResourceScopes` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ApiResourceSecrets` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ApiResourceId` bigint NOT NULL,
    `Description` varchar(2000) CHARACTER SET utf8mb4 NULL,
    `Value` varchar(4000) CHARACTER SET utf8mb4 NOT NULL,
    `Expiration` datetime(6) NULL,
    `Type` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Created` datetime(6) NOT NULL,
    CONSTRAINT `PK_ApiResourceSecrets` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ApiScopeClaims` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ApiScopeId` bigint NOT NULL,
    `Type` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_ApiScopeClaims` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ApiScopeProperties` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ApiScopeId` bigint NOT NULL,
    `Key` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value` varchar(2000) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_ApiScopeProperties` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ClientClaims` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Type` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `ClientId` bigint NOT NULL,
    CONSTRAINT `PK_ClientClaims` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ClientCorsOrigins` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Origin` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ClientId` bigint NOT NULL,
    CONSTRAINT `PK_ClientCorsOrigins` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ClientGrantTypes` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `GrantType` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `ClientId` bigint NOT NULL,
    CONSTRAINT `PK_ClientGrantTypes` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ClientIdPRestrictions` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Provider` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `ClientId` bigint NOT NULL,
    CONSTRAINT `PK_ClientIdPRestrictions` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ClientPostLogoutRedirectUris` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `PostLogoutRedirectUri` varchar(400) CHARACTER SET utf8mb4 NOT NULL,
    `ClientId` bigint NOT NULL,
    CONSTRAINT `PK_ClientPostLogoutRedirectUris` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ClientProperties` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ClientId` bigint NOT NULL,
    `Key` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value` varchar(2000) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_ClientProperties` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ClientRedirectUris` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `RedirectUri` varchar(400) CHARACTER SET utf8mb4 NOT NULL,
    `ClientId` bigint NOT NULL,
    CONSTRAINT `PK_ClientRedirectUris` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ClientScopes` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Scope` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `ClientId` bigint NOT NULL,
    CONSTRAINT `PK_ClientScopes` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ClientSecrets` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ClientId` bigint NOT NULL,
    `Description` varchar(2000) CHARACTER SET utf8mb4 NULL,
    `Value` varchar(4000) CHARACTER SET utf8mb4 NOT NULL,
    `Expiration` datetime(6) NULL,
    `Type` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Created` datetime(6) NOT NULL,
    CONSTRAINT `PK_ClientSecrets` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `IdentityResourceClaims` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `IdentityResourceId` bigint NOT NULL,
    `Type` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityResourceClaims` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `IdentityResourceProperties` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `IdentityResourceId` bigint NOT NULL,
    `Key` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value` varchar(2000) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityResourceProperties` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE UNIQUE INDEX `IX_ApiResourceClaims_ApiResourceId_Type` ON `ApiResourceClaims` (`ApiResourceId`, `Type`);

CREATE UNIQUE INDEX `IX_ApiResourceProperties_ApiResourceId_Key` ON `ApiResourceProperties` (`ApiResourceId`, `Key`);

CREATE UNIQUE INDEX `IX_ApiResources_Name` ON `ApiResources` (`Name`);

CREATE UNIQUE INDEX `IX_ApiResourceScopes_ApiResourceId_Scope` ON `ApiResourceScopes` (`ApiResourceId`, `Scope`);

CREATE INDEX `IX_ApiResourceSecrets_ApiResourceId` ON `ApiResourceSecrets` (`ApiResourceId`);

CREATE UNIQUE INDEX `IX_ApiScopeClaims_ApiScopeId_Type` ON `ApiScopeClaims` (`ApiScopeId`, `Type`);

CREATE UNIQUE INDEX `IX_ApiScopeProperties_ApiScopeId_Key` ON `ApiScopeProperties` (`ApiScopeId`, `Key`);

CREATE UNIQUE INDEX `IX_ApiScopes_Name` ON `ApiScopes` (`Name`);

CREATE UNIQUE INDEX `IX_ClientClaims_ClientId_Type_Value` ON `ClientClaims` (`ClientId`, `Type`, `Value`);

CREATE UNIQUE INDEX `IX_ClientCorsOrigins_ClientId_Origin` ON `ClientCorsOrigins` (`ClientId`, `Origin`);

CREATE UNIQUE INDEX `IX_ClientGrantTypes_ClientId_GrantType` ON `ClientGrantTypes` (`ClientId`, `GrantType`);

CREATE UNIQUE INDEX `IX_ClientIdPRestrictions_ClientId_Provider` ON `ClientIdPRestrictions` (`ClientId`, `Provider`);

CREATE UNIQUE INDEX `IX_ClientPostLogoutRedirectUris_ClientId_PostLogoutRedirectUri` ON `ClientPostLogoutRedirectUris` (`ClientId`, `PostLogoutRedirectUri`);

CREATE UNIQUE INDEX `IX_ClientProperties_ClientId_Key` ON `ClientProperties` (`ClientId`, `Key`);

CREATE UNIQUE INDEX `IX_ClientRedirectUris_ClientId_RedirectUri` ON `ClientRedirectUris` (`ClientId`, `RedirectUri`);

CREATE UNIQUE INDEX `IX_Clients_ClientId` ON `Clients` (`ClientId`);

CREATE UNIQUE INDEX `IX_ClientScopes_ClientId_Scope` ON `ClientScopes` (`ClientId`, `Scope`);

CREATE INDEX `IX_ClientSecrets_ClientId` ON `ClientSecrets` (`ClientId`);

CREATE UNIQUE INDEX `IX_DeviceCodes_DeviceCode` ON `DeviceCodes` (`DeviceCode`);

CREATE INDEX `IX_DeviceCodes_Expiration` ON `DeviceCodes` (`Expiration`);

CREATE UNIQUE INDEX `IX_IdentityProviders_Scheme` ON `IdentityProviders` (`Scheme`);

CREATE UNIQUE INDEX `IX_IdentityResourceClaims_IdentityResourceId_Type` ON `IdentityResourceClaims` (`IdentityResourceId`, `Type`);

CREATE INDEX `IX_IdentityResourceProperties_IdentityResourceId` ON `IdentityResourceProperties` (`IdentityResourceId`);

CREATE UNIQUE INDEX `IX_IdentityResources_Name` ON `IdentityResources` (`Name`);

CREATE INDEX `IX_Keys_Use` ON `Keys` (`Use`);

CREATE INDEX `IX_PersistedGrants_ConsumedTime` ON `PersistedGrants` (`ConsumedTime`);

CREATE INDEX `IX_PersistedGrants_Expiration` ON `PersistedGrants` (`Expiration`);

CREATE INDEX `IX_PersistedGrants_Key` ON `PersistedGrants` (`Key`);

CREATE INDEX `IX_PersistedGrants_SubjectId_ClientId_Type` ON `PersistedGrants` (`SubjectId`, `ClientId`, `Type`);

CREATE INDEX `IX_PersistedGrants_SubjectId_SessionId_Type` ON `PersistedGrants` (`SubjectId`, `SessionId`, `Type`);

CREATE INDEX `IX_ServerSideSessions_DisplayName` ON `ServerSideSessions` (`DisplayName`);

CREATE INDEX `IX_ServerSideSessions_Expires` ON `ServerSideSessions` (`Expires`);

CREATE UNIQUE INDEX `IX_ServerSideSessions_Key` ON `ServerSideSessions` (`Key`);

CREATE INDEX `IX_ServerSideSessions_SessionId` ON `ServerSideSessions` (`SessionId`);

CREATE INDEX `IX_ServerSideSessions_SubjectId` ON `ServerSideSessions` (`SubjectId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230401024224_InitialCreate', '7.0.4');

COMMIT;

