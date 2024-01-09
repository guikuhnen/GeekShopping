using GeekShopping.Web.Services;
using Microsoft.AspNetCore.Authentication;

namespace GeekShopping.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient<IProductService, ProductService>(p =>
                p.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));

            builder.Services.AddHttpClient<ICartService, CartService>(p =>
                p.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CartAPI"]));

            builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
                .AddOpenIdConnect("oidc", o =>
                {
                    o.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
                    o.GetClaimsFromUserInfoEndpoint = true;
                    o.ClientId = "geek_shopping";
                    o.ClientSecret = "my_super_secret";
                    o.ResponseType = "code";
                    o.ClaimActions.MapJsonKey("role", "role", "role");
                    o.ClaimActions.MapJsonKey("sub", "sub", "sub");
                    o.TokenValidationParameters.NameClaimType = "name";
                    o.TokenValidationParameters.RoleClaimType = "role";
                    o.Scope.Add("geek_shopping");
                    o.SaveTokens = true;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
