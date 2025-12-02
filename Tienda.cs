using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickGo
{
    public class Tienda
    {
        public string Nombre { get; set; }
        public double Precio { get; set; }

        public string Display => $"{Nombre}: ${Precio}";
    }

}
