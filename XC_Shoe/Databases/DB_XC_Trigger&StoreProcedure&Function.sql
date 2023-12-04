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