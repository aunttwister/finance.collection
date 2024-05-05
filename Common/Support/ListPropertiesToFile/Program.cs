using Finance.Collection.Domain.FinanceScraper.Results;
using Financial.Collection.Domain.DTOs;
using IntrinsicValue.Calculation.DataSets.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ListPropertiesToFile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PropertyListerCustom.ListPropertiesToFile(typeof(GrahamScrapeResult), "GrahamScrapeResultProperties.txt");
        }
    }
}
