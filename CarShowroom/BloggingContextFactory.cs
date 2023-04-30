using CarShowroom.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarShowroom
{
    public class BloggingContextFactory : IDesignTimeDbContextFactory<CarShowroomContext>
    {
        public CarShowroomContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarShowroomContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-C7ALLTO;Database=CarShowroom;Integrated Security=True");

            return new CarShowroomContext(optionsBuilder.Options);
        }
    }
}
