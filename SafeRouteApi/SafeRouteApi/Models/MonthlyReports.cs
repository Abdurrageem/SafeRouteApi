using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteApi.Models
{
    public class MonthlyReports
    {
        [Key]
        public int ReportId { get; set; }

        // Foreign key for companies
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Companies? Company { get; set; }

        public DateTime ReportDate { get; set; }

        public int TotalRoutes { get; set; }

        public int CompletedRoutes { get; set; }

        public int TotalIncidents { get; set; }

        public int ResolvedIncidents { get; set; }

        public float TotalDistance { get; set; }

        public int AverageSafetyScore { get; set; }

        public int TotalDeliveries { get; set; }

        public int SuccessfulDeliveries { get; set; }

        public int PanicAlerts { get; set; }

        public int ThreatDetections { get; set; }

        public float AverageResponseTime { get; set; }

        public string ReportData { get; set; } = string.Empty;
    }
}
