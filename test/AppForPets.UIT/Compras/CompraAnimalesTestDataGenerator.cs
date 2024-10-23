using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AppForMovies.UIT.Purchases
{
    public class CompraAnimalesTestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
    {

        new object[] { "", "2", "2", "luisa@gmail.com", "664", "313951", "The Telefono field is required" },
        new object[] { "Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "", "664", "313951", "The Email field is required" },
        new object[] { "Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "luisa@gmail.com", "", "313951", "The Prefijo field is required" },
        new object[] { "Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "luisa@gmail.com", "664", "", "The Telefono field is required" },
    };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }



}
