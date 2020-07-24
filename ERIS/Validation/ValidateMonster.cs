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
using U = ERIS.Utilities;

namespace ERIS.Validation
{
    internal class ValidateMonster
    {
        private readonly Dictionary<string, string[]> lookups = new Dictionary<string, string[]>();

        public ValidateMonster(Lookup lookup)
        {
            lookups.Add("USStateCodes", lookup.usStateLookup.Select(s => s.Code).ToArray());
            lookups.Add("MXStateCodes", lookup.mxStateLookup.Select(s => s.Code).ToArray());
            lookups.Add("CAStateCodes", lookup.caStateLookup.Select(s => s.Code).ToArray());
            lookups.Add("CountryCodes", lookup.countryLookup.Select(c => c.Code).ToArray());
            lookups.Add("RegionCodes", lookup.regionLookup.Select(c => c.Code).ToArray());
            lookups.Add("BuildingCodes", lookup.BuildingLookup.Select(c => c.BuildingId).ToArray());
            lookups.Add("EmailCodes", lookup.EmailLookup.Select(c => c.WorkEmail).ToArray());
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
                    .Matches(@"^([A-Za-z \-\‘\’\'\`]{1,40}|[NMN]{1,3})+$")
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
                .WithMessage($"{{PropertyName}}: Invalid input. Accepted inputs are 'Jr.', 'Sr.', 'II', 'III', 'IV', 'V', 'VI', and an empty value");

            RuleFor(Employee => Employee.Person.SocialSecurityNumber)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required Field")
                .Matches(@"^(?!000|666)[0-9]{3}(?!00)[0-9]{2}(?!0000)[0-9]{4}$")
                .WithMessage($"{{PropertyName}}: Invalid input. SSN must be exactly 9 digits long with no spaces or punctuation.");

            RuleFor(Employee => Employee.Person.Gender)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required Field")
                .Matches(@"^(M|F)$")
                .WithMessage($"{{PropertyName}}: Invalid input. Accepted inputs are 'M' or 'F'");

            RuleFor(Employee => Employee.Person.HomeEmail)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required Field")
                .MaximumLength(64)
                .WithMessage($"{{PropertyName}}: Exceeds maximum number of characters")
                .EmailAddress()
                .WithMessage($"{{PropertyName}}: Invalid e-mail address")
                .Matches(@"(?i)^((?!gsa(ig)?.gov).)*$")
                .WithMessage("Home email cannot end in gsa.gov. (Case Ignored)");

            RuleFor(Employee => Employee.Person.HREmail)
                   .NotEmpty()
                   .WithMessage($"{{PropertyName}}: Required Field")
                   .MaximumLength(64)
                   .WithMessage($"{{PropertyName}}: Exceeds maximum number of characters")
                   .EmailAddress()
                   .WithMessage($"{{PropertyName}}: Invalid e-mail address")
                   .Matches(@".*[gsa.gov]$")
                   .WithMessage($"{{PropertyName}}: Invalid gsa e-mail address")
                   .In(lookups["EmailCodes"])
                   .WithMessage("GSA E-Mail Address not found");


            #endregion Person

            #region Birth

            //******************************Birth***********************************************************************
            RuleFor(Employee => Employee.Birth.CityOfBirth)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .Matches(@"^([a-zA-Z-\. \'\‘\’]{1,75})$")
                    .WithMessage($"{{PropertyName}}: Exceeds maximum number of characters");

            RuleFor(Employee => Employee.Birth.CountryOfBirth)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required Field")
                .In(lookups["CountryCodes"])
                .WithMessage($"{{PropertyName}}: Invalid FIPS Country Code");

            When(Employee => Employee.Birth.CountryOfBirth.Equals("US"), () =>
                    {
                            RuleFor(Employee => Employee.Birth.StateOfBirth)
                                    .NotEmpty()
                                    .WithMessage($"{{PropertyName}}: Required for POB Country provided")
                                    .Matches(@"^[a-zA-Z]{2}$")
                                    .WithMessage($"{{PropertyName}}:Invalid FIPS State / Province Code")
                                    .In(lookups["USStateCodes"])
                                    .WithMessage($"{{PropertyName}}: Invalid value for POB country provided");
                    });

            When(Employee => Employee.Birth.CountryOfBirth.Equals("MX"), () =>
                    {
                        RuleFor(Employee => Employee.Birth.StateOfBirth)
                                .NotEmpty()
                                .WithMessage($"{{PropertyName}}: Required for POB Country provided")
                                .Matches(@"^[a-zA-Z]{2}$")
                                .WithMessage($"{{PropertyName}}:Invalid FIPS State / Province Code")
                                .In(lookups["MXStateCodes"])
                                .WithMessage($"{{PropertyName}}: Invalid value for POB country provided");
                    });

            When(Employee => Employee.Birth.CountryOfBirth.Equals("CA"), () =>
                    {
                        RuleFor(Employee => Employee.Birth.StateOfBirth)
                                .NotEmpty()
                                .WithMessage($"{{PropertyName}}: Required for POB Country provided")
                                .Matches(@"^[a-zA-Z]{2}$")
                                .WithMessage($"{{PropertyName}}:Invalid FIPS State / Province Code")
                                .In(lookups["CAStateCodes"])
                                .WithMessage($"{{PropertyName}}: Invalid value for POB country provided");
                    });

            When(Employee => Employee.Birth.CountryOfBirth != "US" &&
                    Employee.Birth.CountryOfBirth != "CA" &&
                    Employee.Birth.CountryOfBirth != "MX", () =>
                    {
                        RuleFor(Employee => Employee.Birth.StateOfBirth)
                                 .Empty()
                                 .WithMessage($"{{PropertyName}}: Must be blank for POB Country provided");
                    });

            RuleFor(Employee => Employee.Birth.Citizen)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .Must(citizen => citizen == true || citizen == false)
                    .WithMessage($"{{PropertyName}}: Invalid Input (non-boolean)");

            When(Employee => Employee.Birth.Citizen == true, () =>
            {
                RuleFor(Employee => Employee.Birth.CountryOfCitizenship)
                    .Equal("US")
                    .WithMessage($"{{PropertyName}}: Citizenship Country field value contradicts the input for US Citizen");
            });

            When(Employee => Employee.Birth.Citizen == false, () =>
            {
                RuleFor(Employee => Employee.Birth.CountryOfCitizenship)
                    .NotEqual("US")
                    .WithMessage($"{{PropertyName}}: Citizenship Country field value contradicts the input for US Citizen");
            });

            RuleFor(Employee => Employee.Birth.CountryOfCitizenship)
                        .NotEmpty()
                        .WithMessage($"{{PropertyName}}: Required Field")
                        .In(lookups["CountryCodes"])
                        .WithMessage($"{{PropertyName}}: Invalid FIPS Country Code");

            RuleFor(Employee => Employee.Birth.DateOfBirth)
                        .NotEmpty()
                        .WithMessage($"{{PropertyName}}: Required Field")
                        .Matches(@"^(((18|19|20)[0-9]{2}[\-](0[13578]|1[02])[\-](0[1-9]|[12][0-9]|3[01]))|((18|19|20)[0-9]{2}[\-](0[469]|11)[\-](0[1-9]|[12][0-9]|30))|((18|19|20)[0-9]{2}[\-](02)[\-](0[1-9]|1[0-9]|2[0-8]))|((((18|19|20)(04|08|[2468][048]|[13579][26]))|2000)[\-](02)[\-]29))+$")
                        //.ValidDate()
                        .WithMessage($"{{PropertyName}}: Must be a valid date");

            #endregion Birth

            #region Address

            //***************************Address*******************************************************************
            RuleFor(Employee => Employee.Address.HomeAddress1)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required Field")
                .Matches(@"^[a-zA-Z0-9 .\\-\\\']{1,60}$")
                .WithMessage($"{{PropertyName}}: Exceeds maximum number of characters.");

            Unless(e => string.IsNullOrEmpty(e.Address.HomeAddress3), () =>
            {
                RuleFor(Employee => Employee.Address.HomeAddress2)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Must be not blank when Home Address 3 is not blank.")
                .Matches(@"^[a-zA-Z0-9 .\\-\\\']{1,60}$")
                .WithMessage($"{{PropertyName}}: Exceeds maximum number of characters.");
            });

            Unless(e => string.IsNullOrEmpty(e.Address.HomeAddress2), () =>
            {
                RuleFor(Employee => Employee.Address.HomeAddress2)
                .Matches(@"^[a-zA-Z0-9 .\\-\\\']{1,60}$")
                .WithMessage($"{{PropertyName}}: Exceeds maximum number of characters.");
            });

            Unless(e => string.IsNullOrEmpty(e.Address.HomeAddress3), () =>
            {
                RuleFor(Employee => Employee.Address.HomeAddress3)
                .Matches(@"^[a-zA-Z0-9 .\\-\\\']{1,60}$")
                .WithMessage($"{{PropertyName}}: Exceeds maximum number of characters.");
            });

            RuleFor(Employee => Employee.Address.HomeCity)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required Field")
                .Matches(@"^[a-zA-Z-. \'\‘\’]{1,60}$")
                .WithMessage($"{{PropertyName}}:  Exceeds maximum number of characters");

            When(e => e.Address.HomeCountry.ToLower().Equals("us"), () =>
                    {
                        RuleFor(Employee => Employee.Address.HomeState)
                                .NotEmpty()
                                .WithMessage($"{{PropertyName}}: Required Field")
                                .In(lookups["USStateCodes"])
                                .WithMessage($"{{PropertyName}}: Invalid FIPS State/Province Code");

                    });

            When(e => e.Address.HomeCountry.ToLower().Equals("mx"), () =>
                    {
                        RuleFor(Employee => Employee.Address.HomeState)
                                .NotEmpty()
                                .WithMessage($"{{PropertyName}}: Required Field")
                                .In(lookups["MXStateCodes"])
                                .WithMessage($"{{PropertyName}}: Invalid FIPS State/Province Code");

                    });

            When(e => e.Address.HomeCountry.ToLower().Equals("ca"), () =>
                    {
                        RuleFor(Employee => Employee.Address.HomeState)
                                .NotEmpty()
                                .WithMessage($"{{PropertyName}}: Required Field")
                                .In(lookups["CAStateCodes"])
                                .WithMessage($"{{PropertyName}}: Invalid FIPS State/Province Code");

                    });

            RuleFor(Employee => Employee.Address.HomeZipCode)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .Matches(@"^(\d{5})$")
                    .WithMessage($"{{PropertyName}}: Invalid ZIP");

            RuleFor(Employee => Employee.Address.HomeCountry)
                .NotEmpty()
                .WithMessage($"{{PropertyName}}: Required for POB Country provided")
                .In(lookups["CountryCodes"])
                .WithMessage($"{{PropertyName}}: Only US residents are eligible for sponsorship at this time");

            #endregion Address

            #region Position

            //**********POSITION******************************************************************************************
            RuleFor(Employee => Employee.Position.JobTitle)
                   .NotEmpty()
                   .WithMessage($"{{PropertyName}}: Required Field")
                   .MaximumLength(70)
                   .WithMessage($"{{PropertyName}}:  Invalid Job Title");

            RuleFor(Employee => Employee.Position.MajorOrg)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .Matches(@"^[A-Z]{1}$")
                    .WithMessage($"{{PropertyName}}: Invalid Major Org");

            RuleFor(Employee => Employee.Position.OfficeSymbol)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .MaximumLength(18)
                    .WithMessage($"{{PropertyName}}: Exceeds maximum number of characters.");

            RuleFor(Employee => Employee.Position.Region)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .In(lookups["RegionCodes"])
                    .MaximumLength(3)
                    .WithMessage($"{{PropertyName}}:  Invalid Region");

            RuleFor(Employee => Employee.Position.IsVirtual)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .Must(isvirtual => isvirtual == true || isvirtual == false )
                    .WithMessage($"{{PropertyName}}: Value must be 1 or 0");

            When(e => e.Position.IsVirtual == true,() =>
            {
                RuleFor(Employee => Employee.Position.VirtualRegion)
                        .NotEmpty()
                        .WithMessage($"{{PropertyName}}: Required Field")
                        .In(lookups["RegionCodes"])
                        .MaximumLength(3)
                        .WithMessage($"{{PropertyName}}: Invalid Region");
            });

            When(e => e.Position.IsVirtual == false, () =>
            {
                RuleFor(Employee => Employee.Position.VirtualRegion)
                        .Empty()
                        .WithMessage($"{{PropertyName}}: Value contradicts Virtual Employee field value");
            });

            #endregion Position

            #region Phone

            //**********PHONE*****************************************************************************************
            //When(e => e.Phone.HomePhone == string.Empty, () =>
            //{
                RuleFor(Employee => Employee.Phone.PersonalCell)
                        .NotEmpty()
                        .WithMessage($"{{PropertyName}}: Required Field")
                        .MaximumLength(24)
                        .WithMessage($"{{PropertyName}} length must be 0-24")
                        .Matches(@"^(([0-9]{3}[0-9]{3}[0-9]{4})|(\+([0-9]{1,3})\.([0-9]{4,14})(([xX]){1}[0-9]{1,4}))|(\+([0-9]{1,3})\.([0-9]{4,14})))+$")
                        .WithMessage($"{{PropertyName}}: Invalid phone number");
            //});

            //When(e => e.Phone.PersonalCell == string.Empty, () =>
            //{
                RuleFor(Employee => Employee.Phone.HomePhone)
                        .NotEmpty()
                        .WithMessage($"{{PropertyName}}: Required Field")
                        .MaximumLength(24)
                        .WithMessage($"{{PropertyName}} length must be 0-24")
                        .Matches(@"^(([0-9]{3}[0-9]{3}[0-9]{4})|(\+([0-9]{1,3})\.([0-9]{4,14})(([xX]){1}[0-9]{1,4}))|(\+([0-9]{1,3})\.([0-9]{4,14})))+$")
                        .WithMessage($"{{PropertyName}}: Invalid phone number");
            //});

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
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}}: Required Field")
                    .In(lookups["BuildingCodes"])
                    .WithMessage($"{{PropertyName}}: Not Found in GCIMS");
            });

            #endregion
        }
    }
}
