namespace PickGo.Views;

public partial class PerfilPage : ContentPage
{
	public PerfilPage()
	{
	  InitializeComponent();
	}
	private async void Guardar_Cliked(object sender, EventArgs e)
	{
		 if (string.IsNullOrEmpty(txtTelefono.Text) || string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlert("Error", "Llenar todos los campos", "OK");
            return;


        }
		await DisplayAlert("Perfil", "Cambios guardados correctamente", "OK");

	}
}