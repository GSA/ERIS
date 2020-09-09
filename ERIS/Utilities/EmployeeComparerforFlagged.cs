using ERIS.Models;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Utilities
{
    class EmployeeComparerforFlagged : BaseTypeComparer
    {
        public EmployeeComparerforFlagged(RootComparer rootComparer) : base(rootComparer)
        {
        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return type1 == type2;
        }

        public override void CompareType(CompareParms parms)
        {
            string[] included = { "Name", "SocialSecurityNumber", "DateOfBirth" };
            string[] excluded = { "FinalResult", "InitialResult", "FinalResultDate", "InitialResultDate" };
            var db = (Employee)parms.Object1;
            var er = (Employee)parms.Object2;
            var properties = typeof(Employee).GetProperties().Where(prop => prop.CanRead && prop.CanWrite).ToArray(); 

            for (int x = 0; x < properties.Count(); x++)
            {
                Type t2 = properties[x].GetValue(db, null).GetType();
                Type s2 = properties[x].GetValue(er, null).GetType();

                var childTargetProperties = t2.GetProperties().Where(p => p.CanRead && p.CanWrite).ToArray();
                var childSourceProperties = s2.GetProperties().Where(p => p.CanRead && p.CanWrite).ToArray();

                for (int y = 0; y < childTargetProperties.Count(); y++)
                {
                    var dbValue = childSourceProperties[y].GetValue(properties[x].GetValue(db, null), null);
                    var erValue = childTargetProperties[y].GetValue(properties[x].GetValue(er, null), null);

                    if (included.Any(q => q == childSourceProperties[y].Name))
                    {
                        string t;
                        if (erValue != null)
                            t = erValue.GetType().ToString();
                        else
                            t = null;
                        switch (t)
                        {
                            case "System.String":
                                {
                                    string targetObj = dbValue as string;
                                    string sourceObj = erValue as string;
                                    targetObj = targetObj == null ? "" : targetObj;
                                    sourceObj = sourceObj == null ? "" : sourceObj;
                                    if ((targetObj.ToLower().Trim().Equals(sourceObj.ToLower().Trim())) && !string.IsNullOrWhiteSpace(sourceObj))
                                    {
                                        Difference match = new Difference
                                        {
                                            PropertyName = childSourceProperties[y].Name,
                                            Object1Value = dbValue == null ? "" : dbValue.ToString(),
                                            Object2Value = erValue == null ? "" : erValue.ToString()
                                        };
                                        parms.Result.Differences.Add(match);
                                    }
                                }
                                break;

                            case "System.DateTime":
                                {
                                    var targetObj = dbValue as DateTime?;
                                    var sourceObj = erValue as DateTime?;
                                    if (targetObj == sourceObj && (sourceObj != null || sourceObj != DateTime.MinValue))
                                    {
                                        Difference match = new Difference
                                        {
                                            PropertyName = childSourceProperties[y].Name,
                                            Object1Value = dbValue == null ? "" : dbValue.ToString(),
                                            Object2Value = erValue == null ? "" : erValue.ToString()
                                        };
                                        parms.Result.Differences.Add(match);
                                    }
                                }
                                break;

                            case "System.Boolean":
                                {
                                    var targetObj = dbValue as bool?;
                                    var sourceObj = erValue as bool?;
                                    if (targetObj == sourceObj)
                                    {
                                        Difference match = new Difference
                                        {
                                            PropertyName = childSourceProperties[y].Name,
                                            Object1Value = dbValue == null ? "" : dbValue.ToString(),
                                            Object2Value = erValue == null ? "" : erValue.ToString()
                                        };
                                        parms.Result.Differences.Add(match);
                                    }
                                }
                                break;

                            case "System.Int64":
                                {
                                    var targetObj = dbValue as Int64?;
                                    var sourceObj = erValue as Int64?;
                                    if (targetObj == sourceObj && (sourceObj != 0))
                                    {
                                        Difference match = new Difference
                                        {
                                            PropertyName = childSourceProperties[y].Name,
                                            Object1Value = dbValue == null ? "" : dbValue.ToString(),
                                            Object2Value = erValue == null ? "" : erValue.ToString()
                                        };
                                        parms.Result.Differences.Add(match);
                                    }
                                }
                                break;

                            case null:
                                //nothing
                                break;

                            default:
                                {
                                    Difference match = new Difference
                                    {
                                        PropertyName = childSourceProperties[y].Name,
                                        Object1Value = dbValue == null ? "" : dbValue.ToString(),
                                        Object2Value = erValue == null ? "" : erValue.ToString()
                                    };
                                    parms.Result.Differences.Add(match);
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (!excluded.Any(z => z == childSourceProperties[y].Name))
                        {
                            
                            
                        }
                    }
                }
            }
        }
    }
}
