using MySql.Data.MySqlClient;
using ResearchSoftPUCPDA.DAO;
using ResearchSoftPUCPDBManager;
using ResearchSoftPUCPModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchSoftPUCPDA.MySQL
{
    public class GrupoInvestigacionMySQL : GrupoInvestigacionDAO
    {
        private MySqlDataReader lector;
        public int insertar(GrupoInvestigacion grupoInvestigacion)
        {
            int resultado = 0;
            MySqlParameter[] parametros = new MySqlParameter[]{
                new MySqlParameter("_fid_departamento_academico", MySqlDbType.Int32) { Value = grupoInvestigacion.DepartamentoAcademico.IdDepartamentoAcademico },
                new MySqlParameter("_nombre", MySqlDbType.VarChar) { Value = grupoInvestigacion.Nombre},
                new MySqlParameter("_acronimo", MySqlDbType.VarChar) { Value = grupoInvestigacion.Acronimo},
                new MySqlParameter("_tipo_investigacion", MySqlDbType.VarChar) { Value = grupoInvestigacion.TipoInvestigacion.ToString()},
                new MySqlParameter("_fecha_fundacion", MySqlDbType.Date) { Value = grupoInvestigacion.FechaFundacion},
                new MySqlParameter("_presupuesto_anual_designado", MySqlDbType.Double) { Value = grupoInvestigacion.PresupuestoAnualDesignado},
                new MySqlParameter("_posee_laboratorio", MySqlDbType.Int16) { Value = grupoInvestigacion.PoseeLaboratorio},
                new MySqlParameter("_posee_equipamiento_especializado", MySqlDbType.Int16) { Value = grupoInvestigacion.PoseeEquipamientoEspecializado},
                new MySqlParameter("_posee_ambiente_trabajo", MySqlDbType.Int16) { Value = grupoInvestigacion.PoseeAmbienteTrabajo},
                new MySqlParameter("_descripcion", MySqlDbType.VarChar) { Value = grupoInvestigacion.Descripcion},
                new MySqlParameter("_foto", MySqlDbType.LongBlob) { Value = grupoInvestigacion.Foto},
            };
            grupoInvestigacion.IdGrupoInvestigacion = DBPoolManager.Instance.EjecutarProcedimiento("INSERTAR_GRUPO_INVESTIGACION", parametros, "_id_grupo_investigacion");
            resultado = grupoInvestigacion.IdGrupoInvestigacion;
            //ahora se debe registrar en la base de datos los miembros asociados a este grupo
            foreach (MiembroPUCP miembro in grupoInvestigacion.Integrantes) {
                parametros = new MySqlParameter[]{
                    new MySqlParameter("_fid_grupo_investigacion", MySqlDbType.Int32) { Value = resultado },
                    new MySqlParameter("_fid_miembro_pucp", MySqlDbType.Int32) { Value = miembro.IdMiembroPUCP},    
                };
                DBPoolManager.Instance.EjecutarProcedimiento("INSERTAR_INTEGRANTE_GRUPO_INVESTIGACION", parametros, "_id_integrante_grupo_investigacion");
            }
            return resultado;
        }

        public BindingList<GrupoInvestigacion> listarPorNombreAcronimo(string nombreAcronimo)
        {
            MySqlParameter[] parametros = new MySqlParameter[] {
                new MySqlParameter("_nombre_acronimo", MySqlDbType.VarChar) { Value = nombreAcronimo }
            };
            lector =
                DBPoolManager.Instance.EjecutarProcedimientoLectura(
                    "LISTAR_GRUPOS_INVESTIGACION_X_NOMBRE_ACRONIMO",
                    parametros);
            //ahora si comienza
            BindingList<GrupoInvestigacion> grupos = new BindingList<GrupoInvestigacion>();
            try
            {
                while (lector.Read())
                {
                    GrupoInvestigacion grupo = new GrupoInvestigacion();
                    if (!lector.IsDBNull(lector.GetOrdinal("id_grupo_investigacion"))) grupo.IdGrupoInvestigacion = lector.GetInt32("id_grupo_investigacion");
                    if (!lector.IsDBNull(lector.GetOrdinal("acronimo"))) grupo.Acronimo = lector.GetString("acronimo");
                    if (!lector.IsDBNull(lector.GetOrdinal("nombre"))) grupo.Nombre = lector.GetString("nombre");
                    grupos.Add(grupo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            lector.Close();
            return grupos;
        }

        public GrupoInvestigacion obtenerPorID(int idGrupoInvestigacion)
        {
            MySqlParameter[] parametros = new MySqlParameter[] {
                new MySqlParameter("_id_grupo_investigacion", MySqlDbType.VarChar) { Value = idGrupoInvestigacion }
            };
            lector =
                DBPoolManager.Instance.EjecutarProcedimientoLectura(
                    "OBTENER_GRUPO_INVESTIGACION_X_ID",
                    parametros);
            //ahora si comienza
            GrupoInvestigacion grupo = new GrupoInvestigacion();
            try
            {
                if (lector.Read())
                {
                    if (!lector.IsDBNull(lector.GetOrdinal("id_grupo_investigacion"))) grupo.IdGrupoInvestigacion = lector.GetInt32("id_grupo_investigacion");
                    DepartamentoAcademico departamento = new DepartamentoAcademico();
                    if (!lector.IsDBNull(lector.GetOrdinal("id_departamento_academico"))) departamento.IdDepartamentoAcademico = lector.GetInt32("id_departamento_academico");
                    if (!lector.IsDBNull(lector.GetOrdinal("nombre_departamento_academico"))) departamento.Nombre = lector.GetString("nombre_departamento_academico");
                    grupo.DepartamentoAcademico = departamento;
                    if (!lector.IsDBNull(lector.GetOrdinal("nombre_grupo"))) grupo.Nombre = lector.GetString("nombre_grupo");
                    if (!lector.IsDBNull(lector.GetOrdinal("acronimo"))) grupo.Acronimo = lector.GetString("acronimo");
                    if (!lector.IsDBNull(lector.GetOrdinal("tipo_investigacion"))) grupo.TipoInvestigacion = (TipoInvestigacion)Enum.Parse(typeof(TipoInvestigacion), lector.GetString("tipo_investigacion"));
                    if (!lector.IsDBNull(lector.GetOrdinal("fecha_fundacion"))) grupo.FechaFundacion = lector.GetDateTime("fecha_fundacion");
                    if (!lector.IsDBNull(lector.GetOrdinal("presupuesto_anual_designado"))) grupo.PresupuestoAnualDesignado = lector.GetDouble("presupuesto_anual_designado");
                    if (!lector.IsDBNull(lector.GetOrdinal("posee_laboratorio"))) grupo.PoseeLaboratorio = lector.GetBoolean("posee_laboratorio");
                    if (!lector.IsDBNull(lector.GetOrdinal("posee_equipamiento_especializado"))) grupo.PoseeEquipamientoEspecializado = lector.GetBoolean("posee_equipamiento_especializado");
                    if (!lector.IsDBNull(lector.GetOrdinal("posee_ambiente_trabajo"))) grupo.PoseeAmbienteTrabajo = lector.GetBoolean("posee_ambiente_trabajo");
                    if (!lector.IsDBNull(lector.GetOrdinal("descripcion"))) grupo.Descripcion = lector.GetString("descripcion");
                    if (!lector.IsDBNull(lector.GetOrdinal("posee_ambiente_trabajo"))) grupo.Foto = (byte[])lector["foto"];
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            lector.Close();
            return grupo;
        }
    }
}
