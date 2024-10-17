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
    public class MiembroPUCPMySQL : MiembroPUCPDAO
    {
        private MySqlDataReader lector;

        public BindingList<MiembroPUCP> listarPorIdGrupoInvestigacion(int idGrupoInvestigacion)
        {
            MySqlParameter[] parametros = new MySqlParameter[]{
                new MySqlParameter("_id_grupo_investigacion", MySqlDbType.Int32) { Value = idGrupoInvestigacion },
            };
            lector = DBPoolManager.Instance.EjecutarProcedimientoLectura("LISTAR_INTEGRANTES_X_ID_GRUPO_INVESTIGACION", parametros);
            BindingList<MiembroPUCP> miembros = new BindingList<MiembroPUCP>();
            MiembroPUCP miembro;
            int tipo;
            try
            {
                while (lector.Read())
                {
                    if (!lector.IsDBNull(lector.GetOrdinal("fid_tipo_miembro_pucp")))
                    {
                        tipo = ((lector.GetString("fid_tipo_miembro_pucp").Equals("P")) ? 0 : 1);
                        if (tipo == 0) miembro = new Profesor();
                        else miembro = new Estudiante();
                        if (!lector.IsDBNull(lector.GetOrdinal("id_miembro_pucp"))) miembro.IdMiembroPUCP = lector.GetInt32("id_miembro_pucp");
                        if (!lector.IsDBNull(lector.GetOrdinal("codigo_pucp"))) miembro.CodigoPUCP = lector.GetString("codigo_pucp");
                        if (!lector.IsDBNull(lector.GetOrdinal("nombre"))) miembro.Nombre = lector.GetString("nombre");
                        if (!lector.IsDBNull(lector.GetOrdinal("apellido_paterno"))) miembro.ApellidoPaterno = lector.GetString("apellido_paterno");
                        if (tipo == 0)
                        {
                            if (!lector.IsDBNull(lector.GetOrdinal("dedicacion"))) ((Profesor)miembro).Dedicacion = lector.GetString("dedicacion");
                        }
                        else if (tipo == 1)
                        {
                            if (!lector.IsDBNull(lector.GetOrdinal("CRAEST"))) ((Estudiante)miembro).CRAEST = lector.GetDouble("CRAEST");

                        }
                        miembros.Add(miembro);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lector.Close();
            }
            return miembros;
        }

        public BindingList<MiembroPUCP> listarPorNombreCodigoPUCP(string nombreCodigoPUCP)
        {
            MySqlParameter[] parametros = new MySqlParameter[]{
                new MySqlParameter("_nombre_codigoPUCP", MySqlDbType.VarChar) { Value = nombreCodigoPUCP },
            };
            lector =  DBPoolManager.Instance.EjecutarProcedimientoLectura("LISTAR_MIEMBROS_PUCP_X_NOMBRE_CODIGOPUCP", parametros);
            //ahora si comienza
            BindingList<MiembroPUCP> miembros = new BindingList<MiembroPUCP>();
            int tipo;
            MiembroPUCP miembro;
            try
            {
                while (lector.Read())
                {
                    if (!lector.IsDBNull(lector.GetOrdinal("fid_tipo_miembro_pucp")))
                    {
                        tipo = ((lector.GetString("fid_tipo_miembro_pucp").Equals("P")) ? 0 : 1);//0 profesor, 1 estudiante
                        if (tipo == 0) miembro = new Profesor();
                        else miembro = new Estudiante();
                        if (!lector.IsDBNull(lector.GetOrdinal("id_miembro_pucp"))) miembro.IdMiembroPUCP = lector.GetInt32("id_miembro_pucp");
                        if (!lector.IsDBNull(lector.GetOrdinal("codigo_pucp"))) miembro.CodigoPUCP = lector.GetString("codigo_pucp");
                        if (!lector.IsDBNull(lector.GetOrdinal("nombre"))) miembro.Nombre = lector.GetString("nombre");
                        if (!lector.IsDBNull(lector.GetOrdinal("apellido_paterno"))) miembro.ApellidoPaterno = lector.GetString("apellido_paterno");
                        if (tipo == 0)
                        {
                            if (!lector.IsDBNull(lector.GetOrdinal("dedicacion"))) ((Profesor)miembro).Dedicacion = lector.GetString("dedicacion");
                        }
                        else if (tipo == 1) {
                            if (!lector.IsDBNull(lector.GetOrdinal("CRAEST"))) ((Estudiante)miembro).CRAEST = lector.GetDouble("CRAEST");

                        }
                        miembros.Add(miembro);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lector.Close();
            }
            return miembros;
        }
    }
}
