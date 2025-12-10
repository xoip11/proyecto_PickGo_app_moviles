using PickGo.Models;
namespace PickGo.Views;

using PickGo.Services;
using System.Collections.Generic;

public partial class FavoritosPage : ContentPage
{
    public FavoritosPage()
    {
        InitializeComponent();
        ListaFavoritos.ItemsSource = FavoritosGlobal.Favoritos;
    }

    private async void EliminarFavorito_Clicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton == null) return;

        
        object ctx = boton.BindingContext;
        PickGo.Models.Tienda tiendaModelo = null;

        if (ctx is PickGo.Models.Tienda m)
        {
            tiendaModelo = m;
        }
        else if (ctx is PickGo.Services.Tienda s)
        {
            
            tiendaModelo = new PickGo.Models.Tienda
            {
                Nombre = s.Nombre,
                Imagen = s.Imagen
            };
        }
        else
        {
           
            return;
        }

        // Llamada a la API para eliminar
        var api = new ApiService();
        bool ok = await api.DeleteFavorito(LoginGlobal.Telefono, tiendaModelo.Nombre);
        if (!ok)
        {
            await DisplayAlert("Error", "No se pudo eliminar favorito en el servidor", "OK");
            return;
        }

        // Eliminar del listado global
        var itemToRemove = FavoritosGlobal.Favoritos.FirstOrDefault(x => x.Nombre == tiendaModelo.Nombre);
        if (itemToRemove != null)
        {
            FavoritosGlobal.Favoritos.Remove(itemToRemove);
        }

        // Refrescar vista
        ListaFavoritos.ItemsSource = null;
        ListaFavoritos.ItemsSource = FavoritosGlobal.Favoritos;

        await DisplayAlert("Eliminado", $"{tiendaModelo.Nombre} fue quitado de favoritos", "OK");
    }
}
