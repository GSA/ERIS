using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ERIS.Lookups;
using ERIS.Models;
using ERIS.Utilities;

namespace ERIS.Validation
{
    internal class ValidateMonster
    {
        private readonly Dictionary<string, string[]> lookups = new Dictionary<string, string[]>();

        public ValidateMonster(Lookup lookup)
        {
            lookups.Add("StateCodes", lookup.stateLookup.Select(s => s.Code).ToArray());
            lookups.Add("CountryCodes", lookup.countryLookup.Select(c => c.Code).ToArray());
            lookups.Add("RegionCodes", lookup.regionLookup.Select(c => c.Code).ToArray());
            lookups.Add("BuildingCodes", lookup.BuildingLookup.Select(c => c.BuildingId).ToArray());
        }

        public ValidationResult ValidateEmployeeCriticalInfo(Employee employeeInformation)
        {
            EmployeeCriticalErrorValidator validator = new EmployeeCriticalErrorValidator(lookups);

            return validator.Validate(employeeInformation);
        }
    }

    internal class EmployeeCriticalErrorValidator : AbstractValidator<Employee>
    {
        public EmployeeCriticalErrorValidator(Dictionary<string, string[]> lookups)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            #region Person

            //**********PERSON***********************************************************************************************
            RuleFor(Employee => Employee.Person.MonsterID)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required Field");

            RuleFor(Employee => Employee.Person.FirstName)
                    .Length(0, 60)
                    .WithMessage($"{{PropertyName}}: exceeds maximum number of characters. Please double-check the field. If value is correct, please reach out to HSPD-12 Security at HSPD12.Security@gsa.gov or at +1 (202) 501-4459.")
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .Matches(@"^[a-zA-Z \-\‘\’\'\`]+$")
                    .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            RuleFor(Employee => Employee.Person.MiddleName)
                    .Length(0, 60)
                    .WithMessage($"{{PropertyName}}: exceeds maximum number of characters. Please double-check the field. If value is correct, please reach out to HSPD-12 Security at HSPD12.Security@gsa.gov or at +1 (202) 501-4459.")
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .Matches(@"^[A-Za-z \-\‘\’\'\`]{1,40}|[NMN]{1,3}$")
                    .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            RuleFor(Employee => Employee.Person.LastName)
                    .Length(0, 60)
                    .WithMessage($"{{PropertyName}}: exceeds maximum number of characters. Please double-check the field. If value is correct, please reach out to HSPD-12 Security at HSPD12.Security@gsa.gov or at +1 (202) 501-4459.")
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .Matches(@"^[a-zA-Z \-\‘\’\'\`]+$")
                    .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            RuleFor(Employee => Employee.Person.Suffix)
                .Matches(@"^(Jr.|Sr.|II|III|IV|V|VI|\s*)$")
                .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            RuleFor(Employee => Employee.Person.SocialSecurityNumber)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required Field")
                .Matches(@"^(?!000|666)[0-9]{3}(?!00)[0-9]{2}(?!0000)[0-9]{4}$")
                .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            Unless(e => string.IsNullOrEmpty(e.Person.Gender), () =>
            {
                RuleFor(Employee => Employee.Person.Gender)
                    .Matches(@"^(M|F)$")
                    .WithMessage($"{{PropertyName}} must be one of these values: 'M', 'F'");
            });


            RuleFor(Employee => Employee.Person.HomeEmail)
                .MaximumLength(64)
                .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            Unless(e => string.IsNullOrEmpty(e.Person.HomeEmail), () =>
            {
                RuleFor(Employee => Employee.Person.HomeEmail)
                    .EmailAddress()
                    .WithMessage($"{{PropertyName}} must be a valid email address")
                    .Matches(@"(?i)^((?!gsa(ig)?.gov).)*$")
                    .WithMessage("Home email cannot end in gsa.gov. (Case Ignored)");
            });

            RuleFor(Employee => Employee.Person.HREmail)
                    .MaximumLength(64)
                    .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            Unless(e => string.IsNullOrEmpty(e.Person.HREmail), () =>
            {
                RuleFor(Employee => Employee.Person.HREmail)
                    .EmailAddress()
                    .WithMessage($"{{PropertyName}} must be a valid email address")
                    .Matches(@".*[gsa.gov]$")
                    .WithMessage("Home email cannot end in gsa.gov. (Case Ignored)");
            });

            #endregion Person

            #region Birth

            //******************************Birth***********************************************************************
            Unless(e => string.IsNullOrEmpty(e.Birth.CityOfBirth), () =>
            {
                RuleFor(Employee => Employee.Birth.CityOfBirth)
                        .NotEmpty()
                        .WithMessage($"{{PropertyName}}: Required Field")
                        .Matches(@"^[a-zA-Z-\. \'\‘\’]{1,75}$")
                        .WithMessage($"{{PropertyName}}: Contains Invalid Characters");
            });

            Unless(e => string.IsNullOrEmpty(e.Birth.CountryOfBirth), () =>
            {
                RuleFor(Employee => Employee.Birth.CountryOfBirth)
                .In(lookups["CountryCodes"]);
            });

            Unless(e => string.IsNullOrEmpty(e.Birth.CountryOfBirth), () =>
            {
                When(Employee => Employee.Birth.CountryOfBirth.ToLower().Equals("us") ||
                    Employee.Birth.CountryOfBirth.ToLower().Equals("ca") ||
                    Employee.Birth.CountryOfBirth.ToLower().Equals("mx"), () =>
                    {
                        Unless(Employee => string.IsNullOrEmpty(Employee.Birth.StateOfBirth), () =>
                        {
                            RuleFor(Employee => Employee.Birth.StateOfBirth)
                                .NotEmpty()
                                .WithMessage($"{{PropertyName}}: Required Field")
                                .In(lookups["StateCodes"]);
                        });
                    });
            });

            RuleFor(Employee => Employee.Birth.Citizen)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field");

            Unless(e => string.IsNullOrEmpty(e.Birth.CountryOfCitizenship), () =>
            {
                RuleFor(Employee => Employee.Birth.CountryOfCitizenship)
                        //.NotEmpty()
                        //.WithMessage($"{{PropertyName}}: Required Field")
                        .In(lookups["CountryCodes"]);
            });

            Unless(e => e.Birth.DateOfBirth.Equals(null), () =>
            {
                RuleFor(Employee => Employee.Birth.DateOfBirth)
                        .NotEmpty()
                        .WithMessage($"{{PropertyName}}: Required Field")
                        .ValidDate()
                        .WithMessage($"{{PropertyName}} must be valid date");
            });


            #endregion Birth

            #region Address

            //***************************Address*******************************************************************
            RuleFor(Employee => Employee.Address.HomeAddress1)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required Field")
                .Matches(@"^[a-zA-Z0-9 .\\-\\\']{1,60}$")
                .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            Unless(e => string.IsNullOrEmpty(e.Address.HomeAddress3), () =>
            {
                RuleFor(Employee => Employee.Address.HomeAddress2)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required Field")
                .Matches(@"^[a-zA-Z0-9 .\\-\\\']{1,60}$")
                .WithMessage($"{{PropertyName}}: Contains Invalid Characters");
            });

            Unless(e => string.IsNullOrEmpty(e.Address.HomeAddress3), () =>
            {
                RuleFor(Employee => Employee.Address.HomeAddress3)
                .Matches(@"^[a-zA-Z0-9 .\\-\\\']{1,60}$")
                .WithMessage($"{{PropertyName}}: Contains Invalid Characters");
            });

            RuleFor(Employee => Employee.Address.HomeCity)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required Field")
                .Matches(@"^[a-zA-Z-. \'\‘\’]{1,60}$")
                .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            Unless(e => string.IsNullOrEmpty(e.Address.HomeCountry), () =>
            {
                When(e => e.Address.HomeCountry.ToLower().Equals("us") ||
                    e.Address.HomeCountry.ToLower().Equals("ca") ||
                    e.Address.HomeCountry.ToLower().Equals("mx"), () =>
                    {
                        Unless(e => string.IsNullOrEmpty(e.Address.HomeState), () =>
                        {
                            RuleFor(Employee => Employee.Address.HomeState)
                                .In(lookups["StateCodes"]);
                        });
                    });
            });

            RuleFor(Employee => Employee.Address.HomeZipCode)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .Matches(@"^(\d{5})$")
                    .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            Unless(e => string.IsNullOrEmpty(e.Address.HomeCountry), () =>
            {
                RuleFor(Employee => Employee.Address.HomeCountry)
                .In(lookups["CountryCodes"]);
            });

            #endregion Address

            #region Position

            //**********POSITION******************************************************************************************
            RuleFor(Employee => Employee.Position.JobTitle)
                   .NotEmpty()
                   .WithMessage($"{{PropertyName}}: Required Field")
                   .MaximumLength(70)
                   .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            RuleFor(Employee => Employee.Position.MajorOrg)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .Matches(@"^[A-Z]{1}$")
                    .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            RuleFor(Employee => Employee.Position.OfficeSymbol)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .MaximumLength(18)
                    .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            RuleFor(Employee => Employee.Position.Region)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .In(lookups["RegionCodes"])
                    .MaximumLength(3)
                    .WithMessage($"{{PropertyName}}: Contains Invalid Characters");

            RuleFor(Employee => Employee.Position.IsVirtual)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field");

            When(e => e.Position.IsVirtual.ToString().Equals("1"),() =>
            {
                RuleFor(Employee => Employee.Position.VirtualRegion)
                        .In(lookups["RegionCodes"])
                        .MaximumLength(3)
                        .WithMessage($"{{PropertyName}}: Contains Invalid Characters");
            });

            When(e => e.Position.IsVirtual.ToString().Equals("0"), () =>
            {
                RuleFor(Employee => Employee.Position.VirtualRegion)
                        .Empty()
                        .WithMessage($"{{PropertyName}}: Contains Invalid Characters");
            });

            #endregion Position

            #region Phone

            //**********PHONE*****************************************************************************************
            When(e => string.IsNullOrEmpty(e.Phone.HomePhone), () =>
            {
                RuleFor(Employee => Employee.Phone.PersonalCell)
                        .NotEmpty()
                        .WithMessage($"{{PropertyName}}: Required Field")
                        .MaximumLength(24)
                        .WithMessage($"{{PropertyName}} length must be 0-24")
                        .ValidPhone()
                        .WithMessage($"{{PropertyName}} must be a valid phone number");
            });

            When(e => string.IsNullOrEmpty(e.Phone.PersonalCell), () =>
            {
                RuleFor(Employee => Employee.Phone.HomePhone)
                        .NotEmpty()
                        .WithMessage($"{{PropertyName}}: Required Field")
                        .MaximumLength(24)
                        .WithMessage($"{{PropertyName}} length must be 0-24")
                        .ValidPhone()
                        .WithMessage($"{{PropertyName}} must be a valid phone number");
            });

            //RuleFor(Employee => Employee.Phone.WorkCell)
            //        .MaximumLength(24)
            //        .WithMessage($"{{PropertyName}} length must be 0-24")
            //        .ValidPhone()
            //        .WithMessage($"{{PropertyName}} must be a valid phone number");
            
            #endregion Phone

            #region Building

            //Building ****************************************************************************************

            Unless(e => e.Building.BuildingLocationCode.In("home,nongsa"), () =>
            {
                RuleFor(e => e.Building.BuildingLocationCode)
                    .In(lookups["BuildingCodes"])
                    .WithMessage($"{{PropertyName}} must be a valid building id");
            });

            #endregion
        }
    }
}
