using PickGo.Models;
using PickGo.Services;
namespace PickGo.Views;
using PickGo.Services;
public partial class InicioPage : ContentPage
{
    ApiService api = new ApiService();

    public InicioPage()
	{
		InitializeComponent();
	}
    private async void Continuar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtTelefono.Text) || string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlert("Error", "Llene todos los campos", "OK");
            return;
        }
        var api = new ApiService();
        var result = await api.Login(txtTelefono.Text, txtPassword.Text);
        if (result == null)
        {
            await DisplayAlert("Error", "Teléfono o contraseña incorrectos", "OK");
            return;
        }

        // Guardar datos globales
        LoginGlobal.Telefono = result.Telefono;
        LoginGlobal.Nombre = result.Nombre;
        LoginGlobal.Password = txtPassword.Text;

        // Ir al catálogo
        await Navigation.PushAsync(new CatalogoPage());
    }


    private async void Registrarse_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistroPage());
    }



}


