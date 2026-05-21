<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Plot.aspx.cs" Inherits="NoPaper.Plot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <link rel="stylesheet" type="text/css" href="~/css/navigation.css?v=3" />
  <link rel="stylesheet" type="text/css" href="~/css/plot.css?v=6" />
  <title>Чертёж</title>
</head>
<body>
  <form id="form1" runat="server">
    <nav>
      <ul class="links-items">
        <li>
          <asp:HyperLink runat="server" NavigateUrl="~/workplace.aspx" Text="Рабочее место" CssClass="link" />
        </li>
        <li>
          <asp:HyperLink runat="server" NavigateUrl="~/ScanBarCodes.aspx" Text="Сканирование" CssClass="link" />
        </li>
        <li>
          <asp:HyperLink runat="server" NavigateUrl="~/SawPlan.aspx" Text="План работы" CssClass="link" />
        </li>
        <li>
          <asp:HyperLink runat="server" NavigateUrl="~/TaskSearchInfo.aspx" Text="Поиск по заказам" CssClass="link" />
        </li>
        <li>
          <asp:HyperLink runat="server" NavigateUrl="~/Defect.aspx" Text="Брак" CssClass="link" />
        </li>
      </ul>
    </nav>
    <div class="sheet hidden">

      <div class="row top">

        <div class="left-box">
          <div class="cell label" id="cellClientLabel">Клиент</div>
          <div class="cell value" id="cellClient"></div>
        </div>

        <div class="center-box" id="cellCenterBox"></div>

        <div class="right-box">
          <div class="cell delivery-label" id="cellDeliveryLabel">Дата доставки:</div>
          <div class="cell delivery-value" id="cellDateComplite"></div>
        </div>

      </div>

      <div class="table">

        <div class="cell head">Заказ</div>
        <div class="cell head">Поз.</div>
        <div class="cell head">Кол<br>
          Во</div>
        <div class="cell head">Шир X Выс</div>
        <div class="cell head">Рамка</div>
        <div class="cell head">Рамок<br>
          штук</div>
        <div class="cell head">Газ</div>
        <div class="cell head">Формула</div>
        <div class="cell head">Раскладка</div>

        <div class="cell" id="cellAccountNum"></div>
        <div class="cell" id="cellPosNum"></div>
        <div class="cell" id="cellCountGP"></div>
        <div class="cell" id="cellSize"></div>
        <div class="cell" id="cellFrame"></div>
        <div class="cell" id="cellFrameCount"></div>
        <div class="cell" id="cellGas"></div>
        <div class="cell" id="cellFormula"></div>
        <div class="cell" id="cellLayout"></div>

      </div>

      <div class="sub-info">

        <div class="sub-info-items">
          <h3 class="left-box">Размеры рамок</h3>

          <div class="table-size-frame" id="sizeFrameTable">
            <div class="cell head">№</div>
            <div class="cell head">Дл.Черт</div>
            <div class="cell head">
              <br>
              Дл.Рам.</div>
            <div class="cell head">R черт.</div>
            <div class="cell head">
              <br>
              R рам</div>
          </div>
        </div>

        <div class="sub-info-items">
          <div class="item-info-text">
            <div class="info-text">Цвет наружу:<span id="ColorOut"></span></div>
            <div class="info-text">Цвет внутрь:<span id="ColorIn"></span></div>
            <div class="info-text">Ширина:<span id="PlotWidth"></span></div>
            <div class="info-text">Производитель<span id="Manufacturer"></span></div>
            <div class="info-text">№ камеры<span id="CamNum"></span></div>
          </div>

            <div class="table-size-frame" id="sizeShprosTable">
              <div class="cell head">Сегмент</div>
              <div class="cell head">К-во</div>
              <div class="cell head">Длина</div>
              <div class="cell head">Л.УГ</div>
              <div class="cell head">П.УГ</div>
            </div>
        </div>


      </div>

      </div>
    <div class="plot">
      <asp:Image ID="Image1" runat="server" CssClass="plot-image" />
    </div>
  </form>
  <script src="./Java/Plot.js?v=5"></script>
</body>
</html>
