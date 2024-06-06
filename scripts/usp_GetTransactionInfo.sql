if exists(select * from sys.objects where OBJECT_ID=OBJECT_ID(N'usp_GetTransactionInfo')and TYPE in(N'P',N'PC'))
 drop procedure usp_GetTransactionInfo
 Go
create procedure usp_GetTransactionInfo
(

@customerId int null,
@PaymentMethodId int null,
@startDate datetime null,
@enddate datetime null,
@storeId int null
)
as

begin

select pr.Id as TransactionId,pr.TransactionDate,ci.CustomerName, NetAmount,DiscountAmount,TotalAmount,PaymentMethod,Remarks 
 from ProductInvoiceInfo pr
left join CustomerInfo ci on pr.CustomerInfoId=ci.Id

where pr.BillStatus=1
and Pr.StoreInfoId=@storeId
and ((@customerId is null) or (pr.CustomerInfoId=@customerId))
and ((@PaymentMethodId is null) or (pr.PaymentMethod=@PaymentMethodId))
AND ((@startDate IS NULL)
    OR (CAST(TransactionDate AS date) >= CONVERT(varchar(10), @startDate, 120)
    AND CAST(TransactionDate AS date) <= CONVERT(varchar(10), ISNULL(@enddate, GETDATE()), 120)))

	end

