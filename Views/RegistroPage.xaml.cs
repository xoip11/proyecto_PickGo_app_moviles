namespace PickGo.Views;

public partial class RegistroPage : ContentPage
{
	public RegistroPage()
	{
		InitializeComponent();
	}
	private async void Registrar_Cliked(object sender, EventArgs e)
	{
		await DisplayAlert("Registro", "Usuario registrado", "OK");
		await Navigation.PopAsync();
	}
}