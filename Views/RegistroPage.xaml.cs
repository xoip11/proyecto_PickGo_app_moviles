using PickGo.Models;
using PickGo.Services;
using System.Data;

namespace PickGo.Views;

public partial class RegistroPage : ContentPage
{
    ConexionBD conexion = new ConexionBD();
    public RegistroPage()
	{
		InitializeComponent();
	}
    private async void Registrar_Cliked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNombre.Text) ||
            string.IsNullOrEmpty(txtTelefono.Text) ||
            string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlert("Error", "Llenar todos los campos", "OK");
            return;
        }

        var api = new ApiService();

        // Verificar si ya existe (lo hacemos solicitando)
        var perfil = await api.GetProfile(txtTelefono.Text);
        if (perfil != null)
        {
            await DisplayAlert("Error", "Ese número ya está registrado", "OK");
            return;
        }

        string result = await api.Register(txtNombre.Text, txtTelefono.Text, txtPassword.Text);
        await DisplayAlert("Resultado", result, "OK");

        await DisplayAlert("Registro", "Usuario registrado correctamente", "OK");
        await Navigation.PopAsync();
    }

}