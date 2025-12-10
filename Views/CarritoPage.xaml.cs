using System.Collections.ObjectModel;
using PickGo.Models;
using PickGo.Services;

namespace PickGo.Views;

public partial class CarritoPage : ContentPage
{
    public CarritoPage()
    {
        InitializeComponent();
        ListaCarrito.ItemsSource = CarritoGlobal.carrito;
    }

    private async void RemoveProduct_Clicked(object sender, EventArgs e)
    {
        var boton = sender as Button;

        // Usa la clase del modelo 
        var tienda = boton?.BindingContext as PickGo.Models.Tienda;

        if (tienda == null)
            return;

        var api = new ApiService();

        bool ok = await api.DeleteCarrito(LoginGlobal.Telefono, tienda.Nombre);

        if (!ok)
        {
            await DisplayAlert("Error", "No se pudo eliminar del carrito", "OK");
            return;
        }

        // Quitar del carrito global
        CarritoGlobal.carrito.Remove(tienda);

        // Refrescar vista
        ListaCarrito.ItemsSource = null;
        ListaCarrito.ItemsSource = CarritoGlobal.carrito;

        await DisplayAlert("Eliminado", $"{tienda.Nombre} fue eliminado del carrito", "OK");
    }
}
