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
    private void RemoveProduct_Clicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        var tienda = boton?.BindingContext as Tienda;

        if (tienda == null)
            return;

        CarritoGlobal.carrito.Remove(tienda);

        ListaCarrito.ItemsSource = null;
        ListaCarrito.ItemsSource = CarritoGlobal.carrito;

        DisplayAlert("Eliminado", $"{tienda.Nombre} fue eliminado de el carrito", "OK");
    }
        

    }