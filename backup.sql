PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS "TipoPersona" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_TipoPersona" PRIMARY KEY AUTOINCREMENT,
    "Nombre" TEXT NULL
);
INSERT INTO TipoPersona VALUES(0,'Por Actualizar');
INSERT INTO TipoPersona VALUES(1,'Persona Fisica');
INSERT INTO TipoPersona VALUES(2,'Persona Moral');
INSERT INTO TipoPersona VALUES(7,'Testing');
INSERT INTO TipoPersona VALUES(8,'Testing');
INSERT INTO TipoPersona VALUES(9,'Testeo2');
INSERT INTO TipoPersona VALUES(10,'NuevaPersona');
INSERT INTO TipoPersona VALUES(11,'sadasd');
INSERT INTO TipoPersona VALUES(12,'dsd');
INSERT INTO TipoPersona VALUES(19,'RFC');
INSERT INTO TipoPersona VALUES(22,'Persona Asalariada');
CREATE TABLE IF NOT EXISTS "User" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_User" PRIMARY KEY AUTOINCREMENT,
    "Ciudad" TEXT NULL,
    "Email" TEXT NULL,
    "Estado" TEXT NULL,
    "Name" TEXT NULL,
    "TipoPersonaId" INTEGER NOT NULL,
    CONSTRAINT "FK_User_TipoPersona_TipoPersonaId" FOREIGN KEY ("TipoPersonaId") REFERENCES "TipoPersona" ("Id") ON DELETE CASCADE
);
INSERT INTO Role VALUES(0,'Customer');
INSERT INTO Role VALUES(1,'Admin');

INSERT INTO User VALUES(16,'Merida','francesca.rojas.10@gmail.com','Yucatan','Jesus Isai Vazquez Valdez',19,0);
INSERT INTO User VALUES(42,'assdfasf','swadfadf@assdf','asdsfasdf','Testing',7,0);
INSERT INTO User VALUES(46,'string','string@gmail','string','string',2,0);
INSERT INTO User VALUES(47,'string','string@gmaill.com','string','Teste',2,0);
INSERT INTO User VALUES(48,'string','string@gmail.com','string','stringdasdasdasd',1,0);
INSERT INTO User VALUES(49,'string','string@gmail.com','dedede','string',1,0);
INSERT INTO User VALUES(50,'string','string@gmail','string','string',0,0);
INSERT INTO User VALUES(59,'dfsdfjk','joajdsai@gmail.com','adfjasdlkf','evevev',0,0);
INSERT INTO User VALUES(70,'adcasca','wcwdcsc@fw','asasd','vwvwd',0,0);
INSERT INTO User VALUES(72,'string','string','string','string',2,0);
INSERT INTO User VALUES(73,'string','string','string','string',2,0);
INSERT INTO User VALUES(74,'testeo','testeo@gmai.com','testeo','Testeo',0,0);
INSERT INTO User VALUES(75,'efwfw','wefwf@fw','wefw','fqefq',22,0);
INSERT INTO User VALUES(76,'string','string','string','string',9,0);
INSERT INTO User VALUES(78,'afaef','teste@aef','asdasd','testee',0,0);
INSERT INTO User VALUES(79,'asda','asdasd@asd','rege','asdad',0,0);
INSERT INTO User VALUES(81,'erve','evrvecd@fwdfa','deaf','efwfeve',0,0);
INSERT INTO User VALUES(82,'eteaf','testpls@gmaol','dfa','testpls',0,0);
INSERT INTO User VALUES(83,'efvefv','rgbvrgb@efv','fevev','grbrgb',0,0);
INSERT INTO User VALUES(84,'efvevf','efvefvefv@efwv','fevefv','efvefv',0,0);
INSERT INTO User VALUES(85,'wcwcd','wccwcw@fv','cwdcw','cdscd',0,0);
INSERT INTO User VALUES(86,'scac','cdcdc@scdc','adcadc','cdcdcd',0,0);
INSERT INTO User VALUES(87,'adfadf','wdfwfsa@adf','asdasd','wefwefw',0,0);
INSERT INTO User VALUES(88,'adfasfd','qwdqwd@asd','asdads','dqdqw',0,0);
INSERT INTO User VALUES(89,'string','string','string','string',2,0);
INSERT INTO User VALUES(91,'wefwefw','wefwefwf@frwvw','wefwef','fewfef',0,0);
INSERT INTO User VALUES(92,'adad','hola@gmail.com','rgegr','holaf',0,0);
INSERT INTO User VALUES(93,'hola','hola@gmail.com','hola','holaw',7,0);
INSERT INTO User VALUES(94,'merida','testing@gmail.com','Yucatán','Testeo',8,0);
INSERT INTO User VALUES(95,'sdsds','tes@ts','frgg','testds',2,0);
INSERT INTO User VALUES(96,'swderfsdaf','asdasd@daf','wdwd','asdasd',19,0);
INSERT INTO User VALUES(97,'wawa','Isai@gmail.com','wawa','HolaI',10,0);
INSERT INTO User VALUES(98,'asdfadf','adasd@asd','efrf','dasdasdasd',2,0);
INSERT INTO User VALUES(99,'septiembre','30@septiembre','sepala','hOLaa',1,0);
INSERT INTO User VALUES(100,'Octubre','octubre@gmail.com','Octubre','TestOct',11,0);

DELETE FROM sqlite_sequence;
INSERT INTO sqlite_sequence VALUES('TipoPersona',22);
INSERT INTO sqlite_sequence VALUES('User',100);
COMMIT;
