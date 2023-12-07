CREATE DATABASE DB_XC_Shoes_Store

USE DB_XC_Shoes_Store

--CREATE TABLE

CREATE TABLE Users (
	ID INT IDENTITY(1,1),
    UserID AS CAST((LEFT('US' + RIGHT(CAST(ID AS VARCHAR(5)), 3),15)) AS VARCHAR(10)) PERSISTED NOT NULL,
    UserName NVARCHAR(155) UNIQUE,
	Email NVARCHAR(155) UNIQUE,
	Password  NVARCHAR(155),
    PhoneNumber VARCHAR(12) UNIQUE,
	Image VARCHAR(50) DEFAULT 'User.jpg',
	Role INT DEFAULT 0,
	CONSTRAINT PK_Users PRIMARY KEY (UserID),
	CONSTRAINT CK_Gender_Type CHECK (Gender IN (N'Nam', 'Nữ')),
	CONSTRAINT CK_Role CHECK (Role IN (0,1, 2, 3))
);
CREATE TABLE Shop_Branchs (
	ID INT IDENTITY(1,1),
	ShopID AS CAST((LEFT('Shop' + RIGHT(CAST(ID AS VARCHAR(5)), 3),15)) AS VARCHAR(10)) PERSISTED NOT NULL,
    ShopBranchAddress NVARCHAR(155),
	BranchManagement VARCHAR(10),
	CONSTRAINT PK_Shop_Branchs PRIMARY KEY (ShopID),
);
CREATE TABLE Related_staff (
	ID INT IDENTITY(1,1),
	EmployeeID AS CAST((LEFT('Emp' + RIGHT(CAST(ID AS VARCHAR(5)), 3),15)) AS VARCHAR(10)) PERSISTED NOT NULL,
	UserID VARCHAR(10),
	ShopBranchs VARCHAR(10) NOT NULL,
    Address NVARCHAR(255),
    StartDate DATE,
    EmploymentStatus NVARCHAR(50),
	CONSTRAINT PK_Related_staff PRIMARY KEY (EmployeeID),
	CONSTRAINT FK_Related_staff_ShopBranchsID FOREIGN KEY (ShopBranchs) REFERENCES Shop_Branchs(ShopID),
	CONSTRAINT FK_Related_staff_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID),
);
CREATE TABLE Users_ShipmentDetails (
	UserID VARCHAR(10) NOT NULL,
    Name NVARCHAR(55),
    PhoneNumber NVARCHAR(12),
    SpecificAddress NVARCHAR(125),
    AdministrativeBoundaries NVARCHAR(125),
    IsDefault bit,
	CONSTRAINT PK_Users_ShipmentDetails PRIMARY KEY (UserID,Name,PhoneNumber,SpecificAddress,AdministrativeBoundaries),
	CONSTRAINT FK_Users_ShipmentDetails_ID FOREIGN KEY (UserID) REFERENCES Users(UserID),
);
CREATE TABLE Shopping_Cart (
	CartID INT IDENTITY(1,1) NOT NULL,
	UserID VARCHAR(10) UNIQUE,
    Subtotal DECIMAL(18, 2) DEFAULT 0,
	CONSTRAINT PK_Cart PRIMARY KEY (CartID),
	CONSTRAINT FK_Cart_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID),
);
CREATE TABLE Favorites (
	FavoriteID INT IDENTITY(1,1) NOT NULL,
	UserID VARCHAR(10) UNIQUE,
	CONSTRAINT PK_Favorites PRIMARY KEY (FavoriteID),
	CONSTRAINT FK_Favorites_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID),
);
CREATE TABLE Colours (
    ColourID INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(255) NOT NULL,
	CONSTRAINT PK_Colours PRIMARY KEY (ColourID),
);
CREATE TABLE Icons (
    IconID VARCHAR(4),
    Name NVARCHAR(155) UNIQUE,
	Quantity INT DEFAULT 0,
	CONSTRAINT PK_Icons PRIMARY KEY (IconID),
);
CREATE TABLE Shoes (
	ID INT DEFAULT 1,
	IconID VARCHAR(4),
    ShoesID VARCHAR(10) NOT NULL,
    StyleType NVARCHAR(20),
	Price DECIMAL(10, 2),
    Discount DECIMAL(5, 2) DEFAULT 0,
	CONSTRAINT PK_Shoes PRIMARY KEY (ShoesID),
	CONSTRAINT FK_Shoes_Details_IconID FOREIGN KEY (IconID) REFERENCES Icons(IconID),
);
CREATE TABLE Sale(
	SaleID INT IDENTITY(1,1) NOT NULL,
	ShoesID VARCHAR(10),
    StartDate DATE,
    EndDate DATE,
    Quantity DECIMAL(5, 2),
	EmployeeID VARCHAR(10),
	CONSTRAINT PK_Sale PRIMARY KEY (SaleID),
    CONSTRAINT FK_Sale_ShoesID FOREIGN KEY (ShoesID) REFERENCES Shoes(ShoesID),
	CONSTRAINT FK_Sale_EmployeeID FOREIGN KEY (EmployeeID) REFERENCES Related_staff(EmployeeID)
);
CREATE TABLE Type_Shoes (
    TypeShoesID INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(255) UNIQUE,
	CONSTRAINT PK_Type_Shoes PRIMARY KEY (TypeShoesID),
);
CREATE TABLE Shoes_Details (
	ShoesDetailsID INT IDENTITY(1,1) NOT NULL,
    ShoesID VARCHAR(10) UNIQUE,
    Name NVARCHAR(255),
	TypeShoesID INT,
	CONSTRAINT PK_Shoes_Details PRIMARY KEY (ShoesDetailsID),
    CONSTRAINT FK_Shoes_Details_ID FOREIGN KEY (ShoesID) REFERENCES Shoes(ShoesID),
	CONSTRAINT FK_Shoes_Details_TypeShoesID FOREIGN KEY (TypeShoesID) REFERENCES Type_Shoes(TypeShoesID),

);
CREATE TABLE Comments (
    CommentID INT IDENTITY(1,1) NOT NULL,
    Content NVARCHAR(555),
    StarRating INT,
	ShoesDetailsID int,
	UserID VARCHAR(10),
	CONSTRAINT PK_Comments PRIMARY KEY (CommentID),
    CONSTRAINT FK_Comments_ShoesDetailsID FOREIGN KEY (ShoesDetailsID) REFERENCES Shoes_Details(ShoesDetailsID),
	CONSTRAINT FK_Comments_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID),
	CONSTRAINT CK_Comments_StarRating CHECK(StarRating > 0)
);
CREATE TABLE Orders (
    ID INT IDENTITY(1,1) NOT NULL,
	OrderID AS CAST((LEFT('Order' + RIGHT(CAST(ID AS VARCHAR(5)), 3),15)) AS VARCHAR(10)) PERSISTED NOT NULL,
	UserID VARCHAR(10) NULL,
    PaymentInfo NVARCHAR(255) DEFAULT N'Payment in cash',
    EstimatedDeliveryHandlingFee DECIMAL(10, 2),
    Total DECIMAL(10, 2),
    PaymentStatus NVARCHAR(50) DEFAULT N'Unpaid',
    RecipientAddress NVARCHAR(255),
    RecipientName NVARCHAR(100),
    RecipientPhoneNumber VARCHAR(20),
	OrderDate DATE DEFAULT GETDATE(),
	CONSTRAINT PK_Orders PRIMARY KEY (OrderID),
    CONSTRAINT FK_Orders_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID),
);

CREATE TABLE OrderSystem (
	OrderID VARCHAR(10) NOT NULL,
	EmployeeID VARCHAR(10) DEFAULT 'None',
    OrderDate DATE DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT N'Wait for confirmation',
	CONSTRAINT PK_OrderSystem PRIMARY KEY (OrderID),
    CONSTRAINT FK_OrderSystem_OrderID FOREIGN KEY (OrderID) REFERENCES Orders(OrderID)--,
	--CONSTRAINT FK_OrderSystem_EmployeeID FOREIGN KEY (EmployeeID) REFERENCES Related_staff(EmployeeID),
);
CREATE TABLE Order_Detail(
	OrderID VARCHAR(10) NOT NULL,
	ShoesID VARCHAR(10),
	Quantity INT,
	Size int,
	StyleType NVARCHAR(20),
	ColourID INT,
	Price DECIMAL(10, 2),
	CONSTRAINT PK_Orders_Detail PRIMARY KEY (OrderID, ShoesID, ColourID, Size),
	CONSTRAINT FK_Order_Detai_OrderID FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
	CONSTRAINT FK_Order_Detai_ShoesID FOREIGN KEY (ShoesID) REFERENCES Shoes(ShoesID),
);
CREATE TABLE List_Products_At_Shop (
	ShopID VARCHAR(10) NOT NULL,
	ShoesID VARCHAR(10) NOT NULL,
	Size VARCHAR(10),
	Quantity INT DEFAULT 0,
	CONSTRAINT PK_List_Products_At_Shop PRIMARY KEY (ShopID,ShoesID),
	CONSTRAINT FK_List_Products_At_Shop_ShopID FOREIGN KEY (ShopID) REFERENCES Shop_Branchs(ShopID),
	CONSTRAINT FK_List_Products_At_Shop_ShoesID FOREIGN KEY (ShoesID) REFERENCES Shoes(ShoesID),
);
CREATE TABLE Cart_Detail (
	CartID INT NOT NULL,
	ShoesID VARCHAR(10) NOT NULL,
	StyleType NVARCHAR(20),
	ColourID INT,
	Size INT,
	Price DECIMAL(10, 2),
	Quantity INT,
    BuyingSelection_Status BIT DEFAULT 0,
	CONSTRAINT PK_Cart_Detail PRIMARY KEY (CartID, ShoesID, ColourID, StyleType, Size),
	CONSTRAINT FK_Cart_Detail_CartID FOREIGN KEY (CartID) REFERENCES Shopping_Cart(CartID),
	CONSTRAINT FK_Cart_Detail_ShoesID FOREIGN KEY (ShoesID) REFERENCES Shoes(ShoesID),
);
CREATE TABLE Colour_Detail(
	ColourID INT NOT NULL,
	ShoesID VARCHAR(10) NOT NULL,
	CONSTRAINT PK_Colour_Detail PRIMARY KEY (ShoesID,ColourID),
	CONSTRAINT FK_Colour_Detail_ColourID FOREIGN KEY (ColourID) REFERENCES Colours(ColourID),
	CONSTRAINT FK_Colour_Detail_ShoesID FOREIGN KEY (ShoesID) REFERENCES Shoes(ShoesID),
);
CREATE TABLE Images(
	ImageID INT IDENTITY(1,1) NOT NULL,
	ShoesID VARCHAR(10),
	ColourID INT,
	Name NVARCHAR(100),
	Url NVARCHAR(100),
	CONSTRAINT PK_Images PRIMARY KEY (ImageID,ShoesID),
	CONSTRAINT FK_Images_ShoesID FOREIGN KEY (ShoesID) REFERENCES Shoes(ShoesID),
	CONSTRAINT FK_Images_ColourID FOREIGN KEY (ColourID) REFERENCES Colours(ColourID),
);
CREATE TABLE Favorite_Detaill (
	FavoriteID INT NOT NULL,
	ShoesID VARCHAR(10) NOT NULL,
	ColourID INT NOT NULL,
	StyleType NVARCHAR(20),
	CONSTRAINT PK_Favorite_Detail PRIMARY KEY (FavoriteID,ShoesID,ColourID,StyleType),
	CONSTRAINT FK_Favorite_Detail_FavoriteID FOREIGN KEY (FavoriteID) REFERENCES Favorites(FavoriteID),
	CONSTRAINT FK_Favorite_Detail_ShoesID FOREIGN KEY (ShoesID) REFERENCES Shoes(ShoesID),
);
CREATE TABLE Size(
	SizeID INT NOT NULL,
	CONSTRAINT PK_Size PRIMARY KEY (SizeID),
);
CREATE TABLE Size_Detail(
	SizeID INT NOT NULL,
	ColourID INT NOT NULL,
	shoesID VARCHAR(10) NOT NULL,
	CONSTRAINT PK_Size_Detail PRIMARY KEY (SizeID,ColourID,shoesID),
	CONSTRAINT FK_Size_Detail_SizeID  FOREIGN KEY (SizeID) REFERENCES Size(SizeID),
	CONSTRAINT FK_Size_Detail_ColourID FOREIGN KEY (ColourID) REFERENCES Colours(ColourID),
	CONSTRAINT FK_Size_Detail_ShoesID FOREIGN KEY (ShoesID) REFERENCES Shoes(ShoesID),
);
GO

SELECT * FROM Users
SELECT *FROM Shop_Branchs
SELECT *FROM Shoes
SELECT*FROM Shoes_Details
SELECT*FROM Images
SELECT *FROM Icons
SELECT *FROM Related_staff
SELECT * FROM Users_ShipmentDetails
Drop table Users
Drop table Icons
Drop table Users_ShipmentDetails
drop table Colours
drop table Shoes