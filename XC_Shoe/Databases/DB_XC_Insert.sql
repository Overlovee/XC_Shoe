USE DB_XC_Shoes_Store

-- Insert dữ liệu vào bảng Shop_Branchs
INSERT INTO Shop_Branchs (ShopBranchAddress, BranchManagement)
VALUES ('Aeon Mall Tân Phú', 'Emp1')

-- Insert dữ liệu vào bảng Users
INSERT INTO Users (UserName, Gender, Email, Password, PhoneNumber, Role)
VALUES (N'Nguyễn Huy Hoàng', N'Nam', N'hoang2011@gmail.com', N'123456', '09123461711', 1),
	(N'Nguyễn Minh Thư', N'Nam', N'minhthu21@gmail.com', N'141222', '03989212641', 0),
	 (N'Vương Kim Dinh', N'Nam', N'dinh0212@gmail.com', N'111111', '03412578891', 0);
 --   (N'Trương Quốc Huy', N'Nam', N'huyngao@gmail.com', N'612233', '03157839578', 0),
	--(N'Trương Ngọc Sơn', N'Nam', N'Sontruong22@gmail.com', N'111233', '03415781234', 0),
	--(N'Trương Quốc Hoàng', N'Nam', N'Hoangtruong20@gmail.com', N'251233', '09236512481', 0),
	--(N'Nguyễn Ngọc Lan', N'Nữ', N'ngoclan13@gmail.com', N'771233', '09856325124', 1),
	--(N'Trương Kim Thư', N'Nam', N'Thutruong95@gmail.com', N'681243', '03245136242', 0),
	--(N'Nguyễn Mộng Mơ', N'Nữ', N'Monguyen75@gmail.com', N'111442', '07658954236', 0),
	--(N'Trương Huy Sơn', N'Nam', N'HuySon00@gmail.com', N'100233', '0785469236', 1);

-- Insert dữ liệu vào bảng Related_staff
INSERT INTO Related_staff (UserID, ShopBranchs, Address, StartDate, EmploymentStatus)
VALUES ('US1', 'Shop1', N'140 Lê Trọng Tấn', '2022-04-01', N'Active'),
    ('US2', 'Shop1', N'153 Nguyễn Chí Thanh', '2021-05-21', N'Inactive'),
	('US3', 'Shop1', N'15 Nguyễn Chí Thanh','2019-05-20', N'Inactive');
 --   ('US1', 'Shop1', N'44 Lê Trọng Tấn','2022-02-18', N'Active'),
	--('US4', 'Shop2', N'256 Bùi Tư Toàn','2021-03-20', N'Active'),
	--('US2', 'Shop2', N'123 Quốc Lộ 1A','2020-10-30', N'Active'),	
	--('US5', 'Shop2', N'23 Nguyễn Hữu Thọ','2021-05-31', N'Active'),
	--('US6', 'Shop1', N'120 Tân Kỳ Tân Quý','2021-07-30', N'Inactive');

-- Insert dữ liệu vào bảng Users_ShipmentDetails
Insert into Users_ShipmentDetails(UserID,Name,PhoneNumber,SpecificAddress,AdministrativeBoundaries,IsDefault)
VALUES('US1',N'Nguyễn Văn Hoàng',N'0124516363',N'Thành phố Hồ Chí Minh',N'140 Lê Trọng Tấn',1),
	  ('US2',N'Nguyễn Văn Thống',N'0956324851',N'Thành phố Hồ Chí Minh',N'13 Nam Kì Khởi Nghĩa',1)
	  --('US004',N'Trần Minh Huy',N'032562958',N'Thành phố Hồ Chí Minh',N'215 Gò Dầu',1),
	  --('US005',N'Phan Ngọc Trinh',N'098547216',N'Thành phố Hồ Chí Minh',N'14 Tú Xương',0),
	  --('US006',N'Nguyễn Ngọc Minh',N'075632140',N'Thành phố Hồ Chí Minh',N'512 3 tháng 2',1);

-- Insert dữ liệu vào bảng Shopping_Cart
INSERT INTO Shopping_Cart (UserID)
VALUES 
    --('US3')
 --   ('US4'),
	('US1'),
	('US2');
	--('US5'),
	--('US6'),
	--('US7');

-- Insert dữ liệu vào bảng Favorites
INSERT INTO Favorites (UserID)
VALUES 
    --('US3');
 --   ('US4'),
	('US1'),
	('US2');
	--('US5'),
	--('US6'),
	--('US7');

-- Insert dữ liệu vào bảng Colours
INSERT INTO Colours (Name)
VALUES 
    ('Red'),
    ('White'),
	('Black'),
	('Beige'),
	('Gray'),
	('Pink'),
	('White Blue'),
    ('Green'),
	('Brown'),
	('Purple'),
	('Blue'),
	('Olive Green');

-- Insert dữ liệu vào bảng Icons
INSERT INTO Icons (IconID, Name)
VALUES 
    ('AF', 'Air Force 1'),
    ('JD', 'Jordan'),
	('AM', 'Air Max'),
	('DK', 'Dunk'),
	('CT', 'CorTez'),
	('BZ', 'Blazer'),
	('PG', 'Pegasus'),
	('SP', 'Sport');

-- Insert dữ liệu vào bảng Shoes
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES 
    ('AF',2500000 ,'Men');
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES 
	('AF',2700000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES 
	('AF',3700000, 'Men');
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AF',4700000, 'Men');
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AF',2800000, 'Men');
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AF',3300000, 'Men');
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AF',2890000, 'Women');
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AF',3700000, 'Women');
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AF',3600000, 'Women');
INSERT INTO Shoes(IconID,Price, StyleType)
VALUES 
	('AM',4700000, 'Men');
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES 
	('AM',3700000, 'Men');
INSERT INTO Shoes (IconID, Price, StyleType)
VALUES
	('AM',2600000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AM',3700000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AM',2700000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AM',3100000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AM',4700000, 'Women')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AM',2500000, 'Women')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('AM',2600000, 'Women');
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('JD',2300000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('JD',3050000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('JD',2900000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('JD',2500000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('JD',3200000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('JD',2200000, 'Women')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('JD',3900000, 'Women')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('JD',3700000, 'Women')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('SP',2700000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('SP',3400000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('SP',2700000, 'Men');
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('SP',1900000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('SP',3800000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('SP',3750000, 'Men')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('SP',4700000, 'Women')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('SP',5200000, 'Women')
INSERT INTO Shoes (IconID,Price, StyleType)
VALUES
	('SP',5700000, 'Women');

-- Insert dữ liệu vào bảng Images
INSERT INTO Images (ShoesID,Name, Url)
VALUES 
    ('AF1', 'Nike Air Force 1  Low ', 'AF1-MenShoes-2-Nike Air Force 07'),
	('AF1', 'Nike Air Force 1  Low ', 'AF1-MenShoes-3-Nike Air Force 07'),
	('AF1', 'Nike Air Force 1  Low ', 'AF1-MenShoes-4-Nike Air Force 07'),
	('AF2', 'Nike Air Force 1  Mid ', 'AF2-MenShoes-1-Nike Air Force 1  Mid'),
	('AF2', 'Nike Air Force 1  Mid ', 'AF2-MenShoes-2-Nike Air Force 1  Mid'),
	('AF2', 'Nike Air Force 1  Mid ', 'AF2-MenShoes-5-Nike Air Force 1  Mid'),
	('AF3', 'Nike Air Force 1  Low ', 'AF3-WomenShoes-4-Nike Air Force 1  Low'),
	('AF3', 'Nike Air Force 1  Low ', 'AF3-WomenShoes-6-Nike Air Force 1  Low'),
	('AF3', 'Nike Air Force 1  Low ', 'AF3-WomenShoes-7-Nike Air Force 1  Low'),
	('AM1', 'Nike Air Max 97 ', 'AM1-MenShoes-2-Nike Air Max 97'),
	('AM1', 'Nike Air Max 97 ', 'AM1-MenShoes-5-Nike Air Max 97'),
	('AM1', 'Nike Air Max 97 ', 'AM1-MenShoes-7-Nike Air Max 97'),
	('AM2', 'Nike Air Max 90 ', 'AM2-MenShoes-1-Nike Air Max 90'),
	('AM2', 'Nike Air Max 90 ', 'AM2-MenShoes-2-Nike Air Max 90'),
	('AM2', 'Nike Air Max 90 ', 'AM2-MenShoes-3-Nike Air Max 90'),
	('AM3','-Nike Air Max 90 Futura ', 'AM3-WomenShoes-5-Nike Air Max 90 Futura'),
	('AM3', 'Nike Air Max 90 Futura ', 'AM3-WomenShoes-6-Nike Air Max 90 Futura'),
	('AM3', 'Nike Air Max 90 Futura ', 'AM3-WomenShoes-12-Nike Air Max 90 Futura'),
	('JD1', 'Air Jordan 1 Low ', 'JD1-MenShoes-2-Air Jordan 1 Low'),
	('JD1', 'Air Jordan 1 Low ', 'JD1-MenShoes-3-Air Jordan 1 Low'),
	('JD2', 'Air Jordan 1 Mid ', 'JD2-MenShoes-2-Air Jordan 1 Mid'),
	('JD2', 'Air Jordan 1 Mid ', 'JD2-MenShoes-6-Air Jordan 1 Mid'),
	('JD2', 'Air Jordan 1 Mid ', 'JD2-MenShoes-7-Air Jordan 1 Mid'),
	('JD3', 'Air Jordan 1 Mid ', 'JD3-WomenShoes-4-Air Jordan 1 Mid'),
	('JD3', 'Air Jordan 1 Mid ', 'JD3-WomenShoes-6-Air Jordan 1 Mid'),
	('JD3','Air Jordan 1 Mid ', 'JD3-WomenShoes-10-Air Jordan 1 Mid'),
	('SP1','Nike Pegasus 40 ', 'SP1-MenShoes-1-Nike Pegasus 40'),
	('SP1','Nike Pegasus 40 ', 'SP1-MenShoes-2-Nike Pegasus 40'),
	('SP1','Nike Pegasus 40 ', 'SP1-MenShoes-3-Nike Pegasus 40'),
	('SP2','Nike Structure 25 ', 'SP2-MenShoes-2-Nike Structure 25'),
	('SP2','Nike Structure 25 ', 'SP2-MenShoes-4-Nike Structure 25'),
	('SP2','Nike Structure 25 ', 'SP2-MenShoes-8-Nike Structure 25'),
	('SP3','Nike Infinity 4 ', 'SP3-WomenShoes-2-Nike Infinity 4'),
	('SP3', 'Nike Infinity 4 ', 'SP3-WomenShoes-3-Nike Infinity 4'),
	('SP3','Nike Infinity 4 ', 'SP3-WomenShoes-6-Nike Infinity 4');

-- Insert dữ liệu vào bảng Sale
INSERT INTO Sale (ShoesID, StartDate, EndDate, Quantity, EmployeeID)
VALUES 
    ('AF1', '2023-01-01', '2023-01-10', 10.5, 'Emp1'),
	('AF2', '2023-02-02', '2023-02-12', 9.0, 'Emp2')
	--('AF3', '2022-05-20', '2022-05-25', 20.5, 'Emp3'),
	--('AM1', '2022-06-21', '2022-06-26', 12.5, 'Emp4'),
	--('AM2', '2022-10-09', '2022-10-14', 10.5, 'Emp5'),
	--('AM3', '2022-01-22', '2022-01-27', 11.5, 'Emp1'),
	--('JD1', '2022-02-12', '2022-02-22', 15.5, 'Emp2'),
	--('JD2', '2021-07-13', '2021-07-18', 8.5, 'Emp4'),
	--('JD3', '2020-05-13', '2020-05-18', 7.5, 'Emp5'),
	--('SP1', '2021-11-12', '2021-11-17', 19.5, 'Emp1'),
	--('SP2', '2023-10-05', '2023-10-10', 14.5, 'Emp2'),
	--('SP3', '2021-10-20', '2021-10-25', 11.5, 'Emp3');

-- Insert dữ liệu vào bảng Type_Shoes
INSERT INTO Type_Shoes (Name)
VALUES 
	('Customise'),
    ('Running Shoes'),
	('Basketball Shoes'),
	('Football Shoes'),
	('Golf Shoes'),
    ('Casual Shoes');

-- Insert dữ liệu vào bảng Shoes_Details
INSERT INTO Shoes_Details (Name, ShoesID, TypeShoesID)
VALUES 
    ('Nike Air Force 1  Low ', 'AF1', 1),
	('Nike Air Force 1  Mid ', 'AF2', 1),
	('Nike Air Force 1  Low ', 'AF3', 1),
	('Nike Air Max 97 ',  'AM1', 6),
	('Nike Air Max 90 ', 'AM2', 6),
	('Nike Air Max 90 Futura ',  'AM3', 6),
	('Air Jordan 1 Low ', 'JD1', 3),
	('Air Jordan 1 Mid ',  'JD2', 3),
	('Air Jordan 1 Mid ', 'JD3', 3),
	('Nike Pegasus 40 ', 'SP1', 5),
	('Nike Structure 25 ',  'SP2', 2),
	('Nike Infinity 4 ', 'SP3', 4);

-- Insert dữ liệu vào bảng Comments
INSERT INTO Comments (Content, StarRating, ShoesDetailsID, UserID)
VALUES 
	('The shoe color is very beautiful!', 5, 5, 'US1'),
	('The shoes are very nice!', 5, 4, 'US2');
 --   ('Great shoes!', 5, 1, 'US4'),
 --   ('Comfortable and stylish.', 4, 2, 'US5'),
 --   ('Not bad, but a bit tight.', 4, 3, 'US3'),
	--('Stability Shoe.', 4, 2, 'US7'),
	--('Good quality and beautiful shoes!', 5, 1, 'US6'),
	--('Shoes are very satisfactory!', 5, 6, 'US5');

-- Insert dữ liệu vào bảng Orders
INSERT INTO Orders (UserID, EstimatedDeliveryHandlingFee, Email, Total, RecipientAddress, RecipientName, RecipientPhoneNumber)
VALUES 
    --('US3', 30000, N'dinh0212@gmail.com', 2730000, N'40a BuSan Korea', N'Vương Kim Dinh', '03412578891');
--    ('US4', 2, 20000,  N'huyngao@gmail.com', 2520000, N'140 Lê Trong Tấn TP Hồ Chí Minh', N'Huy Trương',  '03157839578');
	('US1', 25000,  N'thu13@gmail.com', 2730000, N'140 Lê Trong Tấn TP Hồ Chí Minh', N'Huy Trương',  '03157839578'),
	('US2', 32000,  N'hoangnguyen123@gmail.com', 2520000, N'50 Lê Trong Tấn TP Hồ Chí Minh', N'Dinh Trương',  '03157832421');
--	('US5', 5, 20000,  N'huytruong20@gmail.com', 89.99, N'140 Lê Trong Tấn TP Hồ Chí Minh', N'Huy Trương',  '03157839578'),
--	('US6', 6, 33000,  N'vinhnguyen3@gmail.com', 89.99, N'140 Lê Trong Tấn TP Hồ Chí Minh', N'Huy Trương',  '03157839578'),
--	('US7', 7, 40000,  N'kimdinh72@gmail.com', 89.99, N'140 Lê Trong Tấn TP Hồ Chí Minh', N'Huy Trương',  '03157839578');

Select * from Orders

--Insert dữ liệu vào bảng Order_Detail
INSERT INTO Order_Detail(OrderID, ShoesID, ColourID, size, StyleType, Quantity, Price)
VALUES
	('Order1','AF2',2, 42, 'Men', 2, 2700000),
	('Order1','AF1',2, 37, 'Men', 1, 2700000),
	('Order2','AF1',2, 38, 'Men', 1, 2500000);

SELECT *FROM OrderSystem
 --Insert dữ liệu vào bảng OrderSystem
INSERT INTO OrderSystem (OrderID,EmployeeID , OrderDate, Status)
VALUES 
    (1, 'Emp1','2023-02-20', N'Chờ Xác Nhận'),
	(2, 'Emp2','2023-05-22', N'Xác Nhận');


-- Insert dữ liệu vào bảng Cart_Detail
INSERT INTO Cart_Detail (CartID, ShoesID, StyleType, ColourID, Size, Price, Quantity, BuyingSelection_Status)
VALUES 
    (1, 'AF2', 'Men', 2, 39, 2500000, 1, 0),
	(2, 'AF1', 'Men', 3, 40, 2500000, 1, 1);

-- Insert dữ liệu vào bảng Color_Detail
INSERT INTO Colour_Detail (ColourID, ShoesID)
VALUES 
    (2, 'AF1'),
	(3, 'AF1'),
	(4, 'AF1'),
    (1, 'AF2'),
	(2, 'AF2'),
	(5, 'AF2'),
	(4, 'AF3'),
	(6, 'AF3'),
	(7, 'AF3'),
	(2, 'AM1'),
	(5, 'AM1'),
	(7, 'AM1'),
	(1, 'AM2'),
	(2, 'AM2'),
	(3, 'AM2'),
	(5, 'AM3'),
	(6, 'AM3'),
	(12, 'AM3'),
	(2, 'JD1'),
	(3, 'JD1'),
	(2, 'JD2'),
	(6, 'JD2'),
	(7, 'JD2'),
	(4, 'JD3'),
	(6, 'JD3'),
	(10, 'JD3'),
	(1, 'SP1'),
	(2, 'SP1'),
	(3, 'SP1'),
	(3, 'SP2'),
	(4, 'SP2'),
	(8, 'SP2'),
	(2, 'SP3'),
	(3, 'SP3'),
	(6, 'SP3');

-- Insert dữ liệu vào bảng Favorite_Detail
INSERT INTO Favorite_Detaill (FavoriteID, ShoesID, ColourID, StyleType)
VALUES 
    (1, 'AF2', 1, 'Men');
--    (2, 'AF2', 2, 'Men');

INSERT INTO size
VALUES(36),(37),(38),(39),(40),(41),(42);

INSERT INTO size_Detail
VALUES(38,2,'AF1'),(37,2,'AF1'),(39,2,'AF1'),(42,2,'AF2')