if exists(select * from sys.objects where OBJECT_ID=OBJECT_ID(N'usp_getDashboardIndex')and TYPE in(N'P',N'PC'))
 drop procedure usp_getDashboardIndex
 Go

create proc usp_getDashboardIndex
(
	@storeId int null
)
as
begin


select *,(Sucessful+Canelled) as Completed from

(select SUM(TotalAmount) as TotalTransaction ,COUNT(*) as Sucessful from ProductInvoiceInfo
where BillStatus=1 and StoreInfoId=@storeId) a

cross join

(select COUNT(*) as Canelled from ProductInvoiceInfo
where BillStatus=2 and StoreInfoId=@storeId) b

end