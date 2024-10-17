using ResearchSoftPUCPDA.DAO;
using ResearchSoftPUCPDA.MySQL;
using ResearchSoftPUCPModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ResearchSoftPUCP
{
    /*
     * Colocar datos:
     * --------------------------------------
     * Código PUCP: 20221466
     * Nombre Completo: Rodrigo Alejandro Holguin Huari
     */
    public partial class ListarGruposInvestigacion : System.Web.UI.Page
    {
        private BindingList<GrupoInvestigacion> grupos;
        private GrupoInvestigacionDAO daoGrupoPUCP = new GrupoInvestigacionMySQL();
        protected void Page_Load(object sender, EventArgs e)
        {
            grupos = daoGrupoPUCP.listarPorNombreAcronimo("");
            gvGruposInvestigacion.DataSource = grupos;
            gvGruposInvestigacion.DataBind();
        }

        protected void lbRegistrar_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionarGruposInvestigacion.aspx"); ;
        }

        protected void gvGruposInvestigacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGruposInvestigacion.PageIndex = e.NewPageIndex;
            gvGruposInvestigacion.DataBind();
        }

        protected void gvGruposInvestigacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = DataBinder.Eval(e.Row.DataItem, "IdGrupoInvestigacion").ToString();
                e.Row.Cells[1].Text = DataBinder.Eval(e.Row.DataItem, "Acronimo").ToString();
                e.Row.Cells[2].Text = DataBinder.Eval(e.Row.DataItem, "Nombre").ToString();
            }
        }

        protected void lbBuscar_Click(object sender, EventArgs e)
        {
            string acronimo = txtNombreAcronimo.Text;
            grupos = daoGrupoPUCP.listarPorNombreAcronimo(acronimo);
            gvGruposInvestigacion.DataSource = grupos;
            gvGruposInvestigacion.DataBind();
        }

        protected void visualizar_Click(object sender, EventArgs e)
        {
            int idGrupo = Int32.Parse(((LinkButton)sender).CommandArgument);
            GrupoInvestigacion grupo = grupos.SingleOrDefault(x => x.IdGrupoInvestigacion == idGrupo);
            Session["grupo"] = grupo;
            Response.Redirect("GestionarGruposInvestigacion.aspx?accion=visualizar");
        }
    }
}