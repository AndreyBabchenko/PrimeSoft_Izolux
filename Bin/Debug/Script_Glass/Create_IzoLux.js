// Create.js
// Обработчики событий: -------------------------------------------------------
// Классы и номера:
//  1 - Glass
//  2 - RasVert
//  3 - RasHoriz
//  4 - DimVert
//  5 - DimHoriz
//  6 - DimRad
// 20 - Chess

var  nClass_Glass        =  1,    // Стеклопакеты
     nClass_Ras_Vert     =  2,    // Шпрос вертикальный
     nClass_Ras_Horiz    =  3,    // Шпрос горизонтальный
     nClass_Dim_Vert     =  4,
     nClass_Dim_Horiz    =  5,
     nClass_Dim_Rad      =  6,
     nClass_Drill_Oper   =  7,
     nClass_Notch        =  8,
     nClass_Grind        =  9,
     nClass_Grind_Track  = 10,
     nClass_Chess        = 20,    // Окна
     nClass_Transom      = 21,
     nClass_Leaf         = 22,
     nClass_Connect_Prof = 40,    // Доп.элементы - соединительный профиль
     nClass_Sub_Prof     = 41,    //              - подставочный   профиль
     nClass_Cutout       = 56,    // вырез в контуре изделия
     nClass_Label        = 57,
     nClass_Contur       = 60;    // Служебные

// Направление поиска (сегментов, векторов, привязок).
var  e_Lef    = 1,
     e_Rig    = 2,
     e_Bot    = 3,
     e_Top    = 4,
     e_LefTop = 5,
     e_LefBot = 6,
     e_RigTop = 7,
     e_RigBot = 8;

var  gGlassObj = 0;

var  e_NoSetSnap = 0,
     e_SetSnap = 1;
     
var  sRasLineSnap = "Border";

function Prog::Read()
{
  try
  {
    var  ObjMas = Prog.ObjMas;

    SetupClassNames(ObjMas);
     
    for (var  n = 0;  n < ObjMas.ObjCount;  n++)
    {
      var  Obj = ObjMas.GetObjNum(n);
      
      switch ( Obj.nClass )
      {
        case nClass_Glass: // Стеклопакет
          Obj.Class = "Glass";
          gGlassObj = Obj;
          switch ( Obj.Int("GeomType") )
          {
            case 0:
            case 1: // прямоугольник
              CreateBorderRect(Obj);
              break;
            case 2: // Арка
              CreateArc(Obj);
              break;
            case 3: // Арка с уступом
              CreateSubArc(Obj);
              break;
            case 4: // Треугольник справа снизу
              CreateTriangle(Obj, e_RigBot);
              break;
            case 5: // Треугольник слева снизу
              CreateTriangle(Obj, e_LefBot);
              break;
            case 6: // Треугольник справа сверху
              CreateTriangle(Obj, e_RigTop);
              break;
            case 7: // Треугольник слева сверху
              CreateTriangle(Obj, e_LefTop);
              break;
            case 8: // Трапеция справа снизу
              CreateTrap(Obj, e_RigBot);
              break;
            case 9: // Трапеция слева снизу
              CreateTrap(Obj, e_LefBot);
              break;
            case 10: // Трапеция справа сверху
              CreateTrap(Obj, e_RigTop);
              break;
            case 11: // Трапеция слева сверху
              CreateTrap(Obj, e_LefTop);
              break;
          }
          with ( Obj.Line("Border") )
          {
            SetEnableMove(true);
            CloseContur = true;
            nLine = 0;  // Номер линии задает порядок расчёта 
                        // во время пересчёта привязок.
                        // Чтобы быть уверенным в правильном порядке, присваиваем.
            LineWidth = 3;
            UpdateGeometrySnap();
          }
          Obj.Int("CalcPriceMethod") = Prog.CalcPriceMethod;
          Obj.UpdateProperty (true);

          if ( Obj.SunMas.ObjCount )
            ReadSons(Obj);
          break;
      }
      Obj.UpdateGeometrySnap();
    }

    if ( gGlassObj )
      RecalcRasPrice(gGlassObj);
  }
  catch (e)
  {
    Prog.SaveError("Prog::Read " + e.message, true);
  }
}

function ReadSons(FatherObj)
{
  var  SonCount = FatherObj.SunMas.ObjCount,
       Obj;
       
  for (var  n = 0; n < SonCount; n++)
  {
    Obj = FatherObj.SunMas.GetObjNum(n);
    switch ( Obj.nClass )
    {
      case nClass_Ras_Vert:     // Шпрос вертикальный
        Config_Ras_Vert(Obj, e_NoSetSnap);
        // [af] Расчитаем точку "From", от которой вводится и пересчитывается шпрос:
        /* [ab] Ненужно её считать
        var  X0 = Obj.Line("Body").Seg(0).V.X,
             X1 = Obj.Line("Body").Seg(1).V.X,
             Y0 = Obj.Line("Body").Seg(0).V.Y,
             Y1 = Obj.Line("Body").Seg(1).V.Y;
        Obj.InputFromXY(Math.abs((X0 + X1) / 2), Math.abs((Y0 + Y1) / 2), false); 
        */
        break;

      case nClass_Ras_Horiz:    // Шпрос горизонтальный
        Config_Ras_Horiz(Obj, e_NoSetSnap);
        // [af] 2013-10-30 Расчитаем точку "From", от которой вводится и пересчитывается шпрос:
        /* [ab] Ненужно её считать
        var  X0 = Obj.Line("Body").Seg(0).V.X,
             X1 = Obj.Line("Body").Seg(1).V.X,
             Y0 = Obj.Line("Body").Seg(0).V.Y,
             Y1 = Obj.Line("Body").Seg(1).V.Y;
        Obj.InputFromXY(Math.abs((X0 + X1) / 2), Math.abs((Y0 + Y1) / 2), false);
        */
        break;
        
      case nClass_Drill_Oper:    // сверление
        Obj.Class = "DrillOper";
        break;

      case nClass_Notch:         // вырез
        Obj.Class = "Notch";
        break;

      case nClass_Grind:         // шлиф-зона
        Obj.Class = "Grind";
        break;

      case nClass_Grind_Track:   // шлиф-Track
        Obj.Class = "GrindTrack";
        break;

      case nClass_Cutout:   // вырез в контуре изделия
        Obj.Class = "Cutout";
        break;

      case nClass_Label:         // вырез
        Obj.Class = "Label";
        break;

    }
  }
}

var  CreateDimStage = 0;    // 1 - ввод первой точки
                            // 2 - ввод второй точки
                            // 3 - ввод выносной линии

function Prog::Create()
{
  try
  {
    var  Obj = Prog.CurObj;
    
    switch ( Obj.nClass )
    {
      case nClass_Ras_Vert:           // Шпрос вертикальный.
        Config_Ras_Vert(Obj, e_SetSnap);
        Prog.SetInputDot();
        break;

      case nClass_Ras_Horiz:          // Шпрос горизонтальный.
        Config_Ras_Horiz(Obj, e_SetSnap);
        Prog.SetInputDot();
        break;

      case nClass_Drill_Oper:
      case nClass_Notch:
      case nClass_Grind:
      case nClass_Label:
      //case nClass_Cutout:
        Prog.SetInputDot();
        break;
    }
  }
  catch (e)
  {
    Prog.SaveError("Prog::Create " + e.message, true);
  }
}

function Prog::InputTemp()  // Ввод временных данных --------------------------
{
  try
  {
    var  Obj = Prog.CurObj;
    
    switch ( Obj.Class )
    {
      case "RasVert" :
      case "RasHoriz":
        RasHorizVert_InputTemp (Obj);
        break;
      case "DrillOper":
      case "Notch":
      case "Grind":
      case "Label":
        Obj_InputTemp (Obj);
    }
  }
  catch (e)
  {
    Prog.SaveError("Prog::InputTemp " + e.message, true);
  }
}

function Prog::Input()  // Ввод данных ----------------------------------------
{
  try
  {
    var  Obj     = Prog.CurObj,
         ObjMas  = Prog.ObjMas,
         bUpdate = false;
    
    switch ( Obj.Class )
    {
      case "RasVert" :
      case "RasHoriz":
      {
        RasHorizVert_InputTemp (Obj);

        var   ObjGlass = ObjMas.SearchObj_Class("Glass");
        if ( ObjGlass )
        {
          CalcRasSectionCount();
          RecalcRasPrice(ObjGlass);
        }

        Prog.CreateAllDim();
        bUpdate = true;
        break;
      }
      case "DrillOper":
      case "Notch":
      case "Grind":
      case "Label":
        Obj_InputTemp(Obj);
        bUpdate = true;
    }
    
    if ( bUpdate )
    {
      ObjMas.UpdateDraw(true);
      ObjMas.ZoomAll();
      Prop.Refresh();           // Обновить окно свойств.
    }
  }
  catch (e)
  {
    Prog.SaveError("Prog::Input " + e.message, true);
  }
}

function Prog::BeginMove()
{
  try
  {
    var  MoveObj = Prog.MoveObj,
         ObjMas  = Prog.ObjMas;

    // Если это СП, то размеры расстекловок должны остаться прежними:
    // Запретить пересчёт размеров расстекловок:
    if ( MoveObj )
      if ( MoveObj.Class == "Glass" || MoveObj.Class == "RasVert" || MoveObj.Class == "RasHoriz" )
        for (var  n = 0;  n < ObjMas.ObjCount;  n++)
        {
          var  Obj = ObjMas.GetObjNum (n);

          if ( (Obj.Class == "RasVert" || Obj.Class == "RasHoriz") && Obj.nObject != MoveObj.nObject )    // Другая расстекловка?
            for (var  m = 0;  m < Obj.SunMas.ObjCount;  m++)
            {
              var  Dim = Obj.SunMas.GetObjNum (m);
        
              if ( Dim.Class == "DimHoriz" || Dim.Class == "DimVert" )
                Dim.Text("Dim").EnableCalcTextSnap = false;                 // Запрет изменений размеров
            }
        }
  }
  catch (e)
  {
    Prog.SaveError("Prog::BeginMove " + e.message, true);
  }
}

function Prog::EnableMove()
{
  try
  {
    var  ObjMas          = Prog.ObjMas,
         MoveObj         = Prog.MoveObj,
         MoveSeg         = Prog.MoveSeg,
         MoveVector      = Prog.MoveVector,
         InputVector     = Prog.InputVector,
         InputVectorPrev = Prog.InputVectorPrev;

    Prog.Enable = true;

    if ( MoveObj.Class == "DrillOper" && MoveObj.StringID("nTypeForm") < 100)
    {
      Prog.Enable = false;
      return;
    }

    if ( !MoveVector  &&  MoveSeg.VNext )
    {    
      // Переместить сегмент параллельно, сразу два вектора:
      var  DistX   = InputVector.X   - InputVectorPrev.X,
           DistY   = InputVector.Y   - InputVectorPrev.Y,
           OrientX = MoveSeg.VNext.X - MoveSeg.V.X,
           OrientY = MoveSeg.VNext.Y - MoveSeg.V.Y;
        
      if ( Math.abs (OrientX) > Math.abs (OrientY) )    // Сегмент горизонтальный?
      {
        MoveSeg.V.Y     += DistY;
        MoveSeg.VNext.Y += DistY;

        if ( MoveSeg.IsArc )
             MoveSeg.VArc.Y += DistY;
      }
      else                                              // Сегмент вертикальный?
      {
        MoveSeg.V.X     += DistX;
        MoveSeg.VNext.X += DistX;

        if ( MoveSeg.IsArc )
             MoveSeg.VArc.X += DistX;
      }
      // Пересчитать текстовые привязки, зависимые от данных векторов:
      var  sSnap     = MoveSeg.V.CreateSnap(),
           sSnapNext = MoveSeg.VNext.CreateSnap();

      ObjMas.UpdateGeometrySnapText (sSnap);
      ObjMas.UpdateGeometrySnapText (sSnapNext);

      // Если данный сегмент - привязка, и зависит от текста - пересчитать:
      MoveSeg.V.UpdateGeometrySnap();
      MoveSeg.VNext.UpdateGeometrySnap();
    }
    // Перемещается только вектор?
    else if ( MoveVector )
    {
      // Либо у него не будет привязки, либо необходимо определить новую.
      // Привязки нет:
      MoveVector.Snap = "";
    }
  }
  catch (e)
  {
    Prog.SaveError("Prog::EnableMove " + e.message, true);
  }
}

function Prog::UpdateMove()
// Объект перемещён по привязкам
{
}

function Prog::EndMove()
{
  try
  {
    var  MoveObj = Prog.MoveObj,
         ObjMas  = Prog.ObjMas;

    // Если это СП, то размеры расстекловок должны остаться прежними,
    // Вернуть обратно запрет пересчёта:
    if ( MoveObj )
    {
      if ( MoveObj.Class == "Glass" || MoveObj.Class == "RasVert" || MoveObj.Class == "RasHoriz" )
      {
        for (var  n = 0;  n < ObjMas.ObjCount;  n++)
        {
          var  Obj = ObjMas.GetObjNum (n);

          if ( (Obj.Class == "RasVert" || Obj.Class == "RasHoriz") && Obj.nObject != MoveObj.nObject )    // Другая расстекловка?
            for (var  m = 0;  m < Obj.SunMas.ObjCount;  m++)
            {
              var  Dim = Obj.SunMas.GetObjNum (m);

              if ( Dim.Class == "DimHoriz" || Dim.Class == "DimVert" )
                Dim.Text("Dim").EnableCalcTextSnap = true;                  // Разрешить изменение размеров
            }
        }
      }

      if ( MoveObj.Class == "DrillOper" )
        return;

      Prop.Refresh(); // Обновить окно свойств 
      MoveObj.UpdateDraw(true);
    }
  }
  catch (e)
  {
    Prog.SaveError("Prog::EndMove " + e.message, true);
  }
}

function Prog::Save()
{
  try
  {
    // Если есть шпросы, посчитать количество пересечений, для расчёта крестов:

    var   ObjGlass  = Prog.ObjMas.SearchObj_Class ("Glass");
    if ( !ObjGlass )
       return;
      
    // CalcRasSectionCount();
    Prog.CalcRasLeng(ObjGlass);
    Prog.UpdateRasPriceParam(ObjGlass);
    CalcRasPrice   (ObjGlass);
    CheckRasExist  (ObjGlass);
    CalcFigureParam(ObjGlass);
  }
  catch (e)
  {
    Prog.SaveError("Prog::Save " + e.message, false);
  }
}

// Макрокоманды ---------------------------------------------------------------

function SetupClassNames(CurObjMas)
{
  for (var  n = 0;  n < CurObjMas.ObjCount;  n++)
  {
    var  CurObj = CurObjMas.GetObjNum (n);
    
    switch ( CurObj.nClass )
    {
      case nClass_Glass      : CurObj.Class = "Glass";     break;    // Стеклопакет
      case nClass_Ras_Vert   : CurObj.Class = "RasVert";   break;    // Расстекловка верт.
      case nClass_Ras_Horiz  : CurObj.Class = "RasHoriz";  break;    // Расстекловка гориз.
      case nClass_Dim_Horiz  : CurObj.Class = "DimHoriz";  break;
      case nClass_Dim_Vert   : CurObj.Class = "DimVert";   break;
      case nClass_Drill_Oper : CurObj.Class = "DrillOper"; break;
      case nClass_Notch      : CurObj.Class = "Notch";     break;
      case nClass_Label      : CurObj.Class = "Label";     break;	
      case nClass_Grind      : CurObj.Class = "Grind";     break;
      case nClass_Grind_Track: CurObj.Class = "GrindTrack";break;
      case nClass_Cutout     : CurObj.Class = "Cutout";    break;
    }
    if ( CurObj.SunMas )
      SetupClassNames (CurObj.SunMas);
  }
}

function CreateBorderRect (CurObj)
{
  if ( CurObj.Line("Border").SegCount == 0 )                        // Сегментов нет? - новый объект
  {
    var  W = CurObj.Int("Glass_Width" ),
         H = CurObj.Int("Glass_Height");

    with ( CurObj.Line("Border") )
    {
      Seg(0).V.xy (0, 0);
      Seg(1).V.xy (W, 0);
      Seg(2).V.xy (W, H);
      Seg(3).V.xy (0, H);
    }
  }
}

function CreateArc (CurObj)
{
  if ( CurObj.Line("Border").SegCount == 0 )                       // Сегментов нет? - новый объект
  {
    var  W = CurObj.Int("Glass_Width" ),
         H = CurObj.Int("Glass_Height");

    with ( CurObj.Line("Border") )
    {
      Seg(0).V.xy (0, 0);
      Seg(1).V.xy (W, 0);
      Seg(1).SetArc();
      Seg(1).VArc.X = W/2;
      Seg(1).VArc.Y = H;
    }
  }
}

function CreateSubArc (CurObj)
{
  if ( CurObj.Line("Border").SegCount == 0 )                       // Сегментов нет? - новый объект
  {
    var  W = CurObj.Int("Glass_Width" ),
         H = CurObj.Int("Glass_Height");

    with ( CurObj.Line("Border") )
    {
      Seg(0).V.xy (0, 0);
      Seg(1).V.xy (W, 0);
      Seg(2).V.xy (W, H/2);
      Seg(2).SetArc()
      Seg(2).VArc.X = W/2;
      Seg(2).VArc.Y = H;
      Seg(3).V.xy (0, H/2);
    }
  }
}

function CreateTriangle (CurObj, Type)
{
  if ( CurObj.Line("Border").SegCount == 0 )                       // Сегментов нет? - новый объект
  {
    var  W = CurObj.Int("Glass_Width" ),
         H = CurObj.Int("Glass_Height");

    with ( CurObj.Line("Border") )
    {
      switch ( Type )
      {
        case e_RigBot:  //Справа-снизу
        {
          Seg(0).V.xy (0, 0);
          Seg(1).V.xy (W, 0);
          Seg(2).V.xy (W, H);
        }
        break;
        case e_LefBot:  //Слева-снизу
        {
          Seg(0).V.xy (0, 0);
          Seg(1).V.xy (W, 0);
          Seg(2).V.xy (0, H);
        }
        break;
        case e_RigTop:  //Справа-сверху
        {
          Seg(0).V.xy (0, H);
          Seg(1).V.xy (W, 0);
          Seg(2).V.xy (W, H);
        }
        break;
        case e_LefTop:  //Слева-сверху
        {
          Seg(0).V.xy (0, 0);
          Seg(1).V.xy (W, H);
          Seg(2).V.xy (0, H);
        }
        break;
      }
    }
  }
}

function CreateTrap (CurObj, Type)
{
  if ( CurObj.Line("Border").SegCount == 0 )                       // Сегментов нет? - новый объект
  {
    var  W = CurObj.Int("Glass_Width" ),
         H = CurObj.Int("Glass_Height");

    with ( CurObj.Line("Border") )
    {
      switch ( Type )
      {
        case e_RigBot:  //Справа-снизу
        {
          Seg(0).V.xy (0,   0);
          Seg(1).V.xy (W,   0);
          Seg(2).V.xy (W,   H);
          Seg(3).V.xy (W/2, H);
          Seg(4).V.xy (0,   H/2);
        }
        break;
        case e_LefBot:  //Слева-снизу
        {
          Seg(0).V.xy (0,   0);
          Seg(1).V.xy (W,   0);
          Seg(2).V.xy (W,   H/2);
          Seg(3).V.xy (W/2, H);
          Seg(4).V.xy (0,   H);
        }
        break;
        case e_RigTop:  //Справа-сверху
        {
          Seg(0).V.xy (0,   H/2);
          Seg(1).V.xy (W/2, 0);
          Seg(2).V.xy (W,   0);
          Seg(3).V.xy (W,   H);
          Seg(4).V.xy (0,   H);
        }
        break;
        case e_LefTop:  //Слева-сверху
        {
          Seg(0).V.xy (0,   0);
          Seg(1).V.xy (W/2, 0);
          Seg(2).V.xy (W,   H/2);
          Seg(3).V.xy (W,   H);
          Seg(4).V.xy (0,   H);
        }
        break;
      }
    }
  }
}

// Ввод с экрана --------------------------------------------------------------

function RasHorizVert_InputTemp (CurObj)
{
  Prog.ObjMas.From = Prog.InputVector;

  var  Obj = CurObj;
  
  switch ( Obj.nClass )
  {
    case nClass_Ras_Vert:           // Шпрос вертикальный.
      Config_Ras_Vert(Obj, e_SetSnap);
      break;

    case nClass_Ras_Horiz:          // Шпрос горизонтальный.
      Config_Ras_Horiz(Obj, e_SetSnap);
      break;
  }

  CurObj.InputFromXY (Prog.InputVector.X, Prog.InputVector.Y, false);
  var  Glass = Prog.ObjMas.SearchObj_Class ("Glass");
  if ( Glass )
    Glass.SunMas.MakeSun (CurObj);
  
  CurObj.UpdateGeometrySnap();
  Prog.CreateAllDim();
  Prog.ObjMas.UpdateDraw(true);
}

function Obj_InputTemp(CurObj)
{
  Prog.ObjMas.From = Prog.InputVector;
  
  var X = Prog.InputVector.X,
      Y = Prog.InputVector.Y;
  CurObj.InputFromXY (X, Y, false);
  
  var Glass = Prog.ObjMas.SearchObj_Class ("Glass");
  if ( Glass )
    Glass.SunMas.MakeSun (CurObj);

  CurObj.UpdateGeometrySnap();
  Prog.ObjMas.UpdateDraw(true);
}


// Общеиспользуемые функции ///////////////////////////////////////////////////

function CalcRasSectionCount()
// Расчёт количества секций шпрос и пересечений
{
  // Если опция "пересчет цены при сохранение чертежа" тогда не считаем количество секций шпрос
  if ( Prog.bRecalcRasAfterPlotSave )
    return;
  
  var  nVert  = 0,
       nHoriz = 0,
       Glass  = Prog.ObjMas.SearchObj_Class ("Glass");
  
  if ( !Glass )
     return;
  
  for (var  n = 0;  n < Glass.SunMas.ObjCount;  n++)
  {
    var  Obj = Glass.SunMas.GetObjNum (n);
    
    if ( !Obj.IsDeleted && Obj.Class == "RasVert" )
      nVert++;
    if ( !Obj.IsDeleted && Obj.Class == "RasHoriz" )
      nHoriz++;
  }

	var nBlock = Glass.Int("BlockCalcRasCross")

	if ( nBlock == 0 )
	  Glass.Int("nRasIntersect") = nVert * nHoriz;


  if ( nVert || nHoriz )
    Glass.Int("nCountSection") = (nVert + 1) * (nHoriz + 1);
  else
    Glass.Int("nCountSection") = 0;

  Glass.Int("CapCount") = (nVert + nHoriz) * 2;
}


function Config_Ras(CurObj)
{
  if ( CurObj.ID == 0 ||                               // [ab] Объект новый или:
       ( CurObj.Line("Body").Seg(0).V.Snap != "" &&    // [ab] Только если пользователь не стёр привязку к контуру одного из векторов (то есть ввёл руками координату)
         CurObj.Line("Body").Seg(1).V.Snap != ""
       )
     )
    // Контур привязки. Если есть данный контур, то в него можно ставить шпросс.
    CurObj.String("SnapContur") = "(Class,Glass) (Line, " + sRasLineSnap + ") (Inside)";
    
  dummy = CurObj.V("From");
}

function Config_Ras_Vert(CurObj, bSetSnap)
{
  Config_Ras(CurObj);

  CurObj.Class          = "RasVert";
  CurObj.Float("Width") = 0;          // Создаем объект Float для шпроса.
  CurObj.Float("Angle") = 90;
  
  var   ObjGlass = Prog.ObjMas.SearchObj_Class ("Glass");
  if ( ObjGlass )
  {
		if ( ObjGlass.ID == 0 )
		{
	    var  idTypeConnect = ObjGlass.StringID("ID_Ras_ConnectType");
	    CurObj.StringID("ID_Ras_TypeSection") = idTypeConnect == 2 ? 1 : 0;
		}
  }

  with ( CurObj.Line("Body") )
  {
    nLine   = 1; // Номер линии не менять! Используется при списании.
    Visible = 0;

    // Как искать точку пересечения линии. Поиск в контуре привязки:
    Seg(0).V.SnapIntersect = ""; 
    Seg(1).V.SnapIntersect = ""; 

    if ( bSetSnap == e_SetSnap )	
    {
      // [ab] Искать от точки From иначе шпросу не увидим
      Seg(0).V.Snap = "(Intersect, curSnapContur,(Direction,(XYAngle, (+,thisObj.Float(Angle),180))) (From, thisObj.V(From)))"; 
      Seg(1).V.Snap = "(Intersect, curSnapContur,(Direction,(XYAngle, thisObj.Float(Angle))) (From, thisObj.V(From)))";   
		}
  }

  with ( CurObj.Line("Profile") )
  {
    nLine   = 2; 
    Visible = 1;
    LineWidth = 2;

    // Как искать точку пересечения линии. Поиск в контуре привязки:
    Seg(0).V.SnapIntersect = ""; 
    Seg(1).V.SnapIntersect = "";

    if ( bSetSnap == e_SetSnap )	
    {
      // [ab] Искать от точки From иначе шпросу не увидим
      Seg(0).V.Snap = "(IntersectShift, thisObj.Line(Body).Seg(0), thisObj.Line(Body).Seg(0).V.SnapSeg, thisObj.Line(Body).Seg(0).V)";
      Seg(1).V.Snap = "(IntersectShift, thisObj.Line(Body).Seg(0), thisObj.Line(Body).Seg(1).V.SnapSeg, thisObj.Line(Body).Seg(1).V)";
		}

    SetEnableMove(false);
  }

  CurObj.FloatSnap("Width") = "(X,(Round2,thisObj.Line(Body).Seg(0).V))";
}

function Config_Ras_Horiz(CurObj, bSetSnap)
{
  Config_Ras(CurObj);

  CurObj.Class           = "RasHoriz";
  CurObj.Float("Height") = 0;          // Создаем объект Float для шпроса.
  CurObj.Float("Angle")  = 0;
  
  var   ObjGlass = Prog.ObjMas.SearchObj_Class ("Glass");
  if ( ObjGlass )
  {
    /*
    var  idTypeConnect = ObjGlass.StringID("ID_Ras_ConnectType");
    CurObj.StringID("ID_Ras_TypeSection") = idTypeConnect == 3 ? 1 : 0;
    */
		if ( ObjGlass.ID == 0 )
		{
	    var  idTypeConnect = ObjGlass.StringID("ID_Ras_ConnectType");
	    CurObj.StringID("ID_Ras_TypeSection") = idTypeConnect == 3 ? 1 : 0;
		}
  }

  with ( CurObj.Line("Body") )
  {
    nLine   = 1; // Номер линии не менять! Используется при списании.
    Visible = 0;

    // Как искать точку пересечения линии. Поиск в контуре привязки:

    Seg(0).V.SnapIntersect = "";    
    Seg(1).V.SnapIntersect = "";

    if ( bSetSnap == e_SetSnap )
    {
      // [ab] Искать от точки From иначе шпросу не увидим
      Seg(0).V.Snap = "(Intersect, curSnapContur,(Direction,(XYAngle, (+,thisObj.Float(Angle),180))) (From, thisObj.V(From)))";
      Seg(1).V.Snap = "(Intersect, curSnapContur,(Direction,(XYAngle, thisObj.Float(Angle)) ) (From, thisObj.V(From)))";
		}
  }

  with ( CurObj.Line("Profile") )
  {
    nLine   = 2; 
    Visible = 1;
    LineWidth = 2;

    // Как искать точку пересечения линии. Поиск в контуре привязки:

    Seg(0).V.SnapIntersect = "";    
    Seg(1).V.SnapIntersect = "";

    if ( bSetSnap == e_SetSnap )
    {
      // [ab] Искать от точки From иначе шпросу не увидим
      Seg(0).V.Snap = "(IntersectShift, thisObj.Line(Body).Seg(0), thisObj.Line(Body).Seg(0).V.SnapSeg, thisObj.Line(Body).Seg(0).V)";
      Seg(1).V.Snap = "(IntersectShift, thisObj.Line(Body).Seg(0), thisObj.Line(Body).Seg(1).V.SnapSeg, thisObj.Line(Body).Seg(1).V)";
		}

    SetEnableMove(false);
  }

  CurObj.FloatSnap("Height") = "(Y,(Round2,thisObj.Line(Body).Seg(0).V))";
}

function Config_Drill(CurObj)
{
 // Контур привязки. Если есть данный контур, то в него можно ставить шпросс.

  CurObj.String("SnapContur") = 
      "(Class,Glass) " +
      "(Line,Border) (Inside)";
  CurObj.V("From").Snap = "(/,(+, thisObj.Line(Body).Seg(0).V, thisObj.Line(Body).Seg(1).V), 2)";
}

function RecalcRasPrice(ObjGlass)  // Пересчет хар-к шпросс
{
  if ( Prog.bRecalcRasAfterPlotSave )
    return;
  
  //CalcRasSectionCount();
  Prog.CalcRasLeng(ObjGlass);
  Prog.UpdateRasPriceParam(ObjGlass);
  Prog.AddProtocol("Ц.Шпрос за Секцию = " + ObjGlass.Float("RasPriceSection") + "\n" +
                   " Ц.за Метр = "        + ObjGlass.Float("RasPriceM")       + "\n" +
                   " Ц.за Крест = "       + ObjGlass.Float("CrossPrice")      + "\n" +
                   " Ц.за Пробку = "      + ObjGlass.Float("CapPrice")        + "\n\n");
  
  CalcRasPrice(ObjGlass);          // [ab] Вызов ф-и из индивидуального скрипта клиента
  ObjGlass.UpdateProperty (true);
}

function Prog::RecalcRas()  // Пересчет хар-к шпросс (для вызова из CWinCADDoc после удаления шпросса)
{
  try
  {
    var   ObjGlass = Prog.ObjMas.SearchObj_Class("Glass");
    
    if ( !ObjGlass )
      return;
    CalcRasSectionCount();
    RecalcRasPrice(ObjGlass);

    ChangeRasTypeSection(Prog.CurObj);
  }
  catch (e)
  {
    Prog.SaveError("Prog::RecalcRas " + e.message, true);
  }
}

function CalcFigureParam(CurObj)
{
  try
  {
    var  nSegCount        = CurObj.Line("Border").SegCount,
         nArcSegCount     = 0,
         nRightAngleCount = 0;

    for ( var i = 0; i < nSegCount; i++ )
      if ( CurObj.Line("Border").Seg(i).IsArc() )
        nArcSegCount++;

    if ( nSegCount > 2  &&  nSegCount != nArcSegCount  &&  nSegCount != nArcSegCount - 1 )
      for ( var i = 0; i < nSegCount; i++ )
      {
        if ( CurObj.Line("Border").Seg(i).IsArc()  ||  CurObj.Line("Border").Seg(i + 1 < nSegCount ? i + 1 : 0).IsArc() )
          continue;

        var  X0 = CurObj.Line("Border").Seg(i).V.X,
             X2 = CurObj.Line("Border").Seg(i + 2 < nSegCount ? i + 2 : i + 2 - nSegCount).V.X,
             Y0 = CurObj.Line("Border").Seg(i).V.Y,
             Y2 = CurObj.Line("Border").Seg(i + 2 < nSegCount ? i + 2 : i + 2 - nSegCount).V.Y,
             L0 = CurObj.Line("Border").Seg(i).Leng,
             L1 = CurObj.Line("Border").Seg(i + 1 < nSegCount ? i + 1 : 0).Leng;

        if ( L0 > 0  &&  L1 > 0 )
          if ( Math.pow((X2 - X0), 2) +  Math.pow((Y2 - Y0), 2) ==  Math.pow(L0, 2) +  Math.pow(L1, 2) )
            nRightAngleCount++;
      }

    GP.SegCount        = nSegCount;
    GP.ArcSegCount     = nArcSegCount;
    GP.RightAngleCount = nRightAngleCount;
  }
  catch (e)
  {
    Prog.SaveError("CalcFigureParam " + e.message, false);
  }
}
