using System;

namespace TechSolutions.Core.Reports
{
    public class FinancialReport
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public decimal TotalSales { get; set; }
        public decimal TotalCosts { get; set; }

        public decimal Profit => TotalSales - TotalCosts;

        public DateTime GeneratedAt { get; set; }
        public string GeneratedBy { get; set; } = string.Empty;
    }
}
