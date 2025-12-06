using PickGo.Models;
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
            new Models.Tienda { Nombre = "ElectroShop", Imagen = "boutique.png" },
            new Models.Tienda { Nombre = "Zapatería Luna", Imagen = "" },
            new Models.Tienda { Nombre = "Farmacia Centro", Imagen = "" }
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
    private async void AgregarTienda_Clicked(object sender, EventArgs e)
    {
        string nombre = await DisplayPromptAsync("Nueva tienda", "Nombre de la tienda:");
        if (string.IsNullOrWhiteSpace(nombre))
            return;

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
                // Guardar ruta temporal
                rutaImagen = file.FullPath;

                // Mostrar previsualización
                ImagenPreview.Source = ImageSource.FromFile(rutaImagen);
                ImagenPreview.IsVisible = true;
            }
        }
        catch
        {
            // Si falla el picker, simplemente ignoramos y dejamos rutaImagen = null
        }

        // 2. Preguntar al usuario si quiere agregar la tienda
        bool agregar = await DisplayAlert("Confirmar", "¿Deseas agregar esta tienda?", "Sí", "No");
        if (!agregar)
        {
            ImagenPreview.IsVisible = false;
            return;
        }

        string newFile = null;

        if (rutaImagen != null)
        {
            // 3. Copiar imagen al almacenamiento local
            var fileName = Path.GetFileName(rutaImagen);
            newFile = Path.Combine(FileSystem.AppDataDirectory, fileName);
            using var stream = File.OpenRead(rutaImagen);
            using var newStream = File.OpenWrite(newFile);
            await stream.CopyToAsync(newStream);
        }

        // 4. Agregar la tienda a la colección
        Tiendas.Add(new Models.Tienda
        {
            Nombre = nombre,
            Imagen = newFile // puede ser null si no eligió imagen
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
            await DisplayAlert("Eliminado", $"{tienda.Nombre} fue eliminada", "OK");
        }
    }
}