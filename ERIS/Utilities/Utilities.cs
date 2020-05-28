using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Utilities
{
    sealed class Utilities
    {
        /// <summary>
        /// Checks if birth date given is valid
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Bool</returns>
        public static bool BeAValidBirthDate(string date)
        {
            DateTime _birthDate;

            if (DateTime.TryParse(date, out _birthDate))
            {
                return ((_birthDate <= DateTime.Now.AddYears(-14))&& (_birthDate > DateTime.Now.AddYears(-100)));
            }
            else return false;
        }
    }
}
