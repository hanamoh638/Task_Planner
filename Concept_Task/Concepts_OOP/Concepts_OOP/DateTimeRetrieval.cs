using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts_OOP
{
    public class DateTimeRetrieval
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string FormattedDate => $"{Month}/{Day}/{Year}";

        public DateTimeRetrieval() { } // Parameterless constructor

        //public DateTimeRetrieval(int day, int month, int year)
        //{
        //    Day = day;
        //    Month = month;
        //    Year = year;
        //}

        public DateTimeRetrieval(int day, int month, int year)
        {
            if (IsValidDate(day, month, year))
            {
                Day = day;
                Month = month;
                Year = year;
            }
            else
            {
                throw new ArgumentException("Invalid date entered.");
            }
        }

        private static bool IsValidDate(int month, int day, int year)
        {
            // Validate year
            if (year < 1) return false;

            // Validate month
            if (month < 1 || month > 12) return false;

            // Validate day
            int[] daysInMonth = { 31, IsLeapYear(year) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            return day >= 1 && day <= daysInMonth[month - 1];
        }

        private static bool IsLeapYear(int year)
        {
            return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
        }
    }

}
