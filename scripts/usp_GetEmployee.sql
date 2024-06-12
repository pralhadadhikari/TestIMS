if exists(select * from sys.objects where OBJECT_ID=OBJECT_ID(N'usp_GetEmployee')and TYPE in(N'P',N'PC'))
 drop procedure usp_GetEmployee
 Go
/*
exec usp_GetEmployee @storeId=2
*/
CREATE PROCEDURE usp_GetEmployee
(
    @storeId INT null
)
AS
BEGIN
    SELECT u.Id, RTRIM(CONCAT(u.FirstName + ' ',  u.MiddleName + ' ', u.LastName)) AS FullName,
    Email, s.StoreName, r.Name AS RoleName, u.Address, u.IsActive
    FROM AspNetUsers u 
    LEFT JOIN Roles r ON u.UserRoleId = r.RoleId
	Left Join StoreInfo s on s.Id=u.StoreId
    WHERE ((@storeId is null) or (StoreId = @storeId))
END