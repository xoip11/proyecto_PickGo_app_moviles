using PickGo.Views;

namespace PickGo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.InicioPage());
        }
        public static class ApiConfig
        {
            public const string BaseUrl = "http://192.168.100.44"; // sin puerto o con host
        }

    }
}
