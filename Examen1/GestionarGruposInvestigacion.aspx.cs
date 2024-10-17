using ResearchSoftPUCPDA.DAO;
using ResearchSoftPUCPDA.MySQL;
using ResearchSoftPUCPModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
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
    public partial class GestionarGruposInvestigacion : System.Web.UI.Page
    {
        private byte[] foto;
        private DepartamentoAcademicoDAO daoDepartamento = new DepartamentoAcademicoMySQL();
        private MiembroPUCPDAO daoMiembroPUCP = new MiembroPUCPMySQL();
        private GrupoInvestigacionDAO daoGrupoPUCP = new GrupoInvestigacionMySQL();
        private BindingList<MiembroPUCP> miembros;
        private static BindingList<MiembroPUCP> seleccionados = new BindingList<MiembroPUCP>();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //Cargando areas
            ddlDepartamento.DataSource = daoDepartamento.listarTodos();
            ddlDepartamento.DataTextField = "Nombre";
            ddlDepartamento.DataValueField = "IdDepartamentoAcademico";
            ddlDepartamento.DataBind();
            //Cargando miembrospucp
            miembros = daoMiembroPUCP.listarPorNombreCodigoPUCP("");
            gvMiembrosPUCP.DataSource = miembros;
            gvMiembrosPUCP.DataBind();
            gvIntegrantes.DataSource = seleccionados;
            gvIntegrantes.DataBind();
            if (Session["foto"] != null)
                foto = (byte[]) Session["foto"];
            string accion = Request.QueryString["accion"];
            if (accion == null) {//ingresando por el registro
                lblTitulo.Text = "Registrar Grupo de Investigación";
                if (!IsPostBack)
                {
                    Session["empleado"] = null;
                }
            }
            else if(accion.Equals("visualizar") && Session["grupo"]!=null){//ingresando por la modificacion
                lblTitulo.Text = "'Datos del Grupo de Investigación";
                mostrarDatos();
            }
        }

        public void mostrarDatos()
        {
            lblTitulo.Text = "Datos del Grupo de Investigación";
            //Completar con la asignación
            int id = ((GrupoInvestigacion)Session["grupo"]).IdGrupoInvestigacion;
            GrupoInvestigacion grupo = daoGrupoPUCP.obtenerPorID(id);
            txtIdGrupo.Text = grupo.IdGrupoInvestigacion.ToString();
            txtAcronimo.Text = grupo.Acronimo.ToString();
            txtNombre.Text = grupo.Nombre.ToString();
            int activo;
            activo = ((grupo.DepartamentoAcademico.Nombre.Equals("BÁSICA")) ? 0 : 1);
            if (activo == 0) ddlDepartamento.Items[0].Selected = true;
            else if (activo == 1) ddlDepartamento.Items[1].Selected = true;
            dtpFechaFundacion.Text = grupo.FechaFundacion.ToString("yyyy-MM-dd");
            txtPresupuestoAnual.Text = grupo.PresupuestoAnualDesignado.ToString("N2");
            rbTipoInvestigacion.SelectedValue = grupo.TipoInvestigacion.ToString();
            cbInfraestructura.Items[0].Selected = grupo.PoseeLaboratorio;
            cbInfraestructura.Items[1].Selected = grupo.PoseeEquipamientoEspecializado;
            cbInfraestructura.Items[2].Selected = grupo.PoseeAmbienteTrabajo;
            txtDescripcion.InnerText = grupo.Descripcion;
            string base64String = Convert.ToBase64String(grupo.Foto);
            string imageUrl = "data:image/jpeg;base64," + base64String;
            imgFotoGrupo.ImageUrl = imageUrl;
            seleccionados = daoMiembroPUCP.listarPorIdGrupoInvestigacion(id);
            gvIntegrantes.DataSource = seleccionados;
            gvIntegrantes.DataBind();

            //Desactivamos los controles para evitar su edicion
            txtNombre.Enabled = false;
            txtAcronimo.Enabled = false;
            ddlDepartamento.Enabled = false;
            btnGuardar.Enabled = false;
            btnSubirFotoGrupo.Visible = false;
            fileUploadFotoGrupo.Visible = false;
            rbTipoInvestigacion.Enabled = false;
            cbInfraestructura.Enabled = false;
            dtpFechaFundacion.Enabled = false;
            txtPresupuestoAnual.Enabled = false;
            txtDescripcion.Disabled = true;
            lbBuscarIntegrante.Visible = false;
            lbAgregarIntegrante.Visible = false;
            gvIntegrantes.Columns[4].Visible = false;
        }

        protected void btnSubirFotoGrupo_Click(object sender, EventArgs e)
        {
            //Verificar si se seleccionó un archivo
            if (fileUploadFotoGrupo.HasFile)
            {
                // Obtener la extensión del archivo
                string extension = System.IO.Path.GetExtension(fileUploadFotoGrupo.FileName);
                // Verificar si el archivo es una imagen
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif")
                {
                    // Guardar la imagen en el servidor
                    string filename = Guid.NewGuid().ToString() + extension;
                    string filePath = Server.MapPath("~/Uploads/") + filename;
                    fileUploadFotoGrupo.SaveAs(Server.MapPath("~/Uploads/") + filename);
                    // Mostrar la imagen en la página
                    imgFotoGrupo.ImageUrl = "~/Uploads/" + filename;
                    imgFotoGrupo.Visible = true;
                    // Guardamos la referencia en una variable de sesión llamada foto
                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    Session["foto"] = br.ReadBytes((int)fs.Length);
                    fs.Close();
                }
                else
                {
                    // Mostrar un mensaje de error si el archivo no es una imagen
                    Response.Write("Por favor, selecciona un archivo de imagen válido.");
                }
            }
            else
            {
                // Mostrar un mensaje de error si no se seleccionó ningún archivo
                Response.Write("Por favor, selecciona un archivo de imagen.");
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void lbBuscarIntegrante_Click(object sender, EventArgs e)
        {
            string script = "window.onload = function() { showModalForm()};";
            ScriptManager.RegisterStartupScript(this, GetType(), "", script, true);
        }

        protected void gvIntegrantes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIntegrantes.PageIndex = e.NewPageIndex;
            gvIntegrantes.DataBind();
        }

        protected void gvMiembrosPUCP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMiembrosPUCP.PageIndex = e.NewPageIndex;
            gvMiembrosPUCP.DataBind();
        }

        protected void gvMiembrosPUCP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = DataBinder.Eval(e.Row.DataItem, "TipoMiembro").ToString();
                e.Row.Cells[1].Text = DataBinder.Eval(e.Row.DataItem, "CodigoPUCP").ToString();
                e.Row.Cells[2].Text = DataBinder.Eval(e.Row.DataItem, "NombreCompleto").ToString();
                e.Row.Cells[3].Text = DataBinder.Eval(e.Row.DataItem, "Caracteristica").ToString();
            }
        }

        protected void lbSeleccionarMiembroPUCP_Click(object sender, EventArgs e)
        {
            int idMiembroPUCP = Int32.Parse(((LinkButton)sender).CommandArgument);
            MiembroPUCP miembro = miembros.SingleOrDefault(x => x.IdMiembroPUCP == idMiembroPUCP);
            txtNombreIntegrante.Text = miembro.NombreCompleto;
        }

        protected void lbBuscarMiembroPUCPModal_Click(object sender, EventArgs e)
        {
            string codNombre = txtNombreCodigoPUCP.Text;
            miembros = daoMiembroPUCP.listarPorNombreCodigoPUCP(codNombre);
            gvMiembrosPUCP.DataSource = miembros;
            gvMiembrosPUCP.DataBind();
        }

        protected void lbAgregarIntegrante_Click(object sender, EventArgs e)
        {
            string codNombre = txtNombreIntegrante.Text;
            MiembroPUCP miembro = miembros.SingleOrDefault(x => x.NombreCompleto.Equals(codNombre));
            seleccionados.Add(miembro);
            gvIntegrantes.DataSource = seleccionados;
            gvIntegrantes.DataBind();
        }

        protected void gvIntegrantes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = DataBinder.Eval(e.Row.DataItem, "TipoMiembro").ToString();
                e.Row.Cells[1].Text = DataBinder.Eval(e.Row.DataItem, "CodigoPUCP").ToString();
                e.Row.Cells[2].Text = DataBinder.Eval(e.Row.DataItem, "NombreCompleto").ToString();
                e.Row.Cells[3].Text = DataBinder.Eval(e.Row.DataItem, "Caracteristica").ToString();
                //e.Row.Cells[4].Text = DataBinder.Eval(e.Row.DataItem, "IdMiembroPUCP").ToString(); ;
                //e.Row.Cells[4].Visible = false;
            }
        }

        protected void eliminarMiembro_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(((LinkButton)sender).CommandArgument);
            seleccionados.RemoveAt(id);
            gvIntegrantes.DataSource = seleccionados;
            gvIntegrantes.DataBind();
        }

        protected void guardarDatosGrupo_click(object sender, EventArgs e)
        {//VERIFICAR QUE NO ESTEN VACIOS
            GrupoInvestigacion grupo = new GrupoInvestigacion();
            daoGrupoPUCP = new GrupoInvestigacionMySQL();
            grupo.Acronimo = txtAcronimo.Text;
            grupo.Nombre = txtNombre.Text;
            DepartamentoAcademico departamento = new DepartamentoAcademico();
            departamento.IdDepartamentoAcademico = Int32.Parse(ddlDepartamento.SelectedItem.Value);
            grupo.DepartamentoAcademico = departamento;
            grupo.FechaFundacion = DateTime.Parse(dtpFechaFundacion.Text);
            grupo.PresupuestoAnualDesignado = Double.Parse(txtPresupuestoAnual.Text);
            grupo.TipoInvestigacion = (TipoInvestigacion)Enum.Parse(typeof(TipoInvestigacion), rbTipoInvestigacion.Text);
            grupo.PoseeLaboratorio = cbInfraestructura.Items[0].Selected;
            grupo.PoseeEquipamientoEspecializado = cbInfraestructura.Items[1].Selected;
            grupo.PoseeAmbienteTrabajo = cbInfraestructura.Items[2].Selected;
            grupo.Descripcion = txtDescripcion.InnerText;
            grupo.Foto = (byte[])Session["foto"];
            grupo.Integrantes = new BindingList<MiembroPUCP>();
            foreach (GridViewRow row in gvIntegrantes.Rows)
            {
                MiembroPUCP miembro = new MiembroPUCP();
                miembro.IdMiembroPUCP = miembros.SingleOrDefault(x => x.CodigoPUCP.Equals(row.Cells[1].Text)).IdMiembroPUCP;
                grupo.Integrantes.Add(miembro);
            }
            seleccionados.Clear();
            daoGrupoPUCP.insertar(grupo);
            Response.Redirect("ListarGruposInvestigacion.aspx");
        }
    }
}