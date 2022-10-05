using Kitchen.Kitchen;
using Kitchen.Repositories.CookingApparatusRepository;
using Kitchen.Repositories.CookRepository;
using Kitchen.Repositories.FoodRepository;
using Kitchen.Repositories.GenericRepository;
using Kitchen.Repositories.OrderHistoryRepository;
using Kitchen.Repositories.OrderRepository;
using Kitchen.Services.CookingApparatusServices;
using Kitchen.Services.CookService;
using Kitchen.Services.FoodService;
using Kitchen.Services.OrderHistoryService;
using Kitchen.Services.OrderService;

namespace Kitchen;

public class Startup
{
    private IConfiguration ConfigRoot { get; }

    public Startup(IConfiguration configuration)
    {
        ConfigRoot = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddRazorPages();
        services.AddLogging(config => config.ClearProviders());

        services.AddSingleton<ICookingApparatusRepository, CookingApparatusRepository>();
        services.AddSingleton<ICookRepository, CookRepository>();
        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddSingleton<IFoodRepository, FoodRepository>();
        services.AddSingleton<IOrderHistoryRepository, OrderHistoryRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddSingleton<ICookingApparatusServices, CookingApparatusServices>();
        services.AddSingleton<ICookService, CookService>();
        services.AddSingleton<IOrderService, OrderService>();
        services.AddSingleton<IFoodService, FoodService>();
        services.AddSingleton<IOrderHistoryService, OrderHistoryService>();

        services.AddSingleton<IKitchen, Kitchen.Kitchen>();
        services.AddHostedService<BackgroundTask.BackgroundTask>();
    }

    public static void Configure(WebApplication app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}