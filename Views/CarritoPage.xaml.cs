using System.Collections.ObjectModel;
using PickGo.Models;
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
        var tienda = boton?.BindingContext as Tienda;

        if (tienda == null)
            return;

        var parametros = new Dictionary<string, object>
    {
        { "@tel", LoginGlobal.Telefono },
        { "@nom", tienda.Nombre }
    };

        ConexionBD conexion = new ConexionBD();

        await conexion.Ejecutar(
            "DELETE FROM Carrito WHERE telefono=@tel AND nombreTienda=@nom",
            parametros
        );

        CarritoGlobal.carrito.Remove(tienda);

        ListaCarrito.ItemsSource = null;
        ListaCarrito.ItemsSource = CarritoGlobal.carrito;

        await DisplayAlert("Eliminado", $"{tienda.Nombre} fue eliminado del carrito", "OK");
    }
}