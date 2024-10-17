<%@ Page Title="" Language="C#" MasterPageFile="~/ResearchSoft.Master" AutoEventWireup="true" CodeBehind="ListarGruposInvestigacion.aspx.cs" Inherits="ResearchSoftPUCP.ListarGruposInvestigacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Listar Grupos de Investigación
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container">
        <div class="container row">
            <h2>Listado de Grupos de Investigación</h2>
        </div>
        <div class="container row">
            <div class="row align-items-center">
                <div class="col-auto p-2">
                    <asp:Label  ID="lblNombre" CssClass="form-label" runat="server" Text="Ingrese el nombre o acrónimo del grupo:"></asp:Label>
                </div>
                <div class="col-sm-3 p-2">
                    <asp:TextBox ID="txtNombreAcronimo" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-2 p-2">
                    <asp:LinkButton ID="lbBuscar" runat="server" CssClass="btn btn-info" Text="<i class='fa-solid fa-magnifying-glass pe-2'></i> Buscar" OnClick="lbBuscar_Click"/>
                </div>
                <div class="col text-end p-3">
                    <asp:LinkButton ID="lbRegistrar" runat="server" CssClass="btn btn-success" Text="<i class='fa-solid fa-plus pe-2'></i> Registrar Grupo" OnClick="lbRegistrar_Click"/>
                </div>
            </div>
        </div>
        <div class="container row">
            <asp:GridView ID="gvGruposInvestigacion" runat="server" AllowPaging="true" PageSize="5" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                CssClass="table table-hover table-responsive table-striped" OnPageIndexChanging="gvGruposInvestigacion_PageIndexChanging" OnRowDataBound="gvGruposInvestigacion_RowDataBound">
                <columns>
                    <asp:BoundField HeaderText="Id" />
                    <asp:BoundField HeaderText="Acronimo" />
                    <asp:BoundField HeaderText="Nombre" />
                    <asp:TemplateField>
                        <itemtemplate>
                            <asp:LinkButton CssClass="btn btn-warning" runat="server" Text="<i class='fa-solid fa-eye'></i> Mostrar Datos" OnClick="visualizar_Click" CommandArgument='<%# Eval("IdGrupoInvestigacion") %>'/>
                        </itemtemplate>
                    </asp:TemplateField>
                </columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>