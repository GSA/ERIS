using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Utilities
{
    class ValidationHelper
    {
        public enum Monster { Separation = 1, Monsterfile = 2 };

        public ValidationHelper()
        {
        }

        public string GetErrors(IList<ValidationFailure> failures, Monster hr)
        {
            StringBuilder errors = new StringBuilder();

            foreach (var rule in failures)
            {
                errors.Append(rule.ErrorMessage.Remove(0, rule.ErrorMessage.IndexOf('.') + (int)hr));
                errors.Append(",");
            }

            return errors.ToString();
        }
    }
}
