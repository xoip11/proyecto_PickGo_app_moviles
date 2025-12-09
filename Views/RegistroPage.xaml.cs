using PickGo.Models;
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

        //Verificar si el teléfono ya esta registrado en una cuenta
        var parametrosBusqueda = new Dictionary<string, object>
        {
            { "@tel", txtTelefono.Text }
        };

        DataTable existe = await conexion.Consultar(
            "SELECT * FROM Usuarios WHERE telefono=@tel",
            parametrosBusqueda
        );

        if (existe.Rows.Count > 0)
        {
            await DisplayAlert("Error", "Ese número ya está registrado", "OK");
            return;
        }

        // Registra usuario nuevo 
        var parametrosInsert = new Dictionary<string, object>
        {
            { "@nom", txtNombre.Text },
            { "@tel", txtTelefono.Text },
            { "@pass", txtPassword.Text }
        };

        await conexion.Ejecutar(
            "INSERT INTO Usuarios (nombre, telefono, contrasena) VALUES (@nom, @tel, @pass)",
            parametrosInsert
        );

        // Confirmación el registro exitoso
        await DisplayAlert("Registro", "Usuario registrado correctamente", "OK");

        //  Regresar al login
        await Navigation.PopAsync();
    }
}