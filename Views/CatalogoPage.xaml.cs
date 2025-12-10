using PickGo.Models;
using PickGo.Services;
using System.Collections.ObjectModel;
namespace PickGo.Views;

public partial class CatalogoPage : ContentPage
{
    public ObservableCollection<Models.Tienda> Tiendas { get; set; }

    public CatalogoPage()
    {
        InitializeComponent();
        Tiendas = new ObservableCollection<Models.Tienda>
        {
            new Models.Tienda { Nombre = "Boutique AA", Imagen = "boutique.png" },
            new Models.Tienda { Nombre = "ElectroShop", Imagen = "electronica.png" },
            new Models.Tienda { Nombre = "cloth store", Imagen = "cloth.png" },
            new Models.Tienda { Nombre = "grocery store", Imagen = "super.png" }
        };

        BindingContext = this;
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
        if (tienda == null) return;

        // Agrego localmente
        CarritoGlobal.carrito.Add(tienda);

        // Guardar en BD via API
        var api = new ApiService();
        bool ok = await api.AddCarrito(LoginGlobal.Telefono, tienda.Nombre);

        if (!ok)
        {
            await DisplayAlert("Error", "No se pudo guardar el carrito en el servidor", "OK");
            return;
        }

        await DisplayAlert("Carrito", $"{tienda.Nombre} agregado al carrito", "OK");
    }

    // Agregar a Favoritos
    private async void AgregarFavorito_Clicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        var tienda = boton?.BindingContext as Models.Tienda;
        if (tienda == null) return;

        FavoritosGlobal.Favoritos.Add(tienda);

        var api = new ApiService();
        bool ok = await api.AddFavorito(LoginGlobal.Telefono, tienda.Nombre);

        if (!ok)
        {
            await DisplayAlert("Error", "No se pudo guardar en favoritos en el servidor", "OK");
            return;
        }

        await DisplayAlert("Favorito", $"{tienda.Nombre} agregado a favoritos", "OK");
    }
    private async void IrFavoritos_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavoritosPage());
    }
    private async void AgregarTienda_Clicked(object sender, EventArgs e)
    {
        string nombre = await DisplayPromptAsync("Nueva tienda", "Nombre de la tienda:");
        if (string.IsNullOrWhiteSpace(nombre)) return;

        string rutaImagen = null;
        try
        {
            var file = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una imagen (opcional)",
                FileTypes = FilePickerFileType.Images
            });

            if (file != null)
            {
                rutaImagen = file.FullPath;
                ImagenPreview.Source = ImageSource.FromFile(rutaImagen);
                ImagenPreview.IsVisible = true;
            }
        }
        catch
        {
            // ignore
        }

        bool agregar = await DisplayAlert("Confirmar", "¿Deseas agregar esta tienda?", "Sí", "No");
        if (!agregar)
        {
            ImagenPreview.IsVisible = false;
            return;
        }

        var api = new ApiService();
        var (ok, imageUrl) = await api.UploadTiendaWithImage(nombre, rutaImagen);

        if (!ok)
        {
            await DisplayAlert("Error", "No se pudo crear la tienda en el servidor", "OK");
            ImagenPreview.IsVisible = false;
            return;
        }

        // Agrega localmente con la URL 
        Tiendas.Add(new Models.Tienda
        {
            Nombre = nombre,
            Imagen = imageUrl
        });

        ImagenPreview.IsVisible = false;
    }
    private async void EliminarTienda_Clicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        var tienda = boton?.BindingContext as Models.Tienda;

        if (tienda == null)
            return;

        bool confirmar = await DisplayAlert(
            "Eliminar Tienda",
            $"¿Deseas eliminar {tienda.Nombre}?",
            "Sí",
            "Cancelar"
        );

        if (confirmar)
        {
            Tiendas.Remove(tienda);

            // ELIMINAR DE BD
            ConexionBD db = new ConexionBD();
            var parametros = new Dictionary<string, object>
            {
                { "@nom", tienda.Nombre }
            };

            await db.Ejecutar(
                "DELETE FROM Tiendas WHERE nombre=@nom",
                parametros
            );

            await DisplayAlert("Eliminado", $"{tienda.Nombre} fue eliminada", "OK");
        }
    }
}