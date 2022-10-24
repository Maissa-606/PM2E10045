using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using PM2E12513.Models;

namespace PM2E12513.Controller
{
    public class BaseSQLite
    {
        readonly SQLiteAsyncConnection db;

        //constructor de la clase DataBaseSQLite
        public BaseSQLite(String pathdb)
        {
            //crear una conexion a la base de datos
            db = new SQLiteAsyncConnection(pathdb);

            //creacion de la tabla personas dentro de SQLite
            db.CreateTableAsync<Ubicaciones>().Wait();
        }

        //opaciones CRUD con SQLite
        //READ List Way
        public Task<List<Ubicaciones>> ObtenerListaUbicaciones()
        {
            return db.Table<Ubicaciones>().ToListAsync();

        }

        //retornar una persona //READ one by one
        //(retorna el primero que encuentre ya que pueden a ver varios del dp choluteca)
        public Task<Ubicaciones> ObtenerUbicacion(int pcodigo)
        {
            return db.Table<Ubicaciones>()
                .Where(i => i.codigo == pcodigo)
                .FirstOrDefaultAsync();
        }

        public Task<int> GrabarUbicacion(Ubicaciones ubicacion)
        {
            if (ubicacion.codigo != 0)
            {
                return db.UpdateAsync(ubicacion);
            }
            else
            {

                return db.InsertAsync(ubicacion);
            }
        }

        //delete
        public Task<int> EliminarUbicacion(Ubicaciones ubicacion)
        {
            return db.DeleteAsync(ubicacion);
        }
    }
}
