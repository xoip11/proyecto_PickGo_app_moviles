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
}