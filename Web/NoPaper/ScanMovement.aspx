<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScanMovement.aspx.cs" Inherits="NoPaper.ScanMovement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!---<script src="java\ScanMovement.js"></script>--->

  <script language="javascript">

    var idWorkZone = 0;

    function CheckInput() {

      var TestVar = document.getElementById('barcode').value;

      //alert(TestVar);

      // Ловим баркод изделия
      if ( TestVar.substring(0, 2) == 'SM' && TestVar.length == 12 )
      {
        //alert(TestVar);
        document.getElementById('<%= ButBegin.ClientID %>').click();
      }
      else
      // Ловим баркод зоны типа  cover001
      if ( TestVar.substring(0, 5) == 'cover' && TestVar.length == 8 )
      {
        //alert(TestVar);

        PageMethods.StandbyForScanZone(TestVar, OnSuccess, OnFailure);
      }
    }

    function OnSuccess(result)
    {
      if (result)
      {
        //alert(result);

        //idWorkZone = result;

        //document.getElementById('<= barcode.ClientID %>').value = '';

        // Ищем в ComboBox-е:
        //var Combo = document.getElementById('<= WorkZone.ClientID %>');

        //for (var i = 1; i < Combo.childElementCount; i++)
        //{
        //  var value = Combo.options[i].value;

        //  if ( value == idWorkZone )
        //  {
        //    Combo.selectedIndex = i;
        //    break;
        //  }
        //}

      }
    }

    function OnFailure(error)
    {
      if (error)
      {
      }
    }

  </script>
</head>
<body>

    <form id="form1" runat="server">
    <div>
    <div>

        <asp:ScriptManager runat="server" EnablePageMethods="true" />
    
        <p class="big">Оператор</p>&nbsp;
        <asp:DropDownList ID="Operator" runat="server" Height="80px" OnSelectedIndexChanged="Operator_SelectedIndexChanged" Width="450px" AutoPostBack="True" Font-Size="32">
        </asp:DropDownList>

        <p class="big">Откуда</p>
        <asp:DropDownList ID="DepName_Debet" runat="server" Height="80px"  OnSelectedIndexChanged="DepName_SelectedIndexChanged" Width="390px" Font-Size="32">
        </asp:DropDownList>

        <p fclass="big">Куда</p>
        <asp:DropDownList ID="DepName_Credit" runat="server" Height="80px" OnSelectedIndexChanged="DepName_SelectedIndexChanged" Width="390px" Font-Size="32">
        </asp:DropDownList>

        <p class="big">Штрихкод</p>&nbsp;
        <asp:TextBox ID="barcode" autofocus runat="server" ClientID="barcode" Height="80px" Width="390px" MaxLength="12" Font-Size="32"
          onKeyUp="CheckInput();"
        ></asp:TextBox>

        <asp:Button ID="ButBegin"  clientid= "ButBegin" runat="server" OnClick="InputBarcode" Text="Ввод" Height="80px" Width="244px" Font-Size="32" style="margin-top: 0px" />
        <br />
    
    </div>
        <asp:GridView ID="OperationTable" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4">
            <Columns>
                <asp:BoundField DataField="TimeScan" HeaderText="Время" />
                <asp:BoundField DataField="ScanText" HeaderText="Штрих-Код" />
                <asp:BoundField DataField="TaskObjectName" HeaderText="Объект" />
                <asp:BoundField DataField="AccountNum" HeaderText="Проект" />
                <asp:BoundField DataField="MarkName" HeaderText="Марка" />
                <asp:BoundField DataField="ScanOper" HeaderText="Операция" />
                <asp:BoundField DataField="WorkZoneName_Covering" HeaderText="Зона" SortExpression="WorkZoneName_Covering" />
                <asp:BoundField DataField="ScanResult" HeaderText="Результат" />
            </Columns>

            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
            <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
            <RowStyle BackColor="White" ForeColor="#330099" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
            <SortedAscendingCellStyle BackColor="#FEFCEB" />
            <SortedAscendingHeaderStyle BackColor="#AF0101" />
            <SortedDescendingCellStyle BackColor="#F6F0C0" />
            <SortedDescendingHeaderStyle BackColor="#7E0000" />
        </asp:GridView>

        </div>
    </form>
</body>
</html>
