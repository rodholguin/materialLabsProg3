using GameSoftWA.GameSoftRef;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GameSoftWA
{
    /* Colocar los datos personales
     * ------------------------------------------------
     * Nombre Completo: Rodrigo Alejandro Holguin Huari
     * Codigo PUCP: 20221466
     * ------------------------------------------------
     */
    public partial class ListarVideojuegos : System.Web.UI.Page
    {
        private VideojuegoWSClient daoVideojuego;
        private BindingList<videojuego> videojuegos;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                daoVideojuego = new VideojuegoWSClient();
                videojuego[] vids = daoVideojuego.listarVidPorNombre("");
                if (vids != null) {
                    videojuegos = new BindingList<videojuego>(vids);
                    gvVideojuegos.DataSource = videojuegos;
                    gvVideojuegos.DataBind();
                    ViewState["videojuegos"] = videojuegos;
                }
            }
            else {
                videojuegos = (BindingList<videojuego>)ViewState["videojuegos"];
                gvVideojuegos.DataSource = videojuegos;
                gvVideojuegos.DataBind();
            }
        }

        protected void lbRegistrar_Click(object sender, EventArgs e)
        {
            Response.Redirect("RegistrarVideojuego.aspx");
        }

        protected void lbVisualizar_Click(object sender, EventArgs e)
        {
            int idVideojuego = Int32.Parse(((LinkButton)sender).CommandArgument);
            Response.Redirect("RegistrarVideojuego.aspx?accion=ver&idVideojuego=" + idVideojuego.ToString());
        }

        protected void gvVideojuegos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = DataBinder.Eval(e.Row.DataItem, "idVideojuego").ToString();
                e.Row.Cells[1].Text = DataBinder.Eval(e.Row.DataItem, "nombre").ToString();
                e.Row.Cells[2].Text = ((genero)DataBinder.Eval(e.Row.DataItem, "genero")).nombre;
                e.Row.Cells[3].Text = ((clasificacion)DataBinder.Eval(e.Row.DataItem, "clasificacion")).ToString();
            }
        }

        protected void lbBuscar_Click(object sender, EventArgs e)
        {
            daoVideojuego = new VideojuegoWSClient();
            videojuego[] vids = daoVideojuego.listarVidPorNombre(txtNombre.Text);
            if (vids != null) {
                videojuegos = new BindingList<videojuego>(vids);
                gvVideojuegos.DataSource = videojuegos;
            } else {
                gvVideojuegos.DataSource = null;
                videojuegos = null;
            }
            gvVideojuegos.DataBind();
            ViewState["videojuegos"] = videojuegos;
        }
    }
}