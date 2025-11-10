using Microsoft.EntityFrameworkCore;
using SafeRouteSharedLib.Models;

namespace SafeRouteApi.Data
{
    public class SafeRouteDbContext : DbContext
    {
        public SafeRouteDbContext(DbContextOptions<SafeRouteDbContext> options)
            : base(options)
        {
        }

        // DbSets for all models (from shared library)
        public DbSet<Users> Users { get; set; } = default!;
        public DbSet<Companies> Companies { get; set; } = default!;
        public DbSet<Drivers> Drivers { get; set; } = default!;
        public DbSet<Dispatchers> Dispatchers { get; set; } = default!;
        public DbSet<EmergencyContacts> EmergencyContacts { get; set; } = default!;
        public DbSet<Routes> Routes { get; set; } = default!;
        public DbSet<RiskZones> RiskZones { get; set; } = default!;
        public DbSet<PanicAlerts> PanicAlerts { get; set; } = default!;
        public DbSet<Incidents> Incidents { get; set; } = default!;
        public DbSet<IncidentResponses> IncidentResponses { get; set; } = default!;
        public DbSet<Notifications> Notifications { get; set; } = default!;
        public DbSet<ThreatDetections> ThreatDetections { get; set; } = default!;
        public DbSet<CameraRecordings> CameraRecordings { get; set; } = default!;
        public DbSet<SafetyScore> SafetyScores { get; set; } = default!;
        public DbSet<MonthlyReports> MonthlyReports { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users and Company relationship
            modelBuilder.Entity<Users>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Users and Drivers relationship (one-to-one)
            modelBuilder.Entity<Drivers>()
                .HasOne(d => d.User)
                .WithOne(u => u.Driver)
                .HasForeignKey<Drivers>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Users and Dispatchers relationship (one-to-one)
            modelBuilder.Entity<Dispatchers>()
                .HasOne(d => d.User)
                .WithOne(u => u.Dispatcher)
                .HasForeignKey<Dispatchers>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Dispatcher and Drivers relationship (one-to-many)
            modelBuilder.Entity<Drivers>()
                .HasOne(d => d.Dispatcher)
                .WithMany(dis => dis.Drivers)
                .HasForeignKey(d => d.DispatcherId)
                .OnDelete(DeleteBehavior.SetNull);

            // Driver and EmergencyContacts relationship
            modelBuilder.Entity<EmergencyContacts>()
                .HasOne(ec => ec.Driver)
                .WithMany(d => d.EmergencyContacts)
                .HasForeignKey(ec => ec.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Driver and Routes relationship
            modelBuilder.Entity<Routes>()
                .HasOne(r => r.Driver)
                .WithMany(d => d.Routes)
                .HasForeignKey(r => r.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Route and PanicAlerts relationship
            modelBuilder.Entity<PanicAlerts>()
                .HasOne(pa => pa.Route)
                .WithMany(r => r.PanicAlerts)
                .HasForeignKey(pa => pa.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Route and Incidents relationship
            modelBuilder.Entity<Incidents>()
                .HasOne(i => i.Route)
                .WithMany(r => r.Incidents)
                .HasForeignKey(i => i.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            // RiskZone and Incidents relationship
            modelBuilder.Entity<Incidents>()
                .HasOne(i => i.RiskZone)
                .WithMany(rz => rz.Incidents)
                .HasForeignKey(i => i.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            // PanicAlert and Incidents relationship
            modelBuilder.Entity<Incidents>()
                .HasOne(i => i.PanicAlert)
                .WithMany(pa => pa.Incidents)
                .HasForeignKey(i => i.AlertId)
                .OnDelete(DeleteBehavior.Restrict);

            // Driver and Incidents relationship
            modelBuilder.Entity<Incidents>()
                .HasOne(i => i.Driver)
                .WithMany(d => d.Incidents)
                .HasForeignKey(i => i.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Incident and IncidentResponses relationship
            modelBuilder.Entity<IncidentResponses>()
                .HasOne(ir => ir.Incident)
                .WithMany(i => i.IncidentResponses)
                .HasForeignKey(ir => ir.IncidentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Driver and IncidentResponses relationship
            modelBuilder.Entity<IncidentResponses>()
                .HasOne(ir => ir.Driver)
                .WithMany(d => d.IncidentResponses)
                .HasForeignKey(ir => ir.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Route and ThreatDetections relationship
            modelBuilder.Entity<ThreatDetections>()
                .HasOne(td => td.Route)
                .WithMany(r => r.ThreatDetections)
                .HasForeignKey(td => td.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Driver and ThreatDetections relationship
            modelBuilder.Entity<ThreatDetections>()
                .HasOne(td => td.Driver)
                .WithMany(d => d.ThreatDetections)
                .HasForeignKey(td => td.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // ThreatDetection and CameraRecordings relationship
            modelBuilder.Entity<CameraRecordings>()
                .HasOne(cr => cr.ThreatDetected)
                .WithMany(td => td.CameraRecordings)
                .HasForeignKey(cr => cr.ThreatId)
                .OnDelete(DeleteBehavior.Restrict);

            // Driver and CameraRecordings relationship
            modelBuilder.Entity<CameraRecordings>()
                .HasOne(cr => cr.Driver)
                .WithMany(d => d.CameraRecordings)
                .HasForeignKey(cr => cr.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Route and CameraRecordings relationship
            modelBuilder.Entity<CameraRecordings>()
                .HasOne(cr => cr.Route)
                .WithMany(r => r.CameraRecordings)
                .HasForeignKey(cr => cr.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Driver and Notifications relationship
            modelBuilder.Entity<Notifications>()
                .HasOne(n => n.Driver)
                .WithMany(d => d.Notifications)
                .HasForeignKey(n => n.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Driver and PanicAlerts relationship
            modelBuilder.Entity<PanicAlerts>()
                .HasOne(pa => pa.Driver)
                .WithMany(d => d.PanicAlerts)
                .HasForeignKey(pa => pa.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Driver and SafetyScore relationship
            modelBuilder.Entity<SafetyScore>()
                .HasOne(ss => ss.Driver)
                .WithMany(d => d.SafetyScores)
                .HasForeignKey(ss => ss.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Company and MonthlyReports relationship
            modelBuilder.Entity<MonthlyReports>()
                .HasOne(mr => mr.Company)
                .WithMany(c => c.MonthlyReports)
                .HasForeignKey(mr => mr.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure default values and constraints
            modelBuilder.Entity<Notifications>()
                .Property(n => n.IsRead)
                .HasDefaultValue(false);

            modelBuilder.Entity<ThreatDetections>()
                .Property(t => t.ConfirmedThreat)
                .HasDefaultValue(false);

            modelBuilder.Entity<CameraRecordings>()
                .Property(c => c.Evidence)
                .HasDefaultValue(false);

            modelBuilder.Entity<IncidentResponses>()
                .Property(ir => ir.WasSuccessful)
                .HasDefaultValue(false);

            modelBuilder.Entity<Dispatchers>()
                .Property(d => d.IsOnDuty)
                .HasDefaultValue(false);
        }
    }
}
