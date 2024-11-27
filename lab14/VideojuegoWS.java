package pe.edu.pucp.gamesoft.services;

import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import jakarta.jws.WebService;
import java.net.MalformedURLException;
import pe.edu.pucp.gamesoft.dao.VideojuegoDAO;
import pe.edu.pucp.gamesoft.model.Videojuego;
import java.rmi.Naming;
import java.rmi.NotBoundException;
import java.rmi.RemoteException;
import java.util.ArrayList;
/* Colocar sus datos personales
* ------------------------------------------------
* Nombre Completo: Rodrigo Alejandro Holguin Huari
* Codigo PUCP: 20221466
* ------------------------------------------------
*/

@WebService(serviceName = "VideojuegoWS", targetNamespace = "http://services.gamesoft.pucp.edu.pe/")
public class VideojuegoWS {
    
    private VideojuegoDAO daoVideojuego;
    
    @WebMethod(operationName = "insertarVideojuego")
    public int insertarVideojuego(@WebParam(name = "videojuego") Videojuego videojuego) {
        int resultado = 0;
        try {
            daoVideojuego = (VideojuegoDAO)
                    Naming.lookup("//127.0.0.1:1234/" + "daoVideojuego");
            resultado = daoVideojuego.insertar(videojuego);
        } catch (MalformedURLException | NotBoundException | RemoteException ex) {
            System.out.println(ex.getMessage());
        }
        return resultado;
    }
    
    @WebMethod(operationName = "listarVidPorNombre")
    public ArrayList<Videojuego> listarVidPorNombre(@WebParam(name = "nombre") String nombre) {
        ArrayList<Videojuego> generos = null;
        try {
            daoVideojuego = (VideojuegoDAO)
                    Naming.lookup("//127.0.0.1:1234/" + "daoVideojuego");
            generos = daoVideojuego.listarPorNombre(nombre);
        } catch (MalformedURLException | NotBoundException | RemoteException ex) {
            System.out.println(ex.getMessage());
        }
        return generos;
    }
    
    @WebMethod(operationName = "obtenerVidPorId")
    public Videojuego obtenerVidPorId(@WebParam(name = "idVideojuego") int idVideojuego) {
        Videojuego videojuego = null;
        try {
            daoVideojuego = (VideojuegoDAO)
                    Naming.lookup("//127.0.0.1:1234/" + "daoVideojuego");
            videojuego = daoVideojuego.obtenerPorId(idVideojuego);
        } catch (MalformedURLException | NotBoundException | RemoteException ex) {
            System.out.println(ex.getMessage());
        }
        return videojuego;
    }
}