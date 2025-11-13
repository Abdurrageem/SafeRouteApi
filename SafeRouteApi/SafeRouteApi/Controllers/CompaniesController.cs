using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SafeRoute.Controllers
{
    /// <summary>
    /// API Controller for companies/fleet management
    /// Phase 4: Supporting Features
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(ILogger<CompaniesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: api/companies
        /// Get all companies (admin only)
        /// </summary>
        [HttpGet(Name = "GetAllCompanies")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Getting all companies");

            var companies = new List<Company>
            {
                new Company
                {
                    Id = 1,
                    Name = "SafeRoute Logistics",
                    RegistrationNumber = "2020/123456/07",
                    Email = "info@saferoute.co.za",
                    Phone = "0215551234",
                    Address = "123 Business Park, Cape Town",
                    FleetSize = 25,
                    ActiveDrivers = 22,
                    SubscriptionPlan = "Premium",
                    SubscriptionStatus = "Active",
                    SubscriptionExpiresAt = DateTime.Now.AddMonths(11),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddYears(-2)
                },
                new Company
                {
                    Id = 2,
                    Name = "Express Delivery Co",
                    RegistrationNumber = "2019/987654/07",
                    Email = "contact@expressdelivery.co.za",
                    Phone = "0215559876",
                    Address = "456 Industrial Road, Cape Town",
                    FleetSize = 15,
                    ActiveDrivers = 12,
                    SubscriptionPlan = "Standard",
                    SubscriptionStatus = "Active",
                    SubscriptionExpiresAt = DateTime.Now.AddMonths(5),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddYears(-1)
                }
            };

            return Ok(companies);
        }

        /// <summary>
        /// GET: api/companies/{id}
        /// Get company by ID
        /// </summary>
        [HttpGet("{id}", Name = "GetCompanyById")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation($"Getting company {id}");

            var company = new Company
            {
                Id = id,
                Name = "SafeRoute Logistics",
                RegistrationNumber = "2020/123456/07",
                Email = "info@saferoute.co.za",
                Phone = "0215551234",
                AlternatePhone = "0821234567",
                Address = "123 Business Park, Cape Town",
                City = "Cape Town",
                Province = "Western Cape",
                PostalCode = "8001",
                Country = "South Africa",
                FleetSize = 25,
                ActiveDrivers = 22,
                ActiveRoutes = 8,
                TotalRoutes = 1245,
                SubscriptionPlan = "Premium",
                SubscriptionStatus = "Active",
                SubscriptionStartedAt = DateTime.Now.AddYears(-2),
                SubscriptionExpiresAt = DateTime.Now.AddMonths(11),
                ContactPerson = "John Smith",
                ContactEmail = "john.smith@saferoute.co.za",
                ContactPhone = "0821234567",
                IsActive = true,
                CreatedAt = DateTime.Now.AddYears(-2)
            };

            return Ok(company);
        }

        /// <summary>
        /// GET: api/companies/{id}/drivers
        /// Get all drivers for a company
        /// </summary>
        [HttpGet("{id}/drivers", Name = "GetCompanyDrivers")]
        public IActionResult GetDrivers(int id)
        {
            _logger.LogInformation($"Getting drivers for company {id}");

            var drivers = new List<CompanyDriver>
            {
                new CompanyDriver
                {
                    DriverId = 1,
                    Name = "John Mbeki",
                    LicenseNumber = "CA-123-456",
                    VehicleRegistration = "CAW 12345",
                    Status = "Active",
                    TotalRoutes = 45,
                    SafetyScore = 4.8,
                    LastRouteDate = DateTime.Now.AddHours(-2),
                    JoinedAt = DateTime.Now.AddMonths(-6)
                },
                new CompanyDriver
                {
                    DriverId = 2,
                    Name = "Sarah van der Walt",
                    LicenseNumber = "CA-789-012",
                    VehicleRegistration = "CAW 67890",
                    Status = "Active",
                    TotalRoutes = 38,
                    SafetyScore = 4.9,
                    LastRouteDate = DateTime.Now.AddHours(-5),
                    JoinedAt = DateTime.Now.AddMonths(-4)
                }
            };

            return Ok(drivers);
        }

        /// <summary>
        /// GET: api/companies/{id}/stats
        /// Get company statistics
        /// </summary>
        [HttpGet("{id}/stats", Name = "GetCompanyStats")]
        public IActionResult GetStats(int id)
        {
            _logger.LogInformation($"Getting stats for company {id}");

            var stats = new CompanyStats
            {
                CompanyId = id,
                FleetSize = 25,
                ActiveDrivers = 22,
                InactiveDrivers = 3,
                ActiveRoutes = 8,
                CompletedRoutesToday = 15,
                CompletedRoutesThisWeek = 89,
                CompletedRoutesThisMonth = 342,
                TotalDistance = 45678.5,
                AverageSafetyScore = 4.75,
                TotalIncidents = 12,
                ResolvedIncidents = 10,
                PanicAlerts = 3,
                RiskZonesEncountered = 156,
                OnTimeDeliveryRate = 94.5
            };

            return Ok(stats);
        }

        /// <summary>
        /// GET: api/companies/{id}/routes
        /// Get routes for a company
        /// </summary>
        [HttpGet("{id}/routes", Name = "GetCompanyRoutes")]
        public IActionResult GetRoutes(int id, [FromQuery] string? status = null)
        {
            _logger.LogInformation($"Getting routes for company {id} (status: {status})");

            var routes = new List<CompanyRoute>
            {
                new CompanyRoute
                {
                    RouteId = 1,
                    DriverId = 1,
                    DriverName = "John Mbeki",
                    StartLocation = "Cape Town CBD",
                    EndLocation = "Stellenbosch",
                    Status = "In Progress",
                    StartTime = DateTime.Now.AddHours(-1),
                    EstimatedArrival = DateTime.Now.AddHours(1),
                    DistanceKm = 45.2
                }
            };

            if (!string.IsNullOrEmpty(status))
            {
                routes = routes.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(routes);
        }

        /// <summary>
        /// GET: api/companies/{id}/incidents
        /// Get incidents reported by company drivers
        /// </summary>
        [HttpGet("{id}/incidents", Name = "GetCompanyIncidents")]
        public IActionResult GetIncidents(int id)
        {
            _logger.LogInformation($"Getting incidents for company {id}");

            var incidents = new List<CompanyIncident>
            {
                new CompanyIncident
                {
                    IncidentId = 1,
                    DriverId = 1,
                    DriverName = "John Mbeki",
                    IncidentType = "Crime",
                    Severity = "High",
                    Location = "Nyanga, Cape Town",
                    OccurredAt = DateTime.Now.AddHours(-2),
                    Status = "Verified"
                }
            };

            return Ok(incidents);
        }

        /// <summary>
        /// POST: api/companies
        /// Create a new company
        /// </summary>
        [HttpPost(Name = "CreateCompany")]
        public IActionResult Create([FromBody] CreateCompanyDto dto)
        {
            _logger.LogInformation($"Creating company: {dto.Name}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var company = new Company
            {
                Id = 1,
                Name = dto.Name,
                RegistrationNumber = dto.RegistrationNumber,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                City = dto.City,
                Province = dto.Province,
                PostalCode = dto.PostalCode,
                Country = dto.Country ?? "South Africa",
                ContactPerson = dto.ContactPerson,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                SubscriptionPlan = dto.SubscriptionPlan ?? "Standard",
                SubscriptionStatus = "Active",
                SubscriptionStartedAt = DateTime.Now,
                SubscriptionExpiresAt = DateTime.Now.AddYears(1),
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            return CreatedAtRoute("GetCompanyById", new { id = company.Id }, company);
        }

        /// <summary>
        /// PUT: api/companies/{id}
        /// Update company information
        /// </summary>
        [HttpPut("{id}", Name = "UpdateCompany")]
        public IActionResult Update(int id, [FromBody] UpdateCompanyDto dto)
        {
            _logger.LogInformation($"Updating company {id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Update in database
            return NoContent();
        }

        /// <summary>
        /// PUT: api/companies/{id}/subscription
        /// Update company subscription
        /// </summary>
        [HttpPut("{id}/subscription", Name = "UpdateCompanySubscription")]
        public IActionResult UpdateSubscription(int id, [FromBody] UpdateSubscriptionDto dto)
        {
            _logger.LogInformation($"Updating subscription for company {id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Update subscription details
            return Ok(new { message = "Subscription updated" });
        }

        /// <summary>
        /// DELETE: api/companies/{id}
        /// Delete a company (soft delete)
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteCompany")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Deleting company {id}");

            // TODO: Soft delete - set IsActive to false
            return NoContent();
        }

        /// <summary>
        /// GET: api/companies/stats
        /// Get overall company statistics (admin)
        /// </summary>
        [HttpGet("stats", Name = "GetAllCompaniesStats")]
        public IActionResult GetAllStats()
        {
            _logger.LogInformation("Getting overall company statistics");

            var stats = new
            {
                TotalCompanies = 45,
                ActiveCompanies = 42,
                InactiveCompanies = 3,
                TotalDrivers = 856,
                TotalFleetSize = 923,
                ActiveRoutes = 156,
                SubscriptionRevenue = 125000.00,
                BySubscriptionPlan = new
                {
                    Premium = 15,
                    Standard = 22,
                    Basic = 8
                }
            };

            return Ok(stats);
        }
    }

    // Company Models
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? AlternatePhone { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int FleetSize { get; set; }
        public int ActiveDrivers { get; set; }
        public int ActiveRoutes { get; set; }
        public int TotalRoutes { get; set; }
        public string SubscriptionPlan { get; set; } = string.Empty; // Basic, Standard, Premium
        public string SubscriptionStatus { get; set; } = string.Empty; // Active, Expired, Cancelled
        public DateTime SubscriptionStartedAt { get; set; }
        public DateTime SubscriptionExpiresAt { get; set; }
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CompanyDriver
    {
        public int DriverId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string VehicleRegistration { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int TotalRoutes { get; set; }
        public double SafetyScore { get; set; }
        public DateTime? LastRouteDate { get; set; }
        public DateTime JoinedAt { get; set; }
    }

    public class CompanyStats
    {
        public int CompanyId { get; set; }
        public int FleetSize { get; set; }
        public int ActiveDrivers { get; set; }
        public int InactiveDrivers { get; set; }
        public int ActiveRoutes { get; set; }
        public int CompletedRoutesToday { get; set; }
        public int CompletedRoutesThisWeek { get; set; }
        public int CompletedRoutesThisMonth { get; set; }
        public double TotalDistance { get; set; }
        public double AverageSafetyScore { get; set; }
        public int TotalIncidents { get; set; }
        public int ResolvedIncidents { get; set; }
        public int PanicAlerts { get; set; }
        public int RiskZonesEncountered { get; set; }
        public double OnTimeDeliveryRate { get; set; }
    }

    public class CompanyRoute
    {
        public int RouteId { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EstimatedArrival { get; set; }
        public double DistanceKm { get; set; }
    }

    public class CompanyIncident
    {
        public int IncidentId { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string IncidentType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // DTOs
    public class CreateCompanyDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Province { get; set; } = string.Empty;

        [Required]
        public string PostalCode { get; set; } = string.Empty;

        public string Country { get; set; } = "South Africa";

        [Required]
        public string ContactPerson { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string ContactPhone { get; set; } = string.Empty;

        public string SubscriptionPlan { get; set; } = "Standard";
    }

    public class UpdateCompanyDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public bool? IsActive { get; set; }
    }

    public class UpdateSubscriptionDto
    {
        [Required]
        public string SubscriptionPlan { get; set; } = string.Empty;

        public DateTime? ExpiresAt { get; set; }
    }
}
