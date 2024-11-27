package pe.edu.pucp.gamesoft.services;

import jakarta.jws.WebMethod;
//import jakarta.jws.WebParam;
import jakarta.jws.WebService;
import java.net.MalformedURLException;
import java.util.ArrayList;
import pe.edu.pucp.gamesoft.dao.GeneroDAO;
import pe.edu.pucp.gamesoft.model.Genero;
import java.rmi.Naming;
import java.rmi.NotBoundException;
import java.rmi.RemoteException;

/* Colocar sus datos personales
* ------------------------------------------------
* Nombre Completo: Rodrigo Alejandro Holguin Huari
* Codigo PUCP: 20221466
* ------------------------------------------------
*/


@WebService(serviceName = "GeneroWS", targetNamespace = "http://services.gamesoft.pucp.edu.pe/")
public class GeneroWS {
    
    private GeneroDAO daoGenero;
    
    @WebMethod(operationName = "listarGeneros")
    public ArrayList<Genero> listarGeneros() {
        ArrayList<Genero> generos = null;
        try {
            daoGenero = (GeneroDAO)
                    Naming.lookup("//127.0.0.1:1234/" + "daoGenero");
            generos = daoGenero.listarTodos();
        } catch (MalformedURLException | NotBoundException | RemoteException ex) {
            System.out.println(ex.getMessage());
        }
        return generos;
    }
}