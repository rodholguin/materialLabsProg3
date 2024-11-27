package pe.edu.pucp.gamesoft.server;

import java.net.MalformedURLException;
import java.rmi.RemoteException;
import java.rmi.registry.LocateRegistry;
import pe.edu.pucp.gamesoft.dao.GeneroDAO;
import pe.edu.pucp.gamesoft.dao.VideojuegoDAO;
import java.rmi.Naming;
import pe.edu.pucp.gamesoft.mysql.GeneroMySQL;
import pe.edu.pucp.gamesoft.mysql.VideojuegoMySQL;

public class Principal {
    /* Colocar sus datos personales
    * ------------------------------------------------
    * Nombre Completo: Rodrigo Alejandro Holguin Huari
    * Codigo PUCP: 20221466
    * ------------------------------------------------
    */
    private static String IPServidor = "127.0.0.1";
    private static String puerto = "1234";
    
    public static void main(String[] args){
        try{
            //Registramos el servicio de RMI
            LocateRegistry.createRegistry(Integer.parseInt(puerto));
            
            //Inicializamos los objetos remotos
            GeneroDAO daoGenero = new GeneroMySQL(Integer.parseInt(puerto));
            VideojuegoDAO daoVideojuego = new VideojuegoMySQL(Integer.parseInt(puerto));
            
            //Colocamos los objetos en el servicio RMI
            Naming.rebind("//"+IPServidor+":"+String.valueOf(puerto)+"/daoGenero", daoGenero);
            Naming.rebind("//"+IPServidor+":"+String.valueOf(puerto)+"/daoVideojuego", daoVideojuego);
            
            //Imprimimos mensaje de confirmación
            System.out.println("El servidor RMI se ha inicializado correctamente..");
        }catch(MalformedURLException | RemoteException ex){
            System.out.println("Error inicializando el RMI: " + ex.getMessage());
        }
    }
}
