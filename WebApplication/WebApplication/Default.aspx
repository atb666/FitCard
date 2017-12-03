<%@ Page Title="Estabelecimentos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebApplication._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">

        function updateTips(t) {
            var tips = $(".validateTips");

            tips.text(t).addClass("ui-state-highlight");
            setTimeout(function () {
                tips.removeClass("ui-state-highlight", 1500);
            }, 500);
        }

        function saveEstablishment() {
            $('#<%= btnSave.ClientID %>').click();
        }

        function closeEstablishment() {
            $('#dialog').dialog("close");
        }

        function editEstablishment() {
            $('#dialog').dialog("open");
        }

        function pageLoad(sender, args) {

            //JQuery
            $(function () {
                //Dialog
                var dialog = $('#dialog').dialog({
                    autoOpen: false,
                    modal: true,
                    open: function (type, data) {
                        $(this).parent().appendTo("form");
                    },
                    buttons: {
                        Salvar: saveEstablishment,
                        Cancelar: function () {
                            dialog.dialog("close");
                        }
                    }
                });

                //Opener
                var opener = $('#<%= btnNew.ClientID %>').click(function () {
                    $('#<%= hdfHidden.ClientID %>').val('');
                    dialog.dialog("open");
                    return false;
                });

                //RegistrationDate
                var tbxRegistrationDate = $('#<%= tbxRegistrationDate.ClientID %>');
                tbxRegistrationDate.datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: true,
                    showButtonPanel: true,
                    changeMonth: true,
                    changeYear: true
                });

                //Cnpj
                var tbxCnpj = $('#<%= tbxCnpj.ClientID %>');
                tbxCnpj.mask('00.000.000/0000-00', {
                    placeholder: '__.___.___/____-__',
                    reverse: true
                });
            });
        }

    </script>
    <h2>
        Estabelecimentos
    </h2>
    <br />
    <asp:Button ID="btnNew" runat="server" Text="Novo" />
    <br />
    <div id="dialog" title="Novo estabelecimento">
        <asp:UpdatePanel ID="updtDialog" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <p class="validateTips" style="border: 1px solid transparent; padding: 0.3em;" />
                <table>
                    <tr>
                        <td>
                            <span>Razão social</span>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxCompanyName" runat="server" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>Nome fantasia</span>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxTradingName" runat="server" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>CNPJ</span>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxCnpj" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>E-mail</span>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxMail" runat="server" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>Endereço</span>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxAddress" runat="server" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>Estado</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlState_OnSelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>Cidade</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCity" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>Telefone</span>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxPhone" runat="server" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>Data de cadastro</span>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxRegistrationDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>Categoria</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCategory" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>Status</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <p>
        <asp:UpdatePanel ID="updtGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="grvEstablishment" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                    OnRowDataBound="grvEstablishment_RowDataBound" DataKeyNames="Id" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Ações" ItemStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnEdit" runat="server" Text="Editar" OnClick="lbtnEdit_Click" />
                                <asp:LinkButton ID="lbtnDelete" runat="server" Text="Apagar" OnClick="lbtnDelete_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CompanyName" HeaderText="Razão Social" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="TradingName" HeaderText="Nome Fantasia" Visible="false" />
                        <asp:BoundField DataField="Cnpj" HeaderText="CNPJ" />
                        <asp:BoundField DataField="Mail" HeaderText="E-mail" Visible="false" />
                        <asp:BoundField DataField="Address" HeaderText="Endereço" Visible="false" />
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cidade">
                            <ItemTemplate>
                                <asp:Label ID="lblCity" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Phone" HeaderText="Telefone" />
                        <asp:TemplateField HeaderText="Data de Cadastro" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRegistrationDate" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Categoria" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblCategory" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </p>
    <%-- Hidden fields --%>
    <div style="display: none">
        <asp:UpdatePanel ID="updtHiddenFields" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
                <asp:HiddenField ID="hdfHidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
