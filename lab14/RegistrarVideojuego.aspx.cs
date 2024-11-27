using GameSoftWA.GameSoftRef;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GameSoftWA
{
    /* Colocar sus datos personales
     * ------------------------------------------------
     * Nombre Completo: Rodrigo Alejandro Holguin Huari
     * Codigo PUCP: 20221466
     * ------------------------------------------------
     */

    public partial class RegistrarVideojuego : System.Web.UI.Page
    {
        private GeneroWSClient daoGenero;
        private BindingList<genero> generos;
        private VideojuegoWSClient daoVideojuego;
        protected void Page_Load(object sender, EventArgs e)
        {
            String accion = Request.QueryString["accion"];
            String idVideojuego = Request.QueryString["idVideojuego"];

            if (!IsPostBack) {
                daoGenero = new GeneroWSClient();
                try
                {
                    generos = new BindingList<genero>(daoGenero.listarGeneros());
                    ddlGenero.DataSource = generos;
                    ddlGenero.DataTextField = "nombre";
                    ddlGenero.DataValueField = "idGenero";
                    ddlGenero.DataBind();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Response.Redirect("ListarVideojuegos.aspx");
                }
            }
            
            if (accion != null && accion == "ver" && idVideojuego != null)
            {
                lblTitulo.Text = "Visualizar Videojuego";
                cargarDatos(Int32.Parse(idVideojuego));
                deshabilitarCampos();
            }
            else
                lblTitulo.Text = "Registrar Videojuego";
            if (IsPostBack && fileUploadFotoVideojuego.PostedFile != null && fileUploadFotoVideojuego.HasFile)
            {
                string extension = System.IO.Path.GetExtension(fileUploadFotoVideojuego.FileName);
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif")
                {
                    string filename = Guid.NewGuid().ToString() + extension;
                    string filePath = Server.MapPath("~/Uploads/") + filename;
                    fileUploadFotoVideojuego.SaveAs(Server.MapPath("~/Uploads/") + filename);
                    imgFotoVideojuego.ImageUrl = "~/Uploads/" + filename;
                    imgFotoVideojuego.Visible = true;
                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    Session["foto"] = br.ReadBytes((int)fs.Length);
                    fs.Close();
                }
                else
                {
                    Response.Write("Por favor, selecciona un archivo de imagen válido.");
                }
            }
        }
        private void cargarDatos(int idVideojuego)
        {
            daoVideojuego = new VideojuegoWSClient();
            videojuego videojuego = daoVideojuego.obtenerVidPorId(idVideojuego);
            txtIdVideojuego.Text = videojuego.idVideojuego.ToString();
            txtNombre.Text = videojuego.nombre;
            ddlGenero.SelectedValue = videojuego.genero.idGenero.ToString();
            dtpFechaLanzamiento.Value = videojuego.fechaLanzamiento.ToString("yyyy-MM-dd");
            txtCostoDesarrollo.Text = videojuego.costoDesarrollo.ToString();
            string base64String = Convert.ToBase64String(videojuego.foto);
            string imageUrl = "data:image/jpeg;base64," + base64String;
            imgFotoVideojuego.ImageUrl = imageUrl;
            if (videojuego.clasificacion == clasificacion.TEEN) rbTeen.Checked = true;
            else if (videojuego.clasificacion == clasificacion.MATURE) rbMature.Checked = true;
            else if (videojuego.clasificacion == clasificacion.EVERYONE) rbEveryone.Checked = true;
            else if (videojuego.clasificacion == clasificacion.ADULTSONLY) rbAdultsOnly.Checked = true;
        }
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarVideojuegos.aspx");
        }

        public void deshabilitarCampos()
        {
            txtNombre.Enabled = false;
            ddlGenero.Enabled = false;
            dtpFechaLanzamiento.Disabled = true;
            txtCostoDesarrollo.Enabled = false;
            fileUploadFotoVideojuego.Enabled = false;
            rbEveryone.Disabled = true;
            rbTeen.Disabled = true;
            rbMature.Disabled = true;
            rbAdultsOnly.Disabled = true;
            btnGuardar.Visible = false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //CARGANDO LOS DATOS
            videojuego vid = new videojuego();
            vid.nombre = txtNombre.Text;
            vid.genero = new genero();
            vid.genero.idGenero = Int32.Parse(ddlGenero.SelectedValue);
            vid.fechaLanzamiento = DateTime.Parse(dtpFechaLanzamiento.Value);
            vid.fechaLanzamientoSpecified = true;
            vid.costoDesarrollo = Double.Parse(txtCostoDesarrollo.Text);
            vid.foto = (byte[])Session["foto"];
            if (rbAdultsOnly.Checked) vid.clasificacion = clasificacion.ADULTSONLY;
            else if (rbEveryone.Checked) vid.clasificacion = clasificacion.EVERYONE;
            else if (rbTeen.Checked) vid.clasificacion = clasificacion.TEEN;
            else if (rbMature.Checked) vid.clasificacion = clasificacion.MATURE;
            vid.clasificacionSpecified = true;
            //INSERTANDO EL OBJETO
            daoVideojuego = new VideojuegoWSClient();
            int resultado = daoVideojuego.insertarVideojuego(vid);
            if(resultado != 0) Response.Redirect("ListarVideojuegos.aspx");
            else Response.Redirect("RegistrarVideojuego.aspx");
        }
    }
}