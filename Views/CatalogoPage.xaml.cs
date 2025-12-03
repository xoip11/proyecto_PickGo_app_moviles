using PickGo.Models;
namespace PickGo.Views;

public partial class CatalogoPage : ContentPage
{
    public List<Models.Tienda> Tiendas { get; set; }

    public CatalogoPage()
    {
        InitializeComponent();
        Tiendas = new List<Models.Tienda>
        {
            new Models.Tienda { Nombre = "Boutique AA" },
            new Models.Tienda { Nombre = "ElectroShop" },
            new Models.Tienda { Nombre = "Zapatería Luna" },
            new Models.Tienda { Nombre = "Farmacia Centro" }
        };

        ListaTiendas.ItemsSource = Tiendas;
    }

    
    private async void IrCarrito_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CarritoPage());
    }

    private async void IrPerfil_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PerfilPage());
    }
    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        var tienda = boton?.BindingContext as Models.Tienda;

        if (tienda == null)
            return;
        CarritoGlobal.carrito.Add(tienda);
        await DisplayAlert("Carrito", $"{tienda.Nombre} agregado al carrito", "OK");
    }
    private async void AgregarFavorito_Clicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        var tienda = boton?.BindingContext as Models.Tienda;

        if (tienda == null)
            return;

        FavoritosGlobal.Favoritos.Add(tienda);
        await DisplayAlert("Favorito", $"{tienda.Nombre} agregado a favoritos", "OK");
    }
    private async void IrFavoritos_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavoritosPage());
    }
}