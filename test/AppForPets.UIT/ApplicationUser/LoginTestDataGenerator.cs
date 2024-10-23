using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AppForMovies.UIT.ApplicationUser
{
    public class LoginTestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
    {
        new object[] {"fulanito@uclm.com","password1"},
        new object[] {"pepito@uclm.com","password2"},
        new object[] {"menganito@uclm.com","password2"},
    };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
