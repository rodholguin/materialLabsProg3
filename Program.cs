using MedicalSoftController.DAO;
using MedicalSoftController.MySQL;
using MedicalSoftModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSoft
{
    public class Program
    {
        /* Ingrese sus datos:
         * Codigo PUCP: 20221466
         * Nombre Completo: Rodrigo Alejandro Holguin Huari
         * */

        private static SalaEspecializadaDAO daoSalaEspecializada;
        public static void Main(string[] args)
        {
            int resultado = 0;
            daoSalaEspecializada = new SalaEspecializadaMySQL();
            SalaEspecializada salaEspecializada = new SalaEspecializada();
            BindingList<SalaEspecializada> salasEspecializadas = new BindingList<SalaEspecializada>();
            do
            {
                System.Console.WriteLine();
                System.Console.WriteLine("SISTEMA DE GESTION DE SALAS ESPECIALIZADAS");
                System.Console.WriteLine("---------------------------------------------------");
                System.Console.WriteLine("1. Registrar nueva sala especializada.");
                System.Console.WriteLine("2. Listar todas las salas especializadas.");
                System.Console.WriteLine("3. Obtener los datos de una sala especializada por id.");
                System.Console.WriteLine("4. Eliminar una sala especializada.");
                System.Console.WriteLine("5. Modificar una sala especializada.");
                System.Console.WriteLine("6. Salir del sistema de gestion.");
                System.Console.Write("Ingrese su opcion: ");
                resultado = Int32.Parse(System.Console.ReadLine());
                if (resultado == 1)
                {
                    try
                    {
                        salaEspecializada = solicitarDatosRegistro();
                        daoSalaEspecializada.insertar(salaEspecializada);
                        System.Console.WriteLine("Se ha registrado la Sala Especializada con éxito.");
                    }
                    catch (Exception ex) {
                        System.Console.WriteLine("Error en el registro de la Sala Especializada");
                        throw new Exception(ex.Message);
                    }
                }
                else if (resultado == 2)
                {
                    try
                    {
                        salasEspecializadas = daoSalaEspecializada.listarTodas();
                        foreach(SalaEspecializada sala in salasEspecializadas) sala.mostrarDatos();
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error en el listado de la Sala Especializada");
                        throw new Exception(ex.Message);
                    }
                }
                else if (resultado == 3)
                {
                    try
                    {
                        System.Console.Write("Ingrese del id de la sala cuyos datos desea visualizar: ");
                        salaEspecializada = daoSalaEspecializada.obtenerPorId(Int32.Parse(System.Console.ReadLine()));
                        salaEspecializada.mostrarDatos();
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error en la obtención de la Sala Especializada");
                        throw new Exception(ex.Message);
                    }
                }
                else if (resultado == 4)
                {
                    try
                    {
                        System.Console.Write("Ingrese del id de la sala que desea eliminar: ");
                        resultado = daoSalaEspecializada.eliminar(Int32.Parse((System.Console.ReadLine())));
                        if(resultado > 0) System.Console.WriteLine("La sala se ha eliminado con exito");
                        else System.Console.WriteLine("No se eliminó ninguna sala");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error en la eliminación de la Sala Especializada");
                        throw new Exception(ex.Message);
                    }
                }
                else if (resultado == 5)
                {
                    try
                    {
                        System.Console.Write("Ingrese del id de la sala cuyos datos desea modificar: ");
                        salaEspecializada = solicitarDatosModificar(Int32.Parse((System.Console.ReadLine())));
                        System.Console.WriteLine("datos obtenidos");
                        daoSalaEspecializada.modificar(salaEspecializada);
                        System.Console.WriteLine("La sala se ha modificado con éxito.");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error en la modificación de la Sala Especializada");
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    System.Console.WriteLine("Ingrese una opcion valida");
                }
            } while (resultado != 6);
        }

        
        public static SalaEspecializada solicitarDatosRegistro()
        {
            SalaEspecializada salaEspecializada = new SalaEspecializada();
            System.Console.Write("Ingrese el nombre de la sala: ");
            salaEspecializada.Nombre = System.Console.ReadLine();
            System.Console.Write("Ingrese el espacio en metros cuadrados de la sala: ");
            salaEspecializada.EspacioMetrosCuadrados = Double.Parse(System.Console.ReadLine());
            System.Console.Write("Ingrese la torre donde esta ubicada la sala: ");
            salaEspecializada.Torre = System.Console.ReadLine().ElementAt(0);
            System.Console.Write("Ingrese el piso dodne esta ubicada la sala: ");
            salaEspecializada.Piso = Int32.Parse(System.Console.ReadLine());
            System.Console.Write("Ingrese el tipo de sala [1.UCI - 2.CIRUGIA - 3.EMERGENCIA]: ");
            int tipoSala = Int32.Parse(System.Console.ReadLine());
            salaEspecializada.TipoSala = ((tipoSala==1)?TipoSala.UCI:((tipoSala==2)?TipoSala.CIRUGIA:TipoSala.EMERGENCIA));
            System.Console.Write("Ingrese si tiene o no equipamiento [S/N]: ");
            salaEspecializada.PoseeEquipamientoImagenologia = 
                ((System.Console.ReadLine() == "S") ?true:false);
            return salaEspecializada;
        }
        
        
        public static SalaEspecializada solicitarDatosModificar(int idSalaEspecializada)
        {
            SalaEspecializada salaEspecializada = daoSalaEspecializada.obtenerPorId(idSalaEspecializada);
            System.Console.Write("Ingrese el nombre de la sala (VALOR ACTUAL: " + salaEspecializada.Nombre + "): ");
            string nombre = System.Console.ReadLine();
            if (!nombre.Equals("")) salaEspecializada.Nombre = nombre;
            System.Console.Write("Ingrese el espacio en metros cuadrados de la sala (VALOR ACTUAL: " + salaEspecializada.EspacioMetrosCuadrados + "): ");
            string espacio = System.Console.ReadLine();
            if (!espacio.Equals("")) salaEspecializada.EspacioMetrosCuadrados = Double.Parse(espacio);
            System.Console.Write("Ingrese la torre donde esta ubicada la sala (VALOR ACTUAL: " + salaEspecializada.Torre + "): ");
            string torre = System.Console.ReadLine();
            if (!torre.Equals("")) salaEspecializada.Torre = torre.ElementAt(0);
            System.Console.Write("Ingrese el piso dodne esta ubicada la sala (VALOR ACTUAL: " + salaEspecializada.Piso + "): ");
            string piso = System.Console.ReadLine();
            if (!piso.Equals("")) salaEspecializada.Piso = Int32.Parse(piso);
            System.Console.Write("Ingrese el tipo de sala [1.UCI - 2.CIRUGIA - 3.EMERGENCIA]:  (VALOR ACTUAL: " + salaEspecializada.TipoSala + "): ");
            string tipoSala = System.Console.ReadLine();
            if (!tipoSala.Equals("")) {
                int tipo = Int32.Parse(tipoSala);
                salaEspecializada.TipoSala = ((tipo == 1) ? TipoSala.UCI : ((tipo == 2) ? TipoSala.CIRUGIA : TipoSala.EMERGENCIA));
            }
            System.Console.Write("Ingrese si tiene o no equipamiento [S/N] (VALOR ACTUAL: " + ((salaEspecializada.PoseeEquipamientoImagenologia)?"SI":"NO") + "): ");
            string equipo = System.Console.ReadLine();
            if (!equipo.Equals(""))
            {
                salaEspecializada.PoseeEquipamientoImagenologia = ((equipo == "S") ? true : false);
            }
            return salaEspecializada;
        }
       
    }
}
