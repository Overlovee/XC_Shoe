USE DB_XC_Shoes_Store
-- Trigger 
CREATE TRIGGER UPDATE_Quantity_ShoesIcon
ON Shoes
AFTER INSERT
AS
BEGIN
	UPDATE Icons
	Set Quantity += 1
	WHERE Icons.IconID = (SELECT IconID FROM INSERTED)
END
GO

--
CREATE TRIGGER trg_CreateOrderSystem
ON Orders
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    -- Insert into OrderSystem for each new order with default Status
    INSERT INTO OrderSystem (OrderID, OrderDate)
    SELECT OrderID, OrderDate
    FROM INSERTED;
END;


--
CREATE TRIGGER TRIGGER_Create_ShoesID
ON Shoes
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Lưu các dòng được chèn vào một bảng tạm
    DECLARE @InsertedData TABLE (
        ID INT,
        IconID VARCHAR(4),
		ShoesID AS CAST((LEFT(IconID + RIGHT(CAST(ID AS VARCHAR(5)), 3),15)) AS VARCHAR(10)) PERSISTED,
        StyleType NVARCHAR(20),
		Price DECIMAL(10, 2),
        Discount DECIMAL(5, 2) DEFAULT 0
    );

    -- Chèn dữ liệu từ bảng inserted vào bảng tạm
    INSERT INTO @InsertedData (IconID, StyleType,Price)
    SELECT IconID, StyleType,Price
    FROM inserted;

    -- Cập nhật ID dựa trên giá trị Quantity từ bảng Icons
    UPDATE @InsertedData
    SET ID = Quantity+1
    FROM @InsertedData i
    INNER JOIN Icons ON i.IconID = Icons.IconID;

    -- Chèn dữ liệu từ bảng tạm vào bảng Shoes
    INSERT INTO Shoes (ID, IconID,ShoesID, StyleType,Price, Discount)
    SELECT ID, IconID,ShoesID, StyleType,Price, Discount
    FROM @InsertedData;
END;

GO

-- Store proceduce
CREATE PROCEDURE InsertUser 
@UserName NVARCHAR(155), @Gender NVARCHAR(10), @Email NVARCHAR(155), @Password  NVARCHAR(155), @PhoneNumber VARCHAR(12), @Role INT
AS
BEGIN
	INSERT INTO Users(UserName, Gender, Email, Password, PhoneNumber, Role)
	VALUES(@UserName, @Gender, @Email, @Password, @PhoneNumber, @Role);
	DECLARE @User_ID VARCHAR(10);
	SET @User_ID = (SELECT UserID FROM Users WHERE Email = @Email)
	INSERT INTO Shopping_Cart (UserID)
	VALUES (@User_ID);
	INSERT INTO Favorites(UserID)
	VALUES (@User_ID);
	END
GO
--exec dbo.InsertUser N'Trương Quốc Huy', N'Nam', N'huyngao@gmail.com', N'612233', '03157839578', 0

-- Update User
CREATE PROCEDURE UpdateUser
	@UserID VARCHAR(10),
	@UserName NVARCHAR(155)  = NULL,
	@Gender NVARCHAR(10) = NULL,
	@Email NVARCHAR(155) = NULL,
	@Password  NVARCHAR(155) = NULL,
	@PhoneNumber VARCHAR(12) = NULL,
	@Role INT = NULL
AS
BEGIN
    UPDATE Users
    SET 
        UserName = ISNULL(@UserName,UserName),
        Gender = ISNULL(@Gender, Gender),
		Email = ISNULL(@Email, Email),
        Password = ISNULL(@Password, Password),
		PhoneNumber = ISNULL(@PhoneNumber, PhoneNumber),
        Role = ISNULL(@Role, Role)
    WHERE UserID = @UserID;
END
GO

--Delete User
CREATE PROCEDURE deleteUser
    @UserID VARCHAR(10)
AS
BEGIN
    DELETE FROM Users_ShipmentDetails
    WHERE UserID = @UserID;

	DELETE FROM Shopping_Cart
    WHERE UserID = @UserID;

	DELETE FROM Favorites
    WHERE UserID = @UserID;
	
	DELETE FROM Comments
    WHERE UserID = @UserID;
    -- Xóa user từ bảng Users
    DELETE FROM Users
    WHERE UserID = @UserID;
END
GO

--
CREATE PROCEDURE AddNewShoes
    @IconID VARCHAR(4),
    @TypeShoesID INT,
    @Name NVARCHAR(255),
    @Price DECIMAL(10, 2),
    @StyleType NVARCHAR(20),
    @Image NVARCHAR(100)
AS
BEGIN
    DECLARE @ShoesID VARCHAR(10);

    -- Thêm mới vào bảng Shoes và lấy ShoesID vừa thêm
    INSERT INTO Shoes (IconID, Price, StyleType)
    VALUES (@IconID, @Price, @StyleType);

	SET  @ShoesID = (SELECT TOP(1) ShoesID FROM Shoes WHERE IconID = @IconID ORDER BY ID DESC) 

    -- Thêm mới vào bảng Shoes_Details
    INSERT INTO Shoes_Details (Name, ShoesID, TypeShoesID)
    VALUES (@Name, @ShoesID, @TypeShoesID);

	INSERT INTO Images(ShoesID,Name, Url)
	VALUES(@ShoesID,@Name,@Image)
END
GO
CREATE PROCEDURE AddFavorite
	@UserID VARCHAR(10),
	@ShoesID VARCHAR(10),
	@ColourName NVARCHAR(255),
    @StyleType NVARCHAR(20)
AS
BEGIN
    DECLARE @FavoriteID INT,@ColourID INT;
    SET @FavoriteID =(SELECT f.FavoriteID FROM Favorites F WHERE f.UserID = @UserID )
	SET @ColourID =(SELECT c.ColourID FROM Colours c WHERE c.Name = @ColourName )
	INSERT INTO Favorite_Detaill (FavoriteID, ShoesID, ColourID, StyleType)
	VALUES 
		(@FavoriteID,@ShoesID,@ColourID,@StyleType);
END
GO
--EXEC dbo.AddFavorite 'US3','AM1','White Blue','Men'
GO
CREATE PROCEDURE DeletoShoesInFavorite
	@FavoriteID int,
	@ShoesID VARCHAR(10),
	@ColourName NVARCHAR(255),
    @StyleType NVARCHAR(20)
AS
BEGIN
    DECLARE @ColourID INT;
	SET @ColourID =(SELECT c.ColourID FROM Colours c WHERE c.Name = @ColourName )
	DELETE FROM Favorite_Detaill
	WHERE FavoriteID = @FavoriteID AND ShoesID = @ShoesID AND ColourID = @ColourID AND StyleType = @StyleType;
END
GO
--EXEC dbo.DeletoShoesInFavorite 1,'AM1','White Blue','Men'
--Function
CREATE FUNCTION GetBeforeMailString(@EMAIL NVARCHAR(155))
RETURNS NVARCHAR(100)
AS 
BEGIN
	DECLARE @M NVARCHAR(255)= @EMAIL
	DECLARE @atIndex INT = CHARINDEX('@', @email);
	DECLARE @result NVARCHAR(255) = LEFT(@email, @atIndex - 1);
	RETURN @result
END
GO
--EXEC dbo.AddNewShoes 'SP',5,N'Nike Spoting 28',6200000,N'Nam','Sport.jpg'
CREATE FUNCTION COUNT_Colour(@shoesID VARCHAR(10))
RETURNS INT
AS
BEGIN 
	DECLARE @Number_colour INT;
	SET @Number_colour = (SELECT COUNT(ColourID) AS Number_Colour
						FROM Colour_Detail
						GROUP BY ShoesID
						HAVING ShoesID = @shoesID);
	RETURN @Number_colour
END
GO
--
CREATE FUNCTION GetFavorite(@UserID VARCHAR(10))
RETURNS TABLE
AS
RETURN
	( SELECT S.ShoesID,SD.TypeShoesID,SD.Name as 'ShoeName',S.StyleType,TS.Name as'TypeName',dbo.COUNT_Colour(s.ShoesID) as 'Number_Colour',S.Price,Im.Url,c.Name
	FROM Favorite_Detaill FD, Favorites F,Shoes S,Shoes_Details SD,Type_Shoes TS,Images Im,Colours c
	WHERE F.FavoriteID = FD.FavoriteID AND FD.ShoesID = S.ShoesID AND S.ShoesID = SD.ShoesID AND SD.TypeShoesID = TS.TypeShoesID AND Im.ShoesID = FD.ShoesID AND FD.ColourID = Im.ColourID AND c.ColourID = FD.ColourID AND F.UserID = @UserID
)
GO
--SELECT * FROM dbo.GetFavorite('US3')
CREATE FUNCTION ShowDetailShoes(@ShoesID VARCHAR(10),@ColourName NVARCHAR(100))
RETURNS @new_table TABLE (IconID VARCHAR(10),ShoesID VARCHAR(10),TypeShoesID INT,NameShoes NVARCHAR(255),StyleType NVARCHAR(255),NameTypeShoes NVARCHAR(20),NameColour NVARCHAR(255),NumberColour INT,Price DECIMAL(10, 2),Discount DECIMAL(5, 2),Url NVARCHAR(100))
AS
BEGIN
		INSERT INTO @new_table
		SELECT S.IconID, S.ShoesID,SD.TypeShoesID,SD.Name,S.StyleType,TS.Name,C.Name,dbo.COUNT_Colour(s.ShoesID) as 'Number_Colour',S.Price,S.Discount,Im.Url
		FROM Shoes S,Shoes_Details SD,Type_Shoes TS,Colour_Detail CD,Images Im,Colours C
		WHERE S.ShoesID = SD.ShoesID AND SD.TypeShoesID = TS.TypeShoesID AND S.ShoesID = CD.ShoesID AND Im.ShoesID = S.ShoesID AND C.ColourID = CD.ColourID AND Im.ColourID = C.ColourID
		GROUP BY S.IconID, S.ShoesID,SD.TypeShoesID,SD.Name,S.StyleType,TS.Name,S.Price,S.Discount,Im.Url,C.Name
		HAVING S.ShoesID = @ShoesID AND C.Name = @ColourName 
    RETURN
END
GO
--SELECT * FROM dbo.ShowDetailShoes('SP1','Black')
GO
CREATE FUNCTION ShowDetailShoesWithShoesID(@ShoesID VARCHAR(10))
RETURNS @new_table TABLE (IconID VARCHAR(10),ShoesID VARCHAR(10),TypeShoesID INT,NameShoes NVARCHAR(255),StyleType NVARCHAR(255),NameTypeShoes NVARCHAR(20),NameColour NVARCHAR(255),NumberColour INT,Price DECIMAL(10, 2),Discount DECIMAL(5, 2),Url NVARCHAR(100))
AS
BEGIN
		INSERT INTO @new_table
		SELECT S.IconID, S.ShoesID,SD.TypeShoesID,SD.Name,S.StyleType,TS.Name,C.Name,dbo.COUNT_Colour(s.ShoesID) as 'Number_Colour',S.Price,S.Discount,Im.Url
		FROM Shoes S,Shoes_Details SD,Type_Shoes TS,Colour_Detail CD,Images Im,Colours C
		WHERE S.ShoesID = SD.ShoesID AND SD.TypeShoesID = TS.TypeShoesID AND S.ShoesID = CD.ShoesID AND Im.ShoesID = S.ShoesID AND C.ColourID = CD.ColourID AND Im.ColourID = C.ColourID
		GROUP BY S.IconID, S.ShoesID,SD.TypeShoesID,SD.Name,S.StyleType,TS.Name,S.Price,S.Discount,Im.Url,C.Name
		HAVING S.ShoesID = @ShoesID
    RETURN
END
GO
--SELECT * FROM dbo.ShowDetailShoesWithShoesID('SP1')

-- Tạo hàm truy vấn láy đôi giày đại diện cho mỗi shoesid
-- Tạo hàm truy vấn
GO
CREATE FUNCTION GetFirstShoeInfo(@StyleType NVARCHAR(50))
RETURNS TABLE
AS
RETURN
(
    SELECT
        IconID,
        ShoesID,
        TypeShoesID,
        ShoesName,
        StyleType,
        TypeShoesName,
        ColorName,
        Number_Colour,
        Price,
        Discount,
        Url
    FROM
    (
        SELECT
            S.IconID,
            S.ShoesID,
            SD.TypeShoesID,
            SD.Name AS ShoesName,
            S.StyleType,
            TS.Name AS TypeShoesName,
            C.Name AS ColorName,
            dbo.COUNT_Colour(S.ShoesID) AS 'Number_Colour',
            S.Price,
            S.Discount,
            Im.Url,
            ROW_NUMBER() OVER (PARTITION BY S.ShoesID ORDER BY S.ShoesID) AS RowNum
        FROM
            Shoes S
            INNER JOIN Shoes_Details SD ON S.ShoesID = SD.ShoesID
            INNER JOIN Type_Shoes TS ON SD.TypeShoesID = TS.TypeShoesID
            INNER JOIN Colour_Detail CD ON S.ShoesID = CD.ShoesID
            INNER JOIN Images Im ON Im.ShoesID = S.ShoesID
            INNER JOIN Colours C ON CD.ColourID = C.ColourID
        WHERE
            CD.ColourID = Im.ColourID AND S.StyleType = @StyleType
        GROUP BY
            S.IconID,
            S.ShoesID,
            SD.TypeShoesID,
            SD.Name,
            S.StyleType,
            TS.Name,
            C.Name,
            S.Price,
            S.Discount,
            Im.Url
    ) RankedShoes
    WHERE RowNum = 1
);
GO
--SELECT * FROM dbo.GetFirstShoeInfo('Men')
GO
---
CREATE FUNCTION GetShoesByTypeShoes(@StyleType NVARCHAR(50),@TypeShoes INT)
RETURNS TABLE
AS
RETURN
(
    SELECT
        IconID,
        ShoesID,
        TypeShoesID,
        ShoesName,
        StyleType,
        TypeShoesName,
        ColorName,
        Number_Colour,
        Price,
        Discount,
        Url
    FROM
    (
        SELECT
            S.IconID,
            S.ShoesID,
            SD.TypeShoesID,
            SD.Name AS ShoesName,
            S.StyleType,
            TS.Name AS TypeShoesName,
            C.Name AS ColorName,
            dbo.COUNT_Colour(S.ShoesID) AS 'Number_Colour',
            S.Price,
            S.Discount,
            Im.Url,
            ROW_NUMBER() OVER (PARTITION BY S.ShoesID ORDER BY S.ShoesID) AS RowNum
        FROM
            Shoes S
            INNER JOIN Shoes_Details SD ON S.ShoesID = SD.ShoesID
            INNER JOIN Type_Shoes TS ON SD.TypeShoesID = TS.TypeShoesID
            INNER JOIN Colour_Detail CD ON S.ShoesID = CD.ShoesID
            INNER JOIN Images Im ON Im.ShoesID = S.ShoesID
            INNER JOIN Colours C ON CD.ColourID = C.ColourID
        WHERE
            CD.ColourID = Im.ColourID AND S.StyleType = @StyleType AND SD.TypeShoesID = @TypeShoes
        GROUP BY
            S.IconID,
            S.ShoesID,
            SD.TypeShoesID,
            SD.Name,
            S.StyleType,
            TS.Name,
            C.Name,
            S.Price,
            S.Discount,
            Im.Url
    ) RankedShoes
    WHERE RowNum = 1
);
GO


--SELECT * FROM dbo.GetShoesByTypeShoes('men',6);
GO
CREATE FUNCTION GetFavorite(@UserID VARCHAR(10))
RETURNS TABLE
AS
RETURN
	( SELECT S.ShoesID,SD.TypeShoesID,SD.Name as 'ShoeName',S.StyleType,TS.Name as'TypeName',dbo.COUNT_Colour(s.ShoesID) as 'Number_Colour',S.Price,Im.Url,c.Name,F.FavoriteID
	FROM Favorite_Detaill FD, Favorites F,Shoes S,Shoes_Details SD,Type_Shoes TS,Images Im,Colours c
	WHERE F.FavoriteID = FD.FavoriteID AND FD.ShoesID = S.ShoesID AND S.ShoesID = SD.ShoesID AND SD.TypeShoesID = TS.TypeShoesID AND Im.ShoesID = FD.ShoesID AND FD.ColourID = Im.ColourID AND c.ColourID = FD.ColourID AND F.UserID = @UserID
)
GO
--SELECT * FROM dbo.GetFavorite('US3')
GO
--
CREATE FUNCTION GetBag(@UserID VARCHAR(10))
RETURNS TABLE
AS
RETURN
	(Select  cd.ShoesID,shd.Name as 'ShoesName',s.StyleType,ts.Name as 'TypeName',c.Name as 'ColorName',cd.Size,cd.Quantity,cd.Price,cd.BuyingSelection_Status,im.Url
	from Cart_Detail cd, Shopping_Cart sc, Shoes_Details shd,Type_Shoes TS,Colours C,Shoes s,Images Im
	Where cd.CartID = sc.CartID AND shd.ShoesID = cd.ShoesID AND TS.TypeShoesID = shd.TypeShoesID AND C.ColourID = CD.ColourID AND s.ShoesID = cd.ShoesID AND Im.ShoesID = s.ShoesID AND Im.ColourID = c.ColourID
	AND sc.UserID = @UserID
)
GO
--SELECT *FROM dbo.GetBag('US3')

--Add
CREATE PROCEDURE AddToBag
	@UserID VARCHAR(10),
	@ShoesID VARCHAR(10),
	@ColourName NVARCHAR(255),
	@StyleType NVARCHAR(20),	
	@size INT,
	@Quantity INT
AS
BEGIN
    DECLARE @CartID INT,@ColourID INT;
    SET @CartID =(SELECT f.FavoriteID FROM Favorites F WHERE f.UserID = @UserID )
	SET @ColourID =(SELECT c.ColourID FROM Colours c WHERE c.Name = @ColourName )
	INSERT INTO Cart_Detail(CartID, ShoesID, StyleType, ColourID,Size,Quantity)
	VALUES 
		(@CartID,@ShoesID,@StyleType,@ColourID,@size,@Quantity);
END
GO
--Update
CREATE PROCEDURE UpdateBag
	@UserID VARCHAR(10),
	@ShoesID VARCHAR(10),
	@StyleType NVARCHAR(20),
	@ColourName NVARCHAR(255),
	@size INT,
	@Quantity INT
AS
BEGIN
    DECLARE @CartID INT,@ColourID INT;
    SET @CartID =(SELECT f.FavoriteID FROM Favorites F WHERE f.UserID = @UserID )
	SET @ColourID =(SELECT c.ColourID FROM Colours c WHERE c.Name = @ColourName )
	UPDATE Cart_Detail
	SET 
        ShoesID = ISNULL(@ShoesID,ShoesID),
		StyleType = ISNULL(@StyleType,StyleType),
		ColourID = ISNULL(@ColourID,ColourID),
		Size = ISNULL(@size,Size),
		Quantity = ISNULL(@Quantity,Quantity)
	WHERE CartID = @CartID;
END
GO
-- Delete
CREATE PROCEDURE DeletoShoesInBag
	@UserID VARCHAR(10),
	@ShoesID VARCHAR(10),
	@ColourName NVARCHAR(255),
	@StyleType NVARCHAR(20),
	@size INT 	
AS
BEGIN
	DECLARE @CartID INT,@ColourID INT;
    SET @CartID =(SELECT f.FavoriteID FROM Favorites F WHERE f.UserID = @UserID )
	SET @ColourID =(SELECT c.ColourID FROM Colours c WHERE c.Name = @ColourName )
	DELETE FROM Cart_Detail
	WHERE CartID = @CartID  AND ShoesID = @ShoesID AND ColourID = @ColourID AND StyleType = @StyleType AND size = @size;
END
GO
