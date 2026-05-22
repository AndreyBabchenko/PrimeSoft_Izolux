if OBJECT_ID('sp_Invoice_IZO_Ship_Only', 'P') is not null
  drop procedure sp_Invoice_IZO_Ship_Only
go

-- Для распечатки счетов-фактур, накладных и т.п. только на отгружаемую часть.  
-- STIS 2015-09-04 добавлены из таблицы транспорт num,DeliveryNum номера Расходной накладной и счёта фактуры  
-- @bUnionRejectWithFather если 1  - то В случае отгрузки с переделкой выписывать счёт из отгрузки на первоначальный заказ, но включать в него переделку из отгрузки.  
  
create procedure sp_Invoice_IZO_Ship_Only @idShip int, @idTask int, @bOtherProduction int = 0, @bUnionRejectWithFather int = 0  
as  
begin  
  set nocount on  
  
  if OBJECT_ID('tempdb..#Temp') IS NOT NULL DROP Table #Temp  
    
  /* временная таблица будет создавться во время выполнения запроса  
     это нужно чтобы избежать проблем с размером текстовых полей  
  create table #Temp  
  (  
    forCount int,  
    idTask int,  
    idProject int,  
    idTaskType int,  
    TaskTypeName varchar(32),  
    TaskNum varchar(150) collate Cyrillic_General_CI_AS,  
    AccountNum varchar(150) collate Cyrillic_General_CI_AS,  
    TaskDate datetime,  
    ClientNum varchar(100) collate Cyrillic_General_CI_AS,  
    NumCalcFact varchar(150) collate Cyrillic_General_CI_AS,  
    NumCalcFact_Dealer varchar(150) collate Cyrillic_General_CI_AS,  
    DateComplete datetime,  
    Komission varchar(250) collate Cyrillic_General_CI_AS,  
    DatePayDoc datetime,  
    ClientName varchar(255) collate Cyrillic_General_CI_AS,  
    ClientNameFull varchar(255) collate Cyrillic_General_CI_AS,  
    ClientAdress varchar(255) collate Cyrillic_General_CI_AS,  
    AdressSubDiv varchar(255) collate Cyrillic_General_CI_AS,  
    ClientTel varchar(150) collate Cyrillic_General_CI_AS,  
    ClientOKPO varchar(100) collate Cyrillic_General_CI_AS,  
    ClientUNN varchar(20) collate Cyrillic_General_CI_AS,  
    ClientKPP varchar(50) collate Cyrillic_General_CI_AS,  
    ClientKS varchar(32) collate Cyrillic_General_CI_AS,  
    ClientBIC varchar(32) collate Cyrillic_General_CI_AS,  
    ClientOKOHX varchar(100) collate Cyrillic_General_CI_AS,  
    ClientRS varchar(32) collate Cyrillic_General_CI_AS,  
    ClientBank varchar(255) collate Cyrillic_General_CI_AS,  
    guidClient uniqueidentifier,  
    SellerName varchar(255) collate Cyrillic_General_CI_AS,  
    SellerNameFull varchar(255) collate Cyrillic_General_CI_AS,  
    SellerAlternativeName varchar(255) collate Cyrillic_General_CI_AS,  
    SellerUNN varchar(20) collate Cyrillic_General_CI_AS,  
    SellerAdress varchar(255) collate Cyrillic_General_CI_AS,  
    SellerRS varchar(32) collate Cyrillic_General_CI_AS,  
    SellerBank varchar(255) collate Cyrillic_General_CI_AS,  
    SellerTel varchar(150) collate Cyrillic_General_CI_AS,  
    SellerFax varchar(150) collate Cyrillic_General_CI_AS,  
    SellerEMail varchar(50) collate Cyrillic_General_CI_AS,  
    SellerSite varchar(50) collate Cyrillic_General_CI_AS,  
    SellerOKOHX varchar(100) collate Cyrillic_General_CI_AS,  
    SellerOKPO varchar(100) collate Cyrillic_General_CI_AS,  
    SellerOGRN varchar(64) collate Cyrillic_General_CI_AS,  
    SellerKS varchar(32) collate Cyrillic_General_CI_AS,  
    SellerBIC varchar(32) collate Cyrillic_General_CI_AS,  
    SellerKPP varchar(50) collate Cyrillic_General_CI_AS,  
    SellerAccountantName varchar(64) collate Cyrillic_General_CI_AS,  
    SellerChiefName varchar(64) collate Cyrillic_General_CI_AS,  
    SellerCertificateNDS varchar(255) collate Cyrillic_General_CI_AS,  
    ShipperName varchar(255) collate Cyrillic_General_CI_AS,  
    ShipperChiefName varchar(255) collate Cyrillic_General_CI_AS,  
    -- Тех.компания  
    TechCompanyName            varchar(255) collate Cyrillic_General_CI_AS,  
    TechCompanyNameFull        varchar(255) collate Cyrillic_General_CI_AS,  
    TechCompanyAlternativeName varchar(255) collate Cyrillic_General_CI_AS,  
    TechCompanyUNN             varchar(20)  collate Cyrillic_General_CI_AS,  
    TechCompanyCity            varchar(255) collate Cyrillic_General_CI_AS,  
    TechCompanyAdress          varchar(255) collate Cyrillic_General_CI_AS,  
    TechCompanyAdressSubDiv    varchar(255) collate Cyrillic_General_CI_AS,  
    TechCompanyRS              varchar(32)  collate Cyrillic_General_CI_AS,  
    TechCompanyBank            varchar(255) collate Cyrillic_General_CI_AS,  
    TechCompanyTel             varchar(150) collate Cyrillic_General_CI_AS,  
    TechCompanyFax             varchar(150) collate Cyrillic_General_CI_AS,  
    TechCompanyEMail           varchar(50)  collate Cyrillic_General_CI_AS,  
    TechCompanySite            varchar(50)  collate Cyrillic_General_CI_AS,  
    TechCompanyOKOHX           varchar(100) collate Cyrillic_General_CI_AS,  
    TechCompanyOKPO            varchar(100) collate Cyrillic_General_CI_AS,  
    TechCompanyOGRN            varchar(64)  collate Cyrillic_General_CI_AS,  
    TechCompanyKS              varchar(32)  collate Cyrillic_General_CI_AS,  
    TechCompanyBIC             varchar(32)  collate Cyrillic_General_CI_AS,  
    TechCompanyKPP             varchar(50)  collate Cyrillic_General_CI_AS,  
  
    ShiperName varchar(128) collate Cyrillic_General_CI_AS,  
    SubDivisionAddress varchar(255) collate Cyrillic_General_CI_AS,  
    ConsigneeNameFull varchar(255) collate Cyrillic_General_CI_AS,  
    ConsigneeAdress   varchar(255) collate Cyrillic_General_CI_AS,  
    ConsigneeUNN      varchar(20)  collate Cyrillic_General_CI_AS,  
    PriceS         decimal(13, 2),  
    PriceM2WithNDS decimal(13, 2),  
    PriceOfUnit    decimal(13, 2),                         -- Цена без НДС  
    PriceWithNDS   decimal(13, 2),                         -- Сумма по позиции с НДС  
    NDS            decimal(13, 2),  
    SumNoNDS       decimal(13, 2),                         -- Сумма без НДС  
    PriceNDS       decimal(13, 2),  
    bShpros bit,  
    Mass float,  
    MassSum float,  
    Num     int,  
    Unit varchar(5) collate Cyrillic_General_CI_AS,  
    Tax_rate varchar(2) collate Cyrillic_General_CI_AS,  
    nCount         int,  
    nCountPos      int,  
    nCountArea     float,                                     -- Количество / площадь по накладной в зависимости от вида измерения позиции накладной  
    nCountAreaOriginal  float,  
    GPName         varchar(255) collate Cyrillic_General_CI_AS,  
    GPNameMark     varchar(255) collate Cyrillic_General_CI_AS,  
    GPNameEng      varchar(255) collate Cyrillic_General_CI_AS,  
    GPName_FromFieldProject varchar(255) collate Cyrillic_General_CI_AS,  
    GPName_Common           varchar(255) collate Cyrillic_General_CI_AS,  
    Width  varchar(8) collate Cyrillic_General_CI_AS,  
    Height varchar(8) collate Cyrillic_General_CI_AS,  
    Area float,  
    IsPriceByCount bit,  
    CamCountStr varchar(3) collate Cyrillic_General_CI_AS,  
    Name varchar(32) collate Cyrillic_General_CI_AS,  
    Pricekvm decimal(13, 2),  
    Thickness varchar(3) collate Cyrillic_General_CI_AS,  
    Commentary varchar(255) collate Cyrillic_General_CI_AS,  
    ProductName varchar(50) collate Cyrillic_General_CI_AS,  
    DepotName varchar(100) collate Cyrillic_General_CI_AS,  
    DepotSubDivisionTel varchar(150) collate Cyrillic_General_CI_AS,  
    ManagerName varchar(50) collate Cyrillic_General_CI_AS,  
    SubDivisionManagerName varchar(50) collate Cyrillic_General_CI_AS,  
    AddTo_NumInvoice       varchar(16) collate Cyrillic_General_CI_AS,  
    OrderToSign            varchar(128) collate Cyrillic_General_CI_AS,  
    CurSaldo float,  
    sign_CurSaldo float,  
    Code    varchar(11)  collate Cyrillic_General_CI_AS,   -- STIS 2015-09-11 c 9 на 11  
    Manager varchar(128) collate Cyrillic_General_CI_AS,  
    Tel     varchar(64)  collate Cyrillic_General_CI_AS,  
    CamCount tinyint,  
    TypeOper int,  
    idShip int,  
    HeaderTN varchar(512) collate Cyrillic_General_CI_AS,  
    Signature_ShiperPost varchar(64) collate Cyrillic_General_CI_AS,  
    ProductType int,  
    ProductType_Project int,  
    guidProduct_Project uniqueidentifier,  
    ContractName varchar(128) collate Cyrillic_General_CI_AS,  
    ContractNum  varchar(32)  collate Cyrillic_General_CI_AS,  
    ContractDate datetime,  
    CommentClient varchar(128) collate Cyrillic_General_CI_AS,  
    NumInvoice    varchar(50)  collate Cyrillic_General_CI_AS,  
    TransportDate datetime,  
    idTransport   int,  
    guidTransport uniqueidentifier,  
    RasInfoText varchar(128) collate Cyrillic_General_CI_AS,  
    RasLength float,  
    Unit_Code_OKEI varchar(50) collate Cyrillic_General_CI_AS,  
    PriceNoNDS decimal(13, 2),  
    TaskCommentary varchar(255) collate Cyrillic_General_CI_AS,  
    FormTypeName varchar(255) collate Cyrillic_General_CI_AS,  
    idFormType   int,  
    NamePlot varchar(255) collate Cyrillic_General_CI_AS,  
    NameTemplate varchar(255) collate Cyrillic_General_CI_AS,  
    SellerShiperName varchar(255) collate Cyrillic_General_CI_AS,  
    SellerShiperPost     varchar(255) collate Cyrillic_General_CI_AS,  
    DriverFromShip       varchar(255) collate Cyrillic_General_CI_AS,  
    BarCodeClient        varchar(64)  collate Cyrillic_General_CI_AS,  
  
    CarDriver            varchar(255) collate Cyrillic_General_CI_AS,  
    CarName              varchar(255) collate Cyrillic_General_CI_AS,  
    CarGosNumber         varchar(255) collate Cyrillic_General_CI_AS,  
    CarLicense           varchar(255) collate Cyrillic_General_CI_AS,  
      
    LogistFromShip           varchar(128) collate Cyrillic_General_CI_AS,  
    ClientDriverName         varchar(64)  collate Cyrillic_General_CI_AS,  
    ClientTransportGosNumber varchar(32)  collate Cyrillic_General_CI_AS  
  )  
*/  
  -- Загрузим все СП, которые лежат в отгрузке.  
  --insert into #Temp  
  select  
    1 as forCount,  
  
--    T.ID as idTask,  
  case when @bUnionRejectWithFather = 0  
      then T.ID   
      else isnull(P_F.idTask, T.ID)  
    end as idTask,  
  
    P.ID as idProject,  
    IsNull(T.idTaskType, 1) as idTaskType,  
    IsNull(TT.Name, '')     as TaskTypeName,  
    T.Num as TaskNum,  
    T.AccountNum,  
    T.Date as TaskDate,  
    IsNull(T.ClientNum, '') as ClientNum,  
    case when Len(IsNull(TR.Num, '')) > 0  
         then IsNull(TR.Num, '')  
         else IsNull(T.NumCalcFact, '')  
         end as NumCalcFact,  
    case when Len(IsNull(TR.Num_Dealer, '')) > 0  
         then IsNull(TR.Num_Dealer, '')  
         else IsNull(T.NumCalcFact_Dealer, '')  
         end as NumCalcFact_Dealer,  
    SH.Date as DateComplete,  
    IsNull(T.Komission, '') as Komission,  
    T.DatePayDoc,  
    IsNull(C.Name, '') as ClientName,  
    IsNull(C.NameFull, '') as ClientNameFull,  
    IsNull(C.Adress, '') as ClientAdress,  
    IsNull(C.AdressSubDiv, '') as AdressSubDiv,  
    IsNull(C.Tel, '') as ClientTel,  
    IsNull(C.OKPO, '') as ClientOKPO,  
    IsNull(C.UNN, '') as ClientUNN,  
    IsNull(C.KPP, '') as ClientKPP,  
    IsNull(CB_C.KS, '') as ClientKS,  
    IsNull(BK_C.BIC, '') as ClientBIC,  
    IsNull(C.OKOHX, '') as ClientOKOHX,  
    IsNull(CB_C.RS, '') as ClientRS,  
    IsNull(BK_C.Name, '') as ClientBank,  
    C.GUID                as guidClient,  
    IsNull(S.Name, '') as SellerName,  
    IsNull(S.NameFull, '') as SellerNameFull,  
    IsNull(S.AlternativeName, '') as SellerAlternativeName,  
    IsNull(S.UNN, '') as SellerUNN,  
    IsNull(S.Adress, '') as SellerAdress,  
    IsNull(CB_S.RS, '') as SellerRS,  
    IsNull(BK_S.Name, '') as SellerBank,  
    IsNull(S.Tel, '') as SellerTel,  
    IsNull(S.Fax, '') as SellerFax,  
    IsNull(S.eMail, '') as SellerEMail,  
    IsNull(S.Site, '')  as SellerSite,  
    IsNull(S.OKOHX, '') as SellerOKOHX,  
    IsNull(S.OKPO, '') as SellerOKPO,  
    IsNull(S.OGRN, '') as SellerOGRN,  
    IsNull(CB_S.KS, '') as SellerKS,  
    IsNull(BK_S.BIC, '') as SellerBIC,  
    IsNull(S.KPP, '') as SellerKPP,  
    case when IsNull(DSD.bSignatureFromUser, 0) = 1  
         then U.ManagerName  
         else S.AccountantName  
         end as SellerAccountantName,  
    case when IsNull(DSD.bSignatureFromUser, 0) = 1  
         then U.ManagerName  
         else S.ChiefName  
         end as SellerChiefName,  
    S.CertificateNDS as SellerCertificateNDS,  
    Shipper.Name               as ShipperName,  
    Shipper.ChiefName          as ShipperChiefName,  
    Shipper.Adress             as ShipperAdress,  
  
    -- Тех.компания  
    IsNull(TC.Name,            '')  as TechCompanyName,  
    IsNull(TC.NameFull,        '')  as TechCompanyNameFull,  
    IsNull(TC.AlternativeName, '')  as TechCompanyAlternativeName,  
    IsNull(TC.UNN,             '')  as TechCompanyUNN,  
    IsNull(TC.City,            '')  as TechCompanyCity,  
    IsNull(TC.Adress,          '')  as TechCompanyAdress,  
    IsNull(TC.AdressSubDiv,    '')  as TechCompanyAdressSubDiv,  
  ( select top 1 IsNull(RS, '') from ClientBank where bDef = 1 and TC.ID = idClient )   
                                    as TechCompanyRS,  
  ( select top 1 IsNull(Name, '')   
    from Bank   
      inner join ClientBank on Bank.ID = ClientBank.idBank  
    where bDef = 1 and TC.ID = idClient   
  )                                 as TechCompanyBank,  
    IsNull(TC.Tel,             '')  as TechCompanyTel,  
    IsNull(TC.Fax,             '')  as TechCompanyFax,  
    IsNull(TC.eMail,           '')  as TechCompanyEMail,  
    IsNull(TC.Site,            '')  as TechCompanySite,  
    IsNull(TC.OKOHX,           '')  as TechCompanyOKOHX,  
    IsNull(TC.OKPO,            '')  as TechCompanyOKPO,  
    IsNull(TC.OGRN,            '')  as TechCompanyOGRN,  
  ( select top 1 IsNull(KS, '') from ClientBank where bDef = 1 and TC.ID = idClient )   
                                    as TechCompanyKS,  
  ( select top 1 IsNull(BIC, '')   
    from Bank   
      inner join ClientBank on Bank.ID = ClientBank.idBank  
    where bDef = 1 and TC.ID = idClient   
  )                                 as TechCompanyBIC,  
    IsNull(TC.KPP,             '')  as TechCompanyKPP,  
  
    S.ShiperName,  
    IsNull(S.AdressSubDiv, '') as SubDivisionAddress,  
    IsNull(CSG.NameFull, CSG.Name) as ConsigneeNameFull,  
    --IsNull(CSG.Adress, C.Adress)   as ConsigneeAdress,  
    case  
      when isNull(T.AddressDelivery, '') != '' then T.AddressDelivery  
      when isNull(DA.Name, '')           != '' then DA.Name  
      when isNull(CSG.AdressSubDiv, '')  != '' then CSG.AdressSubDiv  
      when isNull(CSG.Adress, ''  )      != '' then CSG.Adress  
      else null  
    end as ConsigneeAdress,  
    IsNull(CSG.UNN, '')            as ConsigneeUNN,  
    cast(P.PriceS as decimal(13, 2)) as PriceS,                                        -- цена за шпросы  
  
    cast(  
  case when @bUnionRejectWithFather = 0  
   then   
      case when IsNull(P.IsPriceByCount, 0) = 1 and CPU.d_iNum = 0 and ISNULL(P.Area, 0) > 0 then P.PriceNDS/P.Area   else P.PriceNDS   end  
   else   
      case when IsNull(isnull(P_F.IsPriceByCount, P.IsPriceByCount), 0) = 1 and CPU.d_iNum = 0 and ISNULL(isnull(P_F.Area, P.Area), 0) > 0   
       then isnull(P_F.PriceNDS, P.PriceNDS)/isnull(P_F.Area, P.Area)   
       else isnull(P_F.PriceNDS, P.PriceNDS)     
     end  
    end as decimal(13, 2)) as PriceM2WithNDS,    
  
    cast(  
  case when @bUnionRejectWithFather = 0  
   then   
      case when IsNull(P.IsPriceByCount, 0) = 1 and CPU.d_iNum = 0 and ISNULL(P.Area, 0) > 0 then P.PriceNoNDS/P.Area else P.PriceNoNDS end   
   else   
      case when IsNull(isnull(P_F.IsPriceByCount, P.IsPriceByCount), 0) = 1 and CPU.d_iNum = 0 and ISNULL(isnull(P_F.Area, P.Area), 0) > 0   
        then  isnull(P_F.PriceNoNDS, P.PriceNoNDS)/isnull(P_F.Area, P.Area)  
         else isnull(P_F.PriceNoNDS, P.PriceNoNDS)  
    end   
  end as decimal(13, 2)) as PriceOfUnit,   -- В 11 графе выводим цену за М2 без НДС.  
  
    cast(  
  case when @bUnionRejectWithFather = 0  
   then   
      sum(P.SumWithNDS / case IsNull(P.nCount, 0) when 0 then 1 else P.nCount end)   
   else  
      sum(isnull(P_F.SumWithNDS, P.SumWithNDS) / case IsNull( isnull(P_F.nCount, P.nCount), 0) when 0 then 1 else isnull(P_F.nCount, P.nCount) end)   
  end as decimal(13, 2)) as PriceWithNDS,  
  
    cast(  
  case when @bUnionRejectWithFather = 0  
   then   
    sum(P.SumNDS /     case IsNull(P.nCount, 0) when 0 then 1 else P.nCount end)  
   else  
    sum(isnull(P_F.SumNDS, P.SumNDS) / case isnull(isnull(P_F.nCount, P.nCount) , 0) when 0 then 1 else isnull(P_F.nCount, P.nCount) end)  
  end as decimal(13, 2)) as NDS,  
  
    cast(  
  case when @bUnionRejectWithFather = 0  
   then    
    sum(P.SumNoNDS /   case IsNull(P.nCount, 0) when 0 then 1 else P.nCount end)   
   else  
    sum(isnull(P_F.SumNoNDS, P.SumNoNDS) / case IsNull(isnull(P_F.nCount, P.nCount), 0) when 0 then 1 else  isnull(P_F.nCount, P.nCount) end)   
  end as decimal(13, 2)) as SumNoNDS,  
  
    cast(  
  case when @bUnionRejectWithFather = 0  
   then   
        case when IsNull(P.IsPriceByCount, 0) = 1 and CPU.d_iNum = 0 and ISNULL(P.Area, 0) > 0 then P.PriceNDS/P.Area   else P.PriceNDS   end   
   else  
     case when IsNull(isnull(P_F.IsPriceByCount, P.IsPriceByCount), 0) = 1 and CPU.d_iNum = 0 and ISNULL(isnull(P_F.Area, P.Area), 0) > 0   
      then isnull(P_F.PriceNDS, P.PriceNDS) / isnull(P_F.Area, P.Area)  
      else isnull(P_F.PriceNDS, P.PriceNDS)     
     end   
  end  
  as decimal(13, 2)) as PriceNDS,  
  
  
    IsNull(P.bShpros, 0) as bShpros,  
    sum(P.Mass) as Mass,  
    sum(P.Mass)          as MassSum, -- Для совместимости с w_Transport.  
    P.Num,  
    case when IsNull(P.IsPriceByCount, 0) = 1 and CPU.d_iNum = 1  
         then 'шт'  
         else 'м2' -- TODO: Взять из БД  
    end as Unit,  
    NDS.NDS as Tax_rate,  
    count(*) as nCount,  
    count(*) as nCountPos, -- Для совместимости с w_Transport.  
    case when IsNull(P.IsPriceByCount, 0) = 1 and CPU.d_iNum = 1  
         then count(*)  
         else sum(P.Area)  
    end as nCountArea,  
    sum(P.Area) as nCountAreaOriginal,  
    case  
      when PD.Type != 1  
       then PD.Name + P.GPName  
      else dbo.f_GetSpecialProductName(IsNull(CF.d_iNum, 0), PD.Name,   
                                       dbo.f_SupressCoveringMark(P.GPName, CCover.d_iNum),  
                                       P.CamCount, P.Thickness, P.Width, P.Height,  
                                       IsNull(P.ComplexText, ''), IsNull(P.CommentClient, ''), P.ID, P.Num, 0)  
    end as GPName,  
    dbo.f_GetGPFormulaByMark(P.ID, 0, '+', '-') as GPNameMark,  
    left(dbo.f_RUS_To_Eng(P.GPName), 128)  as GPNameEng,  
    P.GPName                               as GPName_FromFieldProject,  
    dbo.f_GetSpecialProductName(IsNull(CF.d_iNum, 0), PD.Name,   
                                       dbo.f_SupressCoveringMark(P.GPName, CCover.d_iNum),  
                                       P.CamCount, P.Thickness, P.Width, P.Height,  
                                       IsNull(P.ComplexText, ''), IsNull(P.CommentClient, ''), P.ID, P.Num, 0) as GPName_Common,  
    cast(P.Width  as varchar(8)) as Width,  
    cast(P.Height as varchar(8)) as Height,  
    P.Area as Area,  
    IsNull(P.IsPriceByCount, 0) as IsPriceByCount,  
    case P.CamCount  
         when 1 then 'СПО'  
         when 2 then 'СПД'  
         else ''  
    end as CamCountStr,  
    case P.CamCount  
         when 1 then 'Стеклопакет однокамерный'  
         when 2 then 'Стеклопакет двухкамерный'  
         else 'Стекло в нарезку'  
    end                                        as Name,       -- Для совместимости с w_Invoice_4  
    P.PriceByM                                 as Pricekvm,   -- В отчете "Счет-договор" (Task_Agreement_Common_2.rpt) используется поле Pricekvm (цена за 1 кв. м.), которого нет. Добавляю.  
    cast(IsNull(P.Thickness, 0) as varchar(3)) as Thickness,  
    IsNull(P.Commentary, '') as Commentary,  
    PD.Name as ProductName,  
    IsNull(DSD.Name, '') as DepotName,  
    IsNull(DSD.Tel, '') as DepotSubDivisionTel,  
    IsNull(DSD.ManagerName,      '') as ManagerName,            -- Для обратной совместимости (напр. с v_InvoiceGroupByGPName)  
    IsNull(DSD.ManagerName, '') as SubDivisionManagerName,  
    IsNull(DSD.AddTo_NumInvoice, '') as AddTo_NumInvoice,  
    IsNull(USA.OrderToSign, USign.OrderToSign) as OrderToSign,  
    0 as CurSaldo,  
    0 as sign_CurSaldo,  
    case P.CamCount  
      when 0 then '00000000037'  -- STIS 2015-09-10   
      when 1 then '00000000038'  
      when 2 then '00000000039'  
      else        ''  
    end as Code,  
    IsNull(U.ManagerName, '') as Manager,  
    IsNull(U.Tel, '') as Tel,  
    P.CamCount,  
    case when P.CamCount > 0 then 0  -- СП  
    else   
      case when IsNull(P.Complex, 0) & 524288 = 524288 then 4 -- Триплекс собственного производства  
           when IsNull(P.Complex, 0) & 1024   = 1024   then 1 -- Триплекс  
           when IsNull(P.Complex, 0) & 8192   = 8192   then 2 -- Эмалит  
           when IsNull(P.Complex, 0) & 16     = 16     then 3 -- Закалка  
           else -1                                            -- Нарезка без обработки и услуги  
      end  
    end as TypeOper,  -- Для определения типа в паспорте качества(расширенный) в Остек    
    TR.idShip,  
    IsNull(DSD.HeaderTN, '') as HeaderTN,  
    IsNull(U.Post, '') as Signature_ShiperPost,  
    PD.Type              as ProductType,  
    IsNull(PDP.Type, 0)  as ProductType_Project,  
    PDP.GUID             as guidProduct_Project,  
    IsNull(CC.Name, '')         as ContractName,  
    IsNull(CC.ContractNum, '')  as ContractNum,  
    IsNull(CC.Date, '')         as ContractDate,  
    IsNull(P.CommentClient, '') as CommentClient,  
    TR.Num as NumInvoice,  
    TR.TransportDate,  
    TR.ID                        as idTransport,  
    TR.GUID                      as guidTransport,  
    dbo.f_GetGPRasInfo(P.ID, P.bShpros) as RasInfoText,  
    (select sum(LengReal) from RasShrink where idProject = P.ID) as RasLength,  
    case   
      when IsNull(P.IsPriceByCount, 0) = 1 and CPU.d_iNum = 1 or P.Area = 0  
      then UCount.Code_OKEI  
      else UArea.Code_OKEI  
    end as Unit_Code_OKEI,  
    cast(P.PriceNoNDS as decimal(13, 2)) as PriceNoNDS,  
    IsNull(T.Commentary, '') as TaskCommentary,  
    IsNull(FT.Name, '') as FormTypeName,  
    FT.ID               as idFormType,  
    case when IsNull(P.bPlot, 0) = 0  
      then ''  
      else 'Черт'  
    end as NamePlot,  
    case when IsNull(P.bTemplate, 0) = 0  
      then ''  
      else 'Шабл'  
    end as NameTemplate,  
    case   
      when S.idPersonnel_Shipper is not null  
      then (select Name from Personnel where ID = S.idPersonnel_Shipper)  
      else IsNull(S.ShiperName, '')  
    end as SellerShiperName,  
  
    case   
      when S.idPersonnel_Shipper is not null  
      then (select top 1 PersonnelPost.Name   
            from PersonnelPost  
            inner join Personnel on Personnel.idPersonnelPost = PersonnelPost.ID  
           where Personnel.ID = S.idPersonnel_Shipper)  
      else 'Ответственный за погрузку'  
    end as SellerShiperPost,  
    IsNull(SD.Name, '') as DriverFromShip,  
    P.BarCodeClient,  
  
    TripTransport.Driver                    as CarDriver,  
    TripTransport.Name                      as CarName,  
    TripTransport.GosNumber                 as CarGosNumber,  
    TripTransport.License                   as CarLicense,  
    IsNull(SL.Name, '')                     as LogistFromShip,  
    IsNull(SH.ClientDriverName, '')         as ClientDriverName,  
    IsNull(SH.ClientTransportGosNumber, '') as ClientTransportGosNumber,  
    IsNull(PO.BarCode, '')                  as PyramidName  
  -- вставим во временную таблицу  
  into #Temp  
  from Transport TR                                             -- Заголовок части или целовго заказа в отгрузке  
    inner join Ship SH               on SH.ID    = TR.idShip     -- Заголовок отгрузки  
    inner join BarCode B             on B.idTransport = TR.ID  
    inner join Project P             on P.ID     = B.idProject  
    inner join Task T                on T.ID     = P.idTask  
    inner join Product PD            on PD.ID    = P.idProd  
    left  join Product PDP           on PDP.ID   = P.idProduct  
    left  join TaskType TT           on TT.ID    = T.idTaskType  
    left  join FormType FT           on FT.ID    = P.idFormType  
    left  join NDS on NDS.ID   = T.idNDS  
    left  join DeliveryAddress DA    on DA.ID    = T.idDeliveryAddress  
    left  join ClientContract CC     on CC.ID    = T.idClientContract  
    left  join Client C              on C.ID     = T.idClient  
    left  join Client S              on S.ID     = T.idSeller  
    left  join Client TC             on TC.nClientSubType = 1  
    left  join Client Shipper        on Shipper.ID = T.idShipper  
    left  join Client CSG            on CSG.ID   = T.idConsignee  
    left  join ClientBank CB_C       on CB_C.ID  = T.idClientBank_Client  
    left  join Bank BK_C             on BK_C.ID  = CB_C.idBank  
    left  join ClientBank CB_S       on CB_S.ID  = T.idClientBank_Seller  
    left  join Bank BK_S             on BK_S.ID  = CB_S.idBank  
    left  join DepotSubDivision DSD  on DSD.ID   = T.idDepotSubDivision  
    left  join Users U               on U.ID     = TR.idUsers  
    left  join Users USign           on lower(USign.Name) = lower(SYSTEM_USER)    -- Пользователь, который подписывает  
    left  join UsersSignAutority USA on USign.GUID = USA.guidUsers and  
                                        SH.Date   >= USA.DateBegin and  
                                        SH.Date   <= USA.DateEnd  
    left  join Config CF             on CF.Name  = 'FormatTypeOfGPName'  
    --left  join Config CF1           on CF1.Name = 'bCalcFactNumUniqueForShipedTask'  
    left  join Config CPU            on CPU.Name = 'bPriceUnitInCalcFact'  
    left  join Config CCover         on CCover.Name  = 'nGlassMarkCovering'   -- тип маркировки покрытия  
    left  join (select top 1 * from Unit where nTypeUnit = 1) UArea  on UArea.nTypeUnit  = 1  
    left  join (select top 1 * from Unit where nTypeUnit = 2) UCount on UCount.nTypeUnit = 2  
    left  join TripTransport         on TripTransport.ID = SH.idTripTransport  
    left  join Personnel SD          on SD.ID            = SH.idDriver  
    left  join Personnel SL          on SL.ID            = SH.idPersonnel  
  
  left join BarCode B_F on B_F.ID = B.idBarCode_Reject_Father  
    left join Project P_F on P_F.ID = B_F.idProject  
    left join PyramidCompleted PC on PC.ID = B.idPyramidCompleted  
    left join PyramidOut       PO on PO.ID = PC.idPyramidOut  
  
  where  
    TR.idShip = @idShip  
    and PD.Type = case when @bOtherProduction = 1 then PD.Type else 1       end  
    and T.ID    = case when @idTask           = 0 then T.ID    else @idTask end  
  group by  
--    T.ID,  
    case when @bUnionRejectWithFather = 0  
      then T.ID   
      else isnull(P_F.idTask, T.ID )  
    end,  
  
    Shipper.Adress,  
    IsNull(T.idTaskType, 1),  
    IsNull(TT.Name, ''),  
    T.Num,  
    T.AccountNum,  
    T.Date,  
    case when Len(IsNull(TR.Num, '')) > 0  
         then IsNull(TR.Num, '')  
         else IsNull(T.NumCalcFact, '')  
         end,  
    case when Len(IsNull(TR.Num_Dealer, '')) > 0  
         then IsNull(TR.Num_Dealer, '')  
         else IsNull(T.NumCalcFact_Dealer, '')  
         end,  
    IsNull(T.ClientNum, ''),  
    --T.DateComplite,  
    SH.Date,  
    IsNull(T.Komission, ''),  
    T.DatePayDoc,  
    IsNull(C.Name, ''),  
    IsNull(C.NameFull, ''),  
    IsNull(C.Adress, ''),  
    IsNull(C.AdressSubDiv, ''),  
    IsNull(C.Tel, ''),  
    IsNull(C.OKPO, ''),  
    IsNull(C.UNN, ''),  
    IsNull(C.KPP, ''),  
    IsNull(CB_C.KS, ''),  
    IsNull(BK_C.BIC, ''),  
    IsNull(C.OKOHX, ''),  
    T.AddressDelivery,  
    DA.Name,  
    CSG.AdressSubDiv,  
    CSG.Adress,  
    IsNull(CB_C.RS, ''),  
    IsNull(BK_C.Name, ''),  
    C.GUID,  
    IsNull(S.Name, ''),  
    IsNull(S.NameFull, ''),  
    IsNull(S.AlternativeName, ''),  
    IsNull(S.UNN, ''),  
    IsNull(S.Adress, ''),  
    IsNull(CB_S.RS, ''),  
    IsNull(BK_S.Name, ''),  
    IsNull(S.Tel, ''),  
    IsNull(S.Fax, ''),  
    IsNull(S.eMail, ''),  
    IsNull(S.Site, ''),  
    IsNull(S.OKOHX, ''),  
    IsNull(S.OKPO, ''),  
    IsNull(S.OGRN, ''),  
    IsNull(CB_S.KS, ''),  
    IsNull(BK_S.BIC, ''),  
    IsNull(S.KPP, ''),  
    S.CertificateNDS,  
    Shipper.Name,  
    Shipper.ChiefName,  
    TC.ID,  
    TC.Name,             
    TC.NameFull,         
    TC.AlternativeName,  
    TC.UNN,              
    TC.City,             
    TC.Adress,           
    TC.AdressSubDiv,     
    TC.Tel,              
    TC.Fax,              
    TC.eMail,            
    TC.Site,             
    TC.OKOHX,            
    TC.OKPO,             
    TC.OGRN,             
    TC.KPP,              
    case when IsNull(DSD.bSignatureFromUser, 0) = 1  
         then U.ManagerName  
         else S.AccountantName  
         end,  
    case when IsNull(DSD.bSignatureFromUser, 0) = 1  
         then U.ManagerName  
         else S.ChiefName  
         end,  
    S.ShiperName,  
    IsNull(S.AdressSubDiv, ''),  
    IsNull(CSG.NameFull, CSG.Name),  
    IsNull(CSG.Adress, C.Adress),  
    IsNull(CSG.UNN, ''),  
    P.Num,  
    P.ID,  
    P.PriceS,  
    P.PriceNDS,  
    P.bShpros,  
    P.PriceNoNDS,  
    P.GPName,  
    case when PD.Type != 1  
       then PD.Name + P.GPName  
       else left(dbo.f_GetSpecialProductName(IsNull(CF.d_iNum, 0), PD.Name, P.GPName, P.CamCount, P.Thickness, P.Width, P.Height, IsNull(P.ComplexText, ''), IsNull(P.CommentClient, ''), P.ID, P.Num, 0), 128)  
       end,  
    cast(P.Width  as varchar(8)),  
    cast(P.Height as varchar(8)),  
    P.Area,  
  
    IsNull(P.IsPriceByCount, 0),  
  isnull(isnull(P_F.IsPriceByCount, P.IsPriceByCount), 0) ,  
   isnull(P_F.Area, P.Area) ,  
   isnull(P_F.PriceNDS, P.PriceNDS),  
  isnull(P_F.PriceNoNDS, P.PriceNoNDS),  
  
    P.PriceByM,  
    cast(IsNull(P.Thickness, 0) as varchar(3)),  
    IsNull(P.Commentary, ''),  
    PD.Name,  
    IsNull(DSD.Name, ''),  
    IsNull(DSD.Tel, ''),  
    IsNull(DSD.ManagerName, ''),  
    IsNull(DSD.AddTo_NumInvoice, ''),  
    IsNull(USA.OrderToSign, USign.OrderToSign),  
    IsNull(U.ManagerName, ''),  
    IsNull(U.Tel, ''),  
    P.CamCount,  
    P.Thickness,  
    P.Width,  
    P.Height,  
    TR.idShip,  
    C.ID,  
    left(dbo.f_GetSpecialProductName(IsNull(CF.d_iNum, 0), PD.Name, P.GPName, P.CamCount, P.Thickness, P.Width, P.Height, IsNull(P.ComplexText, ''), IsNull(P.CommentClient, ''), P.ID, P.Num, 0), 128),  
    IsNull(DSD.HeaderTN, ''),  
    IsNull(U.Post, ''),  
    PD.Type,  
    IsNull(PDP.Type, 0),  
    PDP.GUID,  
    IsNull(CC.Name, ''),  
    IsNull(CC.ContractNum, ''),  
    CC.Date,  
    NDS.NDS,  
    CF.d_iNum,  
    CPU.d_iNum,  
    CCover.d_iNum,  
    IsNull(P.CommentClient, ''),  
    P.ComplexText,  
    P.Complex,  
    TR.Num,  
    TR.TransportDate,  
    TR.ID,  
    TR.GUID,  
    UCount.Code_OKEI,  
    UArea.Code_OKEI,  
    IsNull(T.Commentary, ''),  
    IsNull(FT.Name, ''),  
    FT.ID,  
    case when IsNull(P.bPlot, 0) = 0  
      then ''  
      else 'Черт'  
    end,  
    case when IsNull(P.bTemplate, 0) = 0  
      then ''  
      else 'Шабл'  
    end,  
    S.idPersonnel_Shipper,  
    IsNull(SD.Name, ''),  
    P.BarCodeClient,  
  
    TripTransport.Driver,  
    TripTransport.Name,  
    TripTransport.GosNumber,  
    TripTransport.License,  
    IsNull(SL.Name, ''),  
    IsNull(SH.ClientDriverName, ''),  
    IsNull(SH.ClientTransportGosNumber, ''),  
    PO.BarCode  
  
  
  create table #TaskProp  
  (  
    idTask         int,  
    TaskPrice      decimal(18, 2),                             -- Сумма по отгрузке  
    MassPhraseTonn varchar(64) collate Cyrillic_General_CI_AS,  -- [SB] От этого поля можно отказаться, если отчеты перевести в стимул. TTN_1T.mrt обходится без него.  
    MassPhraseKg   varchar(64) collate Cyrillic_General_CI_AS   -- [SB] От этого поля можно отказаться, если отчеты перевести в стимул. TTN_1T.mrt обходится без него.  
  )    
  
  insert into #TaskProp  
  select  
    idTask,  
    sum(round(PriceWithNDS, 2)) as TaskPrice,  
    dbo.MassPhrase(round(sum(Mass),        0), 0) as MassPhraseTonn,  
    dbo.MassPhrase(round(sum(Mass * 1000), 0), 1) as MassPhraseKg  
  from #Temp  
  group by  
    idTask  
      
  -- STIS 2015-09-04 добавлены из таблицы транспорт num,DeliveryNum номера Расходной накладной и счёта фактуры  
select  
    T.*,  
    TP.TaskPrice,  
    TP.MassPhraseKg,  
    TP.MassPhraseTonn,  
    TR.Num         as RashodnayaNakladnaya,  
    TR.DeliveryNum as SchetFaktura,  
    dbo.f_GetShipPyrNameListByPos(T.idProject) as PyrNameList  
  from #Temp T  
    inner join #TaskProp TP   on TP.idTask = T.idTask  
    left  join Transport TR    on TR.idTask = T.idTask  --[SE] Если заказ в нескольких отгрузках, то множатся позиции  
  where  
    IsNull(TR.idShip, @idShip) = @idShip             --[ab]->[se] Может быть так надо было ошибку исправить?  
  
  drop table #TaskProp  
  drop table #Temp  
  
  set nocount off  
end  