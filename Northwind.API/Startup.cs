﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.DataAccess.Abstract;
using Northwind.DataAccess.Concrete.EntityFramework;
using Swashbuckle.AspNetCore.Swagger;

namespace Northwind.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<IProductService, ProductManager>();
			services.AddScoped<IProductDal, EfProductDal>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "Northwind API", Version = "v1" });
			});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseCors(options => {
				options.AllowAnyHeader();
				options.AllowAnyOrigin();
			});

			app.UseHttpsRedirection();

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind API V1");
				c.RoutePrefix = string.Empty;
			});

			app.UseMvc();
		}
	}
}