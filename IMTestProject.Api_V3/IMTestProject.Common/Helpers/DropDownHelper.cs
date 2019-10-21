using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IMTestProject.Common.Helpers
{
    public static class DropDownHelper
    {

        public static Tuple<string, string> GetDropDownProperty(Type type)
        {
            string valueAttributeName = "Id";
            string textAttributeName = "Name";

            var propertyList = type.GetProperties().ToList();

            foreach (var property in propertyList)
            {

                var dropdownValue = property.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DropdownValue));
                var dropdownText = property.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DropdownText));
                if (dropdownValue != null)
                {
                    valueAttributeName = property.Name;
                }
                if (dropdownText != null)
                {
                    textAttributeName = property.Name;
                }
            }

            return new Tuple<string, string>(valueAttributeName, textAttributeName);
        }

        public static IEnumerable<SelectListItem> GetCountries()
        {
            RegionInfo country = new RegionInfo(new CultureInfo("en-US", false).LCID);
            List<SelectListItem> countryNames = new List<SelectListItem>();

            //To get the Country Names from the CultureInfo installed in windows
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);
                countryNames.Add(new SelectListItem() { Text = country.DisplayName, Value = country.DisplayName });
            }

            //Assigning all Country names to IEnumerable
            IEnumerable<SelectListItem> nameAdded = countryNames.GroupBy(x => x.Text).Select(x => x.FirstOrDefault()).ToList<SelectListItem>().OrderBy(x => x.Text);
            return nameAdded;
        }
        
        public static List<SelectListItem> ControlType()
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Text = "N/A", Value = "N/A"},
                new SelectListItem {Text = "Text Box", Value = "input"},
                new SelectListItem {Text = "Text Area", Value = "textarea"},
                new SelectListItem {Text = "Date Time", Value = "DateTime"}
            };
        }

        public static List<SelectListItem> DataType()
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Text = "Select", Value = "Select"},
                new SelectListItem {Text = "String", Value = "string"},
                new SelectListItem {Text = "Int", Value = "int"},
                new SelectListItem {Text = "Long ", Value = "long"},
                 new SelectListItem {Text = "DateTime", Value = "DateTime"}
            };
        }

        public static List<SelectListItem> RaferanceTable()
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Text = "Select", Value = "0"},
                new SelectListItem {Text = "Continent", Value = "Continent"}
            };
        }
    }

    //helper class
    public class DropdownValue : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return true;
        }
    }

    public class DropdownText : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return true;
        }

    }


}
