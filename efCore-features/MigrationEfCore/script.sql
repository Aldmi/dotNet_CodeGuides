Build started...
Build succeeded.
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Persones" (
    "Id" uuid NOT NULL,
    "Name" text NULL,
    "Age" integer NOT NULL,
    "Address" text NULL,
    CONSTRAINT "PK_Persones" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20231205154025_Init', '7.0.14');

COMMIT;


