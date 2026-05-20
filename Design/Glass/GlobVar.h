#pragma once

#include "..\OGD\CuttingType.h"
#include "..\Model\MType.h"

struct SelArrItem
{
  long  idProject,
        idTask;
};

// Типы экспорта отчётов
enum eReportExportType
{
  e_ret_None = 0,
  e_ret_PDF  = 1,
  e_ret_XLS  = 2,
  e_ret_DOC  = 3
};

enum EPiramidType    // Piramid.nType
{
  e_pt_arfa = 0,     // Арфа-пирамида
  e_pt_a    = 1      // А-пирамида
};

// Раскладка по пирамидам: 
enum EGPSOptExportGlassPyrType
{
  e_GlassPyr_None                        = -1,    // Никакой раскладки 
  e_GlassPyr_A_Type                      =  0,    // А-образные
  e_GlassPyr_Arfa_Type                   =  1,    // Арфа-образные
  e_GlassPyr_Arfa_Type_DifGlassInDifPyr  =  2,    // Арфы, разные стёкла в разные ячейки пирамиды (Арфа-образным, где разные стекла в разных пирамидах,)
  e_GlassPyr_Arfa_Type_DifGlassInOnePyr  =  3,    // Арфа-образным, где разные стекла в одной пирамиде.
  e_GlassPyr_A_and_Arfa                  =  4,
  e_GlassPyr_A_and_ArfaPack              =  5,
  e_GlassPyr_A_and_Arfa_Spacing          =  6,
  e_GlassPyr_Arfa_Type_Assembly          =  7,    // Раскладка в сборочные арфы
  e_GlassPyr_A_Diff_Pack                 =  8,    // А-пирамиды с разными размерами в пачке в порядке обработки
  e_GlassPyr_A_H_Limit                   =  9,    // А-пирамиды разной высоты    
  e_GlassPyr_A_zak_Arfa_other            = 10,    // А-пирамиды по закалке, остальное в арфы согласно порядку А-пирамид   
  e_GlassPyr_A_main_Arfa_other           = 11,    // Гл.раскрой по А-пир, ответные в Арфы.
};

enum ESawTaskOrderType
{
  e_sort_None                   = -1, // никакой сортироки
  e_sort_Dim_NoGroup            = 0,  //"по Размеру без группировки"                 // 0
  e_sort_TaskPos                = 1,  //"по Заказу, Позиции"                         // 1
  e_sort_Task_Cam_Dim           = 2,  //"по Заказу, кол.Камер, Размеру"              // 2
  e_sort_Task_Dim_Pos           = 3,  //"по Заказу, Размеру, Позиции"                // 3
  e_sort_Client_Cam_Dim         = 4,  //"по Клиенту, кол.Камер, Размеру"             // 4
  e_sort_Dim_Group              = 5,  //"по Размеру с группировкой"                  // 5
  e_sort_Client_Task_Cam_Dim    = 6,  //"по Клиенту, Заказу, Кол-ву камер, Размеру"  // 6
  e_sort_UserScript             = 7,  //"Пользовательская (настраивается в скрипте)" // 7
  e_sort_UserPlugin             = 8,  //"Пользовательская (плагин       )            // 8
  e_sort_NR_Client_Cam_Task_Dim = 9,  // Клиент, камерность, задание, размер, нарезка в конце // 9
  e_sort_CustomSQL              = 10, // Задаётся SQL 
  e_sort_Dim_NoGroup_Reverse    = 11, // "по Размеру без группировки - обратная"      // 11
};

enum EGlassMarkCovering               // Тип маркировки покрытия
{
  e_GlassMarkCovering_None      = 0,  // Без маркировки
  e_GlassMarkCovering_OneSymbol = 1,  // Маркировка: #4и или 4и#
  e_GlassMarkCovering_SharpNum  = 2,  // Маркировка номера покрытия: 4и#1-16-.../ 4и#2-16-...
  e_GlassMarkCovering_Manual    = 3   // Ручная, # задаётся в колонке # SideOper_Manual
};

// Тип расчета сортировки штрихкодов.
enum ETypeCalcBarcodeOrder
{
  e_TypeCalcBarcodeOrder_None                 = 0, // 0 - не использовать
  e_TypeCalcBarcodeOrder_VStisLineReportShort = 1  // 1 - по отчету Стиса
};

// Тип выбора клиента новому заданию
enum ENewTaskClientSelectPolicy
{
  e_ntcs_None        = 0, // обычно как и было в программе, т.е. ID = 0
  e_ntcs_Default     = 1, // выбрать первого по ID у которого Client.bDef <> 0
  e_ntcs_Ask         = 2, // Запросить путём показа диалога - на отмену поставит клиента по умолчанию, если есть
  e_ntcs_AskForcibly = 3  // Запросить путём показа диалога с обязательным выбором, т.е. отмену нажать нельзя
};

/*
enum e_TypeCalcGabarit
{
  e_TCG_Default        = 0,   // По умолчанию, как и раньше было (по габариту рамки)
  e_TCG_ByOuterMargin  = 1    // по габариту зуба или выступа  
};
*/

enum ETypeDefineAreaGP                     // Тип определения площади
{
  e_TypeDefineAreaGP_EquallyMaxValue = 0,  // "[Значение Min] <  [Площадь СП] <= [Значение Max]"
  e_TypeDefineAreaGP_EquallyMinValue = 1   //  [Значение Min] <= [Площадь СП] <  [Значение Max]
};

enum ETypeRejectDialog              // Тип отображения диалога выбора комбинации браков
{
  e_TypeRejectDialog_Tree     = 0,  // В виде дерева
  e_TypeRejectDialog_ComboBox = 1   // в виде  нескольких комбобоксов
};

// Тип резервирования ячеек в пирамидах: 
enum ESawTypeRezervSpace
{
  e_SawTypeRezervSpace_None       = 0,    // Не резервировать
  e_SawTypeRezervSpace_Operation  = 1,    // Только для стекол с обработками
  e_SawTypeRezervSpace_All        = 2     // Для стекол, которые находятся в других раскроях
};

// Тип чертежа сохраняемого в таблице Plot: 
enum ETypePlot
{
  e_TypePlot_Unknown     = 0,   // Не определен
  e_TypePlot_FrameAndRas = 1,   // Рамка и шпрос 
  e_TypePlot_JPGImport   = 2    // импортирован из jpeg
};

enum EMarginCoef_Show               // Как отображать MarginCoef "Маржа" пользователю:
{
  e_MarginCoef_Show_Persent = 0,    // Хранить коэффициент от 1, отображать в пересчёте проценты
  e_MarginCoef_Show_Value   = 1     // Хранить как число процент и отображать то же число
};

enum EPlotShprosMode
{
  e_spm_ordinary = 0,               // Обычный режим лесенкой          
  e_spm_onelewel = 1                // В один уровень по средним линиям     
};

enum EPlotAreaMethod
{
  e_rectangle = 0,    // Описанный прямоугольник
  e_integral  = 1     // Фактическая площадь непрямоугольника расчёт по интегралу
};

enum EPathAttachmetsMethod
{
  e_db   = 0,         // Сохранение вложенных файлов в базу
  e_file = 1          // Сохранение вложенных файлов в файловую систему
};

enum ETypeMassCalc                   // Метод расчета массы изделия
{
  e_TypeMassCalc_ByGlass_Frame = 0,  // По массе стекол и рамок (по умолчанию)
  e_TypeMassCalc_ByMaterial    = 1   // По массе рассчитанных материалов
};

enum ETaskPriorityCalcType            // Тип расчёта приоритета заданий
{
  e_tpc_None                     = 0, // Не считать (как и было по умолчанию)
  e_tpc_NS_and_ClientManufactory = 1, // По сложности и приоритету цеха клиента  
};

// Ключи в реестре:
extern const TCHAR
// Ключи для св-в БД.
  cszDBDepotSubDivisionToSaw[],
  cszDBDepotSubDivisionShip [],
  cszDBCalcFact             [],
  cszDBNewTaskType          [],
  cszDBTaxRate              [],
  cszDBRoundArea            [],
  cszDBRoundArea_UnitPos    [],

  cszDBRoundArea_Sum        [],  // Скрытая!!! 1 - Округлять сумму площадей позиции или 0 - суммировать округленую площадь
  cszDBClearTrash_BeforeDel [],  // Скрытая!!! Очищать сбойные элементы при удалении из справочников

  // [AF] Больше не используется, считается по ёмкости арф.
  // Теперь ещё смотрим на лимит по стеклам.
  cszDBSawingGlassCount  [],  // Максимальное количество стёкол в раскрое.
  cszDBCuttingOffset    [],  // Смещение реза используется при экспорте Optima
  cszDBAutosawGlassCount [],  // Количество стёкол в автораскрое, минимальное.
  //cszDBSawingGPCount   [],  // Максимальное количество пакетов в раскрое.
  cszDBAccuracy          [],  // Точность при поиске резервного СП со склада косяков.
  cszDBTaskAccountNum    [],
  cszDBCalcFactEnumerate [],
  cszDBTaskNumEnumerate  [],  // Метод нумерации счетов
  cszDBManufTaskEnumerate[],
  cszDBAccountNumPrefix  [],
  cszDBCalcFactPrefix    [],
  cszDBTaskNumPrefix     [],
  cszDBManufTaskPrefix   [],
  cszDBCalcFactCurNum    [],
  cszDBTaskNumCurNum     [],
  cszDBManufTaskCurNum   [],
  cszDBiAccountNumCurNum [],
  cszDBGlassOrderType    [],
  cszDBGlassPyramidPut   [],
  cszDBCatalogForm       [],
  cszDBTaskTimeSet       [],
  cszDBCheckNum          [],
  cszDBCheckAccountNum   [],
  cszDBSawDateShift      [],
  cszDBShipDateShift     [],
  cszDBHour_ShipDate_NextDay[],
  cszDBFrameSizeDecrement   [],
  cszDBDistFrameGlass       [],
  cszDBType                 [],
  cszTaskStateForPayment    [],
  cszDBCrossPrice      [],
  cszDBMarginRas       [],
  cszDBMinWidthGP      [],
  cszDBMaxWidthGP      [],
  cszDBMinHeightGP     [],
  cszDBMaxHeightGP     [],
  cszDBMinAreaGP       [],
  cszDBMaxAreaGP       [],
  cszDBGroupPos        [],
  cszDBTaskPos         [],
  cszDBGPSOptExport    [],
  cszDBSaveCurve_XY    [],    // [ab] Как писать размеры на гибочник
  cszDBGlassInOneLine  [],
  cszDBShowReportSet   [],
  cszDBOptimExportFigure[],
  cszDBDelimiterEtiket  [],
  cszDBNarezkaMark      [],
  cszDBIdNumFormula     [],
  cszDBEtiket_BSV_WithoutSaw[],
  cszDBMaxAreaShipDate      [],
  cszDBReportDateBegin      [],
  cszDBReportDateEnd        [],
  cszDBSelClient_Keyboard   [],
  cszDBRefPrice_ShowClient  [],
  cszDBSawSort              [],
  cszDBSeparateSectorPyramid[],
  cszDBNoReverseNameGlass   [],
  cszDBAddNewClient         [],
  cszDBUseTaskToManuf       [],
  cszDBReplaceEmptyCommentary         [],
  cszDBTypeDisplaySide_TripPyramidPack[],
  cszDBCreateAutomaticAccountNum      [],
  cszDBSawNumbering                   [],
  cszDBAlwaysCloseTreeGrid   [],
  cszDBInsertNarezkaToArfa   [],
  cszDBInsertCoatingGlassToA [],
  cszDBUseFindReservedGP     [],
  cszDBReplaceFormulaGPImport[],
  cszDBUpdateDateGiveManufact[],
  cszDBUpdateDateComplete    [],
  cszDBShortDateComplete     [],
  cszDBShortDateCreate       [],
  cszDBResetAccountNumNewYear[],
  cszDBCalcFactNumUniqueForShipedTask[],
  cszDBRasPriceClientType            [],
  cszDBSetStateGPMakedAfterShip      [],
  cszDBCheckClientCredit    [],
  cszDBCheckSellerCredit    [],
  cszDBDateGiveManufactCheck[],
  cszDBPriceUnitInCalcFact  [],
  cszDBAutoSetShipedState   [],
  cszDBAddStateTaskApproval [],             // Вывод дополнительных статусов согласования заказа
  cszDBAddShipedStateFromBarCode     [],    // Проставлять статус "Отгружен" при добавлении заказа в отгрузку
                                            // Иначе только после подтверждения
  cszDBShowShippingInfoOnTaskSaw     [],    // Вывод информационного окна отгрузки при закрытии и сохранении заказа
  cszDBDoNotMakeTaskByClientOverdraft[],
  cszDBDoNotMakeTaskBySellerOverdraft[],
  cszDBRoundPriceM2NDS[],
  cszDBAllowBackManufDate[],
  cszDBAllowBackTaskDate[],
  cszDBSetDateGiveManufact[],
  cszDBSetDateComplite[],
  cszDBSetDatePayDoc[],
  cszDBArgon[],
  cszDBSeparateSawTeamAssembly[],
  cszDBSeparateSawPack[],
  cszDBAutoSetStatesForTaskManuf[],
  cszDBPriceGPRound[],
  cszDBDenyEditOldTasks[],
  cszDBPrintRepComplect_NoPack[],
  cszDBGlassWithOperOnArfaPyr[],
  cszDBExportOptimaDiffnGlass[],
  cszDBChangeBarCode_idSawTaskAssembly_ByLatest[],
  cszDBPrjSizeCopy[],
  cszDBPrjPriceCopy[],
  cszDBTaskNew_bSetDateGiveManufact[],
  cszDBbPermanentSaving[],
  cszDBbStandartProductionLibrary[],
  cszDBCurveTranslit[],
  cszDBSeparatePosNum[],
  cszDBSeparateNonStandard[],
  cszDBSeparatePos_ByGPName[],
  cszDBSeparatePos_WithOper[],
  cszDBSeparatePos_WithFilm[],
  cszDBSeparatePos_WithIDGlass[],
  cszComby_A_and_Arfa_A_Diff_Pack[],
  cszBalanceAPyramid             [],    // Балансировать стёкла на А-пирамидах?
  cszDBSeparate_ByGlassName[],
  cszDBCyclePyramid        [],
  cszDBStraightAFill       [],
  cszDBSolidFillOperPyramid[],

  cszDBMaxFillAPyramid     [],
  cszDBMaxFillArfaPyramid  [],

  cszDBDecreaseAPackSize   [],

  cszDBCorkPadGlassThikness[],                      // Толщина пробковой прокладки между стёклами на А-пирамидах в процессе обработки (не для отгрузки)

  cszDBExportAssemblyLineSetFrameFirstGlass[], // [SE] При экспорте на линию сборки первому стеклу назначать рамку
  cszDBExportAssemblyLineSwapSize          [], // [SE] Если "Ширина" < "Высоты", то поменять их местами на линии сборки
  cszDBSawLargerSizeIsHeight[],                     // [SE] При создании раскроев больший размер детали будет "Высотой"
  cszDBMaxAngle_SkipBeginPoint[],

  cszDBTypeExportOptyWay        [],
  cszDBCreateBarCode            [],
  cszDBCreateBarCodeGlass       [],
  cszDBBarCodeGPLeng            [],  // Длина штрих-кода СП
  cszDBCopyShprosWithoutAnswer  [],

  cszDBAutoSetCurveForTask      [],
  cszDBCeilSumWithNDS           [],
  cszDBOverrideSealingDepth     [],
  cszDBNumberDefTabInProjectView[],
  cszDBTypeCalcBarcodeOrder     [],
  cszDBCalcFactNumUniqueForTypeOrderTask[],
  cszDBCheckPlotConsistency     [],
  cszDBRotateHeglaHeaderOffcuts [],          // Вращать правую хеглу на левую
  cszDBGroupPyramidBySectorManufact[],
  cszDBRezervMaterialForTask    [],
  cszDBAddGlassToPackSawTask    [],
  cszDBNumberSmenaMarkAsLetter  [],
  cszDBFixBarCodeWrongPrefix    [], // Исправлять штрихкоды с неправильным размером и префиксом
  cszSupressMsgForNoArfa        [],
  cszCalcDebtOnTaskChange       [], // [OK] Скрытая!!! Рассчитывать задолженность клиента при изменении заказа.

  cszNoCalcPriceOperOnTaskChange[], // [OK] Скрытая!!! НЕ считать цену операций С++ при расчёте цены заказа.
  cszCalcArgonPriceOper           [], // [AO] Скрытая!!! Считать аргон как обработку

  cszCreateReservAfterOptim     [], // [OK] Скрытая!!! Создавать резервы после завершения оптимизаций
  cszDBShippingRejectProduct    [], // Отгружать бракованные изделие через модуль отгрузки
  cszDBAssignZeroPriceRemake    [], // Всегда присваивать нулевую цену переделанным по браку позициям
  cszAllowManualAddSpToPyramid  [], // [MR] разрешить добавлять изделия на пирамиды ручками (через контекстное меню)
  cszChangeSizeProjectAfterPlotChange[], //[YK] СКРЫТАЯ!! Менять размер изделия после редактирования чертежа. по умолчанию: 1-да
  cszDBCreateComExpReadyProd_LockShip[], //[YK] создать документы прихода и расхода готовой продукции после подтверждения отгрузки
  cszDBShowPartialManufAsNew[], //[ОК] Показывать частично произведённые заказы как новые
  cszDBCreateSawFromPackUnbrokenPyr  [], // Создавать раскрои из упаковки из целых пирамид (изделия с упак. пирамиды в одном раскрое)
  cszDBSawTypeRezervSpace   [], 
  cszDBFilmMark             [],

  // Ключи для св-в реестра.
  cszKeyImpConStr            [],
  cszKeyConStr               [],
  cszKeyConStrDBSource       [],
  cszKeyRestoreDBStr         [],
  cszKeySetupMSDE            [],
  cszKeyReplicaPath          [],
  cszKeyReportAccessPath     [],
  cszKeyJavaPath             [],
  cszKeyImagePath            [],
  cszKeyReportPath           [],
  cszKeyBarcodePath          [],
  cszKeyPyrEtiketShablonPath [],
  cszKeyGPEtiketShablonPath  [],
  cszKeyHeglaPath_Inbound    [],
  cszKeyHeglaPath_Outbound   [],
  cszKeyLisecInbound         [],
  cszKeyLisecOutbound        [],
  cszKeyOtdOutbound          [],
  cszKeyDateSinceOptioDat    [],  // Дата начала импорта Lisec optio.dat файлов
  cszKeyLisecTraioOptio      [],
  cszKeyPathSawTaskRegistration[],
  cszKeyPathTaskAttachments  [],
  cszKeyUserLogin            [],
  cszKeyReportPreview        [],
  cszKeyReportsetPreview     [],
  cszKeyWriteBarcodeDepot    [],
  cszKeyRequestExistPiramid  [],
  cszKeyPrintRepComplectOnGPSOpt[],
  cszKeyLiOptExport          [],
  cszKeyDBList               [],
  cszKeyServerList           [],
  cszKeyLastLogin            [],
  cszKeyLastDB               [],
  cszKeyLastServer           [],
  cszKeyWinProfile           [],
  cszKeyUserRemember         [],
  cszKeyLoginTemplate        [],
//#ifdef _DEBUG
  cszKeyLastPassword         [],    // [ab] Только для нас! Пользователям не ставить сохранение пароля!
//#endif
  cszKeyPort                 [],
  cszKeyCurPathSawFilesExport[],
  cszKeyCurPathSawDrawExport [],
  cszKeyCurPathTableSawPos   [],
  cszKeyCurPathTableSawTask  [],
  cszKeyAllowSettings        [],
  cszKeyAllowTaskList        [],
  cszKeyAllowTaskPlanList    [],
  cszKeyAllowObzList         [],
  cszKeyAllowTransport       [],
  cszKeyAllowSaw             [],
  cszKeyAllowClient          [],
  cszDBAllowPacking          [],
  cszDBAllowPlan             [],
  cszDBAllowOperPlan         [],
  cszDBAllowOptim            [],
  cszDBAllowOptim_APyramid     [],  // Разрешён алгоритм оптимизации по стопкам А-пирамид?
  cszDBAllowOptim_NoRect_Insert[],  // Разрешён алгоритм оптимизации вставки?
  cszDBAllowTransport        [],
  cszDBAllowGlassProcessing  [],    // Обработка стекла разрешена?
  cszDBShowError             [],    // [ab] Показывать пользователю сообщения об ошибках?
  cszDBHideDateCompliteOnImport[], //  [ao] Не вставлять дату отгрузки при импорте из XLS
  cszDBEditProjectPlotProperty[],
  cszgMsg_OnTaskToManuf       [],   // Сообщение при постановке заказа в Производство?
  cszDBAllowCalcSectorManufact[],   // Рассчитывать обработки по производственным участкам?
  cszDBAllowWaste             [],   // Разрешить работу с хранением деловых отходов?
  cszDBAllowTemper            [],   //[SE] СКРЫТАЯ!!! Разрешить работу подсистемы закалки?
  cszKeyAssemblyLine         [],
  cszKeyPrintEtiketByCrystal [],
  cszKeyDescOrderLabelPrint  [],
  cszKeyCurLabelRep          [],
  cszParseTriplex            [],
  cszParseAddNotFound        [],
  cszParseSectorManufact     [],
  cszKeySawTripPackType      [],
  cszKeySaveFileCurve        [],
  cszDBNameProgram           [],
  cszAutoClacMaterOnCloseTask[],

  cszAutoClacMaterOnSave     [],
  cszAutoClacPriceOnSave     [],
  cszTypePriceBaseIsFirst    [], // [ao] Разрешить базовому прайсу всегда быть первым
                                 // У Базового прайса nOrder = 0 по нему и сортировать
  cszPricePostRecalc         [],
  cszPriceInvoiceCorrect     [],
  cszRasExtraction           [],
  cszRasShowWarning          [],  // [ab] Выводить предупреждения об отсутствии настройки шпрос?
  cszFormIndentsIsEqual      [],
  cszNoTruncateSawTaskDate   [],
  cszSimpleClientDebtQuery   [],  // [OK] СКРЫТАЯ!!!!! Упрощённый запрос для списка клиентов (старого образца, без дерева)
  cszCeilGlassGabarit        [],  // [OK] СКРЫТАЯ!!!!! Округлять ли в большую сторону габарит стекла, в случае нецелого габарита
  cszRecalcAfterPlotChange   [],  // Пересчёт при изменении\удалении чертежа
  cszRecalcRasOnAfterPlotSave[],  // Пересчёт Шпрос после сохранения чертежа

  cszbShowPaymentInfo        [],  // Показывать информацию о раскроях в диалоге клиентов.

  cszPlotSizeX               [],
  cszPlotSizeY               [],
  cszPlotProjectSizeX        [],
  cszPlotProjectSizeY        [],
  cszCheckPencilDraw         [],
  cszFontSize                [],
  cszFontSizeR               [],
  cszMinFontSize             [],
  cszMinFontSizeR            [],
  cszDimLevelHeight          [],
  cszDimLevelDeltaHeightCoef [],
  cszRoundDimValue           [],  // [AO] Округлять размеры размерных линий
  cszDimLevelDeltaHeightCoefR[],
  cszPlotRas3d               [],
  cszPlotRoundFontHeight     [],  // Округлять ли высоту шрифта на чертеже? 
  cszPlotManualOffCut        [],  // Ручное изменение кромок на чертеже
  cszPlotGlassTexture        [],  // Ручное изменение кромок на чертеже
  cszbPlotDimRad             [],  // Показывать радиусы на чертеже

  csznTypeOperMain           [],
  csznTypeCalcPriceNewClient [],
  csznTypeCalcPriceNDS       [],   // СКРЫТАЯ: Тип расчета НДС по методу округления

  csznBigGlassSizeX          [],   // Размер Х больше которого будет считаться большой стекло-пакет
  csznBigGlassSizeY          [],   // Размер Y больше которого будет считаться большой стекло-пакет
  cszdBigGlassThickness      [],   // Толщина стекла, больше которой будет считаться большой стекло-пакет
  cszdBigGlassMass           [],   // Масса одного стекла, больше которой будет считаться большой стекло-пакет
  csznSerialProjectCount     [],   // Кол-во изделий в прожекте, чтобы прожект считался серийным

  cszWidthGrindTool          [],   // Ширина шлифовального круга (20.0 мм)
  cszGrindToolShift          [],   // Смещение шлифовального круга (0.0 мм)

  csznGlassMarkCovering      [],   // [SE] Настройка обозначения стороны покрытия стекла в формуле СП
  csznSidePlot               [],   // [YK] Настройка с какой стороны вид на чертеж
  csznSawSelect_ForWretched  [],   // [OK] СКРЫТАЯ!!! Упрощённый выбор на раскрой.
  csznUseSawForFilm          [],   // [OK] СКРЫТАЯ!!! Разделение по плёнкам.
  cszbFigureToFreeFormForGPS [],   // [YK] Выгружать в GPSOpt фигуры, как свободные формы (для выгрузки рассчитаных кромок)
  cszbSeparatePyramidTask    [],   // [OK] Отдельная пирамида для заказов (Чимкент)
  cszbSerialBySawTask        [],   // [OK] Искать серийные позиции в пределах раскроя

  csznTypeDefineAreaGP       [],   // [SE] Тип определения площади
  csznTypeRejectDialog       [],   // [SE] Тип отображения диалога выбора комбинации браков

  cszTripPyramid_UseSinglePass          [], // Использовать упаковку за один проход.
  cszTripPyramid_SortByQuant            [], // Сортировать по квантам по пачкам? Для Стиса.
  cszTripPyramid_GroupQuantGlass        [], // Группировать стекло в нарезку в отдельный квант по пачкам? Для Стиса.
  cszTripPyramid_QuantUnion             [], // Объединять кванты если более кол-ва СП
  cszTripPyramid_QuantLarge_Width       [],
  cszTripPyramid_QuantLarge_Height      [],
  cszTripPyramid_DifferentPriorityOnPack[], // Разрешить ставить пакеты с разным приоритетом в одну стопку.
  cszTripPyramid_UpdateExistingTowers   [], // Дополнять существующие стопки.
  cszTripPyramid_ReduceVerticalStairs   [], // Сокращать вертикальные лесенки (многопроходный алгоритм).
  cszTripPyramid_OptimizeLongPopulation [], // Создавать популяции большого размера.
  cszTripPyramid_UseBTemplate           [], // Паковать стеклопакеты с признаком bTemplate == 1; иначе не паковать.
  cszTripPyramid_UseBSelfDelivery       [], // Паковать стеклопакеты с признаком "Самовывоз"; иначе не паковать.
  cszTripPyramid_UseSingleGlass         [], // Паковать стеклопакеты с признаком "Стекло"; иначе не паковать.
  cszTripPyramid_CuttingLastPacking     [], // Нарезку в упаковке размещать в конце. 

  cszCutting_UseThread1                 [], // Использовать поток оптимизации раскроя 1.
  cszCutting_UseThread2                 [], // Использовать поток оптимизации раскроя 2.
  cszCutting_UseThread3                 [], // Использовать поток оптимизации раскроя 3.
  cszCutting_Depth                      [],
  cszCutting_Portion                    [], // Количество деталей в порции. Используется и для оптимизации на уровне полосы, и для оптимизации на уровне края листа.
  cszCutting_WarmingUpTime              [], // Время прогрева алгоритма оптимизации раскроя, максимальный % времени на расчёт одного варианта.
  cszCutting_MinSize                    [],
  cszCutting_MaxSize                    [],
  cszCutting_EvoStep                    [],
  cszCutting_OptTime                    [],
  cszCutting_OptTime_10_Details         [],
  cszCutting_MaxXStripeWidth            [],
  cszCutting_JoinRestToInnerMargin      [],
  cszCutting_UseStripeXMargin           [],
  cszCutting_UseThroughYCut             [],
  cszCuttingOperationsEnabled           [], // Оптимизация раскроя разрешена?
  cszCutting_TextSizeOfSheet            [],
  cszCutting_TextSizeOfDetailSize       [],
  cszCutting_TextSizeOfDetailName       [],
  cszCutting_TextSizeOfMargin           [], // Размер шрифта кромки в карте раскроя
  cszCutting_Text_DetailMark            [],
  cszCutting_UseCuttingDrawIniForHeadless[], // Скрытая! Применять draw-настройки CuttingDraw.ini в headless-отрисовке карт

  cszCutting_RotateBillet               [], // Поворачивать заготовки в горизонтальное положение?
  cszCutting_RotateBilletRest           [],

  cszCutting_TypeFileName               [],
  cszCutting_PlotSizeX                  [], // Размер чертежа карты по X в пикселях
  cszCutting_PlotSizeY                  [], // Размер чертежа карты по Y в пикселях
  cszCutting_FillAdditionalBySawTask    [],
  cszCutting_FillAdditionalByDay        [],
  cszCutting_TypeAbsenceGrinding        [], // Тип обозначения отсутствия снятия покрытия
  cszCutting_TypeDopInfoFileHeader      [], // Тип внесения доп.информации в заголовок файла выгрузки
  cszCutting_FileWritePyramid           [], // Записывать в файл экспортов пирамиды 
  cszCutting_TypeBeginFileHeader        [], // Тип внесения начала заголовка в файл выгрузки
  cszCutting_WithoutRightTopMargin      [], // Не соблюдать минимальный отлом MarginInner с правого и верхнего края листа.
  cszCutting_ReduceCutoffCount          [], // Уменьшать количество отломов (постобработка после завершения оптимизации раскроя).
  cszCutting_DisableHorizontalOffcutOnLastSheet[], // Запретить горизонтальный ДО на последнем листе.
  cszCutting_UseFolderForFileName        [], // Использовать подпапки для файлов экспорта на стол раскроя
	cszCutting_SortingYStripesPriority     [], // Сортировать Y полосы по приоритету (постобработка после завершения оптимизации раскроя).
  cszCutting_BreakSortingWidthForPriority[], // Нарушать сортировку лесенкой для выхода по приоритету (постобработка после завершения оптимизации раскроя).
  cszCutting_CreateContainerWithMargins  [], // Создавать контейнер с внутренним отступом
  cszCutting_SortSawOnOptimize           [], // Сортировать раскрой при оптимизации
  cszCashOrderEnumerate                  [], // Тип нумерации кассовых ордеров (0 - по клиенту, 1 - сквозная)
	cszCutting_SortingXStripesByAscAPyramid[], // Сортировать X полосы по возрастанию положения на А-пирамиде
  cszCutting_SortingYStripesByDscAPyramid[], // Сортировать Y - полосы на листе по убыванию положения деталей на A - пирамиде
  cszCutting_ExportDopParam              [], // Дополнительные параметры для экспорта на стол раскроя

  cszbWriteRestInDepot                   [], // Записать деловые остатки в Depot

                                            //--- ПОЧТА ---
  cszSMTPServer                         [], // Сервер SMTP
  cszSMTPPort                           [], // Порт SMTP
  cszSMTPAccount                        [], // Аккаунт SMTP
  cszSMTPPassword                       [], // Пароль SMTP
  cszSMTPAuth                           [], // Использовать SMTP аутентификацию 
  cszSMTPUseTLS                         [], // Использовать SMTP TLS\SSL
  cszSMTPAuthPOP3                       [], // Предварительная аутентификация POP3 перед работой с SMTP
  cszSMTPDomainUser                     [], // Домен для рассылки
  
  cszPOP3Server                         [], // Сервер POP3
  cszPOP3Port                           [], // Порт POP3
  cszPOP3Account                        [], // Аккаунт POP3 (может быть различен с SMTP, в частности не содержать @mailserver.com)
  cszPOP3Password                       [], // пароль для рассылки
  cszPOP3UseTLS                         [], // Использовать POP3 TLS\SSL
                                            //--- ПОЧТА КОНЕЦ---

  cszMailListForChangeSD                [], // Сервер отправки
  cszDBWeekEndWork                      [], // Рабочая суббота
  cszDBSetManufactSateOnPlanning        [], // Выставлять статус производить при планировании заказа
  cszDBSawTaskLimitByArgonAndNonStandard[], //[AO] СКРЫТАЯ!! Ограничения для разделени раскроев по аргону и сложным формам. При вызове команды создать раскрои из плана
  cszDBWeekDaySmena                     [], // Кол-во смен в будни
  cszDBWeekEndSmena                     [], // Кол-во смен в субботу
  cszbTabletInterface                   [],
  cszbFontBold                          [],
  cszbFontItalic                        [],
  cszbFontUnderline                     [],
  cszbFontStrikeout                     [],
  cszsFontFaceName                      [],
  cszlFontFaceWidth                     [],
  cszlFontFaceHeight                    [],
  cszbFontGridBold                      [],
  cszbFontGridItalic                    [],
  cszbFontGridUnderline                 [],
  cszbFontGridStrikeout                 [],
  cszsFontGridName                      [],
  cszlFontGridWidth                     [],
  cszlFontGridHeight                    [],
  cszsFontGridApproveName               [],
  cszlFontGridApproveSize               [],

  cszlDiffBetweenGlass                  [], // Разница в размере стёкол на А-пирамиде в одной стопке внутри цеха

  cszlDiffBetweenGlass_X                [], // Разница в размере стёкол в стопке А-пирамиды - ширина - настройка для комбинированной сортировки
  cszlDiffBetweenGlass_Y                [], // Разница в размере стёкол в стопке А-пирамиды - высота - настройка для комбинированной сортировки

  cszMailListForTaskNotSent             [], // Список Email на который слать уведомления, если заказ не отправлен
  cszNewTaskClientSelectPolicy          [], // Политика назначения клиента новому заданию 
  cszPathCuttingTableProg               [], // Пусть к программе стола раскроя 
  cszCuttingTableLastDateRead           [], // Дата/Время последнего импортированного задания 
  cszCuttingTableToDateRead             [], // Дата/Время по какое будем импортировать задания 
  cszCuttingTableLastTask               [], // Номер последнего импортированного задания 
  cszTriplexPrefix                      [], // Префиксы триплекса собственного производства
  cszTriplexParseMethod                 [], // Метод разбора триплекса собственного производства
  cszTypeFormatComplexTextProject       [], // СКРЫТАЯ!!! Тип формирования текста комплексности для прожекта 
  cszDBTagGlassStorageLocation_CutOff   [], // Скрытая! Метка места хранения стекла для заготовки из обрезка для Hegla
  cszDBTagGlassStorageLocation          [], // Скрытая! Метка места хранения стекла для заготовки для Hegla
  cszMinCutterOffsetFromEdge            [], // Скрытая! Минимальный отступ чтобы резец не прошёл по краю 

  csznNeedPrintState                    [], // СКРЫТАЯ: Проставлять статус Распечатан в заказе и раскрое

  cszTypeCalcGabarit                    [], // Тип счёта габарита
  cszPlotTypeCalcOffCut                 [], // Тип определения кромок

  cszdAngleArcOffCutMin                 [], // Критический угол примыкания арки, при котором ещё не ставится кромка. По умолчанию 30 градусов

  cszEnablePlan                         [], // Доступ к подсистеме планирования 
  cszEnablePacking                      [], // Доступ к подсистеме упаковки
  cszEnableTransport                    [], // Доступ к подсистеме отгрузки
  cszEnableRareGlass                    [], // Доступ к подсистеме редких видов стекла
  cszEnablePayment                      [], // Доступ к подсистеме оплат

  cszAddMarkDetailForBystronic          [], // Экспорт в файл для стола Bystronic маркировки детали из чертежа раскроя (первые 9 символов).

  cszDBMaxCountFrameRack                [], // Макс. кол-во "гусей" на раскрой
  cszDBCountCellFrameRack               [], // Кол-во вешалок в "гусе"
  cszSaw_ReverseOrderArfa               [], // В Арфа-пирамиде раскладывать стёкла СП в обратном порядке.
  cszSaw_ReverseOrderFirstGlassCover    [], // Изменить порядок расположения стекол на пирамиде и для сборки, если в СП есть стекла с покрытием
                                            // Если первое стекло с покрытием, то оно на линии сборки должно идти последним

  cszModeGrind                          [], // Режим формирования областей снятия покрытия
  cszPlotShprosMode                     [], // Режим образмерки чертежа шпрос
  cszPlotAreaMethod                     [], // Метод расчета площади фигуры (по умолчанию метод прямоугольника)
  cszPathAttachmetsMethod               [], // Метод вложения файлов (по умолчанию в базу данных)

  cszHeglaExportTextLabel               [], // Печать текстовых меток для хеглы
  cszHeglaCorrectFigureCut              [], // Корректировать резы фигур на хеглу
                                            // [ab]->[ok] Как они корректируются?
  cszHeglaClockwise                     [], // Запись фигур и шлифования по часовой стрелке.
  cszLaserMarkW                         [], // Лазерная маркировка: ширина мишени (W)
  cszLaserMarkH                         [], // Лазерная маркировка: высота мишени (H)
  cszLaserMarkB                         [], // Лазерная маркировка: расстояние снизу (B)
  cszLaserMarkL                         [], // Лазерная маркировка: расстояние слева (L)
  cszLaserMarkCorner                    [], // Лазерная маркировка: угол
  cszLisecExportDetectSignArc           [], // Определить знак перед радиусом - без этого всегда  + 
  cszMultiplySameFrames                 [], // Разделять одиниковые рамки из СП на гибочнике
  cszCheckCountDrill                    [], // проверять кол-во отверстий на чертеже и в спецификации изделия (пока только перед закрытем чертежа)
  cszbCalcPriceDrill_ByRefDrill         [], // Скрытая!!!! обновлять операцию сверления, в соответствии с введеным количеством отверстий
  cszEnableSawSort                      [], // Скрытая!!!!  Можно ли сортировать  список раскроев по колонкам.
  cszMaxCountDayLocateOnTechPyramid     [], // ПОКА СКРЫТАЯ!! Максимальное кол-во дней присутствия детали на технологической пирамиде
  cszCuttingMarkCutOffAsDetail          [], // Помечать деловые остатки как детали
  cszCuttingUseBlackWhiteSchemeColor    [], // Использовать черно-белую схему раскраски карты раскроя
  cszMaxCountDayFillTechPyramid         [], // ПОКА СКРЫТАЯ!! Максимальное кол-во дней для заполнения деталей на технологические пирамиды
  cszCuttingShowMargins                 [], // Показывать значения кромок на карте раскроя
  cszCuttingShowLaserMark               [], // Показывать область лазерной маркировки на карте раскроя
  cszCuttingUseStepAglo                 [], // Ступеньчетый алгоритм заполнения остатков деталями из других заданий
  cszCutting_UseSpecialLisecVCut        [], // Использовать специальный V-рез для Лисек. // Создавать только одну деталь на полосе 4 уровня.
  cszCutting_UseSpecialVCutOnlyEquals   [], // Использовать специальный V-рез для Лисек. // Создавать только одинаковые детали на полосе 4 уровня.
  cszCutting_AutoDetectCutOff           [], // Автообнаружение ДО с добавлением в справочник
  cszCutting_AddNumAndCountDetailOnType [], // добавлять к маркировке детали на карте раскроя номер и кол-во однотипных деталей

  cszProjectTypeRebate                  [], // Тип применения скидки для позиции
  cszProject_MarginCoef_Show            [], // Как отображать пользователю маржу по позиции
  cszPricingAlgoCalcNewClient           [], // Алгоритм ценообразования при создании нового клиента
  cszSaveTaskOnCalcPrice                [], // сохранение заказа при расчете цены

  cszCheckDuplicateUNN                  [], // Проверять ли на ИНН на задвоенность?
  cszbUseSimpleParseGP                  [], // [SE] СКРЫТАЯ!! Использовать упрощенный парсер(пока для отладки)
  csznDialogEditPosTask                 [], // [SE] СКРЫТАЯ!! Диалог редактирования позиции заказа(0 - DlgFilm, 1 - DlgProduct)
  cszbUseTechPyramid                    [], // СКРЫТАЯ!! Использовать технологические пирамиды?
  cszbInputRasWithoutPlot               [], // СКРЫТАЯ!! Вносить шпросы без использования чертежа
  cszbSelectAllRowInTaskView            [], // СКРЫТАЯ!! Выделять всю строчку в списке заказов 
  cszbGroupTaskByClientInShip           [], // СКРЫТАЯ!! Сгруппировать накладные по одному клиенту в отгрузке
  cszbSortByClientInTaskView            [], // СКРЫТАЯ!! Сортировать в списке заказов по клиентам
  cszbUpdateStateTaskWithStateManufTask [], // СКРЫТАЯ!! Обновлять статус "заказа"  при обновлениие статуса "заказ в производстве"
  cszbTruncDim                          [], // СКРЫТАЯ!! не выводить в чертеже радиусы размером меньше 1 мм и округлять дробные части в размерных линиях
  cszbDecreaseDateRange                 [], // СКРЫТАЯ!! уменьшить диапозон дат выборки заказов (используется при долгом ожидании)
  cszTypeRoundPriceTask                 [], // Тип округления конечной стоимости заказа
  cszbWriteDrill                        [], // СКРЫТАЯ!! запись сверлений при выгрузке раскроя (Lisec optio.dat)

  cszbCombineNotchShpros                [], // СКРЫТАЯ!! комбинировать шпросы на врезках, стоящие в стык, при импорте XML
  cszbLockRecalcPriceOnAllColumn        [], // СКРЫТАЯ!! колонка блокировки пересчета цены действует на все колонки(как ранее)
  cszbUseFixedArea                      [], // СКРЫТАЯ!! использовать значение фиксированной площади, если площадь меньше заданной 
  cszbCalcWithPriceNDS                  [], // СКРЫТАЯ!! Использовать при рассчете цены, прайс с НДС
  cszbOnlyOnceRemake                    [], // СКРЫТАЯ!! Возможность создавать только одну переделку 1 - да, 0 - нет
  cszsOptionsProductGOSTGP              [], // [SE] ГОСТ на стеклопакеты
  cszbEnabledSetTaskStatusSaw           [], // Скрытая!! Разрешение ставить вручную статус заказа "Раскроен"
  cszbEnableConditionalFormulaReplace   [], // Скрытая!! Разрешение на разную замену значений в формуле для стекла и стеклопакетов
  cszbStrictSheduleOperatorAssign       [], // Скрытая!! Назначение SheduleOperator для процессингов строго по таблице календаря

  cszbEnableOpenCV                      [], // Скрытая!! Разрешено оцифровывать шаблоны с использованием OpenCV?

  csznTypeMassCalc                      [], // [SE] Тип расчета массы изделия
  csznTaskPriorityCalc                  [], // [OK] Тип расчета приоритета заказа

  // Параметры для закалки:
  cszTempering_DetailSquare_Ratio       [], // Разница в площади деталей в одной порции закалки, 1.25 == 25% больше размер набольшей детали над наименьшей.
  cszTempering_DetailHeight_Ratio       [], // Разница в высоте деталей в одной порции закалки, для второго критерия определения деталей в одной порции.
  cszTempering_DetailWidth_Ratio        [], // Разница в ширине деталей в одной порции закалки, для второго критерия определения деталей в одной порции.
  cszTempering_BigDetail_Mass           [], // Масса большой детали в закалке, которая снимается со стола влево, кг.
  cszTempering_BigDetail_Square         [], // Площадь большой детали в закалке, которая снимается со стола влево, кв.м.
  cszTempering_BigSmallDetail_Square_Ratio[], // Отношение площадей деталей в порции, при превышении которого детали считаются большими и маленькими. Большие детали в порции НЕ должны идти за маленькими деталями, иначе начинается новая порция.
  cszTempering_InOrder_RowRightToLeft   [], // Направление закладки деталей на стол закалки: рядами справа налево; иначе рядами слева направо.
  cszTempering_OutOrder_RowLeftToRight    [], // Направление выхода деталей со стола закалки: рядами слева направо; иначе колонками сверху вниз.
  cszTempering_MarginTempering            [], // Расстояние между деталями в печи для закалки стекла.
  // параметры для закалки рядами
  cszTemperingCreateByRow_RowRatio          [], // Максимальное допустимое соотношение между самым широким рядом и самым узким рядом.
  cszTemperingCreateByRow_HeightInRowRatio  [], // Максимальное допустимое соотношение между максимальной высотой детали в ряду и минимальной высотой детали в ряду.
  cszTemperingCreateByRow_DetailSquareRatio [], // Максимальное допустимое соотношение между максимальной площадью детали на листе и минимальной площадью детали на листе.
  cszTempering_UseDetailRotation            [], // Разрешить поворот деталей во время оптимизации закалки. 
  cszTempering_UseMultiStripe               [], // Разрешить раскладывать детали в несколько колонок во время оптимизации закалки.
  // Настройки для оптимизации контейнеров непрямоугольных деталей:
  cszCutting_ContainerMinEstimation         []; // Минимальная желаемая оценка контейнера: Во сколько раз площадь контейнера меньше площади габаритов составляющих его деталей.

// Константы:
extern const TCHAR
  cszConnDefStr    [];

// Константы для Enum
extern const TCHAR
  // ETypeOper
  cszTypeOper_None        [],
  cszTypeOper_Film        [],
  cszTypeOper_Harding     [],
  cszTypeOper_Triplex     [],
  cszTypeOper_Drilling    [],
  cszTypeOper_Grinding    [],
  cszTypeOper_Emalit      [],
  cszTypeOper_Blunting    [],
  cszTypeOper_Vitrage     [],
  cszTypeOper_Vent        [],
  cszTypeOper_HardingBend [],
  cszTypeOper_Cutout      [],
  cszTypeOper_SandBlast   [],
  cszTypeOper_Notch       [],
  //ETypeProd
  cszTypeProd_None     [],
  cszTypeProd_OtherProd[],
  cszTypeProd_Glass    [],
  cszTypeProd_Frame    [],
  cszTypeProd_Film     [],
  cszTypeProd_Obrab    [],
  cszTypeProd_Paint    [],
  cszTypeProd_Teokol   [],
  cszTypeProd_Sito     [],
  cszTypeProd_Ras      [],
  cszTypeProd_Argon    [],
  cszTypeProd_Assembly [],
  cszTypeProd_Triplex  [],
  //ETypeSide
  cszTypeSide_OutSide  [],
  cszTypeSide_InSide   [],
  //ETypeCoatingOnCutting
  cszTypeCoatingOnCutting_Up  [],
  cszTypeCoatingOnCutting_Down[],
  //ETypeCoatingOnGP
  cszTypeCoatingOnGP_OutGP [],
  cszTypeCoatingOnGP_InGP  [],
  cszShowLogo              [],
  //EPyramidTypeOper
  cszPyramidTypeOper_Ordinary    [],
  cszPyramidTypeOper_Obrab       [],
  cszPyramidTypeOper_Triplex     [],
  cszPyramidTypeOper_Technologic [],
  cszPyramidTypeOper_Covering    [],
  //EPyramidTypeGP
  cszPyramidTypeGP_GlassPack     [],
  cszPyramidTypeGP_Cutting       [],
  cszPyramidTypeGP_Common        [],
  cszVersionStone                [],
  cszRecalcTimeGlassProcNextSaw  [],
  // ETypeCutOut
  cszTypeCutOut_OnContur         [],
  cszTypeCutOut_InSideContur     [],

  // Автоформирование НЗ
  cszDistribute_Zak              [],
  cszDistribute_Figure           [],
  cszDistribute_Shpros           [],
  cszDistribute_Film             [],
  cszDistribute_FilmArm          [];

///////////////////////////////////////////////////////////////////////////////
// Настройки:

extern const TCHAR
  g_szUpdateVersion[],      // [ab] Строка в реестре с именем последнего обновления.
                            // Прописывает инсталляция(или пишем при закрытиии программы) после успешного завершения.

  g_szbUpdateWork[];        // Строка в реестре с флагом, что автообновление было начато
                            // Прописываем при закрытиии программы
extern CString
  g_sPathUpdate,               // Закачанное обновление программы
  g_sUpdateFile,               // Файл обновления без пути
  g_sCurPathReplica,
  g_sCurPathScript,
  g_sCurPathImage,
  g_sCurPathCrashRpt,          // Директория для записи CrashRpt
  g_sCurPathReport,
  g_sCurBarCodePath,
  g_sCurPyrEtiketShablonPath,
  g_sCurGPEtiketShablonPath,
  g_sCurHeglaPath_Inbound,
  g_sCurHeglaPath_Outbound,
  g_sCurLisecInbound,
  g_sCurLisecOutbound,
  g_sCurOtdOutbound,
  g_sCurLisecTraioOptio,
  g_sCurPathSawTaskRegistration,    // Путь для хранения файлов для регистрации раскроев в других программах
  g_sCurPathTaskAttachmets,         // Путь для хранения вложенных файлов 
  g_sCurDBList,
  g_sCurServerList,
  g_sLastLogin,
  g_sLastDB,
  g_sLastServer,
  g_sPassword,
  g_sCompName,              // Имя данного компьютера, с которого запустили прогу
  g_sNarezkaMark,           // Приписка к стеклу в нарезку при экспорте в GPSOpt. (В Соларексе: "01").
  g_sPort,
  g_sCurPathSawFilesExport, // Путь до файла выгрузки раскроя.
  g_sCurPathSawDrawExport,  // Путь до файла чертежа .sag выгрузки раскроя.
                            // Название таблиц и путь к ним - пока жестко
  g_sCurPathTableSawTask,
  g_sCurPathTableSawPos,
  g_sCurPathSaw_Edge,        // Директория записи файлов на кромочник
  g_sModuleFileName,         // Имя и путь исполняемого *.exe файла
  g_sModuleFileNamePath,     // Путь к *.exe
  g_sNameProgram,            // [SE] Для определения имени програмного продукта
  g_sIdNumFormula,           // Формула создания идентификационного номера (Gps.opt)
  g_sArgon,                  // строка вариантов обазначения аргона
  g_sTriplexPrefix,          // Префиксы триплекса собственного производства
  g_sBarCodePrefix,          // [ab] Префикс штрих-кода
  g_sMailListForChangeSD,    // Лист рассылки о смене подразделения
  g_sMailListForTaskNotSent, // Лист рассылки если заказ не отправлен
  g_sTagGlassStorageLocation,         // Скрытая! Метка места хранения стекла для заготовки для Hegla
  g_sTagGlassStorageLocation_CutOff,  // Скрытая! Метка места хранения стекла для заготовки из обрезка для Hegla
  g_sFilmMark,                        // Приписка к стеклу у которого пленка при экспорте в GPSOpt.
  g_sGOSTGP;                          // ГОСТ на СП

extern int
  g_iBarCodeGPLeng;          // Длина штрих-кода СП


extern bool
  g_bSetFactReject,         // Пользователь может устанавливать фактический брак
  g_bShipLock,              // Пользователь может заблокировать отгрузку
  g_bShipUnlock,            // Пользователь может разблокировать отгрузку
  g_bSetProcessComplete,    // Право на заполнение информации о выполнении этапов производства
  g_bClearProcessComplete,  // Право на удаление информации о выполнении этапов производства
  g_bWagesEdit,             // Настройка параметров расчета ЗП (спавочники Должности ст. КТО, Стоимость УЕ)
  g_bOleCall,
  g_bWebServiceMode,
  g_bUserLogin,
  g_bUserRemember,
  g_bReportPreview,
  g_bReportsetPreview,
  g_bWriteBarcodeDepot,
  g_bRequestExistPiramid,
  g_bTaskTimeSet,
  g_bCheckNum,
  g_bCheckAccountNum,
  g_bCheckZeroPrice,           // [ab] Проверять нулевую цену?
  g_bCalcFact,
  g_bPrjCommentCopy,
  g_bWinTaskPos,

  g_bAllowSettings,
  g_bAllowTaskList,
  g_bAllowTaskPlanList,        // Отображать список запланированных заказов [ab] Дублирует g_bAllowPlan
  g_bAllowTask_Export,         // Разрешать экспорт заказов в *.dbf, *.xml ...
  g_bAllowObzList,
  g_bAllowTransport,
  g_bAllowSaw,                 // Разрешить подсистему раскроя?
  g_bAllowSaw_Export,          // Экспорт во внешние программы раскроя?
  g_bAllowSaw_Export_Curve,    // Экспорт файла в станки для гибочника?

  g_bAllow_DXF,                // Импорт DXF?
  g_bAllow_DrawExtra,          // Сложный чертёж?

  g_bAllowClient,
  g_bAllowTaskToManuf,         // Отображать документ "Задание в производство"
  g_bAllowPacking,             // Разрешить упаковку?
  g_bAllowPlan,                // Планирование?
  g_bAllowOperPlan,            // Операционное Планирование?
  g_bAllowGlassProcessing,     // [ab] Обработка стекла
  g_bAllowCalcSectorManufact,  // Расчет производственных участков на операции
  g_bAllowWaste,               // Разрешена работа с хранением деловых отходов?
  g_bAllowTemper,              // [SE] СКРЫТАЯ!!! Разрешить подсистему закалки

  g_bShowError,                // [ab] По умолчанию показываем пользователю ошибки.
                               // А очень дурным скрываем, чтобы нам же небыло хуже.
  g_bEditProjectPlotProperty,  // [ab] Позволить редактировать галку "Чертёж" в позиции заказа?
                               // Данная галка проставляется автоматом.
                               // Но некоторые хотят её снимать, если у них размер на чертеже не связан с реальными размерами деталей.
  g_bMsg_OnTaskToManuf,        // Выдавать сообщение при постановке заказа в состояние Производить?
  g_bOwner,
  g_bPlanSetter,
  g_bPrioritySetter,
  g_bPrintEtiketByCrystal,
  g_bDescOrderLabelPrint,
  g_bDelimiterEtiket,
  g_bPrintRepComplectOnGPSOpt,
  g_bParseTriplex,
  g_bParseAddNotFound,
  g_bParseSectorManufact,         // Галка "При разборе формулы СП рассчитывать производственные участки"?
  g_bRecalcTimeGlassProcNextSaw,  // Программа может пересчитывать время обработки в последующих раскроях автоматически 
  g_bGroupPos,
  g_bTaskPos,
  g_bGlassInOneLine,
  g_bShowReportSet,
  g_bOptimExportFigure,
  g_bEtiket_BSV_WithoutSaw,
  g_bSelectClient_Keyboard,
  g_bRefPrice_ShowClient,
  g_bSawSort,
  g_bSeparateSectorPyramid,
  g_bNoReverseNameGlass,
  g_bAddNewClient,
  g_bReplaceEmptyCommentary,
  g_bTypeDisplaySide_TripPyramidPack,
  g_bCreateAutomaticAccountNum,
  g_bUseTaskToManuf,
  g_bAlwaysCloseTreeGrid,
  g_bInsertNarezkaToArfa,
  g_bInsertCoatingGlassToA,
  g_bUseFindReservedGP,
  g_bReplaceFormulaGPImport,
  g_bUpdateDateGiveManufact,
  g_bUpdateDateComplete,
  g_bShortDateComplete,
  g_bHideDateCompliteOnImport,
  g_bShortDateCreate,
  g_bResetAccountNumNewYear,
  g_bCalcFactNumUniqueForShipedTask,
  g_bRasPriceClientType,
  g_bSetStateGPMakedAfterShip,
  g_bCheckClientCredit,
  g_bCheckSellerCredit,
  g_bDateGiveManufactCheck,
  g_bSetDateGiveManufact,
  g_bSetDateComplite,
  g_bSetDatePayDoc,
  g_bSeparateSawTeamAssembly,
  g_bSeparateSawPack,
  g_bRedrawPyramidPlotOnShow,                  // [AF] Перестроить чертеж пирамид при переходе на закладку с чертежом пирамид?
  g_bAutoSetStatesForTaskManuf,
  g_bPrintRepComplect_NoPack,
  g_bGlassWithOperOnArfaPyr,
  g_bExportOptimaDiffnGlass,                   // Экспортировать номер пачки в оптиму на А пирамиды
  g_bChangeBarCode_idSawTaskAssembly_ByLatest, //[OK] Переносить ли всегда сборку на более позднюю оптимизацию.
  g_bAutoCalcMaterOnCloseTask,
  g_bSaveTaskOnCalcPrice,
  g_bPricePostRecalc,
  g_bPriceInvoiceCorrect,
  g_bRasExtraction,
  g_bRasShowWarning,                           // [ab] Выводить предупреждения если не настроены параметры шпрос
  g_bCurveTranslit,
  g_bAutoSetCurveForTask,
  g_bGroupPyramidBySectorManufact,            // Группировать пирамиды по производственным участкам (иначе по единицам оборудования на участке)
  g_bSeparatePyramidTask,                     // Разные заказы в разные Арфа-пирамиды
  g_bSerialBySawTask,                         // Искать серийные позиции в пределах раскроя
                                              // Группировать одинаковые размеры одного вида стекла из разных позиций и разных заказов на одну А-пирамиду в одну пачку

  g_bAllowManualAddSpToPyramid,               // [MR] разрешить добавлять изделия на пирамиды ручками (через контекстное меню)
  g_bCreateSawFromPackUnbrokenPyr,            // Создавать раскрои из упаковки из целых пирамид (изделия с упак. пирамиды в одном раскрое)
  g_bAllowOptimGlass,                         // Разрешить оптимизацию раскроя?
  g_bAllowOptimGlass_Export_Bystronic,        // Разрешить экспорт оптимизации раскроя в Bystronic? 
  g_bAllowOptimGlass_Export_WinSzyby,         // Разрешить экспорт оптимизации раскроя в WinSzyby? 
  g_bAllowOptimGlass_Export_Hegla,            // Разрешить экспорт оптимизации раскроя в Hegla?  
  g_bAllowOptimGlass_Export_HeglaV3,          // Разрешить экспорт оптимизации раскроя в Hegla V3?  
  g_bAllowOptimGlass_Export_Botero,           // Разрешить экспорт оптимизации раскроя в Botero? 
  g_bAllowOptimGlass_Export_Botero_Evo,       // Разрешить экспорт оптимизации раскроя в Botero Evo? 
  g_bAllowOptimGlass_Export_Lisec,            // Разрешить экспорт оптимизации раскроя в Lisec?  
  g_bAllowOptimGlass_Export_LisecPrime,       // LisecPrime
  g_bAllowOptimGlass_Export_CuttingPrime,     // CuttingPrime
  g_bAllowOptimGlass_Export_PGX,              // Разрешить экспорт оптимизации раскроя в PGX? 
  g_bAllowOptimGlass_Export_CNI,              // Разрешить экспорт оптимизации раскроя в CNI? 
  g_bAllowOptimGlass_Export_Turomas,          // Разрешить экспорт оптимизации раскроя в Turomas? 
  g_bAllowOptimGlass_Export_Bavelloni,
  g_bAllowOptimGlass_Export_BaltSystem,       // Lisec Balt System
  g_bAllowOptimGlass_Export_CMS,              // CMS
  g_bAllowOptimGlass_Export_Optima,           // Optima *.opt
  g_bAllowOptimGlass_Export_OTD,              // Intermac *.opt

  g_bAllowOptimGlass_Export_DXF,              // *.DXF-формат

  g_bAllowOptimGlass_Export_Grind_Wide,          // Разрешён экспорт широкого шлифования на Intermac?
  g_bAllowOptimGlass_Export_Grind_Wide_Lisec,    // Разрешён экспорт широкого шлифования на Lisec?
  g_bOptimGlass_XCutFullHeight;                  // Скрытая!!! Вертикальные X-резы на полную высоту листа (через кромки)
extern double
  g_dAllowOptimGlass_Export_Grind_Wide_MsgDelay,        // Сообщение о тестировании опции широкого шлифования Intermac
  g_dAllowOptimGlass_Export_Grind_Wide_MsgDelay_Lisec,  // Сообщение о тестировании опции широкого шлифования Lisec
//extern COleDateTime
  g_dDateSinceOptioDat;                                 // Фильтр optio.dat файлов по дате создания
extern bool
  g_bExportLisecIn_Config,                              // Включение опций и опции экспорта в Lisec Optio.dat
  g_bExportLisecIn_WriteRezept,   
  g_bExportLisecIn_WriteGLAI,     
  g_bExportLisecIn_WriteBatch,    
  g_bExportLisecIn_WritePLA_INFO, 
  g_bExportLisecIn_WriteProduct,  
  g_bExportLisecIn_WriteOptdetail,
  g_bExportLisecIn_grind_separate, 

  g_bAllowOptim_APyramid,                       // Разрешён алгоритм оптимизации по стопкам А-пирамид?
  g_bAllowOptim_NoRect_Insert,                  // Разрешён алгоритм оптимизации вставки?

  g_bExportAssemblyLineSetFrameFirstGlass,      // [SE] При экспорте на линию сборки первому стеклу назначать рамку
  g_bExportAssemblyLineSwapSize,                // [SE] Если "Ширина" < "Высоты", то поменять их местами на линию сборки
  g_bSawLargerSizeIsHeight,                     // [SE] При создании раскроев больший размер детали будет "Высотой" 

  g_bWriteRestInDepot,                          // Записать деловые остатки на склад?

  g_bAutoCalcMaterOnSave,                       // [OK] Авторасчёт материалов при сохранении
  g_bAutoCalcPriceOnSave,                       // [OK] Авторасчёт цены       при сохранении      
  g_bTypePriceBaseIsFirst,                      // [AO] Скрытая!!! Разрешить базовому типу прайса всегда быть первым
                                                // У Базового прайса nOrder = 0 по нему и сортировать
  // Права пользователей
  g_bTaskAdd,
  g_bTaskDelete,
  g_bTaskProcessedDelete,             // Пользователь имеет право удалять заказы?
  g_bPositionEdit,
  g_bTaskPlan,
  g_bDateManufactEdit,
  g_bToSaw,
  g_bExportGPS,
  g_bTaskReady,
  g_bTaskReopen,
  g_bLockedTaskEdit,
  g_bShippedTaskEdit,
  g_bPreManufaktTaskEdit,             // Редактирование заказов в состоянии Производить
  g_bUserEdit,
  g_bCreditEdit,                      // Право на редактирование кредита.
  g_bFireClientOverdraftTask,         // Право на запуск заказов клиента, превысившего кредит.
  g_bAllowEditPaidTaskProperty,
  g_bRecalcTimeGlassProcSaw,          // Пользователь может пересчитывать время обработки в раскроях
  g_bIgnoreErrorParseFormule,         // Пользователь имеет право при импорте из Эксель в заказ вставлять формулы с ошибками
                                      // [ab] Опции на программу:
  g_bPriceUnitInCalcFact,             // "Штучная цена в сч-фактуру и накладную"
  g_bAutoSetShipedState,
  g_bDoNotMakeTaskByClientOverdraft,  // Запрет на запуск заказа при певышении клиентом кредита.
  g_bDoNotMakeTaskBySellerOverdraft,  // Запрет на запуск заказа при певышении ПРОДАВЦОМ кредита.
  g_bRoundPriceM2NDS,                 // [bf] Округлять цену за м2 с НДС до рублей
  g_bAllowBackManufDate,              // [bf] Разрешить вводить дату производства задним числом
  g_bAllowBackTaskDate,               // Дата заказа задним числом
  g_bAllowIgnoreDayLimitSP,           // [MR] Разрешить превышение производственного ограничения по количеству СП
  g_bPriceGPRound,                    // [bf] Округлять цену за стеклопакет
  g_bDenyEditOldTasks,                // [bf] Запретить редактирование заказов не запущенных в производство в предыдущем месяце
  g_bPrjSizeCopy,                     // [SE] Копировать размеры из последней строки
  g_bPrjPriceCopy,                    // [SE] Копировать начальную цену из последней строки
  g_bTaskNew_bSetDateGiveManufact,    // [OK] Проставлять дату изготовления новому заказу
  g_bAddStateTaskApproval,            // Вывод дополнителных статусов согласования заказа
  g_bAddShipedStateFromBarCode,       // Проставлять статус "Отгружен" при добавлении заказа в отгрузку
                                      // Иначе только после подтверждения
  g_bShowShippingInfoOnTaskSaw,       // Вывод информационного окна отгрузки при закрытии и сохранении заказа
  g_bPermanentSaving,                 // [OK] Постоянное сохранение

  g_bStandartProductionLibrary,       // [OK] Применять библиотеку чертежей стандартной продукции

  g_bFormIndentsIsEqual,              // [SE] Для каталога форм проставляемые отступы К1..K4 должны быть одинаковые
  g_bNoTruncateSawTaskDate,                       

  g_bSimpleClientDebtQuery,           // [OK] Упрощённый запрос для списка клиентов (старого образца, без дерева)
                                      // [OK] СКРЫТАЯ!!!!!   
  g_bCeilGlassGabarit,                // [OK] Округлять ли в большую сторону габарит стекла, в случае нецелого габарита
                                      // [OK] Скрытая

  g_bSawSelect_ForWretched,           // [OK] Выбор на раскрой одним кликом   !!!Скрытая!!!  

  g_bUseSawForFilm,                   // [OK] Разделение по плёнкам в окне выбора на раскрой !!!Скрытая!!!   

  g_bRecalcAfterPlotChange,             // [OK] Пересчёт цены после изменения/удаления чертежа
  g_bRecalcRasOnAfterPlotSave,          // Не пересчитывать цену шпрос после сохранения чертежа
  g_bSeparatePosNum,                    // Разные позиции заказа в отдельные стопки
  g_bSeparateNonStandard,               // Стандарт / нестандарт в разные стопки.
  g_bSeparatePos_ByGPName,              // Позиции с различной формулой в разные стопки
  g_bSeparatePos_WithOper,              // Позиции с обработками в отдельную стопку 
  g_bSeparatePos_WithFilm,              // Позиции с пленкой в отдельную стопку 
  g_bSeparatePos_WithIDGlass,           // Стёкла позиции с разным ID в отдельную стопку
  g_bSeparate_ByGlassName,              // Разделять стопки на А-пир-х по наименованию стекла
  g_bCyclePyramid,                      // Циклировать пирамиды
  g_bStraightAFill,                     // Однопроходное заполнение А-пирамид
  g_bSolidFillOperPyramid,              // Сплошное заполнение обработочных пирамид

  g_bMaxFillAPyramid,                   // Максимальное заполнение А-пирамид
  g_bMaxFillArfaPyramid,                // Максимальное заполнение Арфа-пирамид

  g_bDecreaseAPackSize,                 // Уменьшение / увеличение стекла в стопке А-пирамиды при раскладке (в пределах допусков)

  g_bComby_A_and_Arfa_A_Diff_Pack,      // В случае комбинированной раскладки (А+Арфа) заполнять А-пирамиды по маршрутам и видам стекла
                                        // Разные маршруты и виды стекла в разные А-пирамиды

  g_bBalanceAPyramid,                   // Балансировать А-пирамиды и разделять стопки по сторонам

  g_bWeekEndWork,                       // Рабочая суббота
  g_bSetManufactSateOnPlanning,         // Выставлять статус производить при планировании заказа, если планируется заказ
  g_bSawTaskLimitByArgonAndNonStandard, //[AO] СКРЫТАЯ!! Ограничения для разделени раскроев по аргону и сложным формам. При вызове команды создать раскрои из плана
  g_bCeilSumWithNDS,                    // Округлять сумму с НДС до большего целого
  g_bOverrideSealingDepth,              // Глубина герметизации выгружается на BSV из настройки в Config
  g_bCalcFactNumUniqueForTypeOrderTask, //[SE] СКРЫТАЯ!! Номер счет фактуры заказа уникален в границах статуса заказа (Б, Ч, ОБ)
  g_bCheckPlotConsistency,              //[SE] СКРЫТАЯ!! Проверять чертежи на целостность

  g_bRotateHeglaHeaderOffcuts,          //[OK] СКРЫТАЯ!!  Вращать правую хеглу на левую  
  g_bFigureToFreeFormForGPS,            //[YK] Выгружать в GPSOpt фигуры, как свободные формы (для выгрузки рассчитаных кромок)
  g_bCreateBarCode,                     //[SS] СКРЫТАЯ!! Создавать ШК ( LastBarCode != 0 - true, LastBarCodeGlass == 0 - false )
  g_bCreateBarCodeGlass,                //[YK] СКРЫТАЯ!! Создавать ШК для стекол ( LastBarCodeGlass != 0 - true, LastBarCodeGlass == 0 - false )
  g_bCopyShprosWithoutAnswer,           //[SE] ПОКА СКРЫТАЯ!! Копировать шпроссы с предыдущей позиции без запроса разрешения у пользователя
  g_bRezervMaterialForTask,             //[SE] Резервировать материалы  на заказ при установке признака "Производить"
  g_bAddGlassToPackSawTask,             //[SE] ПОКА СКРЫТАЯ!! Добавлять стекла в уже упакованные раскрои
  g_bNumberSmenaMarkAsLetter,           //[SE] Скрытая пока!!! Номер смены обозначать через букву
  g_bFixBarCodeWrongPrefix,             //[SE] Скрытая пока!!! Исправлять штрихкоды с неправильным размером и префиксом
  
  g_bSupressMsgForNoArfa,               //[OK] Скрытая!!! Подавлять сообщение об отсутствии арфа-пирамид при раскладке. 
                                        //     У инпруса счас нету арф вообще, и при каждом раскрое оно его выкидывает сообщение это
  g_bCalcDebtOnTaskChange,              //[OK] Скрытая!!! Рассчитывать задолженность клиента при изменении заказа.

  g_bNoCalcPriceOperOnTaskChange,       //[OK] Скрытая!!! НЕ рассчитывать стоимость операций С++ при расчёте цены заказа.
  g_bCalcArgonAsPriceOper,              //[AO] Скрытая!!! Считать аргон как обработку

  g_bCreateReservAfterOptim,            //[OK] Скрытая!!! Создавать резервы после завершения оптимизаций
  g_bShippingRejectProduct,             //[SE] Отгружать бракованные изделие через модуль отгрузки
  g_bAssignZeroPriceRemake,             //[SE] Всегда присваивать нулевую цену переделанным по браку позициям
  g_bChangeSizeProjectAfterPlotChange,  //[YK] СКРЫТАЯ!! Менять размер изделия после редактирования чертежа. по умолчанию: 1-да
  g_bCreateComExpReadyProd_LockShip,    //[YK] создать документы прихода и расхода готовой продукции после подтверждения отгрузки
  g_bShowPartialManufAsNew,             //[ОК] Показывать частично произведённые заказы как новые

  g_bEnablePlan,                        // Доступ к подсистеме планирования 
  g_bEnablePacking,                     // Доступ к подсистеме упаковки [ab] Дублирует g_bAllowPacking
  g_bEnableTransport,                   // Доступ к подсистеме отгрузки
  g_bEnablePayment,                     // Доступ к подсистеме оплат

  g_bEnableRareGlass,                   // Разрешить работу с редкими видами стёкол?

  g_bNoCreateDim,                       // флаг отключения образмерки

  g_bVersionMetal,                      // Версия для металла - будем вводить свои особенности
  g_bVersionStone,                      // Версия для камня - будем вводить свои особенности
  g_bShowLogo,                          // Показ логотипов и сплэша
                                        // TODO: Заменить на целый тип
  g_bHeglaExportTextLabel,              // Экспортировать текстовые метки на хеглу  
  g_bHeglaCorrectFigureCut,             // Корректировать резы фигур на хеглу
                                        // [ab]->[ok] Как они корректируются?
  g_bHeglaClockwise,                    // Резы фигур на хеглу по часовой стрелке 

  g_bLisecExportDetectSignArc,          // Определить знак перед радиусом - без этого всегда  + 
  g_bLegacyMode,                        // Использовать ли старый режим работы для отладки и сравнения результатов.
  g_bMultiplySameFrames,                // Разделять одиниковые рамки из СП на гибочнике
  g_bCheckCountDrill,                   // проверять кол-во отверстий на чертеже и в спецификации изделия (пока только перед закрытем чертежа)
  g_bCalcPriceDrill_ByRefDrill,         // Скрытая!!!! Считать цену из справочника свелений
  g_bEnableSawSort,                     // Можно ли сортировать  список раскроев по колонкам
  g_bPlotRoundFontHeight,               // Округлять ли высоту шрифта на чертеже? 
  g_bCheckDuplicateUNN,                 // Проверять ли на ИНН на задвоенность?
  g_bUseSimpleParseGP,                  // [SE] СКРЫТАЯ!! Использовать упрощенный парсер(пока для отладки)
  g_bUseTechPyramid,                    // СКРЫТАЯ!! Использовать технологические пирамиды?
  g_bPlotManualOffCut,                  // Ручное изменение кромок на чертеже
  g_bInputRasWithoutPlot,               // СКРЫТАЯ!! Вносить шпросы без использования чертежа
  g_bSelectAllRowInTaskView,            // СКРЫТАЯ!! Выделять всю строчку в списке заказов
  g_bGroupTaskByClientInShip,           // СКРЫТАЯ!! Сгруппировать накладные по одному клиенту в отгрузке
  g_bSortByClientInTaskView,            // СКРЫТАЯ!! Сортировать в списке заказов по клиентам
  g_bUpdateStateTaskWithStateManufTask, // СКРЫТАЯ!! Обновлять статус "заказа"  при обновлениие статуса "заказ в производстве"
  g_bDecreaseDateRange,                 // СКРЫТАЯ!! уменьшить диапозон дат выборки заказов (используется при долгом ожидании)

  g_bRoundArea_Sum,                     // СКРЫТАЯ!! 1 - округлить СУММАРНУЮ площадь (по умолчанию: 0 - СУММИРОВАТЬ округлённую площадь изделия)
  g_bClearTrash_BeforeDel,              // Скрытая!! Очищать сбойные элементы при удалении из справочников

  g_bCombineNotchShpros,                // СКРЫТАЯ!! комбинировать шпросы на врезках, стоящие в стык, при импорте XML
  g_bLockRecalcPriceOnAllColumn,        // СКРЫТАЯ!! колонка блокировки пересчета цены действует на все колонки(как ранее)
  g_bUseFixedArea,                      // СКРЫТАЯ!! Использовать значение фиксированной площади, если площадь меньше заданной 
  g_bCalcWithPriceNDS,                  // СКРЫТАЯ!! Использовать при рассчете цены, прайс с НДС
  g_bCheckBarCode,                      // Проверять ли на правильность баркодов при создании   
  g_bOnlyOnceRemake,

  g_bUseReplic,                         // Скрытая! Используется репликация?

  g_bNeedPrintState,                    // СКРЫТАЯ: Проставлять статус Распечатан в заказе и раскрое
  g_bChangeAddDepartment,               // Возможность добавлять/изменять отделы участвующие в согласовании заказа
  g_bEnableDelFreeZone,                 // Возможность удаления записей в свободной зоне
  g_bViewAnotherTask,                   // Разрешён просмотр чужих заказов?
  g_bShowAnotherTask,                   // Разрешено видеть чужие заказы в списке заказов
  g_bCanSavePlot,                       // Возможность сохранять чертёж

  g_bTypeOrder0,                        // Показывать пользователю заказы типа 0, 1, 2
  g_bTypeOrder1,
  g_bTypeOrder2,
  g_bEnabledSetTaskStatusSaw,           // Скрытая!! Разрешение ставить вручную статус заказа "Раскроен"
  g_bEnableConditionalFormulaReplace,   // Скрытая!! Разрешение на разную замену значений в формуле для стекла и стеклопакетов
  g_bStrictSheduleOperatorAssign,       // Скрытая!! Назначение SheduleOperator для процессингов строго по таблице календаря

  g_bEnableOpenCV;                      // Скрытая. Разрешено оцифровывать шаблоны с использованием OpenCV?

extern long
  g_idUser,              // ID пользователя, под которым выполнен вход
  g_lNewTaskType,
  g_lTaxRate,
  g_lRoundArea,          // число знаков округления для суммарной площади всех изделий  в позиции
  g_lRoundArea_UnitPos,  // число знаков округления для площади одного изделия в позиции
  g_lSawingGlassCount,   // Максимальное количество стёкол в раскрое.
  g_lCuttingOffset,      // Смещение реза используется при экспорте Optima
  g_lAutosawGlassCount,  // Количество стёкол в автораскрое, минимальное.
//g_lSawingGPCount,      // Максимальное количество пакетов в раскрое.
  g_lAccuracy,           // Разрешаем поиск резервных СП со склада.
  g_lTaskAccountNum,     // нумерация заказов: 0 - по клиентам, 1 - сквозная, 2 - по продавцу.
  g_lCalcFactEnumerate,  // нумерация счетов фактур: 0 - по номеру заказа, 1 - сквозная, 2 - вручную, 3 - вручную (по продавцам).
  g_lTaskNumEnumerate,   // нумерация счетов: 0 - по номеру счет фактуры, 1 - сквозная, 2 - по номеру заказа, 3 - по продавцу.
  g_lManufTaskEnumerate, // нумерация заданий в пр-во: 0 - сквозная, 1 - по номеру наряд заказа.
  g_lGlassOrderType,     // тип сортировки стёкол:
                         // 0 - по Размеру,
                         // 1 - по Заказу, Позиции,
                         // 2 - Заказу, Кол-ву камер, Размеру,
                         // 3 - Заказу, Размеру
                         // 4 - Клиенту, Размеру
                         // 5 - по убыванию Ширины
                         // 6 - Клиенту, Заказу, Кол-ву камер, Размеру
                         // 7 - Пользовательская (настраивается в скрипте)
  g_lDepotSubDivisionToSaw,     // Подразделение, заказы которого можно добавлять в раскрой
  g_lDepotSubDivisionShip,      // Подразделение, отгрузки которого высвечиваются
  g_nPlotSizeX,                 // Размер эскиза для отчётов Х (пишем эскиз в Plot.Plot)
  g_nPlotSizeY,                 // Размер эскиза для отчётов Y (пишем эскиз в Plot.Plot)
  g_nPlotProjectSizeX,          // Размер эскиза для отчётов Х (пишем эскиз в Project.Plot)
  g_nPlotProjectSizeY,          // Размер эскиза для отчётов Y (пишем эскиз в Project.Plot) 

  g_dAngleArcOffCutMin,         // Критический угол примыкания арки, при котором ещё не ставится кромка. По умолчанию 30 градусов

  g_nTypeOperMain,              // Приоритетная операция
  g_nTypeCalcPriceNewClient,    // Тип расчета цены для нового клиента 

  g_nTypeCalcPriceNDS,          // СКРЫТАЯ: Тип расчета НДС по методу округления

  g_lCashOrderEnumerate,        // Тип нумерации кассовых ордеров (0 - по продавцу, 1 - сквозная)
  g_lTypeExportOptyWay,         // [YK] Тип экспорта данных для оптивэй 0 - стандарт, 1 - Стекломир Астана/Павлодар
  g_lNumberDefTabInProjectView, // [SE] !!!СКРЫТАЯ ОПЦИЯ !!! Номер закладки по умолчанию в окне позиций.

  g_lBilletMaxSizeX,            // [YK] Максимальный Размер заготовки(листа) для оптимизации Х прописан в коде
  g_lBilletMaxSizeY,            // [YK] Максимальный Размер заготовки(листа) для оптимизации Y прописан в коде
  g_lDiffBetweenGlass,          // Разница в размере стёкол в стопке А-пирамиды

  g_lDiffBetweenGlass_X,        // Разница в размере стёкол в стопке А-пирамиды - ширина - настройка для комбинированной сортировки
  g_lDiffBetweenGlass_Y,        // Разница в размере стёкол в стопке А-пирамиды - высота - настройка для комбинированной сортировки

  g_lDistBetweenGlassPack,      // Расстояние мм. между пачками на А-пирамиде

  g_lMinDetWidth_APyr,          // Минимальная ширина детали для А-пирамиды (чтобы не упала между опорами), если меньше, кладём лёжа

  g_idDepotSubDivision,         // К какому подразделению привязан пользователь, такие заказы и отображаем (раскрои, отгрузки)
  g_idDepartment,            // К какому отделу принадлежит пользователь

  g_lLaserMarkW,             // Лазерная маркировка: ширина мишени (W) по умолчанию
  g_lLaserMarkH,             // Лазерная маркировка: высота мишени (H) по умолчанию
  g_lLaserMarkB,             // Лазерная маркировка: расстояние снизу (B) по умолчанию
  g_lLaserMarkL,             // Лазерная маркировка: расстояние слева (L) по умолчанию
  g_lLaserMarkCorner;        // Лазерная маркировка: угол по умолчанию

extern EGPSOptExportGlassPyrType
         g_lGlassPut;                  // Раскладка по пирамидам: 0 - А-образные, 1 - Арфа-образные, 2 - Арфа-образным, где разные стекла в разных пирамидах.

extern EGlassMarkCovering
         g_nGlassMarkCovering;         // [SE] Настройка обозначения стороны покрытия стекла в формуле СП

extern ESidePlot
         g_nSidePlot;                  // [YK] Настройка вида чертежа с улицы или из помещения

extern ETypeCalcBarcodeOrder
         g_lTypeCalcBarcodeOrder;      // [SE] !!!СКРЫТАЯ ОПЦИЯ !!! Тип расчета сортировки штрихкодов.

extern ENewTaskClientSelectPolicy
         g_nNewTaskClientSelectPolicy; // [OK] !!!СКРЫТАЯ ОПЦИЯ !!! Политика выбора клиента для нового задания

extern ETypeDefineAreaGP
         g_nTypeDefineAreaGP;          // [SE] Настройка тип определения диапазона площади

extern ETypeRejectDialog
         g_nTypeRejectDialog;          //[SE] Тип отображения диалога выбора комбинации браков

extern ESawTypeRezervSpace
         g_lSawTypeRezervSpace;        // Тип резервирования свободных ячеек в пирамиде: 0 - не резервировать, 1 - для стекол с обработкой, 2 - для всех стекол, которые в другом раскрое

extern EMarginCoef_Show  g_eMarginCoef_Show;    // Как отображать пользователю маржу

extern ETypeMassCalc
         g_nTypeMassCalc;             //[SE] Метод расчета массы изделия

extern ETaskPriorityCalcType
         g_eTaskPriorityCalc;         //[SE] Метод расчета массы изделия

extern long
  g_idLastRasManuf,      // Свойства шпрос последнего редактируемого чертежа
  g_idLastRasColor,      // по умолчанию эти свойства будут применены при создании новых чертежей
  g_idLastRasColorIns,
  g_idLastRasWidth,
  g_lCatalogForm,       // Каталог форм  0-нет 1-Бистроник 2-Лисик 3-плагин
  g_lAssemblyLine,      // Линия сборки
  g_lCurLabelRep,       // Индекс текущего шаблона отчётов для этикеток в комбобоксе.
  g_lSawTripPackType,   // Тип упаковки заказов выбранного раскроя.
                        // 0 - (Не выбран)
                        // 1 - Стопками
                        // 2 - Пирамидами
                        // 3 - Заказ в стопке
                        // 4 - Заказ на пирамиде
  g_lSawDateShift,
  g_lShipDateShift,
  g_lHour_ShipDate_NextDay, // Час после которого текущую дату отгрузки нужно сдвинуть на завтра, 0 - без сдвига
  g_lTaskStateForPayment,
  g_lFrameSizeDecrement,
  g_lDistFrameGlass,    // раcстояние от края рамки до края стекла, используется при выгрузке на гибочник BSV
  g_lDBType;            // 0 - обычная, 1 - офис, 2 - цех

extern double
  g_dMinCutterOffsetFromEdge,  // Минимальный отступ чтобы резец не прошёл по краю 
  g_dMaxAreaShipDate,          // Максимальная площадб СП на данную дату отгрузки.
  g_dReportDateBegin,          // начало периода для вызываемых отчетов.
  g_dReportDateEnd,            // конец периода для вызываемых отчетов.
  g_dTaskReplicTestDateBegin;  // [ab] Начальная дата, с которой проверять время отсылки и получения заказов по реплике.
extern int
  g_iTaskReplicTestMin;        // Кол-во минут допустимое для ожидания пересылки и получения.

extern float
  g_fCrossPrice,        // Цена креста шпросс.
  g_fMarginRas,         // Наценка на нестандартные шпросы
  g_fMinWidthGP,        // Минимальная  ширина  СП.
  g_fMaxWidthGP,        // Максимальная ширина  СП.
  g_fMinHeightGP,       // Минимальная  высота  СП.
  g_fMaxHeightGP,       // Максимальная высота  СП.
  g_fMinAreaGP,         // Минимальная  площадь СП.
  g_fMaxAreaGP;         // Максимальная площадь СП.

// Опции оптимизации раскроя:
extern int
  g_bCuttingUseThread1,                   // Использовать поток оптимизации раскроя 1.
  g_bCuttingUseThread2,                   // Использовать поток оптимизации раскроя 2.
  g_bCuttingUseThread3;                   // Использовать поток оптимизации раскроя 3.
extern int
  g_lCuttingDepth,
  g_lCuttingPortion,                      // Количество деталей в порции. Используется и для оптимизации на уровне полосы, и для оптимизации на уровне края листа.
  g_lCuttingWarmingUpTime,                // Время прогрева алгоритма оптимизации раскроя, максимальный % времени на расчёт одного варианта.
  g_lCuttingMinSize,                      // Минимальное количество особей в популяции ГА.
  g_lCuttingMaxSize,                      // Максимальное количество особей в популяции ГА.
  g_lCuttingEvoStep,                      // Максимальное количество шагов ГА оптимизации раскроя.
  g_lCuttingOptTime,
  g_lCuttingOptTime_10_Details,           // Время на оптимизацию 10 деталей раскроя, для автоматического расчёта времени оптимизации раскроя в зависимости от количества деталей.
  g_lCuttingMaxXStripeWidth,              // Максимальная ширина X-полосы.
  g_lCuttingJoinRestToInnerMargin,        // Включать остаток листа во внутренний отступ, мм.
  g_lCuttingTextSizeOfSheet,
  g_lCuttingTextSizeOfDetailSize,
  g_lCuttingTextSizeOfDetailName,
  g_lCuttingTextSizeOfMargin,             // Размер шрифта для кромок  
  g_lCuttingPlotSizeX,                    // Размер чертежа карты по Х в пикселях
  g_lCuttingPlotSizeY,                    // Размер чертежа карты по Y в пикселях
  g_lCuttingFillAdditionalBySawTask,      // Заполнять остатки деталями из следующих оптимизаций, количество следующих оптимизаций.
  g_lCuttingFillAdditionalByDay,          // Заполнять остатки деталями из следующих оптимизаций, количество следующих дней.
  g_lMaxCountDayLocateOnTechPyramid,      // ПОКА СКРЫТАЯ!! Максимальное кол-во дней присутствия детали на технологической пирамиде
  g_lMaxCountDayFillTechPyramid,          // ПОКА СКРЫТАЯ!! Максимальное кол-во дней для заполнения деталей на технологические пирамиды
  g_lDialogEditPosTask;                   // [SE] СКРЫТАЯ!! Диалог редактирования позиции заказа(0 - DlgFilm, 1 - DlgProduct
extern int
  g_bCuttingUseStepAglo,                  // Использовать ступенчатый поиск при  заполнении остатков деталями из других заданий на раскрой 
  g_bCuttingUseStripeXMargin,
  g_bCuttingUseThroughYCut,
  g_bCuttingOperationsEnabled,            // Включить функции оптимизации раскроя стекла в попап-меню гридов окна заданий на раскрой

  g_bRotateBillet,                        // Поворачивать заготовки в горизонтальное положение.
  g_bRotateBilletRest,                    // Поворачивать дел. отход в горизонтальное положение.

  g_bSortByTwinPriority,                  // Обеспечить порядок выхода деталей из раскроя в соответствии с двойным приоритетом деталей.
  g_bCuttingFileWritePyramid,             // Писать в файл выгрузки  Арфа пирамиды 
  g_bAddMarkDetailForBystronic,           // Экспорт в файл для стола Bystronic маркировки детали из чертежа раскроя (первые 9 символов).
  g_bCuttingMarkCutOffAsDetail,           // Помечать деловые остатки как детали.
  g_bCuttingUseBlackWhiteSchemeColor,     // Использовать черно-белую схему раскраски карты раскроя
  g_bCuttingShowMargins,                  // Показывать значения кромки на чертеже карт раскроя
  g_bCuttingShowLaserMark,                // Показывать область лазерной маркировки на картах раскроя
  g_bCuttingUseCuttingDrawIniForHeadless, // Применять draw-настройки CuttingDraw.ini в headless-отрисовке карт
  g_bCuttingUseSpecialLisecVCut,          // Использовать специальный V-рез для Лисек. // Создавать только одну деталь на полосе 4 уровня.
  g_bCuttingUseSpecialVCutOnlyEquals,     // Использовать специальный V-рез для Лисек. // Создавать только одинаковые детали на полосе 4 уровня.
  g_bAutoDetectCutOff,                    // Автообнаружение ДО с добавлением в справочник
  g_bAddNumAndCountDetailOnType,          // добавлять к маркировке детали на карте раскроя номер и кол-во однотипных деталей
  g_bCuttingWithoutRightTopMargin,        // Не соблюдать минимальный отлом MarginInner с правого и верхнего края листа.
	g_bCuttingSortingYStripesPriority,      // Сортировать Y полосы по приоритету (постобработка после завершения оптимизации раскроя).
  g_bCuttingBreakSortingWidthForPriority,        // Нарушать сортировку лесенкой для выхода по приоритету (постобработка после завершения оптимизации раскроя). 
  g_bCuttingDisableHorizontalOffcutOnLastSheet,  // Запретить горизонтальный ДО на последнем листе.
  g_bCreateContainerWithMargins,                 // Создавать контейнер с внутренним отступом
  g_bSortSawOnOptimize,                   // Сортировать раскрой при оптимизации
  g_bCuttingUseFolderForFileName,         // Использовать подпапки для файлов экспорта на стол раскроя
	g_bCuttingSortingXStripesByAscAPyramid, // Сортировать X полосы по возрастанию положения на А-пирамиде
  g_bCuttingSortingYStripesByDscAPyramid; // Сортировать Y - полосы на листе по убыванию положения деталей на A - пирамиде

extern ECut_Detail_Mark  g_eCutDetMark;   // Как маркировать деталь на чертеже

extern e_TypeCalcGabarit g_eTypeCalcGabarit;     // Тип счёта габарита

extern EModeGrind        g_eModeGrind;           // Режим формирования областей снятия покрытия
extern e_TypeCalcOffCut  g_ePlotTypeCalcOffCut;  // Тип определения кромок

extern eExportDopParam   g_eExportDopParam;      // Дополнительные параметры для экспорта на стол раскроя

// Опции настройки упаковки:
extern bool
  g_bUseEqualWidth,             // Ставить в стопку пакет одинаковой ширины и меньшей высоты.
  g_bUseEqualWidth2,            // Ставить в стопку пакет одинаковой ширины и любой высоты (при одинаковой ширине перепад высот не регламентируется).
  g_bUseEqualHeight,
  g_bUseAutoCloseAllPyramids,   // [AF] Закрывать все пирамиды.
  g_bUseAutoClosePyramids,      // [AF] Закрывать все пирамиды, кроме последней.
  g_bUseLevels,                 // [AF] Разрешить упаковку этажами.
  g_bUseBalancing,              // [AF] Разрешить балансировку пирамид после упаковки.
  g_bUseCentering,              // [AF] Разрешить центирировать стопки после упаковки.
  g_bUseCrossPostProcessing,    // [AF] Разрешить центирировать стопки после упаковки.
  g_bUseNewMethod,              // [AF] Новый алгоритм оптимизации Стопок.
  g_bUseGreedyMethod,           // [AF] Использовать жадный алгоритм.
  g_bUseSinglePassPacking,      // [AF] Использовать упаковку за один проход.
  g_bUsePackingSettings,        // [AF] Использовать правила отгрузки.
  g_bWarnOfVirtualPyramids,     // [AF] Предупреждать об использовании виртуальных пирамид в упаковке.
  g_bGroupGpOnVirtualPyramids,  // [AF] Группировать СП на виртуальных пирамидах.
  g_bGroupByPriority,           // [AF] Группировать СП в раскрое по приоритету.  
  g_bGroupByClient,             // [AF] Группировать СП в раскрое по клиенту.
  g_bGroupByCamera,             // [AF] Группировать СП в раскрое по количеству камер.
  g_bGroupByFrameThick,         // [AF] Группировать СП в раскрое по толщине рамки.
  g_bGroupByFrameFormula,       // [AF] Группировать СП в раскрое по формуле рамки.
  g_bSortByQuant,               // [ab] Сортировать по кванту по пачкам
  g_bGroupQuantGlass,           // [ab] Группировать стекло в нарезку в отдельный квант
  g_bAddToAnyDate,              // [AF] При упаковке раскроя Дополнять открытые пирамиды на любую дату.
  g_bAskTripDate,               // [AF] При упаковке раскроя Предлагать выбрать дату отгрузки.
  g_bUseBTemplate,              // [AF] Паковать стеклопакеты с признаком "По шаблону" (с bTemplate == 1).
  g_bUseBSelfDelivery,          // [AF] Паковать стеклопакеты с признаком "Самовывоз".
  g_bUseSingleGlass,            // [AF] Паковать стеклопакеты с признаком "Стекло".
  g_bDifferentPriorityOnPack,   // [AF] Разрешить ставить пакеты с разным приоритетом в одну стопку.
  g_bPackOneLayer,              // [YK] Паковать в один слой, порядок производства изделй определяется внутри пирамиды
  g_bUpdateExistingTowers,      // [AF] Дополнять существующие стопки.
  g_bReduceVerticalStairs,      // [AF] Сокращать вертикальные лесенки (многопроходный алгоритм).
                                // TODO: [ab]->[af] Цель сократить не только вертикальные лесенки, но и горизонтальные
  g_bReverseOrderArfa,          // [DP] В Арфа-пирамиде раскладывать стёкла СП в обратном порядке.
  g_bUpdateProjectNumAfterDel,  // [MR] Обновлять ли номера позиций заказа после удаления позиций
                                // необходимость возникла в Стисе
                                // В БД Электросталь - true, в остальных БД - false
  g_bCuttingLastPacking,        // [SE] Скрытая пока!!! Нарезку в упаковке размещать в конце. 
                                //  Для Тольятти Стеклопакетка = true, для остальных = false;

  g_bShowPaymentInfo,              // Показывать информацию о раскроях в диалоге клиентов.
  g_bReverseOrderFirstGlassCover;  // [SE]Скрытая пока!!! Обратная сортировка стекол, если первое стекло с покрытием

extern long
  g_lWidthInc,
  g_lWidthDec,
  g_lToleranceEqualWidth,
  g_lHeightInc,
  g_lHeightDec,
  g_lHeightIncToLowest,
  g_lWidthIncGL,
  g_lWidthDecGL,
  g_lHeightIncGL,
  g_lHeightDecGL,
  g_lHeightIncToLowestGL,
  g_lPackSpace,
  g_lGPSpace,
  g_lFrameWidth,
  g_lMaxPackOnSide,
  g_lMaxLevelCount,             // Максимальное количество стопок в этажерке.
  g_lMaxLevelHeight,            // Максимальная высота этажерки.
  g_lLevelDistance,             // Расстояние между этажами.
  g_lTimeFactor,                // Время упаковки каждых 10 пакетов, секунд.
  g_lQuantOver,                 // [ab] Перехлёст квантов
  g_lQuantUnion,                // Объединять Кванты менее указанного количества штук СП
  g_lQuantLarge_Width,          // Большой СП в отдельный квант
  g_lQuantLarge_Height,
  g_lPackOrder,
  g_lGlassAPyrOrder,
  g_lTripPyrOrder,
  g_lWidthIncG,                 // Увеличение ширины след. стекла на А-пирамиде.
  g_lHeightIncG,                // Увеличение высоты след. стекла на А-пирамиде.

  g_lCorkPadGlassThikness,      // Толщина пробковой прокладки между стёклами на А-пирамидах в процессе обработки (не для отгрузки)
  
  g_lWeekEndSmena,              // Кол-во смен в рабочую субботу
  g_lWeekDaySmena,              // Кол-во смен в будние дни
  g_lPackingCountPyramid;       // Упаковка кол-во пирамид для упаковки 

// Опции настройки размерных линий:
extern bool
  g_bDimOneLevel,               // размерные линии в одном уровне
  g_bDimClose,                  // замыкать размеры
  g_bDimGabarit,                // отображать габарит
  g_bDimAngle,                  // отображать величину углов
  g_bPlotRas3D,                 // Шпросы с толщиной 
  g_bShowGrindTrack,            // показать треки шлифовки
  g_bPlotGlassTexture,          // Показать текстуру стекла
  g_bDimRad,                    // СКРЫТАЯ!!! Отображать величину радиусов

  g_bCombineShprosIfNeed;       // При сохранении - комбинировать врезные шпросы, если надо
                                // Это опция управления графикой, выставляется импортом XML при импорте,
                                // если включена опция g_bCombineNotchShpros

extern EPlotShprosMode       g_eSprosPlotMode;
                                
extern EPlotAreaMethod       g_ePlotAreaMethod;         // Метод расчета площади фигуры (по умолчанию метод прямоугольника)
                                
extern EPathAttachmetsMethod g_ePathAttachmentsMethod;  // Метод вложения файлов (по умолчанию в базу данных)
                                
extern long
  g_lMagnetRadius,
  g_lLevelWeight,
  g_lFontHeight,
  g_lFontHeight_ForReport,
  g_lMinFontHeight,
  g_lMinFontHeight_ForReport,
  g_lTxtRight,
  g_lDeltaLef,
  g_lOutSize,

  g_nBigGlassSizeX,                         // Мин. размер по Х стекла, которое считается большим
  g_nBigGlassSizeY,                         // Мин. размер по Y стекла, которое считается большим
  g_nSerialProjectCount,                    // К-во штук в прожекте, чтобы считать его серийным

  g_nMaxCountFrameRack,                     // Макс. кол-во "гусей" на раскрой
  g_nCountCellFrameRack;                    // Кол-во вешалок в "гусе"

extern double
  g_dBigGlassThickness,                     // Мин. толщина стекла, которое считается большим
  g_dBigGlassMass,                          // Мин. масса стекла, которое считается большим

  g_dDeltaHeightCoef,                       // Коэффициент выноса размерных линий для чертежа в окне      
  g_dDeltaHeightCoef_ForReport,             // Коэффициент выноса размерных линий для эскиза отчота
  g_dWidthGrindTool,                        // Ширина шлифовального инструмента
  g_dGrindToolShift,                        // Смещение шлифовального инструмента
  g_dMaxAngle_SkipBeginPoint;               // Макс. разница м/у углами поворота линий фигуры для их объединения при выгрузке

// [AF] Настройки для оптимизации закалки стекла:
extern double
  g_dTempering_DetailSquare_Ratio,          // Разница в площади деталей в одной порции закалки, 1.25 == 25% больше размер набольшей детали над наименьшей.
  g_dTempering_DetailHeight_Ratio,          // Разница в высоте деталей в одной порции закалки, для второго критерия определения деталей в одной порции.
  g_dTempering_DetailWidth_Ratio,           // Разница в ширине деталей в одной порции закалки, для второго критерия определения деталей в одной порции.
  g_dTempering_BigDetail_Mass,              // Масса большой детали в закалке, которая снимается со стола влево, кг.
  g_dTempering_BigDetail_Square,            // Площадь большой детали в закалке, которая снимается со стола влево, кв.м.
  g_dTempering_BigSmallDetail_Square_Ratio, // Отношение площадей деталей в порции, при превышении которого детали считаются большими и маленькими.
                                            // Большие детали в порции НЕ должны идти за маленькими деталями, иначе начинается новая порция.
  g_dTempering_MarginTempering;             // Расстояние между деталями в печи для закалки стекла.
extern bool
  g_bTempering_InOrder_RowRightToLeft,      // Направление закладки деталей на стол  закалки: рядами справа налево; иначе рядами    слева  направо.
  g_bTempering_OutOrder_RowLeftToRight;     // Направление выхода   деталей со стола закалки: рядами слева направо; иначе колонками сверху вниз.

// [AF] Настройки для раскладки закалки стекла рядами:
extern double
  g_dTemperingCreateByRow_RowRatio,           // Максимальное допустимое соотношение между самым широким рядом и самым узким рядом.
  g_dTemperingCreateByRow_HeightInRowRatio,   // Максимальное допустимое соотношение между максимальной высотой детали в ряду и минимальной высотой детали в ряду.
  g_dTemperingCreateByRow_DetailSquareRatio;  // Максимальное допустимое соотношение между максимальной площадью детали на листе и минимальной площадью детали на листе.
extern bool
  g_bTempering_UseDetailRotation,             // Разрешить поворот деталей во время оптимизации закалки.
  g_bTempering_UseMultiStripe;                // Разрешить раскладывать детали в несколько колонок во время оптимизации закалки.

// [AF] Настройки для оптимизации контейнеров непрямоугольных деталей:
extern double
  g_dCuttingContainerMinEstimation;           // Минимальная желаемая оценка контейнера: Во сколько раз площадь контейнера меньше площади габаритов составляющих его деталей.

extern ETemperCreateType  g_eTemperChessType;    // Тип шахматной раскладки

// [OK] Глобальные переменные фильтра настройки списания материалов
extern long
  g_idObjectType,
  g_idProdType,
  g_idProduct,
  g_idManufacteur,
  g_idWidthType,
  g_idColor,
  g_idColorIns;

// [OK] Интервалы для автоформирования НЗ
extern long
  g_nDistribute_Zak,             // Закалку ставить заранее на столько дней    
  g_nDistribute_Figure,          // Фигуры 
  g_nDistribute_Shpros,          // Шпросы     
  g_nDistribute_Film,            // Плёнка декор 
  g_nDistribute_FilmArm;         // Плёнка арм    


// Настройки шрифтов 
extern CFont g_FontApproval;

extern bool     
  g_bFontUnderline,
  g_bFontBold,      
  g_bFontStrikeout, 
  g_bFontItalic,
  g_bFontGridUnderline,
  g_bFontGridBold,      
  g_bFontGridStrikeout, 
  g_bFontGridItalic;

extern CString  
  g_sFontFaceName,
  g_sFontGridName,
  g_sFontGridApproveName,
  g_sidProjectCopyList;

extern int      
  g_lFontFaceWidth,
  g_lFontFaceHeight,
  g_lFontGridWidth,
  g_lFontGridHeight,
  g_lFontGridApproveSize,
  g_iRoundDimValue,

  g_lTimeForCheckUpdate;                          // Таймер проверки обновления

extern COLORREF  g_color_Hilight_Glass,           // Подсветка в гриде соответствующей строки выбору в другом гриде
                 g_color_Hilight_BarCode,         // Подсветка штрих-кода
                 g_color_Hilight_GlassDetails;    // Подсветка детали GlassDetails

//--- ПОЧТА ---
extern CString
  g_sSMTPServer,             // Сервер SMTP
  g_sSMTPAccount,            // Аккаунт SMTP
  g_sSMTPPassword,           // Пароль SMTP
  g_sSMTPDomainUser,         // Домен для рассылки
  
  g_sPOP3Server,             // Сервер POP3
  g_sPOP3Account,            // Аккаунт POP3
  g_sPOP3Password;           // пароль POP3

extern long
  g_lSMTPPort,               // Порт SMTP
  g_lPOP3Port;               // Порт POP3

extern bool
  g_bSMTP_Autentification,   // Использовать SMTP аутентификацию 
  g_bSMTP_UseTLS,            // Использовать SMTP TLS\SSL
  g_bSMTP_AuthPOP3_Before,   // Предварительная аутентификация POP3 перед работой с SMTP
  g_bPOP3_UseTLS;            // Использовать POP3 TLS\SSL
//--- ПОЧТА КОНЕЦ---

extern HANDLE  g_hMutex_RunProc_Update;    // Запущена ли параллельная процедура, которая может вызвать взаимоблокировку?


enum ELiOptExport
{
  e_liopt_pool     = 0,
  e_liopt_transfil = 1
};
extern ELiOptExport g_eLiOptExport;

enum ESawNumbering  // Нумерация раскроя.
{
  e_through = 0,    // сквозная
  e_daily   = 1     // подневная
};
extern ESawNumbering g_eSawNumbering;

// Текущие статусы nState для Task.nState, Project.State, BarCode.nState
enum ETaskState
{
  // Переводим статусы на битовую маску
  e_TaskState_Unknown         =       -1,
  e_TaskState_Before          =        0,  // Предзаказ
  e_TaskState_New             =        1,  //  1 бит Новый
  e_TaskState_ToManuf         =        2,  //  2     Производить
  e_TaskState_Saw             =        4,  //  3     Раскроен
  e_TaskState_Deliv           =        8,  //  4     Отгружен
  e_TaskState_ChangeDep       =       16,  //  5     Изменено подразделение
  e_TaskState_Print           =       32,  //  6     Распечатан
  e_TaskState_InManuf         =       64,  //  7     В производстве
  e_TaskState_Complite        =      128,  //  8     Изготовлен
  e_TaskState_Planed          =      256,  //  9     Запланирован
  e_TaskState_Reject          =      512,  // 10     Штрих-код забракован
  e_TaskState_Pack            =     1024,  // 11     Упакован
  e_TaskState_Remake          =     2048,  // 12     Переделан
  e_TaskState_SentEmail       =     4096,  // 13     Отправлено письмо на email, что заказ не получен по реплике
  e_TaskState_PartShip        =     8192,  // 14     Частично отгружен
  e_TaskState_PartComplite    =    16384,  // 15     Частично готов
  e_TaskState_Suspend         =    32768,  // 16     Приостановлен
  e_TaskState_Temper_Exist    =    65536,  // 17     Закалка присутствует
  e_TaskState_Temper_All      =   131072,  // 18     Все стёкла в закалке
  e_TaskState_Approved        =  1048576,  // 19     Проверен, одобрен в производство
  e_TaskState_Coordination    =  2097152,  // 20     На согласование
  e_TaskState_Agreed          =  4194304,  // 21     Согласовано
  e_TaskState_Rejected        =  8388608,  // 22     Отклонено
  e_TaskState_Client_Rejected = 16777216,  // 23     Отказ клиента
  e_TaskState_Plot_Processing = 33554432,  // 24     Обработка чертежей
  e_TaskState_Prod_Processing = 67108864,  // 25    Обработка ПДО
  e_TaskState_Scan            = 0x8000000, // 26    Частично отсканированный 134217728
  e_TaskState_FullScan        = 0x10000000,// 27    Отсканированный 268435456
};

enum EGPSOptExport
{
  e_gpsopt_PyrSel                 = 0,
  e_gpsopt_TaskPos                = 1,
  e_gpsopt_OptimPos               = 2,
  e_gpsopt_SawGlassPos_by_TaskPos = 3,
  e_gpsopt_PyramidOrder           = 4, 
  e_gpsopt_PyrSelPriority         = 5 
};

extern EGPSOptExport g_eGPSOptExport;

// Сохранение файла на гибочник
enum ESaveFileCurve
{
  e_SaveFileCurve_SawTask = 0,    // "1 файл на раскрой"; 1-"1 файл на заказ в раскрое"
  e_SaveFileCurve_Task    = 1,    // "1 файл на заказ в раскрое"
};

extern ESaveFileCurve g_eSaveFileCurve;

enum ESaveCurve_XY                 // Как записывать размеры на гибочник
{
  e_SaveCurve_Height_Width = 0,    // Сначала Высота затем Ширина
  e_SaveCurve_Width_Height = 1     // Наоборот Ширина, Высота
};

extern ESaveCurve_XY  g_eSaveCurve_XY;

// [SS]  В протоколе контекст операций задвоен - непонятно почему 
//       так же не вижу использования признаков bGlass bWinCad - зачем оно надо - где жа наша универсальность
//       
enum EProtocolContexts
{
  e_pc_ChangePrice             = 1,      // 1	Изменение цены
  e_pc_AddCHtoOBZ              = 2,      // 2	Добавление ЧЗ к ОБЗ
  e_pc_Edit                    = 3,      // 3	Редактирование
  e_pc_Delete                  = 4,      // 4	Удаление
  e_pc_Add                     = 5,      // 5	Добавление
  e_pc_ReadReplic              = 6,      // 6	Чтение реплики
  e_pc_EndReadReplic           = 7,      // 7	Окончание чтения реплики
  e_pc_ChangeClient            = 8,      // 8	Изм.Клиента
  e_pc_ChangeAccount           = 9,      // 9	Изм.Номера заказа
  e_pc_RecalcCalcFact          = 10,     // 10	Пересчёт счёт-фактуры
  e_pc_CalcCalcFact            = 11,     // 11	Расчёт счёт-фактуры
  e_pc_UpdateOBZtoCHZ          = 13,     // 13	Обновление ОБЗ от ЧЗ
  e_pc_DeleteDepotDoc          = 20,     // 20	Удаление склад.док.
  e_pc_e_pc_ReadReplic2        = 21,     // 21	Чтение реплики
  e_pc_Add2                    = 22,     // 22	Добавление
  e_pc_EndReadReplic2          = 23,     // 23	Окончание чтения реплики
  e_pc_Edit2                   = 24,     // 24	Редактирование
  e_pc_Replic                  = 30,     // 30	Реплицирование
  e_pc_Entery                  = 31,     // 31	Вход
  e_pc_Exit                    = 32,     // 32	Выход
  e_pc_PlanToTeam              = 33,     // 33	Планирование на бригаду
  e_pc_ExportToGPSopt          = 34,     // 34	Экспорт в GPS.opt
  e_pc_PrintEtiket             = 35,     // 35	Печать этикеток
  e_pc_ChangeState             = 36,     // 36	Изменение статуса
  e_pc_OutputGibichnik         = 37,     // 37	Выдача на гибочник
  e_pc_ParseTask               = 38,     // 38	Разбор заказа
  e_pc_Impor                   = 40,     // 40	Импорт
  e_pc_PlanInsetTask           = 41,     // 41	Планир.: вставка заказа
  e_pc_PlanToDate              = 42,     // 42	Планир.на дату
  e_pc_AddtoReg                = 43,     // 43	Добавление док-а в регистр
  e_pc_DeltetoReg              = 44,     // 44	Удаление док-а из регистра
  e_pc_AlertEqual              = 45,     // 45	Предупр.: совпад. номера заказа
  e_pc_AddGPSaw                = 46,     // Добавление СП в раскрой
  e_pc_DelGPSaw                = 47,     // Удаление СП из раскроя
  e_pc_SendWaitReplic          = 52,     // Отправка редких стёкол в производство
  e_pc_ReCalcTimeGlassProcSaw  = 54,     // Пересчет планируемого времени выполнения изделия в раскрое
  e_pc_ScanGP                  = 998,    // 998	Сканирование СП
  e_pc_ScanPyr                 = 999     // 999	Сканирование пирамиды
};

enum EProtocolTableNames
{
  e_pt_Application      = 0,    //   0	Приложение
  e_pt_Task             = 1,    //   1	Заказ
  e_pt_Project          = 2,    //   2	Позиция заказа
  e_pt_Plan             = 3,    //   3	Планирование
  e_pt_Barcode          = 4,    //   4	Штрихкода
  e_pt_OutputCash       = 5,    //   5	Выдача наличных
  e_pt_Cash             = 6,    //   6	Касса
  e_pt_Material         = 7,    //   7	Материалы
  e_pt_SetCriteria      = 8,    //   8	Набор критериев для фурнитуры
  e_pt_Criteria         = 9,    //   9	Критерии фурнитуры
  e_pt_RangeSet         = 10,   //  10	Матрица диапазонов
  e_pt_Pay              = 11,   //  11	Система оплат
  e_pt_Depot            = 12,   //  12	Складские документы
  e_pt_Client           = 13,   //  13	Справочник клиентов
  e_pt_Pyramid          = 14,   //  14	Пирамиды на отгрузку
  e_pt_Commission       = 15,   //  15	Комиссионные
  e_pt_SawTaskMain      = 23    //  23	Раскрои
};

enum EProtocolColumn
{
  e_pc_Task_TimeSendWait    = 89,   // Task.TimeSendWait Время отправки в пр-во редких стёкол
  e_pc_IdProduct            = 380,  // ID Продукции
  e_pc_GlassDetails_idGlass = 402   // Стекло в раскрое
};

// Единицы измерения
enum EUnit
{
  e_unit_area               = 1, // Квадратные метры
  e_unit_piece              = 2, // Штуки
  e_unit_linear_meter       = 3, // Погонные метры
  e_unit_weight_gm          = 4, // Вес в граммах
  e_unit_volume_balloon     = 5, // Объем в балонах
  e_unit_weight_kg          = 6, // Вес в килограммах
  e_unit_piece_pair         = 7, // в парах (по 2 штуки)
  e_unit_volume_liter       = 8, // Объем в литрах
  e_unit_volume_cubic_meter = 9, // Объем в кубометрах
  e_unit_volume_milliliter  = 10 // Объем в миллилитрах
};

enum ETypeOper : long           // ProjectItem.nTypeOper - Тип Операции обработки стекла
{                               // [ab] Укажем тип, чтобы можно было в Recordset писать
  e_TypeOper_None        =  0,  // Нет типа обработки
  e_TypeOper_Film        =  1,  // Пленка
  e_TypeOper_Harding     =  2,  // Закалка
  e_TypeOper_Triplex     =  3,  // Триплекс
  e_TypeOper_Drilling    =  4,  // Сверление
  e_TypeOper_Grinding    =  5,  // Шлифовка
  e_TypeOper_Emalit      =  6,  // Эмалит (покраска стекла)
  e_TypeOper_Blunting    =  7,  // Притупление
  e_TypeOper_Vitrage     =  8,  // Витраж
  e_TypeOper_Vent        =  9,  // Клапан
  e_TypeOper_HardingBend = 10,  // Закалка c моллированием
  e_TypeOper_Cutout      = 11,  // вырез
  e_TypeOper_SandBlast   = 12,  // Пескоструй
  e_TypeOper_Notch       = 13   // вырез внутри контура
};

enum EbitTypeOper                  // ProjectItem.bitTypeOper
{
  e_bitTypeOper_Harding     =   1, // Закалка
  e_bitTypeOper_Film        =   2, // Пленка
  e_bitTypeOper_Triplex     =   4, // Триплекс
  e_bitTypeOper_Drilling    =   8, // Сверление
  e_bitTypeOper_Grinding    =  16, // Шлифовка
  e_bitTypeOper_Emalit      =  32, // Эмалит (покраска стекла)
  e_bitTypeOper_Blunting    =  64, // Притупление
  e_bitTypeOper_Vitrage     = 128, // Витраж
  e_bitTypeOper_Vent        = 256, // Клапан
  e_bitTypeOper_HardingBend = 512, // Закалка c моллированием
  e_bitTypeOper_SandBlast   = 1024 // Пескоструй
};


enum ETypeProd : long                // Соответствует: ProjectItem.nType
{
  e_TypeProd_None            =   0,  // Нет типа продукции
  e_TypeProd_Group_GP        =   1,  // Группа продукций "Cтеклопакет"
  e_TypeProd_Group_Material  =   2,  // Группа продукций "Материалы"
  e_TypeProd_Group_OtherProd =   4,  // Группа продукций "Иная продукция"
  e_TypeProd_Glass           =   5,  // Стекло
  e_TypeProd_Frame           =   6,  // Рамка
  e_TypeProd_Film            =   7,  // Пленка
  e_TypeProd_Obrab           =   8,  // Обработка
  e_TypeProd_Ras             =   9,  // Шпросы
  e_TypeProd_Argon           =  10,  // Аргон
  e_TypeProd_Assembly        =  11,  // Сборка
  e_TypeProd_Paint           =  12,  // Краска
  e_TypeProd_Triplex         =  13,  // Триплекс
  e_TypeProd_OtherProd       =  14,  // Иная продукция
  e_TypeProd_Teokol          =  15,  // Герметик
  e_TypeProd_Sito            =  16,  // Сито
  e_TypeProd_StandardGP      =  20,  // Стандартные СП
  e_TypeProd_Service         =  21,  // Услуги
  e_TypeProd_GlassAgregate   = 105,  // Агрегат стекла 
  e_TypeProd_FrameAgregate   = 106,  // Агрегат рамки 
  e_TypeProd_GPAgregate      = 114   // Агрегат стекло-пакета
};

// Типы сторон у стекла для обозначения напыления, покрытия пленками и т.п.
enum ETypeSide
{
  e_TypeSide_OutSide  = 0, // Наружу
  e_TypeSide_InSide   = 1  // Внутрь
};

// Типы распложения покрытия стекла на столе резки
enum ETypeCoatingOnCutting
{
  e_TypeCoatingOnCutting_Up   = 0, // Вверх
  e_TypeCoatingOnCutting_Down = 1  // Вниз
};

// Типы расположения покрытия стекла в стеклопакете по умолчанию
enum ETypeCoatingOnGP
{
  e_TypeCoatingOnGP_OutGP = 0,  // Наружу СП
  e_TypeCoatingOnGP_InGP  = 1 // Внутрь СП
};

// Тип методики присвоения номера счет-фактуры
enum ETypeCalcFactSetting
{
  e_CF_ByTaskNum        = 0, // 0 - по номеру заказа
  e_CF_Straight         = 1, // 1 - сквозная
  e_CF_Manual           = 2, // 2 - вручную
  e_CF_ManualBySeller   = 3, // 3 - вручную (по продавцам)
  e_CF_StraightOnDemand = 4  // 5 - вручную по команде 
};

enum ETypeCashOrderEnumerate
{
  e_TCOE_BySellet        = 0, // 0 - по продавцу
  e_TCOE_Straight        = 1  // 1 - сквозная
};

// Тип планирования
enum ETypePlan
{
  e_TP_Task              = 0, // 0 - планируем заказы
  e_TP_SawTask           = 1  // 1 - планируем раскрои
};

// [MR] Методы разбора триплекса собственного производства
enum e_TriplexParseMethod
{
  e_TPM_Unique           = 1, // По уникальности имени стекла и плёнки
  e_TPM_Position         = 2  // По положению состовляющих триплекса (четные - пленки, нечетные - стекла)
};

extern e_TriplexParseMethod g_TriplexParseMethod;

// [OK] про методику формирования комплексности известно только это
/*
--[SE] Тип формирования строки ComplexText и ComplexTextRest для Project
-- (0 - по умолчанию, как и раньше было )
-- (1 - добавляем понятие структурного пакета, аргон, эмалит, и если есть форма, то пишем в скобках номер формы) Для ССК
-- (2 - Стис) - [ab] теперь так по умолчанию
*/
enum e_TypeFormatComplexTextProject
{
  e_TFCTP_default        = 0, // по умолчанию, как и раньше было
  e_TFCTP_methodSSK      = 1, // добавляем понятие структурного пакета, аргон, эмалит, и если есть форма, то пишем в скобках номер формы) Для ССК
  e_TFCTP_methodSTIS      = 2, // Стис
  e_TFCTP_methodTehStekla = 3  // Технология стекла (Нижний  Новгород)
};

extern e_TypeFormatComplexTextProject g_eTypeFormatComplexTextProject;

enum ECuttingTypeFileName         // Тип наименования файла на стол раскроя
{
  e_CuttingTypeFileName_Saw                        = 0,  // Как <имя раскроя>
  e_CuttingTypeFileName_Saw_DopGlassName           = 1,  // Как <имя раскроя>_<Дополнительное имя стекла>
  e_CuttingTypeFileName_Saw_GlassName              = 2,  // Как <имя раскроя>_<Имя стекла>
  e_CuttingTypeFileName_Saw_GlassName_DecorCode    = 3,  // Как <имя раскроя>_<Имя стекла>_<Код декора>
  e_CuttingTypeFileName_Saw_DopGlassName_NameLisec = 4   // Как <имя раскроя>_<Дополнительное имя стекла>_<ИмяЛисек>
};

extern ECuttingTypeFileName g_eCuttingTypeFileName;

/////////////////////////////////////////////////////////////////////////////
// Начало группы Enum для Bystronic
enum ECuttingTypeAbsenceGrinding           // Тип отсутствия снятия покрытия 
{
  e_CuttingTypeAbsenceGrinding_Zero = 0,   // Как 0 ( Для всех столов Bystronic столов )
  e_CuttingTypeAbsenceGrinding_Two  = 1,   // Как 2 ( Для стола Bystronic компании Максиформ) 
};

extern ECuttingTypeAbsenceGrinding g_eCuttingTypeAbsenceGrinding;

enum ECuttingTypeDopInfoFileHeader           // Тип доп информации в заголовке файла
{
  e_CuttingTypeDopInfoFileHeader_None          = 0,   // Без доп информации
  e_CuttingTypeDopInfoFileHeader_Saw_GlassName = 1,   // Как <имя раскроя>,<Имя стекла>
};

extern ECuttingTypeDopInfoFileHeader g_eCuttingTypeDopInfoFileHeader;

enum ECuttingTypeBeginFileHeader           // Тип начала заголовка файла Bystronic
{
  e_CuttingTypeBeginFileHeader_PN1  = 0,   // Как PN1
  e_CuttingTypeBeginFileHeader_P1N1 = 1,   // Как P1N1
};

extern ECuttingTypeBeginFileHeader g_eCuttingTypeBeginFileHeader;

// Окончание группы Enum для Bystronic
/////////////////////////////////////////////////////////////////////////////

enum ETypeSelectBarCodes                   // Тип выбора баркодов
{
  e_TypeSelectBarCodes_All          = 0,   // Все штрихкоды
  e_TypeSelectBarCodes_nStateReject = 1,   // Баркоды, у которых стоит признак брака (nState = 512)
  e_TypeSelectBarCodes_Remake       = 2,   // Баркоды, которые является переделкой
};

enum EPyramidTypeOper                // Тип пирамид по обработкам
{
  e_PyramidTypeOper_Ordinary    = 0, // Пирамида обычная
  e_PyramidTypeOper_Obrab       = 1, // Пирамида для "Обработка/Пленка"
  e_PyramidTypeOper_Triplex     = 2, // Пирамида для триплекса
  e_PyramidTypeOper_Technologic = 3, // Пирамида технологическая
  e_PyramidTypeOper_Covering    = 4, // Пирамида для стекол с покрытием
};

enum EPyramidTypeGP                  // Тип пирамид  по видам СП
{
  e_PyramidTypeGP_GlassPack = 0,     // Пирамида  только для стеклопакета
  e_PyramidTypeGP_Cutting   = 1,     // Пирамида  только для нарезки
  e_PyramidTypeGP_Common    = 2,     // Пирамида и для пакета и для нарезки
};

enum EProjectTypeRebate              // Тип применения скидки для позиции
{
  e_ProjectTypeRebate_OnM2         = 0, // Скидка применяется к квадратному метру позиции
  e_ProjectTypeRebate_OnSumWithNDS = 1  // Скидка применяется к конечной цене позициии
};

extern EProjectTypeRebate g_eProjectTypeRebate;

enum EPricingAlgoCalc                     // Алгоритм расчёта НДС цены строки заказа
{
  e_PricingAlgoCalc_NotSet         = -1,  // Не установлено
  e_PricingAlgoCalc_FromSumWithNDS =  0,  // От суммы с НДС
  e_PricingAlgoCalc_FromPriceNoNDS =  1,  // От цены без НДС
  e_PricingAlgoCalc_FromSumM2      =  2,  // От суммы м2
  e_PricingAlgoCalc_NoRounding     =  3   // Без округления
};

extern EPricingAlgoCalc  g_ePricingAlgoCalcNewClient;  // Алгоритм расчёта НДС для нового клиента

// Тип вырезов
enum ETypeCutOut
{
  e_TypeCutOut_OnContur       = 0, // Вырез на контуре изделия     (MObj_Cutout)
  e_TypeCutOut_InsideContur   = 1  // Вырез внутри контура изделия (MObj_Notch)
};

// Сюда посносить IDTempStore.nType из SPID для хренилища.
//
// sp_CreateReject_STM
//
enum eType_SPID : long
{
                                    // 2   - CProjectViewGrid::AddProjectToManufTask
  e_SPID_Reject_BarCode      = 3,   // v_GetNewSawTaskListBySPID, sp_CheckProjectItem_OnStore
                                    // 5   - DlgPyramidNoShip::OnBnClickedOk
                                    // 100 - DlgBarCode_Glass::OnBnClickedBRemake, DlgBarCode::RemakeBarCode
  e_SPID_DeleteFromSawTask   = 4,   // Удаление GlassDetails из CGlassPackGlassGrid::DeleteGlassDetails
                                    // 6 - CTripView::PasteGP
  e_SPID_RejectFormSawTask   = 7,   // Создание брака в CGlassPackGlassGrid::OnGlassCreateReject()
  e_SPID_Reject_Task         = 100, // Заказы созданные с переделкой брака
                                    // CCuttingView::OnPopupDetailReject
  e_SPID_GlassProcessingTree = 333  // Построение древовидных списков обработки v_GlassProcessingTree
                                    // 1101 - COperPlanCrossGrid::OnPopupDelete
                                    //        COperPlanCrossGridTree::OnPopupDelete
                                    // 1102 - COperPlanCrossGridTree::ApplyPropRC
                                    // 1103 - COperPlanCrossGridTree::OnPopupAdd
};

// Тип округления для цен
enum ETypeRoundPrice
{
  e_TypeRoundPrice_None          = 0,  // не округлять, ну или округление до копеек
  e_TypeRoundPrice_More_10_Kop   = 1,  // Округлять в большую сторону до 10 копеек 
  e_TypeRoundPrice_More_1_RUB    = 2,  // Округлять в большую сторону до рубля
  e_TypeRoundPrice_More_10_RUB   = 3,  // Округлять в большую сторону до 10 рублей
  e_TypeRoundPrice_More_100_RUB  = 4   // Округлять в большую сторону до 100 рублей
};

extern ETypeRoundPrice g_eTypeRoundPriceTask;

// тип этикетки
enum ELabelType
{
  e_LabelType_FinishProduct = 1,  // для готовой продукции
  e_LabelType_GlassDetail   = 2,  // внутрицеховые на детали
  e_LabelType_Billet        = 3,  // на заготовки
  e_LabelType_BilletRest    = 4   // на деловые отходы
};

enum ETypeSource                  // Тип источника данных
{
  e_TypeSource_Task        = 0,   // Данные строим по заказу 
  e_TypeSource_SawTaskMain = 1,   // Данные строим по раскроям
};

CString GetFileNameConfig();         // Путь к файлу конфигурации хранимом 
void    SaveIniData();
void    ReadIniData();

extern CString g_sActiveCode,        // Введеный код активации
               g_sActivated_Code,    // Уже ранее активированный код - только для вывода в About
               g_sRegCode,           // Код регистрации - получен от сервера - ипользуется для проверки веб сервисом.
               g_sPathIni;

extern bool    g_bWebServiceMode;           // Режим работы веб-сервиса (без UI)

