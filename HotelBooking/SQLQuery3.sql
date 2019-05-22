
drop database HotelBooking

CREATE DATABASE HotelBooking
 ON  PRIMARY 
 (NAME = N'HotelBoking_mdf', FILENAME = N'C:\Users\User\Desktop\������\4 ���\������\HotelBooking\HotelBooking.mdf' ,
 SIZE = 5MB , MAXSIZE = 10MB, FILEGROWTH = 1MB)
 LOG ON 
(NAME = N'HotelBoking_log', FILENAME = N'C:\Users\User\Desktop\������\4 ���\������\HotelBooking\HotelBooking_log.ldf' ,
 SIZE = 5MB , MAXSIZE = UNLIMITED, FILEGROWTH = 1MB);

use HotelBooking;

create table Client (
Client_code		int identity constraint PK_Client_code primary key,
Full_name		nvarchar(35) not null,
Birthdate		date not null,
Phone			nvarchar(20) not null,
Passport_number varchar(9) not null,
Email			varchar(30) not null
);

create table TypeOfFood (
Type_code		char(2) not null constraint PK_Type_code primary key,
[Type_name]		nvarchar(15) not null,
Summa			decimal(10,2) not null

);

insert into TypeOfFood (Type_code, [Type_name], Summa)
values  ('RO','��� �������', 0),
		('BB','�������', 15),
		('HB','������� � ����', 35),
		('AI','��� ��������', 60);

alter table TypeOfFood drop PK_Type_code;


create table Autorization (
Login			nvarchar(30) constraint PK_Login primary key,
Userr			nvarchar(30),
Password		nvarchar(30) not null
);


create table Pay (
Payment_number int constraint PK_Payment_number primary key,
DateOfPay		date not null,
Payer			int not null,
Card_number		varchar(15) not null,
Summ			decimal(10,2) not null
);

alter table Pay add foreign key (Payer) references Client(Client_code);

create table Workers (
Worker_code		int constraint PK_Worker_code primary key,
Full_name		nvarchar(35) not null,
Birthdate		date not null,
Position		nvarchar(40) not null,
Salary			decimal(8, 2) not null
);

insert into Workers (Worker_code, Full_name, Birthdate, Position, Salary)
values	( 7142476, '�������� ����� ���������', '19.09.1998', '������������� �����', 900),
		( 8154956, '����� ������ ������������', '15.04.1994', '������ ���� ��������������', 700);

create table CategoryOfRoom (
Category_code	varchar(5) constraint PK_Category_code primary key,
Category_name	nvarchar(50) not null,
NumberOfSeats	char(1) not null,
NumbersOfRoom   char(1) not null,
Servicess		nvarchar(max) not null,
Cost			decimal not null
);
--ALTER TABLE CategoryOfRoom ALTER COLUMN Cost decimal;

--alter table CategoryOfRoom drop PK_Category_code;

insert into CategoryOfRoom (Category_code, Category_name, NumberOfSeats, NumbersOfRoom, Servicess, Cost)
values  ('SGL','����������� �����','1','1', '�������, �����������, ���������� Wi-Fi, ����, �����, ����, ������� ��� ������, ��������� � ������� �������, ���,
			���������� ��������� ��������������, ������, ������ �������, �����������, �����', 98)

insert into CategoryOfRoom (Category_code, Category_name, NumberOfSeats, NumbersOfRoom, Servicess, Cost)
values  ('DBL','����������� ����� � ����� ������� ��������','2','1', '�������, �����������, ���������� Wi-Fi, ����, ����-���, �����, ����, ������� ��� ������, ��������� � ������� �������, ���,
			���������� ��������� ��������������, ������, ������ �������, �����������, �����', 112)
insert into CategoryOfRoom (Category_code, Category_name, NumberOfSeats, NumbersOfRoom, Servicess, Cost)
values('DBLL','����������� ����� ���� � ����� ������� ��������','2','1', '�������, �������, �����������, ���������� Wi-Fi, ����, ����-���, �����, ����, ������� ��� ������, ��������� � ������� �������, ���,
			���������� ��������� ��������������, ������, �����������, �����', 160)
insert into CategoryOfRoom (Category_code, Category_name, NumberOfSeats, NumbersOfRoom, Servicess, Cost)
values('TWIN','����������� ����� � ����� ���������','2','1', '�������, �����������, ���������� Wi-Fi, ����, ����-���, �����, ����, ������� ��� ������, ��������� � ������� �������, ���,
			���������� ��������� ��������������, ������, ������ �������, �����������, �����', 112)
insert into CategoryOfRoom (Category_code, Category_name, NumberOfSeats, NumbersOfRoom, Servicess, Cost)
values('TRPL','����������� �����','3','1', '�������, �����������, ���������� Wi-Fi, ����, ����-���, �����, ����, ������� ��� ������, ��������� � ������� �������, ���,
			���������� ��������� ��������������, ������, ������ �������, �����������, �����', 240)
insert into CategoryOfRoom (Category_code, Category_name, NumberOfSeats, NumbersOfRoom, Servicess, Cost)
values('QDPL','�������������� �����','4','2', '�������, �����������, ���������� Wi-Fi, ����, ����-���, �����, ����, ������� ��� ������, ��������� � ������� �������, ���,
			���������� ��������� ��������������, ������, ������ �������, �����������, �����', 306);



create table Room(
Room_code		nvarchar(5) constraint PK_Room_code primary key,
Code_category   varchar(5) not null,
Repair			nvarchar(3) not null check(Repair = '��' or Repair = '���'),
Booking         nvarchar(3) not null check(Booking = '��' or Booking = '���'),
Free			nvarchar(3) not null check(Free = '��' or Free = '���')
);
alter table Room add constraint FK_Code_category foreign key (Code_category) references CategoryOfRoom(Category_code);


create table Booking (
Booking_number int constraint PK_Booking_number primary key,
Room_code		nvarchar(5) not  null,
Client_code		int not null,
TypeOfFood		char(2) not null,
Payment_number  int not null,
Arrivall		date not null,
Departuree		date not null,
DateOfBooking   date not null
);

alter table Booking add constraint FK_Room_code foreign key (Room_code)			references Room(Room_code);
alter table Booking add constraint FK_Client_code foreign key (Client_code)		references Client(Client_code);
alter table Booking add constraint FK_TypeOfFood foreign key (TypeOfFood)		references TypeOfFood(Type_code);
alter table Booking add constraint FK_Payment_number foreign key (Payment_number)	references Pay(Payment_number);

--procedure
go
   CREATE PROCEDURE Enter
   @login nvarchar(30),
   @password nvarchar(30)
   AS
   select a.[Login], a.[Password], a.Userr
from Autorization as a
where a.[Login] = @login and a.[Password] = @password
 go 

  create PROCEDURE FreeRoomsForBooking
   @date1 date,
   @date2 date
   AS
   select r.Room_code, c.Category_name
	from Booking as b inner join Room as r
	on b.Room_code = r.Room_code
	inner join CategoryOfRoom as c
	on r.Code_category = c.Category_code
	where (@date1 < b.Arrivall or @date1 > b.Departuree) and (@date2 < b.Arrivall or  @date2 > b.Departuree)  and (@date2 < b.Departuree or @date1 > b.Arrivall)
	union
	select r.Room_code, c.Category_name
	from Room as r inner join CategoryOfRoom as c
	on r.Code_category = c.Category_code
	where r.Booking = '���' and r.Free = '��'
go
--30.05 02.06
 exec FreeRoomsForBooking @date1 ='30.05.2018', @date2 = '02.06.2018';
 go
  create PROCEDURE Search
   @date1 date,
   @date2 date,
   @people int
   AS
   select r.Room_code, c.Category_name, c.Servicess, c.Cost, c.NumberOfSeats
	from Booking as b inner join Room as r
	on b.Room_code = r.Room_code
	inner join CategoryOfRoom as c
	on r.Code_category = c.Category_code
	where (@date1 < b.Arrivall or @date1 > b.Departuree) and (@date2 < b.Arrivall or  @date2 > b.Departuree)  and (@date2 < b.Departuree or @date1 > b.Arrivall)
	and c.NumberOfSeats >= @people
	union
	select r.Room_code, c.Category_name, c.Servicess, c.Cost, c.NumberOfSeats
	from Room as r inner join CategoryOfRoom as c
	on r.Code_category = c.Category_code
	where r.Booking = '���' and r.Free = '��' and r.Repair = '���'  and c.NumberOfSeats >= @people
	go

	 exec Search @date1 ='30.05.2018', @date2 = '02.06.2018', @people = 2;
