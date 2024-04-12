using CasaAPI.CustomAttributes;
using CasaAPI.Middlewares;
using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using CasaAPI.Models;
using CasaAPI.Services;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Repositories;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

//Add services to the container - Services related configurations
{
    services.AddControllers();
    services.AddHttpContextAccessor();
    services.AddSignalR();

    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    
    //JWT configuration
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

    //To validate parameters (Model State)
    services.Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory = (actionContext) =>
        {
            ResponseModel response = ModelStateHelper.GetValidationErrorsList(actionContext);
            return new BadRequestObjectResult(response);
        };
    });

    //Swagger configurations
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "CASA Panel Management API",
            Version = "v1",
        });

        options.OperationFilter<SwaggerCustomFilter>();
        options.SchemaFilter<SwaggerFormDataSchemaFilter>();

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    #region DI Setup
    services.AddSingleton<IFileManager, FileManager>();
    services.AddScoped<IJwtUtilsService, JwtUtilsService>();
    services.AddScoped<IProfileService, ProfileService>();
    services.AddScoped<IProfileRepository, ProfileRepository>();
    services.AddScoped<IAdminService, AdminService>();
    services.AddScoped<IAdminRepository, AdminRepository>();
    services.AddScoped<IManageTerritoryService, ManageTerritoryService>();
    services.AddScoped<IManageTerritoryRepository, ManageTerritoryRepository>();
    services.AddScoped<IDriverService, DriverService>();
    services.AddScoped<IDriverRepository, DriverRepository>();
    services.AddScoped<IVendorService, VendorService>();
    services.AddScoped<IVendorRepository, VendorRepository>();
    services.AddScoped<IDealerService, DealerService>();
    services.AddScoped<IDealerRepository, DealerRepository>();
    services.AddScoped<IVendorsService, VendorsService>();
    services.AddScoped<IVendorsRepository, VendorsRepository>();
    services.AddScoped<IPanelService, PanelService>();
    services.AddScoped<IPanelRepository, PanelRepository>();
    services.AddScoped<IPortfolioService, PortfolioService>();
    services.AddScoped<IPortfolioRepository, PortfolioRepository>();
    services.AddScoped<IOrderServices, OrderServices>();
    services.AddScoped<IOrderRepository, OrderRepository>();
    services.AddScoped<ICuttingPlanService, CuttingPlanService>();
    services.AddScoped<ICuttingPlanRepository, CuttingPlanRepository>();
    services.AddScoped<INotificationService, NotificationService>();
    services.AddScoped<INotificationRepository, NotificationRepository>();
    services.AddScoped<IBroadCastService, BroadCastService>();
    services.AddScoped<IBroadCastRepository, BroadCastRepository>();
    services.AddScoped<ICustomerService, CustomerService>();
    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<IManageDesignService, ManageDesignService>();
    services.AddScoped<IManageDesignRepository, ManageDesignRepository>();
    services.AddScoped<ILeaveService, LeaveService>();
    services.AddScoped<ILeaveRepository, LeaveRepository>();
    services.AddScoped<IVisitService, VisitService>();
    services.AddScoped<IVisitRepository, VisitRepository>();
    #endregion
}

var app = builder.Build();

//Web Application related configurations
{
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseMiddleware<JwtMiddleware>();

    //Global CORS policy - To disable CORS error
    app.UseCors(cors => cors
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    //app.UseHttpsRedirection();
    app.UseRouting();

    #region Swagger Configurations
    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "CASA Panel Management");
        //s.RoutePrefix = string.Empty;
    });
    //}
    #endregion

    //app.UseStaticFiles(); // For the wwwroot folder

    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Uploads")),
        RequestPath = new PathString("/Uploads")
    });

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHub<MessageHubClient>("/notification");
}

app.Run();
