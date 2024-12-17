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
        public string FormattedDate => $"{Day}/{Month}/{Year}";

        public DateTimeRetrieval() { } // Parameterless constructor

        public DateTimeRetrieval(int year, int month, int day)
        {
            Day = day;
            Month = month;
            Year = year;
        }

    }
}