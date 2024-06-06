if exists(select * from sys.objects where OBJECT_ID=OBJECT_ID(N'usp_GetDetailTransactionInfo')and TYPE in(N'P',N'PC'))
 drop procedure usp_GetDetailTransactionInfo
 Go
create procedure usp_GetDetailTransactionInfo
(

@customerId int null,
@PaymentMethodId int null,
@startDate datetime null,
@enddate datetime null,
@storeId int null,
@supplierId int null,
@categogyId int null,
@productId int null
)
as

begin

select pr.Id as TransactionId,pr.TransactionDate,ci.CustomerName, 
c.CategoryName,p.ProductName,pd.Quantity,u.UnitName,pd.Amount,s.SupplierName,
pr.PaymentMethod
from ProductInvoiceInfo pr
left join ProductInvoiceDetailInfo pd on pd.ProductInvoiceInfoId=pr.Id
left join ProductRateInfo r on r.Id = pd.ProductRateInfoId
left join CategoryInfo c on c.Id=r.CategoryInfoId
left join ProductInfo p on p.Id=r.ProductInfoId
left join UnitInfo u on u.Id=p.UnitInfoId
left join SupplierInfo s on s.Id=r.SupplierInfoId
left join CustomerInfo ci on ci.Id=pr.CustomerInfoId

where pr.BillStatus=1 and Pr.StoreInfoId=@storeId
and ((@customerId is null) or (pr.CustomerInfoId=@customerId))
and ((@PaymentMethodId is null) or (pr.PaymentMethod=@PaymentMethodId))
and ((@supplierId is null) or (r.SupplierInfoId=@supplierId))
and ((@categogyId is null) or (r.CategoryInfoId=@categogyId))
and ((@productId is null) or (r.ProductInfoId=@productId))
AND ((@startDate IS NULL)
    OR (CAST(TransactionDate AS date) >= CONVERT(varchar(10), @startDate, 120)
    AND CAST(TransactionDate AS date) <= CONVERT(varchar(10), ISNULL(@endDate, GETDATE()), 120)))

	end

