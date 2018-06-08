
--- View this for reference: https://docs.microsoft.com/en-us/sql/relational-databases/security/row-level-security?view=sql-server-2017
--- Needs to be connected to Master DB
go
CREATE LOGIN Contoso WITH PASSWORD = 'Azure$123';
GO
CREATE LOGIN Fabrikam WITH PASSWORD = 'Azure$123';
go
CREATE LOGIN Management WITH PASSWORD = 'Azure$123';

--- Needs to be connected to AdventureWorksMultitenant
GO
CREATE USER Contoso FOR LOGIN Contoso;
GO
CREATE USER Fabrikam FOR LOGIN Fabrikam;
GO
CREATE USER Management FOR LOGIN Management;
GO
CREATE TABLE Sales  
    (  
    OrderID int,  
    SalesRep sysname,  
    Product varchar(10),  
    Qty int, 
	Tenant varchar(10),  
    ); 

GO


INSERT Sales VALUES   
(1, 'Sales1', 'Valve', 5, 'Contoso'),   
(2, 'Sales1', 'Wheel', 2, 'Contoso'),   
(3, 'Sales1', 'Valve', 4, 'Contoso'),  
(4, 'Sales2', 'Bracket', 2, 'Contoso'),   
(5, 'Sales2', 'Wheel', 5, 'Contoso'),   
(6, 'Sales2', 'Seat', 5, 'Contoso');  

INSERT Sales VALUES   
(1, 'Sales1', 'Valve', 5, 'Fabrikam'),   
(2, 'Sales1', 'Wheel', 2, 'Fabrikam'),   
(3, 'Sales1', 'Valve', 4, 'Fabrikam'),  
(4, 'Sales2', 'Bracket', 2, 'Fabrikam'),   
(5, 'Sales2', 'Wheel', 5, 'Fabrikam'),   
(6, 'Sales2', 'Seat', 5, 'Fabrikam');  

GO

SELECT * FROM dbo.Sales;

GO

GRANT SELECT ON Sales TO Contoso; 
GO
GRANT SELECT ON Sales TO Fabrikam;
GO
GRANT SELECT ON Sales TO Management;
GO

CREATE SCHEMA Security;  
GO  

ALTER FUNCTION Security.fn_securitypredicate(@Tenant AS sysname)  
    RETURNS TABLE  
WITH SCHEMABINDING  
AS  
    RETURN SELECT 1 AS fn_securitypredicate_result   
WHERE @Tenant = USER_NAME() OR USER_NAME() = 'Management'; 

GO

CREATE SECURITY POLICY TenantFilter  
ADD FILTER PREDICATE Security.fn_securitypredicate(Tenant)   
ON dbo.Sales  
WITH (STATE = ON); 


EXECUTE AS USER = 'Contoso';  
SELECT * FROM Sales;   
REVERT;  

EXECUTE AS USER = 'Fabrikam';  
SELECT * FROM Sales;   
REVERT;  

EXECUTE AS USER = 'Management';  
SELECT * FROM Sales;   
REVERT;  