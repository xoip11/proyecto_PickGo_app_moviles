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

        if (tienda == null)
        {
            return;
        }
        CarritoGlobal.carrito.Add(tienda);

        // GUARDAR EN BD
        ConexionBD db = new ConexionBD();
        var parametros = new Dictionary<string, object>
        {
            { "@tel", LoginGlobal.Telefono },
            { "@nom", tienda.Nombre }
        };

        await db.Ejecutar(
            "INSERT INTO Carrito (telefono, nombreTienda) VALUES (@tel, @nom)",
            parametros
        );

        await DisplayAlert("Carrito", $"{tienda.Nombre} agregado al carrito", "OK");

    }
    // Agregar a Favoritos
    private async void AgregarFavorito_Clicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        var tienda = boton?.BindingContext as Models.Tienda;

        if (tienda == null)
            return;

        FavoritosGlobal.Favoritos.Add(tienda);

        // GUARDAR EN BD
        ConexionBD db = new ConexionBD();
        var parametros = new Dictionary<string, object>
        {
            { "@tel", LoginGlobal.Telefono },
            { "@nom", tienda.Nombre }
        };

        await db.Ejecutar(
            "INSERT INTO Favoritos (telefono, nombreTienda) VALUES (@tel, @nom)",
            parametros
        );

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
            // Si no se inserta alguna imagem se queda en null
        }

       
        bool agregar = await DisplayAlert("Confirmar", "¿Deseas agregar esta tienda?", "Sí", "No");
        if (!agregar)
        {
            ImagenPreview.IsVisible = false;
            return;
        }

        string newFile = null;

        if (rutaImagen != null)
        {
            //  Copia la imagen al almacenamiento local
            var fileName = Path.GetFileName(rutaImagen);
            newFile = Path.Combine(FileSystem.AppDataDirectory, fileName);
            using var stream = File.OpenRead(rutaImagen);
            using var newStream = File.OpenWrite(newFile);
            await stream.CopyToAsync(newStream);
        }

        //  Agrega la tienda a la colección
        Tiendas.Add(new Models.Tienda
        {
            Nombre = nombre,
            Imagen = newFile
        });

        // GUARDA EN Base De Datos
        ConexionBD db = new ConexionBD();
        var parametros = new Dictionary<string, object>
        {
            { "@nom", nombre },
            { "@img", newFile }
        };

        await db.Ejecutar(
            "INSERT INTO Tiendas (nombre, imagen) VALUES (@nom, @img)",
            parametros
        );

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