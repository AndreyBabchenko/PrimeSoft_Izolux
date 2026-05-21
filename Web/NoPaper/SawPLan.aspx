<%@Page Language="C#" AutoEventWireup="true" CodeBehind="SawPLan.aspx.cs" Inherits="NoPaper.SawPlan" Async="true"  %>

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
        <asp:HyperLink runat="server" NavigateUrl="~/SawPlan.aspx" Text="План работы" CssClass="link active" />
      </li>
      <li>
          <asp:HyperLink runat="server" NavigateUrl="~/TaskSearchInfo.aspx" Text="Поиск по заказам" CssClass="link" />
        </li>
        <li>
        <asp:HyperLink runat="server" NavigateUrl="~/Defect.aspx" Text="Брак" CssClass="link" />
      </li>
    </ul>
  </nav>
    <header>
      <div class="header-item">
        <label class="item__label">
          Участок
            <asp:DropDownList
              ID="ddListSector"
              runat="server"
              DataTextField="Name"
              DataValueField="ID"
              OnSelectedIndexChanged="ddListSector_SelectedIndexChanged"
              AutoPostBack="true" />
        </label>
      </div>
      <div class="header-item">
        <label class="item__label item__label_small">
          С
        <asp:TextBox ID="DateTextBoxStart" runat="server" type="date" />
        </label>
      </div>
      <div class="header-item">
        <label class="item__label item__label_small">
          ПО
          <asp:TextBox ID="DateTextBoxEnd" runat="server" type="date" />
        </label>
      </div>
      <div class="header-item">
        <asp:Button ID="ButBegin" runat="server" OnClick="ButBegin_Click" Text="Найти" />
      </div>
    </header>
    <div class="flex-center">
      <asp:GridView
        ID="PlanSawTaskGrid"
        runat="server"
        DataKeyNames="ID"
        OnRowCommand="PlanSawTaskGrid_RowCommand"
        OnRowDataBound="PlanSawTaskGrid_RowDataBound"
        AutoGenerateColumns="False"
        CssClass="grid saw-grid">
        <Columns>
          <asp:TemplateField HeaderText="Номер" HeaderStyle-CssClass="grid-header-item">
            <ItemTemplate>
              <asp:Label runat="server" Text='<%# Eval("Name") %>' />
            </ItemTemplate>
          </asp:TemplateField>

          <asp:TemplateField HeaderText="Дата" HeaderStyle-CssClass="grid-header-item">
            <ItemTemplate>
              <asp:Label runat="server" Text='<%# Eval("Data", "{0:dd.MM.yyyy}") %>' />
            </ItemTemplate>
          </asp:TemplateField>


          <asp:TemplateField HeaderText="Комментарий" HeaderStyle-CssClass="grid-header-item">
            <ItemTemplate>
              <asp:Label runat="server" Text='<%# Eval("Comment") %>' />
            </ItemTemplate>
          </asp:TemplateField>

          <asp:TemplateField HeaderText="Список стёкол" HeaderStyle-CssClass="grid-header-item">
            <ItemTemplate>
              <asp:Label runat="server" Text='<%# Eval("ListGlass") %>' />
            </ItemTemplate>
          </asp:TemplateField>

          <asp:TemplateField HeaderText="Количество стёкол" HeaderStyle-CssClass="grid-header-item">
            <ItemTemplate>
              <asp:Label runat="server" Text='<%# Eval("GlassCount") %>' />
            </ItemTemplate>
          </asp:TemplateField>

          <asp:TemplateField HeaderText="Сбор." HeaderStyle-CssClass="grid-header-item">
            <ItemTemplate>
              <asp:Label runat="server" Text='<%# Eval("nCount_Assembly") %>' />
            </ItemTemplate>
          </asp:TemplateField>

          <asp:TemplateField HeaderStyle-CssClass="grid-header-item">
            <ItemTemplate>
              <asp:Button
                ID="RedirectToWorkplace_Button"
                runat="server"
                Text="Перейти"
                ButtonType="Button"
                CommandName="OnRedirectButton"
                HeaderStyle-CssClass="grid-header-item" />
            </ItemTemplate>
          </asp:TemplateField>
        </Columns>
      </asp:GridView>
    </div>
  </form>
  <div class="message"></div>
</body>
</html>
