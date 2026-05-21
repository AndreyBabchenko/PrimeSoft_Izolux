<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="scan1.aspx.cs" Inherits="NoPaper.Scan1" Async="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
  <title>Сканирование</title>
  <link rel="stylesheet" type="text/css" href="~/css/scan1.css?v=4" />
  <link rel="stylesheet" type="text/css" href="~/css/message.css?v=5" />
</head>
<body>
    <%--
     [AO] -- Тестовые кнопки
      <div style="display: flex;">`
      <asp:Button ID="ServerButton" Width="150px" Height="70px" runat="server" Text="Серверная кнопка" OnClick="ServerButton_Click" />`
      <button style="width: 150px; height: 70px" ID="ClientButton">Клиентская кнопка</button>`
      <asp:Label ID="ResultLabel" runat="server" Text="" />`
     </div>`
     --%>
     <div>
        <header>
          <div class="select-operator">
            <form id="form1" runat="server">
            <asp:ScriptManager runat="server" EnablePageMethods="true" />
              <p>Оператор</p>
              <asp:DropDownList
                runat="server"
                ID="Operator"
                CssClass="operator-dropdown" />
              <p>Штрихкод</p>
            </form>
          </div>
        <div class="info">
          <label id="ScanTextInfo" class="info-label">Отскан ШК: <span id="ScanTextValue" /></label>
          <label id="TextInfo" class="info-label" />
        </div>
        <div class="barcode-input">
          <input id="BarCode" class="barcode-textbox" maxlength="12" autofocus />
        </div>

        <button ID="ButBegin" class="submit-button">Ввод</button>
      </header>

      <table id="OperationTable" class="gridview">
        <thead>
          <tr class="grid-header">
            <th>Время</th>
            <th>Штрих-Код</th>
            <th>Заказ</th>
        </tr>
      </thead>
      <tbody class="grid-pager">
          <!-- Динамические данные будут добавляться сюда -->
      </tbody>
    </table>
  </div>
  <div class="message"></div>
  <div class="count-failed-barcode"></div>
  <script src="./Java/Scan1.js?v=6"></script>
  <script src="./Java/Messages.js?v=3"></script>
  <%--<script src="./Java/Test.js?v=2"></script>--%>
</body>
</html>
