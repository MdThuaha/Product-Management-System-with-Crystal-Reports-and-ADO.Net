CREATE TABLE Sellers
(
	sellerid INT IDENTITY PRIMARY KEY,
	sellername NVARCHAR(50) NOT NULL,
	selleraddress NVARCHAR(100) NOT NULL

)
GO
CREATE TABLE Products 
(
	productid INT IDENTITY PRIMARY KEY,
	productname NVARCHAR(50) NOT NULL,
	unitprice MONEY NOT NULL,
	picture NVARCHAR(40) NOT NULL
)
GO 
CREATE TABLE Sales
(
	saleid INT IDENTITY PRIMARY KEY,
	[date] DATE NOT NULL,
	productid INT NOT NULL REFERENCES products (productid),
	sellerid INT NOT NULL REFERENCES Sellers(sellerid),
	quantity INT NOT NULL
)
GO