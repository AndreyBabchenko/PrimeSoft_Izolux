<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Defect.aspx.cs" Inherits="NoPaper.Defect" Async="true"  %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
   <title>Брак</title>
   <link rel="stylesheet" type="text/css" href="~/css/styles.css?v=2" />
  <link rel="stylesheet" type="text/css" href="~/css/navigation.css?v=2" />
  <link rel="stylesheet" type="text/css" href="~/css/barCode.css?v=1" />
  <link rel="stylesheet" type="text/css" href="~/css/message.css?v=2" />
  <link rel="stylesheet" type="text/css" href="~/css/defect.css?v=2" />
</head>
<body>
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
        <asp:HyperLink runat="server" NavigateUrl="~/Defect.aspx" Text="Брак" CssClass="link active" />
      </li>
      <li>
        <a href="defect_help.html" class="link">Справка</a>
      </li>
    </ul>
  </nav>
  <form class="form" id="FormDefect" runat="server">
    <div class="defect-types-list">
      <h1 class="title">Фактический Брак</h1>
      <div class="defect-type-item">
        <label>Вид акта</label>
        <asp:DropDownList  ID="IDRejectAct" runat="server" />
        <asp:SqlDataSource ID="SqlDSRejectAct" runat="server" 
                           ConnectionString="<%$ ConnectionStrings:GLASSConnectionString %>"
                           SelectCommand="select ID, Name from RejectAct order by Name" />
      </div>
      <div class="defect-type-item">
        <label>Место обнаружения</label>
        <asp:DropDownList  ID="IDRejectPlace" runat="server" />
        <asp:SqlDataSource ID="SqlDSRejectPlace" runat="server" 
                           ConnectionString="<%$ ConnectionStrings:GLASSConnectionString %>"
                           SelectCommand="select ID, Name from RejectPlace order by Name " />
      </div>
      <div class="defect-type-item">
        <label>Вид брака</label>
        <asp:DropDownList  ID="IDRejectType" runat="server" />
        <asp:SqlDataSource ID="SqlDSRejectType" runat="server" 
                           ConnectionString="<%$ ConnectionStrings:GLASSConnectionString %>"
                           SelectCommand="select ID, Name from RejectType order by Name " />
      </div>
      <div class="defect-type-item">
        <label>Наименование Брака</label>
        <asp:DropDownList  ID="IDReject" runat="server" />
        <asp:SqlDataSource ID="SqlDSReject" runat="server" 
                   ConnectionString="<%$ ConnectionStrings:GLASSConnectionString %>"
                   SelectCommand="select ID, Name from Reject order by Name " />
      </div>
      <div class="defect-type-item">
        <label>Вид издержек</label>
        <asp:DropDownList  ID="IDTypeExpense" runat="server" />
        <asp:SqlDataSource ID="SqlDSTypeExpense" runat="server" 
                   ConnectionString="<%$ ConnectionStrings:GLASSConnectionString %>"
                   SelectCommand="select ID, Name from TypeExpense order by Name " />
      </div>
      <div class="defect-type-item">
        <label>Виновный</label>
        <asp:DropDownList ID="IDOperator_Guilty" runat="server" />
        <asp:SqlDataSource ID="SQLOperatorGuilty" runat="server"
          ConnectionString="<%$ ConnectionStrings:GLASSConnectionString %>"
          SelectCommand="select ID, Name from Operator order by Name " />
      </div>
      <div class="defect-type-item">
        <label>Комментарий</label>
        <asp:TextBox runat="server" ID="IDRejectComment" />
      </div>
    </div>
    <div class="barcode-section">
      <div class="select-operator">
        <p>Оператор</p>
        <asp:DropDownList
          runat="server"
          ID="ddOperatorList"
          CssClass="operator-dropdown" />
      </div>
    <div class="defect-input">
        <asp:TextBox ID="DefectBarCode" runat="server" Placeholder="Штрих код" CssClass="barcode__textbox" />
      <asp:Button ID="DefectButton" runat="server" OnClick="DefectButton_Click" Text="Применить" CssClass="barCode__button" />
      </div>
    </div>
  </form>
  <div class="message"></div>
  <script src="./Java/Messages.js?v=3"></script>
  <script src="./Java/Defect.js?v=1"></script>
</body>
</html>
