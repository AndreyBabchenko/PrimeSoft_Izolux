<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomError.aspx.cs" Inherits="NoPaper.CustomError" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <title>Ошибка</title>
  <link rel="stylesheet" type="text/css" href="~/css/message.css?v=6" />
</head>
<body>
  <form id="form1" runat="server">
    <div class="error-message">
      <h1>Произошла ошибка</h1>
      <p style="font-size: 18px">Вы будете перенаправлены обратно через <span id="timer" style="font-size: 20px">3</span> секунды.</p>
      <asp:Literal ID="ErrorDetails" runat="server" Visible="false"></asp:Literal>
    </div>
  </form>
  <script src="./Java/Messages.js?v=3"></script>
  <script src="./Java/CustomError.js?v=2"></script>
</body>
</html>
