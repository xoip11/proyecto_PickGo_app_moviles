using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickGo.Models
{
    internal class CarritoGlobal
    {
        public static List<Models.Tienda> carrito { get; set; } = new List<Tienda>();
    }
}
