if exists(select * from sys.objects where OBJECT_ID=OBJECT_ID(N'usp_getDashboardList')and TYPE in(N'P',N'PC'))
 drop procedure usp_getDashboardList
 Go

create proc usp_getDashboardList
(
	@storeId int null
)
as
begin
select c.CategoryName,count(*) as Count,sum(pd.Amount) as TotalAmount from ProductInvoiceInfo p
left join ProductInvoiceDetailInfo pd on p.Id=pd.ProductInvoiceInfoId
left join ProductRateInfo pr on pr.Id=pd.ProductRateInfoId
left join CategoryInfo c on c.Id=pr.CategoryInfoId

where BillStatus=1 
and p.StoreInfoId=@storeId

group by c.CategoryName

end