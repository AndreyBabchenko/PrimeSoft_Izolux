<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskSearchInfo.aspx.cs" Inherits="NoPaper.TaskSearchInfo" Async="true" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <title>План работы</title>
  <link rel="stylesheet" type="text/css" href="~/css/styles.css?v=4" />
  <link rel="stylesheet" type="text/css" href="~/css/header.css?v=8" />
  <link rel="stylesheet" type="text/css" href="~/css/navigation.css?v=2" />
  <link rel="stylesheet" type="text/css" href="~/css/message.css?v=2" />
  <link rel="stylesheet" type="text/css" href="~/css/grid.css?v=6" />
</head>
<body>
  <form class="form" id="SawPlanForm" runat="server">
    <asp:ScriptManager runat="server" />
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
          <asp:HyperLink runat="server" NavigateUrl="~/TaskSearchInfo.aspx" Text="Поиск по заказам" CssClass="link active" />
        </li>
        <li>
          <asp:HyperLink runat="server" NavigateUrl="~/Defect.aspx" Text="Брак" CssClass="link" />
        </li>
      </ul>
    </nav>
    <header>
      <div class="header-item">
        № Заказа
      <asp:TextBox ID="AccountNum_TextBox" runat="server" Placeholder="№ Заказа" />
      </div>

      <div class="header-item">
        <asp:CheckBox ID="bShowFinishedGlass_CheckBox" runat="server" Checked="true" Text="Отображать готовые" />
      </div>

      <div class="header-item">
        <asp:CheckBox ID="bShowComplitedGlass_CheckBox" runat="server" Checked="true" Text="Отображать принятые" />
      </div>

      <div class="header-item">
        <asp:Button ID="ButBegin" runat="server" OnClick="ButBegin_Click" Text="Найти" OnClientClick="" />
      </div>
    </header>

    <main>
      <div class="flex-center">
        <asp:GridView
          ID="TaskGrid"
          runat="server"
          AutoGenerateColumns="False"
          CssClass="grid saw-grid">
          <Columns>
            <asp:TemplateField HeaderText="Раскрой №" HeaderStyle-CssClass="grid-header-item">
              <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("NameSawTask") %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Заказ №" HeaderStyle-CssClass="grid-header-item">
              <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("AccountNum") %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Стекло" HeaderStyle-CssClass="grid-header-item">
              <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("NameGlassProduct") %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Высота" HeaderStyle-CssClass="grid-header-item">
              <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Height") %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Ширина" HeaderStyle-CssClass="grid-header-item">
              <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Width") %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Участок" HeaderStyle-CssClass="grid-header-item">
              <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("NameSectorManufact") %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Штрих код" HeaderStyle-CssClass="grid-header-item">
              <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("CurrentPyramidBarCode") %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Количество" HeaderStyle-CssClass="grid-header-item">
              <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("nCountGlass") %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Время готовности" HeaderStyle-CssClass="grid-header-item">
              <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("ReadyDateTime") %>' />
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Время изготовления" HeaderStyle-CssClass="grid-header-item">
              <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("ReceiveDateTime") %>' />
              </ItemTemplate>
            </asp:TemplateField>

          </Columns>
        </asp:GridView>
      </div>
    </main>
  </form>
  <div class="message"></div>
</body>
</html>
