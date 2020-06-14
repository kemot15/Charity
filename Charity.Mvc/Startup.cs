﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Charity.Mvc.Context;
using Charity.Mvc.Models.Db;
using Charity.Mvc.Services;
using Charity.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Charity.Mvc
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
            services.AddDbContext<CharityDonationContext>(builder => builder.UseSqlServer(Configuration.GetConnectionString("SQL")));
            services.AddIdentity<User, IdentityRole<int>>().AddEntityFrameworkStores<CharityDonationContext>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
			{
				options.User.RequireUniqueEmail = true;
			});

			services.AddScoped<IInstitutionService, InstitutionService>();
			services.AddScoped<IDonationService, DonationService>();


			services.AddControllersWithViews();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();


			//app.SeedAdminUser();

			app.UseAuthentication();
			app.UseRouting();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}