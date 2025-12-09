using PickGo.Models;
namespace PickGo.Views;
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
        var tienda = boton?.BindingContext as Tienda;

        if (tienda == null)
            return;

        // 1. Eliminar de la base de datos
        ConexionBD conexion = new ConexionBD();
        var parametros = new Dictionary<string, object>
        {
            { "@tel", LoginGlobal.Telefono },
            { "@nom", tienda.Nombre }
        };

        await conexion.Ejecutar(
            "DELETE FROM Favoritos WHERE telefono=@tel AND nombreTienda=@nom",
            parametros
        );

        // 2. Eliminar de la lista local
        FavoritosGlobal.Favoritos.Remove(tienda);

        // 3. Actualizar lista en pantalla
        ListaFavoritos.ItemsSource = null;
        ListaFavoritos.ItemsSource = FavoritosGlobal.Favoritos;

        // 4. Aviso
        await DisplayAlert("Eliminado", $"{tienda.Nombre} fue quitado de favoritos", "OK");
    }
}