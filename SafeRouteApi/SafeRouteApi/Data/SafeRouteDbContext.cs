using Microsoft.EntityFrameworkCore;

namespace SafeRouteApi.Data
{
    public class SafeRouteDbContext : DbContext
    {
        public SafeRouteDbContext(DbContextOptions<SafeRouteDbContext> options)
            : base(options) { }

        // Replace model types with local models (ensure they exist under SafeRouteApi.Models)
        public DbSet<Models.Users> Users { get; set; } = default!;
        public DbSet<Models.Companies> Companies { get; set; } = default!;
        public DbSet<Models.Drivers> Drivers { get; set; } = default!;
        public DbSet<Models.Dispatchers> Dispatchers { get; set; } = default!;
        public DbSet<Models.EmergencyContacts> EmergencyContacts { get; set; } = default!;
        public DbSet<Models.Routes> Routes { get; set; } = default!;
        public DbSet<Models.RiskZones> RiskZones { get; set; } = default!;
        public DbSet<Models.PanicAlerts> PanicAlerts { get; set; } = default!;
        public DbSet<Models.Incidents> Incidents { get; set; } = default!;
        public DbSet<Models.IncidentResponses> IncidentResponses { get; set; } = default!;
        public DbSet<Models.Notifications> Notifications { get; set; } = default!;
        public DbSet<Models.ThreatDetections> ThreatDetections { get; set; } = default!;
        public DbSet<Models.CameraRecordings> CameraRecordings { get; set; } = default!;
        public DbSet<Models.SafetyScore> SafetyScores { get; set; } = default!;
        public DbSet<Models.MonthlyReports> MonthlyReports { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Use fully-qualified type names for clarity
            modelBuilder.Entity<Models.Users>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Drivers>()
                .HasOne(d => d.User)
                .WithOne(u => u.Driver)
                .HasForeignKey<Models.Drivers>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Dispatchers>()
                .HasOne(d => d.User)
                .WithOne(u => u.Dispatcher)
                .HasForeignKey<Models.Dispatchers>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Drivers>()
                .HasOne(d => d.Dispatcher)
                .WithMany(dis => dis.Drivers)
                .HasForeignKey(d => d.DispatcherId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Models.EmergencyContacts>()
                .HasOne(ec => ec.Driver)
                .WithMany(d => d.EmergencyContacts)
                .HasForeignKey(ec => ec.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Routes>()
                .HasOne(r => r.Driver)
                .WithMany(d => d.Routes)
                .HasForeignKey(r => r.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.PanicAlerts>()
                .HasOne(pa => pa.Route)
                .WithMany(r => r.PanicAlerts)
                .HasForeignKey(pa => pa.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Incidents>()
                .HasOne(i => i.Route)
                .WithMany(r => r.Incidents)
                .HasForeignKey(i => i.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Incidents>()
                .HasOne(i => i.RiskZone)
                .WithMany(rz => rz.Incidents)
                .HasForeignKey(i => i.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Incidents>()
                .HasOne(i => i.PanicAlert)
                .WithMany(pa => pa.Incidents)
                .HasForeignKey(i => i.AlertId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Incidents>()
                .HasOne(i => i.Driver)
                .WithMany(d => d.Incidents)
                .HasForeignKey(i => i.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.IncidentResponses>()
                .HasOne(ir => ir.Incident)
                .WithMany(i => i.IncidentResponses)
                .HasForeignKey(ir => ir.IncidentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.IncidentResponses>()
                .HasOne(ir => ir.Driver)
                .WithMany(d => d.IncidentResponses)
                .HasForeignKey(ir => ir.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.ThreatDetections>()
                .HasOne(td => td.Route)
                .WithMany(r => r.ThreatDetections)
                .HasForeignKey(td => td.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.ThreatDetections>()
                .HasOne(td => td.Driver)
                .WithMany(d => d.ThreatDetections)
                .HasForeignKey(td => td.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.CameraRecordings>()
                .HasOne(cr => cr.ThreatDetected)
                .WithMany(td => td.CameraRecordings)
                .HasForeignKey(cr => cr.ThreatId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.CameraRecordings>()
                .HasOne(cr => cr.Driver)
                .WithMany(d => d.CameraRecordings)
                .HasForeignKey(cr => cr.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.CameraRecordings>()
                .HasOne(cr => cr.Route)
                .WithMany(r => r.CameraRecordings)
                .HasForeignKey(cr => cr.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Notifications>()
                .HasOne(n => n.Driver)
                .WithMany(d => d.Notifications)
                .HasForeignKey(n => n.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.PanicAlerts>()
                .HasOne(pa => pa.Driver)
                .WithMany(d => d.PanicAlerts)
                .HasForeignKey(pa => pa.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.SafetyScore>()
                .HasOne(ss => ss.Driver)
                .WithMany(d => d.SafetyScores)
                .HasForeignKey(ss => ss.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.MonthlyReports>()
                .HasOne(mr => mr.Company)
                .WithMany(c => c.MonthlyReports)
                .HasForeignKey(mr => mr.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Notifications>()
                .Property(n => n.IsRead)
                .HasDefaultValue(false);
            modelBuilder.Entity<Models.ThreatDetections>()
                .Property(t => t.ConfirmedThreat)
                .HasDefaultValue(false);
            modelBuilder.Entity<Models.CameraRecordings>()
                .Property(c => c.Evidence)
                .HasDefaultValue(false);
            modelBuilder.Entity<Models.IncidentResponses>()
                .Property(ir => ir.WasSuccessful)
                .HasDefaultValue(false);
            modelBuilder.Entity<Models.Dispatchers>()
                .Property(d => d.IsOnDuty)
                .HasDefaultValue(false);
        }
    }
}
