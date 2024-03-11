create database AdsProject;
go

use AdsProject;

create table Category(
Id int not null identity(1,1),
[Name] nvarchar(50) not null,
primary key(Id)
);
go

create table Ad(
Id int not null identity(1,1),
IdCategory int not null,
Title nvarchar(200) not null, 
[Description] nvarchar(max) not null,
Price money not null,
RegistrationDate date not null,
[State] nvarchar(20) not null,
primary key(Id),
foreign key(IdCategory) references Category(Id)
);
go

create table AdImage(
Id int not null identity(1,1),
IdAd int not null,
[Path] nvarchar(max) not null,
primary key(Id),
foreign key(IdAd) references Ad(Id)
);
go

--***actualización de la base de datos**--
create table [Role](
Id int not null identity(1,1),
[Name] nvarchar(30) not null,
primary key(Id)
);
go

create table [User](
Id int not null identity(1,1),
IdRole int not null,
[Name] nvarchar(30) not null,
LastName nvarchar(30) not null,
[Login] nvarchar(25) not null,
[Password] nvarchar(100) not null,
[Status] tinyint not null,
RegistrationDate datetime not null,
primary key(Id),
foreign key(IdRole) references [Role](Id)
);
go

--*** inserción de datos iniciales ***--
insert into [Role]([Name]) values('Administrador');

--****password = admin2024****---
insert into [User](IdRole, [Name], LastName, [Login], [Password], [Status], RegistrationDate) values
(1, 'Julio César', 'Tula', 'jc-tula', '8d4db54daf7d67db5f3c96e43f61c609', 1, SYSDATETIME());
