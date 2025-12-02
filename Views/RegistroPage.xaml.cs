namespace PickGo.Views;

public partial class RegistroPage : ContentPage
{
	public RegistroPage()
	{
		InitializeComponent();
	}
	private async void Registrar_Cliked(object sender, EventArgs e)
	{
        if (string.IsNullOrEmpty(txtTelefono.Text) || string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlert("Error", "Llenar todos los campos", "OK");
            return;


        }
        await DisplayAlert("Registro", "Usuario registrado", "OK");
		await Navigation.PopAsync();
    }
     
    
    
}

