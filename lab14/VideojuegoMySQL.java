package pe.edu.pucp.gamesoft.mysql;

import pe.edu.pucp.gamesoft.dao.VideojuegoDAO;
import java.rmi.server.UnicastRemoteObject;
import java.rmi.RemoteException;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.sql.Types;
import pe.edu.pucp.gamesoft.config.DBManager;
import pe.edu.pucp.gamesoft.model.Clasificacion;
import pe.edu.pucp.gamesoft.model.Genero;
import pe.edu.pucp.gamesoft.model.Videojuego;

public class VideojuegoMySQL extends UnicastRemoteObject implements VideojuegoDAO{

    private ResultSet rs;
    
    public VideojuegoMySQL(int puerto) throws RemoteException{
        super(puerto);
    }
    
    @Override
    public int insertar(Videojuego videojuego) throws RemoteException {
        Map<String, Object> parametrosEntrada = new HashMap<>();
        parametrosEntrada.put("_fid_genero", videojuego.getGenero().getIdGenero());
        parametrosEntrada.put("_nombre", videojuego.getNombre());
        parametrosEntrada.put("_fecha_lanzamiento", videojuego.getFechaLanzamiento());
        parametrosEntrada.put("_costo_desarrollo", videojuego.getCostoDesarrollo());
        parametrosEntrada.put("_foto", videojuego.getFoto());
        parametrosEntrada.put("_clasificacion", videojuego.getClasificacion().toString());
        
        Map<String, Object> parametrosSalida = new HashMap<>();
        parametrosSalida.put("_id_videojuego", Types.INTEGER);
        
        DBManager.getInstance().ejecutarProcedimiento("INSERTAR_VIDEOJUEGO", parametrosEntrada, parametrosSalida);
        videojuego.setIdVideojuego((int) parametrosSalida.get("_id_videojuego"));
        return videojuego.getIdVideojuego();
    }

    @Override
    public ArrayList<Videojuego> listarPorNombre(String nombre) throws RemoteException {
        ArrayList<Videojuego> videojuegos = new ArrayList<>();
        HashMap<String, Object> parametrosEntrada = new HashMap<>();
        parametrosEntrada.put("_nombre", nombre);
        rs = DBManager.getInstance().ejecutarProcedimientoLectura("LISTAR_VIDEOJUEGOS_POR_NOMBRE", parametrosEntrada);
        try{
            while(rs.next()){
                Videojuego videojuego = new Videojuego();
                videojuego.setIdVideojuego(rs.getInt("id_videojuego"));
                videojuego.setNombre(rs.getString("nombre_videojuego"));
                videojuego.setGenero(new Genero());
                videojuego.getGenero().setIdGenero(rs.getInt("id_genero"));
                videojuego.getGenero().setNombre(rs.getString("nombre_genero"));
                videojuego.setClasificacion(Clasificacion.valueOf(rs.getString("clasificacion")));
                videojuegos.add(videojuego);
            }
        }catch(SQLException ex){
            System.out.println(ex.getMessage());
        }finally{
            DBManager.getInstance().cerrarConexion();
        }
        return videojuegos;
    }

    @Override
    public Videojuego obtenerPorId(int idVideojuego) throws RemoteException {
        Videojuego videojuego = new Videojuego();
        HashMap<String, Object> parametrosEntrada = new HashMap<>();
        parametrosEntrada.put("_id_videojuego", idVideojuego);
        rs = DBManager.getInstance().ejecutarProcedimientoLectura("OBTENER_VIDEOJUEGO_POR_ID", parametrosEntrada);
        try{
            if(rs.next()){
                videojuego.setIdVideojuego(rs.getInt("id_videojuego"));
                videojuego.setNombre(rs.getString("nombre_videojuego"));
                videojuego.setGenero(new Genero());
                videojuego.getGenero().setIdGenero(rs.getInt("id_genero"));
                videojuego.getGenero().setNombre(rs.getString("nombre_genero"));
                videojuego.setClasificacion(Clasificacion.valueOf(rs.getString("clasificacion")));
                videojuego.setCostoDesarrollo(rs.getDouble("costo_desarrollo"));
                videojuego.setFechaLanzamiento(rs.getDate("fecha_lanzamiento"));
                videojuego.setFoto(rs.getBytes("foto"));
            } else videojuego = null;
        }catch(SQLException ex){
            videojuego = null;
            System.out.println(ex.getMessage());
        }finally{
            DBManager.getInstance().cerrarConexion();
        }
        return videojuego;
    }
    
}
