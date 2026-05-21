<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScanBarcodes.aspx.cs" Inherits="NoPaper.ScanBarcodes" Async="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
  <title>Сканирование</title>
  <link rel="stylesheet" type="text/css" href="~/css/message.css?v=6" />
  <link rel="stylesheet" type="text/css" href="~/css/navigation.css?v=3" />
  <link rel="stylesheet" type="text/css" href="~/css/ScanBarcodes.css?v=2" />
</head>
<body>
  <form id="form1" runat="server">
    <nav>
      <ul class="links-items">
        <li>
          <asp:HyperLink runat="server" NavigateUrl="~/workplace.aspx" Text="Рабочее место" CssClass="link" />
        </li>
        <li>
          <asp:HyperLink runat="server" NavigateUrl="~/ScanBarCodes.aspx" Text="Сканирование" CssClass="link active" />
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
    <div class="scanning">
        <asp:ScriptManager runat="server" EnablePageMethods="true" />
        <div class="sector-list">
          Участок:
          <asp:DropDownList
            runat="server"
            ID="ddSector"
            CssClass="operator-dropdown" />
        </div>
        <div class="operator-container">
          Выбран оператор:
          <span class="operator" />
        </div>
        <p class="barcode">Штрихкод</p>
        <div class="barcode-input">
          <asp:TextBox ID="BarCode" runat="server" CssClass="barcode-textbox" MaxLength="12" autofocus="autofocus" placeholder="Введите штрихкод" />
        </div>

        <asp:Button ID="ButBegin" runat="server" Text="Ввод" CssClass="submit-button" OnClientClick="BarcodeInput(); return false;" />
    </div>
  </form>
  <div class="message"></div>
  <script src="./Java/Messages.js?v=3"></script>
  <script src="./Java/ScanBarcodes.js?v=2"></script>
</body>
</html>
