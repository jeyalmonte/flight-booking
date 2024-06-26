﻿using Api.Infrastructure;
using Microsoft.OpenApi.Models;

namespace Api;

public static class DependencyInjection
{
	public static IServiceCollection AddPresentation(this IServiceCollection services)
	{
		services.AddRouting(options => options.LowercaseUrls = true);

		services.AddControllers()
			.ConfigureApiBehaviorOptions(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

		services.AddSwaggerGen(c =>
		{
			c.EnableAnnotations();
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "ProjectName",
				Description = "ProjectName API Description.",
				Version = "v1"
			});
		});

		services.AddExceptionHandler<GlobalExceptionHandler>();
		services.AddProblemDetails();

		return services;
	}
}
