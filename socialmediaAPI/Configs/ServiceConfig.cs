using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace socialmediaAPI.Configs
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddServicesConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSignalR();
            services.ConfigDbContext(config);
            services.ConfigRepositories(config);
            services.ConfigAuthentication(config);
            services.ConfigDI(config);

            return services;
        }
        public static IServiceCollection ConfigDbContext(this IServiceCollection services, IConfiguration config)
        {
            DatabaseConfigs databaseConfigs = new DatabaseConfigs();
            config.Bind(databaseConfigs);
            databaseConfigs.SetupDatabase();
            services.AddSingleton(databaseConfigs);
            return services;
        }
        public static IServiceCollection ConfigRepositories(this IServiceCollection services, IConfiguration config)
        {
            //services.AddSingleton<IConversationRepository, ConversationRepository>();
            //services.AddSingleton<IMessageLogRepository, MessageLogRepository>();
            //services.AddSingleton<IUserRepository, UserRepository>();
            //services.AddSingleton<IPostRepository, PostRepository>();
            //services.AddSingleton<ICommentLogRepository, CommentLogRepository>();
            return services;
        }
        public static IServiceCollection ConfigAuthentication(this IServiceCollection services, IConfiguration config)
        {
            //TokenConfiguration tokenConfiguration = new TokenConfiguration();
            //config.Bind("TokenConfiguration", tokenConfiguration);
            //services.AddSingleton(tokenConfiguration);
            //services.AddSingleton<TokenGenerator>();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.AccessTokenSecret)),
            //        ValidIssuer = tokenConfiguration.Issuer,
            //        ValidAudience = tokenConfiguration.Audience,
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateIssuerSigningKey = true,
            //        ClockSkew = TimeSpan.FromMinutes(6)
            //    };
            //});
            return services;
        }
        public static IServiceCollection ConfigDI(this IServiceCollection services, IConfiguration config)
        {
            //Cloudinary
            //CloudinaryConfigs cloudinarySettings = new CloudinaryConfigs();
            //config.Bind("CloudinarySettings", cloudinarySettings);
            //services.AddSingleton(cloudinarySettings);
            //services.AddSingleton<CloudinaryHandler>();
            return services;
        }
    }
}
