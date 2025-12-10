using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using PickGo.Models;

namespace PickGo.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private readonly string BaseUrl;

        public ApiService()
        {
            BaseUrl = "http://10.0.2.2:5008"; // usar el puerto HTTP de la api
            _client = new HttpClient { BaseAddress = new Uri(BaseUrl), Timeout = TimeSpan.FromSeconds(60) };
        }

        // LOGIN
        public async Task<LoginResponse?> Login(string telefono, string contrasena)
        {
            var payload = new { Telefono = telefono, Contrasena = contrasena };
            var res = await _client.PostAsJsonAsync("/login", payload);
            if (!res.IsSuccessStatusCode) return null;
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        //  REGISTRAR USUARIO
        public async Task<string> Register(string nombre, string telefono, string contrasena)
        {
            var payload = new { Nombre = nombre, Telefono = telefono, Contrasena = contrasena };
            var res = await _client.PostAsJsonAsync("/register", payload);

            var content = await res.Content.ReadAsStringAsync();
            if (res.IsSuccessStatusCode) return "OK";
            else return $"ERROR: {res.StatusCode} - {content}";
        }

        //  PERFIL DEL USUARIO
        public async Task<Usuario?> GetProfile(string telefono)
        {
            var res = await _client.GetAsync($"/profile/{telefono}");
            if (!res.IsSuccessStatusCode) return null;
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Usuario>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> UpdateProfile(ProfileUpdateRequest req)
        {
            var res = await _client.PutAsJsonAsync("/profile", req);
            return res.IsSuccessStatusCode;
        }

        //TIENDAS EXISTENTES
        public async Task<List<Tienda>?> GetTiendas()
        {
            var res = await _client.GetAsync("/tiendas");
            if (!res.IsSuccessStatusCode) return null;
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Tienda>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<(bool ok, string? imageUrl)> UploadTiendaWithImage(string nombre, string? localFilePath)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(nombre), "nombre");

            if (!string.IsNullOrEmpty(localFilePath) && File.Exists(localFilePath))
            {
                var stream = File.OpenRead(localFilePath);
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(fileContent, "imagen", Path.GetFileName(localFilePath));
            }

            var res = await _client.PostAsync("/tiendas/upload", content);
            if (!res.IsSuccessStatusCode) return (false, null);

            var json = await res.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("imagenUrl", out var p) && p.ValueKind == JsonValueKind.String)
                return (true, p.GetString());

            return (true, null);
        }

        public async Task<bool> AddTienda(string nombre, string? imagenFileName)
        {
            var payload = new { Nombre = nombre, Imagen = imagenFileName };
            var res = await _client.PostAsJsonAsync("/tiendas", payload);
            return res.IsSuccessStatusCode;
        }

        // FAVORITOS
        public async Task<bool> AddFavorito(string telefono, string nombreTienda)
        {
            var payload = new { Telefono = telefono, NombreTienda = nombreTienda };
            var res = await _client.PostAsJsonAsync("/favoritos", payload);
            return res.IsSuccessStatusCode;
        }

        public async Task<List<string>?> GetFavoritos(string telefono)
        {
            var res = await _client.GetAsync($"/favoritos/{telefono}");
            if (!res.IsSuccessStatusCode) return null;
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> DeleteFavorito(string telefono, string nombreTienda)
        {
            var payload = new { Telefono = telefono, NombreTienda = nombreTienda };
            var req = new HttpRequestMessage(HttpMethod.Delete, "/favoritos")
            {
                Content = JsonContent.Create(payload)
            };
            var res = await _client.SendAsync(req);
            return res.IsSuccessStatusCode;
        }

        // CARRITO
        public async Task<bool> AddCarrito(string telefono, string nombreTienda)
        {
            var payload = new { Telefono = telefono, NombreTienda = nombreTienda };
            var res = await _client.PostAsJsonAsync("/carrito", payload);
            return res.IsSuccessStatusCode;
        }

        public async Task<List<string>?> GetCarrito(string telefono)
        {
            var res = await _client.GetAsync($"/carrito/{telefono}");
            if (!res.IsSuccessStatusCode) return null;
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> DeleteCarrito(string telefono, string nombreTienda)
        {
            var payload = new { Telefono = telefono, NombreTienda = nombreTienda };
            var req = new HttpRequestMessage(HttpMethod.Delete, "/carrito")
            {
                Content = JsonContent.Create(payload)
            };
            var res = await _client.SendAsync(req);
            return res.IsSuccessStatusCode;
        }
    }

    // MODELOS AUXILIARES
    public class LoginResponse
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
    }

    public class Usuario
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
    }

    public class Tienda
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Imagen { get; set; }
    }

    public class ProfileUpdateRequest
    {
        public string Nombre { get; set; }
        public string TelefonoNuevo { get; set; }
        public string Contrasena { get; set; }
        public string TelefonoViejo { get; set; }
    }
}
