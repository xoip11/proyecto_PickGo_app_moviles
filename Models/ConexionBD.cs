using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickGo.Models
{
    internal class ConexionBD
    {
        private string connectionString = "workstation id=tiendas_boutique.mssql.somee.com;packet size=4096;user id=xoip06_SQLLogin_1;pwd=4i6dzag1re;data source=tiendas_boutique.mssql.somee.com;persist security info=False;initial catalog=tiendas_boutique;TrustServerCertificate=True";

        public async Task<DataTable> Consultar(string query, Dictionary<string, object>? parametros = null)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parametros != null)
                    {
                        foreach (var p in parametros)
                            cmd.Parameters.AddWithValue(p.Key, p.Value);
                    }

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        tabla.Load(reader);
                    }
                }
            }

            return tabla;
        }

        public async Task Ejecutar(string query, Dictionary<string, object>? parametros = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parametros != null)
                    {
                        foreach (var p in parametros)
                            cmd.Parameters.AddWithValue(p.Key, p.Value);
                    }

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<DataRow?> ObtenerUsuarioPorTelefono(string tel)
        {

            var parametros = new Dictionary<string, object>
            {
                { "@tel", tel }
            };

            DataTable tabla = await this.Consultar(
                "SELECT * FROM Usuarios WHERE telefono=@tel",
                parametros
            );

            if (tabla.Rows.Count == 0)
                return null;

            return tabla.Rows[0];
        }


    }
}
