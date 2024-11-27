package pe.edu.pucp.gamesoft.mysql;

import pe.edu.pucp.gamesoft.dao.GeneroDAO;
import java.rmi.server.UnicastRemoteObject;
import java.rmi.RemoteException;
import java.util.ArrayList;
import pe.edu.pucp.gamesoft.model.Genero;
import java.sql.SQLException;
import java.sql.ResultSet;
import java.util.HashMap;
import pe.edu.pucp.gamesoft.config.DBManager;

public class GeneroMySQL extends UnicastRemoteObject implements GeneroDAO{
    
    private ResultSet rs;
    
    public GeneroMySQL(int puerto) throws RemoteException{
        super(puerto);
    }
    
    @Override
    public ArrayList<Genero> listarTodos() throws RemoteException{
        ArrayList<Genero> generos = new ArrayList<>();
        HashMap<String, Object> parametrosEntrada = new HashMap<>();
        rs = DBManager.getInstance().ejecutarProcedimientoLectura("LISTAR_GENEROS_TODOS", parametrosEntrada);
        try{
            while(rs.next()){
                Genero genero = new Genero();
                genero.setIdGenero(rs.getInt("id_genero"));
                genero.setNombre(rs.getString("nombre"));
                genero.setActivo(true);
                generos.add(genero);
            }
        }catch(SQLException ex){
            System.out.println(ex.getMessage());
        }finally{
            DBManager.getInstance().cerrarConexion();
        }
        return generos;
    }
    
}
