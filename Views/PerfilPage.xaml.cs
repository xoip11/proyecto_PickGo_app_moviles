using PickGo.Models;

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

        // Actualizar en la BD
        var parametros = new Dictionary<string, object>
        { 
            { "@nom", txtNombre.Text },
            { "@tel", txtTelefono.Text },
            { "@pass", txtPassword.Text },
            { "@telViejo", LoginGlobal.Telefono }
        };

        await conexion.Ejecutar(
            "UPDATE Usuarios SET nombre=@nom, telefono=@tel, contrasena=@pass WHERE telefono=@telViejo",
            parametros
        );

        // Actualizar los globales
        LoginGlobal.Nombre = txtNombre.Text;
        LoginGlobal.Telefono = txtTelefono.Text;
        LoginGlobal.Password = txtPassword.Text;

        await DisplayAlert("Perfil", "Cambios guardados correctamente", "OK");
    }
}