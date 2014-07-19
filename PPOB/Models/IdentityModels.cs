using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;

namespace PPOB.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class PPOBContext : IdentityDbContext<ApplicationUser>
    {
        public PPOBContext()
            : base("PPOB")
        {
        }

        public static PPOBContext Create()
        {
            return new PPOBContext();
        }
        public DbSet<City> City { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ClientCity> ClientCity { get; set; }
        public DbSet<ClientDocument> ClientDocument { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeEducation> EmployeeEducation { get; set; }
        public DbSet<EmployeeEnterDoc> EmployeeEnterDoc { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }

    public class PPOBInitializer : DropCreateDatabaseIfModelChanges<PPOBContext>
    {
        protected override void Seed(PPOBContext db)
        {
            var cities = new List<City>
                        { 
                            new City { Name = "北京" },
                            new City { Name = "上海" },
                            new City { Name = "广州" },
                        };
            foreach (var item in cities)
            {
                db.City.Add(item);
            }

            var clients = new List<Client>
                        { 
                            new Client { Name = "Sony" },
                            new Client { Name = "花旗" },
                            new Client { Name = "第一资讯" },
                        };
            foreach (var item in clients)
            {
                db.Client.Add(item);
            }

            var clientDocuments = new List<ClientDocument>
                        { 
                            new ClientDocument { ClientId = 1, Name = "身份证" },
                            new ClientDocument { ClientId = 1, Name = "户口簿" },
                            new ClientDocument { ClientId = 1, Name = "健康证" },
                            new ClientDocument { ClientId = 1, Name = "2寸照" },
                            new ClientDocument { ClientId = 2, Name = "身份证" },
                            new ClientDocument { ClientId = 2, Name = "健康证" },
                            new ClientDocument { ClientId = 3, Name = "身份证" },
                            new ClientDocument { ClientId = 3, Name = "户口簿" },
                        };
            foreach (var item in clientDocuments)
            {
                db.ClientDocument.Add(item);
            }

            var clientCities = new List<ClientCity>
                        { 
                            new ClientCity { ClientId = 1, CityId = null, ClientEnterDocuments = new List<ClientDocument>{ clientDocuments[0], clientDocuments[1]} },
                            new ClientCity { ClientId = 1, CityId = 1, ClientEnterDocuments = new List<ClientDocument>{ clientDocuments[0], clientDocuments[1], clientDocuments[2], clientDocuments[3] } },
                            new ClientCity { ClientId = 1, CityId = 2, ClientEnterDocuments = new List<ClientDocument>{ clientDocuments[0], clientDocuments[1] } },
                            new ClientCity { ClientId = 1, CityId = 3, ClientEnterDocuments = new List<ClientDocument>{ clientDocuments[0], clientDocuments[1] } },
                            new ClientCity { ClientId = 2, CityId = null, ClientEnterDocuments = new List<ClientDocument>{ clientDocuments[4] } },
                            new ClientCity { ClientId = 2, CityId = 1, ClientEnterDocuments = new List<ClientDocument>{ clientDocuments[4], clientDocuments[5] } },
                            new ClientCity { ClientId = 2, CityId = 2, ClientEnterDocuments = new List<ClientDocument>{ clientDocuments[4], clientDocuments[5] } },
                            new ClientCity { ClientId = 2, CityId = 3, ClientEnterDocuments = new List<ClientDocument>{ clientDocuments[4], clientDocuments[5] } },
                        };
            foreach (var item in clientCities)
            {
                db.ClientCity.Add(item);
            }
            db.SaveChanges();
        }
    }
}