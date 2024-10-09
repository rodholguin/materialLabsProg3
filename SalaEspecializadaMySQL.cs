using MedicalSoftController.DAO;
using MedicalSoftDBManager;
using MedicalSoftModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSoftController.MySQL
{
    public class SalaEspecializadaMySQL : SalaEspecializadaDAO
    {
        private MySqlConnection con;
        private MySqlCommand comando;
        private MySqlDataReader lector;
        public int eliminar(int idArea)
        {
            int resultado = 0;
            try
            {
                con = DBManager.Instance.Connection;
                con.Open();
                comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "ELIMINAR_SALA_ESPECIALIZADA";
                comando.Parameters.AddWithValue("_id_sala_especializada", idArea);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return resultado;
        }

        public int insertar(SalaEspecializada salaEspecializada)
        {
            int resultado = 0;
            try
            {
                con = DBManager.Instance.Connection;
                con.Open();
                comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "INSERTAR_SALA_ESPECIALIZADA";
                comando.Parameters.Add("_id_sala_especializada", MySqlDbType.Int32)
                    .Direction = System.Data.ParameterDirection.Output;
                comando.Parameters.AddWithValue("_espacio_en_m2", salaEspecializada.EspacioMetrosCuadrados);
                comando.Parameters.AddWithValue("_torre", salaEspecializada.Torre);
                comando.Parameters.AddWithValue("_piso", salaEspecializada.Piso);
                comando.Parameters.AddWithValue("_nombre", salaEspecializada.Nombre);
                comando.Parameters.AddWithValue("_tipo_sala", salaEspecializada.TipoSala.ToString());
                comando.Parameters.AddWithValue("_posee_equipamiento_imagenologia", salaEspecializada.PoseeEquipamientoImagenologia);
                comando.ExecuteNonQuery();
                salaEspecializada.IdAmbienteClinico = Int32.Parse(comando.Parameters["_id_sala_especializada"].Value.ToString());
                resultado = salaEspecializada.IdAmbienteClinico;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return resultado;
        }

        public BindingList<SalaEspecializada> listarTodas()
        {
            BindingList<SalaEspecializada> salasEspecializadas = new BindingList<SalaEspecializada>();
            try
            {
                con = DBManager.Instance.Connection;
                con.Open();
                comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "LISTAR_SALAS_ESPECIALIZADAS_TODAS";
                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    SalaEspecializada salaEspecializada = new SalaEspecializada();
                    if (!lector.IsDBNull(lector.GetOrdinal("id_sala_especializada"))) salaEspecializada.IdAmbienteClinico = lector.GetInt32("id_sala_especializada");
                    if (!lector.IsDBNull(lector.GetOrdinal("espacio_en_m2"))) salaEspecializada.EspacioMetrosCuadrados = lector.GetDouble("espacio_en_m2");
                    if (!lector.IsDBNull(lector.GetOrdinal("torre"))) salaEspecializada.Torre = lector.GetChar("torre");
                    if (!lector.IsDBNull(lector.GetOrdinal("piso"))) salaEspecializada.Piso = lector.GetInt32("piso");
                    if (!lector.IsDBNull(lector.GetOrdinal("nombre"))) salaEspecializada.Nombre = lector.GetString("nombre");
                    if (!lector.IsDBNull(lector.GetOrdinal("tipo_sala"))) salaEspecializada.TipoSala = (TipoSala)Enum.Parse(typeof(TipoSala), lector.GetString("tipo_sala"));
                    if (!lector.IsDBNull(lector.GetOrdinal("posee_equipamiento_imagenologia"))) salaEspecializada.PoseeEquipamientoImagenologia = lector.GetBoolean("posee_equipamiento_imagenologia");
                    salaEspecializada.Activo = true;
                    salasEspecializadas.Add(salaEspecializada);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return salasEspecializadas;
        }

        public int modificar(SalaEspecializada salaEspecializada)
        {
            int resultado = 0;
            try
            {
                con = DBManager.Instance.Connection;
                con.Open();
                comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "MODIFICAR_SALA_ESPECIALIZADA";
                comando.Parameters.AddWithValue("_id_sala_especializada", salaEspecializada.IdAmbienteClinico);
                comando.Parameters.AddWithValue("_espacio_en_m2", salaEspecializada.EspacioMetrosCuadrados);
                comando.Parameters.AddWithValue("_torre", salaEspecializada.Torre);
                comando.Parameters.AddWithValue("_piso", salaEspecializada.Piso);
                comando.Parameters.AddWithValue("_nombre", salaEspecializada.Nombre);
                comando.Parameters.AddWithValue("_tipo_sala", salaEspecializada.TipoSala.ToString());
                comando.Parameters.AddWithValue("_posee_equipamiento_imagenologia", salaEspecializada.PoseeEquipamientoImagenologia);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return resultado;
        }

        public SalaEspecializada obtenerPorId(int idArea)
        {
            SalaEspecializada salaEspecializada = new SalaEspecializada();
            try
            {
                con = DBManager.Instance.Connection;
                con.Open();
                comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "OBTENER_SALA_ESPECIALIZADA_X_ID";
                comando.Parameters.AddWithValue("_id_sala_especializada", idArea);
                lector = comando.ExecuteReader();
                if (lector.Read())
                {
                    if (!lector.IsDBNull(lector.GetOrdinal("id_sala_especializada"))) salaEspecializada.IdAmbienteClinico = lector.GetInt32("id_sala_especializada");
                    if (!lector.IsDBNull(lector.GetOrdinal("espacio_en_m2"))) salaEspecializada.EspacioMetrosCuadrados = lector.GetDouble("espacio_en_m2");
                    if (!lector.IsDBNull(lector.GetOrdinal("torre"))) salaEspecializada.Torre = lector.GetChar("torre");
                    if (!lector.IsDBNull(lector.GetOrdinal("piso"))) salaEspecializada.Piso = lector.GetInt32("piso");
                    if (!lector.IsDBNull(lector.GetOrdinal("nombre"))) salaEspecializada.Nombre = lector.GetString("nombre");
                    if (!lector.IsDBNull(lector.GetOrdinal("tipo_sala"))) salaEspecializada.TipoSala = (TipoSala)Enum.Parse(typeof(TipoSala), lector.GetString("tipo_sala"));
                    if (!lector.IsDBNull(lector.GetOrdinal("posee_equipamiento_imagenologia"))) salaEspecializada.PoseeEquipamientoImagenologia = lector.GetBoolean("posee_equipamiento_imagenologia");
                    salaEspecializada.Activo = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return salaEspecializada;
        }
    }
}
