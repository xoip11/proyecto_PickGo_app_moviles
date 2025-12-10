using PickGo.Models;
using PickGo.Services;

namespace PickGo.Views;

public partial class PerfilPage : ContentPage
{
    ConexionBD conexion = new ConexionBD();
    public PerfilPage()
	{
	  InitializeComponent();

        // Carga los datos del usuario
        txtNombre.Text = LoginGlobal.Nombre;
        txtTelefono.Text = LoginGlobal.Telefono;
        txtPassword.Text = LoginGlobal.Password;
    }
    private async void Guardar_Cliked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNombre.Text) ||
            string.IsNullOrEmpty(txtTelefono.Text) ||
            string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlert("Error", "Llenar todos los campos", "OK");
            return;
        }

        var api = new ApiService();
        var req = new ProfileUpdateRequest
        {
            Nombre = txtNombre.Text,
            TelefonoNuevo = txtTelefono.Text,
            Contrasena = txtPassword.Text,
            TelefonoViejo = LoginGlobal.Telefono
        };

        bool ok = await api.UpdateProfile(req);
        if (!ok)
        {
            await DisplayAlert("Error", "No se pudo actualizar en el servidor", "OK");
            return;
        }

        LoginGlobal.Nombre = txtNombre.Text;
        LoginGlobal.Telefono = txtTelefono.Text;
        LoginGlobal.Password = txtPassword.Text;

        await DisplayAlert("Perfil", "Cambios guardados correctamente", "OK");
    }

}