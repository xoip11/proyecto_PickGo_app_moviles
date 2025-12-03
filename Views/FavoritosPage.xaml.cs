using PickGo.Models;
namespace PickGo.Views;

public partial class FavoritosPage : ContentPage
{
	public FavoritosPage()
	{
		InitializeComponent();
        ListaFavoritos.ItemsSource = FavoritosGlobal.Favoritos;
    }
	private void EliminarFavorito_Clicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        var tienda = boton?.BindingContext as Tienda;

        if (tienda == null)
            return;

        FavoritosGlobal.Favoritos.Remove(tienda);

        ListaFavoritos.ItemsSource = null;
        ListaFavoritos.ItemsSource = FavoritosGlobal.Favoritos;

        DisplayAlert("Eliminado", $"{tienda.Nombre} fue quitado de favoritos", "OK");
    }
}