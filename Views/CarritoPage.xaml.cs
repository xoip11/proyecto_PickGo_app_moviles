using System.Collections.ObjectModel;

namespace PickGo.Views;

public partial class CarritoPage : ContentPage
{
     public ObservableCollection<Tienda> Carrito { get; set; }
        public CarritoPage()
        {
            InitializeComponent();

            Carrito = new ObservableCollection<Tienda>();
            BindingContext = this;
        }

        private void AddProduct_Clicked(object sender, EventArgs e)
        {
            var random = new Random();

            Carrito.Add(new Tienda
            {
                Nombre = "Producto " + (Carrito.Count + 1),
                Precio = random.Next(10, 200)
            });
        }

        private void RemoveProduct_Clicked(object sender, EventArgs e)
        {
            var button = (Button) sender;
            var product = (Tienda) button.BindingContext;

            Carrito.Remove(product);
        }
        

    }