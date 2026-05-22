if OBJECT_ID('v_InvoiceUKD', 'V') is not NULL
  drop view dbo.v_InvoiceUKD
go
-- [AO] Вьюха для распечатки корректировочной счет-фактуры и кредит-ноты
create view dbo.v_InvoiceUKD
as
select
  -- Клиент
  IsNull(C.Name,                     '') as ClientName,
  IsNull(C.NameFull, IsNull(C.Name, '')) as ClientNameFull,
  IsNull(C.Adress,                   '') as ClientAdress,
  IsNull(C.Tel,                      '') as ClientTel,
  IsNull(C.OKPO,                     '') as ClientOKPO,
  IsNull(C.UNN,                      '') as ClientUNN,
  IsNull(C.KPP,                      '') as ClientKPP,
  IsNull(CB_C.KS,                    '') as ClientKS,
  IsNull(BK_C.BIC,                   '') as ClientBIC,
  IsNull(C.OKOHX,                    '') as ClientOKOHX,
  IsNull(CB_C.RS,                    '') as ClientRS,
  IsNull(BK_C.Name,                  '') as ClientBank, 
  IsNull(DA.Name, T.AddressDelivery    ) as AddressDelivery,  

  -- Продавец
  IsNull(S.Name,                     '') as SellerName,
  IsNull(S.NameFull,                 '') as SellerNameFull,
  IsNull(S.AlternativeName,          '') as SellerAlternativeName,
  IsNull(S.UNN,                      '') as SellerUNN,
  IsNull(S.City,                     '') as SellerCity,
  IsNull(S.Adress,                   '') as SellerAdress,
  IsNull(CB_S.RS,                    '') as SellerRS,
  IsNull(BK_S.Name,                  '') as SellerBank,
  IsNull(S.Tel,                      '') as SellerTel,
  IsNull(S.Fax,                      '') as SellerFax,
  IsNull(S.eMail,                    '') as SellerEMail,
  IsNull(S.Site,                     '') as SellerSite,
  IsNull(S.OKOHX,                    '') as SellerOKOHX,
  IsNull(S.OKPO,                     '') as SellerOKPO,
  IsNull(S.OGRN,                     '') as SellerOGRN,
  IsNull(CB_S.KS,                    '') as SellerKS,
  IsNull(BK_S.BIC,                   '') as SellerBIC,
  IsNull(S.KPP,                      '') as SellerKPP,
  IsNull(S.CertificateNDS,           '') as SellerCertificateNDS,

  case when IsNull(DSD.bSignatureFromUser, 0) = 1
       then IsNull(U.ManagerName, '')
       else IsNull(S.ChiefName, '')
       end as SellerChiefName,
  case when IsNull(DSD.bSignatureFromUser, 0) = 1
       then IsNull(U.ManagerName, '')
       else IsNull(S.AccountantName, '')
       end as SellerAccountantName,

  T.ID                   as idTask,
  IsNull(T.bWarranty, 0) as bWarranty,
  T_OLD.ID               as idTaskOld,
  IsNull(T.NumCalcFact, T_OLD.NumCalcFact + '-К') as NumCalcFact,
  IsNull(T_OLD.NumCalcFact, '') as NumCalcFactOld,
  IsNull(T.AccountNum,      '') as AccountNum,
  IsNull(T_OLD.AccountNum,  '') as AccountNumOld,
  IsNull(T.Date,            '') as TaskDate,
  IsNull(T_OLD.Date,        '') as TaskDateOld,
  IsNull(T.ForAccountNum, ''       ) as ForAccountNum,
  IsNull(T_OLD.ForAccountNum, ''   ) as ForAccountNum_OLD,
 
  dbo.f_GetDateComplite_Correct(T.DateComplite, T.Date, IsNull(CF.d_iNum, 0))    as DateComplete,
  dbo.f_GetDateComplite_Correct(T_OLD.DateComplite, T_OLD.Date, IsNull(CF.d_iNum, 0)) as DateCompleteOld,
  
  P.Num as PrjNum,
  P.PriceS,    -- цена за шпросы
  case when IsNull(P.IsPriceByCount, 0) = 1 and CPU.d_iNum = 0 and ISNULL(P.Area, 0) > 0 then P.PriceNDS/P.Area   else P.PriceNDS   end as PriceM2WithNDS,
  case when IsNull(P.IsPriceByCount, 0) = 1 and CPU.d_iNum = 0 and ISNULL(P.Area, 0) > 0 then P.PriceNoNDS/P.Area else P.PriceNoNDS end as PriceOfUnit,
  P.SumWithNDS      as PriceWithNDS,
  P.SumNDS          as NDS,
  P.SumNoNDS,
  P.PriceByMNoNDS,
  P.PriceNoNDS_M2,
  case when IsNull(P.IsPriceByCount, 0) = 1 and CPU.d_iNum = 0 and ISNULL(P.Area, 0) > 0 then P.PriceNDS/P.Area   else P.PriceNDS   end as PriceNDS,
  P.Mass * P.nCount as Mass,
  P.nCount          as nCount,
  P.Area,
  P.nCount * P.Area as nCountArea,

  P_OLD.Num as PrjNum_OLD,
  P_OLD.PriceS              as PriceS_Old,    -- цена за шпросы
  case when IsNull(P_OLD.IsPriceByCount, 0) = 1 and CPU.d_iNum = 0 and ISNULL(P_OLD.Area, 0) > 0 then P_OLD.PriceNDS/P_OLD.Area   else P_OLD.PriceNDS   end as PriceM2WithNDS_Old,
  case when IsNull(P_OLD.IsPriceByCount, 0) = 1 and CPU.d_iNum = 0 and ISNULL(P_OLD.Area, 0) > 0 then P_OLD.PriceNoNDS/P_OLD.Area else P_OLD.PriceNoNDS end as PriceOfUnit_Old,
  P_OLD.SumWithNDS          as PriceWithNDS_Old,
  P_OLD.SumNDS              as NDS_Old,
  P_OLD.SumNoNDS            as SumNoNDS_Old,
  P_OLD.PriceByMNoNDS       as PriceByMNoNDS_Old,
  P_OLD.PriceNoNDS_M2       as PriceNoNDS_M2_Old,
  case when IsNull(P_OLD.IsPriceByCount, 0) = 1 and CPU.d_iNum = 0 and ISNULL(P_OLD.Area, 0) > 0 then P_OLD.PriceNDS/P_OLD.Area   else P_OLD.PriceNDS   end as PriceNDS_Old,
  P_OLD.Mass * P_OLD.nCount as Mass_Old,
  P_OLD.nCount              as nCount_Old,
  P_OLD.Area                as Area_Old,
  P_OLD.nCount * P_OLD.Area as nCountArea_Old,

  dbo.f_GetSpecialProductName(IsNull(CF.d_iNum, 0), PD.Name, P.GPName, P.CamCount, P.Thickness, P.Width, P.Height, IsNull(P.ComplexText, ''), IsNull(P.CommentClient, ''), P.ID, P.Num, 0) as GPName,
  dbo.f_RUS_To_Eng(P.GPName) as GPNameEng,

  case when isNull(Nds.Name,'') = '' then cast(NDS.NDS as varchar(2)) else Nds.Name end as Tax_rate,
  NDS.Name as NameNDS,
  IsNull(T.CreditNoteNum, '') as CreditNoteNum,
  IsNull(T.CreditNoteDate, 0) as CreditNoteDate,
  IsNull(T.CreditNoteDescription, '') as CreditNoteDescription,
  IsNull(T.KSF_NumCalcFact, '') as KSF_NumCalcFact,
  case when P_OLD.CamCount = 0 then 'Стекло ' else 'Стеклопакет ' end +
  case when P_OLD.CamCount > 0 then ''        else ''             end +
    P_OLD.GPName + ' ' + ltrim(str(P_OLD.Width)) + ' x ' +  ltrim(str(P_OLD.Height)) +
  case when P_OLD.bShpros <> 0 then ' ' + dbo.f_GetGPRasInfo(P_OLD.ID, P_OLD.bShpros) + 
                                ' ' + ltrim(str((select sum(LengReal) from RasShrink where idProject = P_OLD.ID))) + 'мм' 
  else '' 
  end
    + ', ' + ltrim(str(P_OLD.nCount)) + ' шт.' as GPNameStr
from Task T
  inner join Project P       on P.idTask    = T.ID
  inner join Product PD      on PD.ID       = P.idProd
  left  join BarCode B       on B.idProject = P.ID
  inner join Task T_OLD      on T_OLD.ID    = T.idTask_Parent
  inner join Project P_OLD   on T_OLD.ID    = P_OLD.idTask and P_OLD.Num = P.Num
  left  join BarCode B_OLD   on P_OLD.ID    = B_OLD.idProject
  left  join Config CF       on CF.Name     = 'FormatTypeOfGPName'
  left  join Config CPU      on CPU.Name    = 'bPriceUnitInCalcFact'
  left  join Config CGC      on CGC.Name    = 'nGlassMarkCovering'   -- тип маркировки покрытия
  left  join NDS             on NDS.ID      = T_OLD.idNDS
  left  join Client C        on C.ID        = T.idClient
  left  join Client S        on S.ID        = T.idSeller
  left  join ClientBank CB_C on CB_C.ID     = T.idClientBank_Client
  left  join Bank BK_C       on BK_C.ID     = CB_C.idBank          
  left  join ClientBank CB_S on CB_S.ID     = T.idClientBank_Seller
  left  join Bank BK_S       on BK_S.ID     = CB_S.idBank          
  left  join DeliveryAddress DA   on DA.ID  = T.idDeliveryAddress
  left  join DepotSubDivision DSD on DSD.ID = T.idDepotSubDivision
  left  join Users U              on lower(U.Name) = lower(SYSTEM_USER)  
where 
  T.idTask_Parent           is not null
group by
  C.Name,
  P.bShpros,
  C.NameFull,
  C.Adress,
  C.Tel,
  C.OKPO,
  C.UNN,
  C.KPP,
  CB_C.KS,
  BK_C.BIC,
  C.OKOHX,
  CB_C.RS,
  BK_C.Name,
  DA.Name,
  T.AddressDelivery,

  S.Name,
  S.NameFull,
  S.AlternativeName,
  S.UNN,
  S.City,
  S.Adress,
  CB_S.RS,
  BK_S.Name,
  S.Tel,
  S.Fax,
  S.eMail,
  S.Site,
  S.OKOHX,
  S.OKPO,
  S.OGRN,
  CB_S.KS,
  BK_S.BIC,
  S.KPP,
  S.CertificateNDS,
  DSD.bSignatureFromUser,
  U.ManagerName,
  S.ChiefName,
  S.AccountantName,

  T.ID,
  T.bWarranty,
  T.NumCalcFact,
  T.AccountNum,
  T.ForAccountNum, 
  T.Date,
  T.DateComplite,

  T_OLD.ID,
  T_OLD.NumCalcFact,
  T_OLD.AccountNum,
  T_OLD.ForAccountNum, 
  T_OLD.Date,
  T_OLD.DateComplite,

  CF.d_iNum,

  P.Num,
  P.PriceS,
  P.IsPriceByCount,
  CPU.d_iNum,
  P.Area,
  P.PriceNDS,
  P.PriceNoNDS,
  P.SumWithNDS,
  P.SumNDS,
  P.SumNoNDS,
  P.PriceByMNoNDS,
  P.PriceNoNDS_M2,
  P.Mass,
  P.nCount,
  P.Width,
  P.Height,
  P.Thickness,
  P.GPName,
  P.CamCount,
  P.ComplexText,
  P.CommentClient,
  P.ID,

  P_OLD.ID,
  P_OLD.Num,
  P_OLD.PriceS,
  P_OLD.IsPriceByCount,
  P_OLD.Area,
  P_OLD.PriceNDS,
  P_OLD.PriceNoNDS,
  P_OLD.SumWithNDS,
  P_OLD.SumNDS,
  P_OLD.SumNoNDS,
  P_OLD.PriceByMNoNDS,
  P_OLD.PriceNoNDS_M2,
  P_OLD.Mass,
  P_OLD.nCount,
  P_OLD.CamCount,
  P_OLD.GPName,
  P_OLD.Width,
  P_OLD.Height,
  P_OLD.bShpros,

  PD.Name,
  Nds.Name,
  NDS.NDS,
  T.CreditNoteNum,
  T.CreditNoteDate,
  T.CreditNoteDescription,
  T.KSF_NumCalcFact
go
