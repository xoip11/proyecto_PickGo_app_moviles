namespace PickGo.Views;

public partial class InicioPage : ContentPage
{
	public InicioPage()
	{
		InitializeComponent();
	}



	private async void Continuar_Clicked(object sender,EventArgs e)
	{
		if (string.IsNullOrEmpty(txtTelefono.Text) || string.IsNullOrEmpty(txtPassword.Text))
		{
            await DisplayAlert("Error", "Llenar todos los campos", "OK");
            return;


        }
        await Navigation.PushAsync(new CatalogoPage());



	}

	private async void Registrarse_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new RegistroPage());
	}

		
}