using PickGo.Models;

namespace PickGo.Views;

public partial class InicioPage : ContentPage
{
    ConexionBD conexion = new ConexionBD();
    public InicioPage()
	{
		InitializeComponent();
	}
    private async void Continuar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtTelefono.Text) || string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlert("Error", "Llenar todos los campos", "OK");
            return;
        }

        // Buscar usuario en la BD
        var user = await conexion.ObtenerUsuarioPorTelefono(txtTelefono.Text);

        if (user == null)
        {
            await DisplayAlert("Error", "Teléfono incorrecto", "OK");
            return;
        }

        // Validar contraseña
        if (user["contrasena"].ToString() != txtPassword.Text)
        {
            await DisplayAlert("Error", "Contraseña incorrecta", "OK");
            return;
        }

        // Guardar datos globales
        LoginGlobal.Telefono = user["telefono"].ToString();
        LoginGlobal.Nombre = user["nombre"].ToString();
        LoginGlobal.Password = user["contrasena"].ToString();

        // Ir al catálogo
        await Navigation.PushAsync(new CatalogoPage());
    }

    private async void Registrarse_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistroPage());
    }



}


