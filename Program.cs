using CRM.Helper;

namespace CRM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAppServices(builder);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "Cors",
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin().AllowAnyHeader();
                                  });
            });

            var app = builder.Build();

            await DataSeed.InitializeAsync(app.Services);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("Cors");

            app.MapControllers();

            app.Run();
        }
    }
}
