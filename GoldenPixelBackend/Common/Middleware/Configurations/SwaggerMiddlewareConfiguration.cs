using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

//TODO: раскомментировать аутентификацию после добавления crm

namespace GoldenPixel.Common.Middleware.Configurations
{
	/// <summary>
	/// Класс для конфигурации
	/// Swagger Middleware
	/// </summary>
	public static class SwaggerMiddlewareConfigurations
	{
		private const string SWAGGER_API_VERSION = "v1";

		/// <summary>
		/// Конфигурирует <see cref="SwaggerGenOptions"/>
		/// </summary>
		/// <param name="options">Экземпляр опций</param>
		public static void ConfigureSwaggerGenOptions(SwaggerGenOptions options)
		{
			options.ConfigureSwaggerDocuments();
			options.ConfigureSwaggerDocumentation();
			//options.ConfigureSwaggerAuth();
		}

		/// <summary>
		/// Конфигурирует <see cref="SwaggerUIOptions"/>
		/// </summary>
		/// <param name="options">Экземпляр опций</param>
		public static void GetSwaggerUIOptions(SwaggerUIOptions options)
		{
			options.SwaggerEndpoint($"test/swagger.json", "Orders");
			options.RoutePrefix = "api/swagger";

			//раскомментировать, если при очень больших JSON упадет Swagger
			//options.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
			//{
			//    ["activated"] = false
			//};
		}

		/// <summary>
		/// Настройки генерации документации для <see cref="SwaggerGenOptions"/>
		/// </summary>
		/// <param name="options">Экземпляр опций</param>
		private static void ConfigureSwaggerDocumentation(this SwaggerGenOptions options)
		{
			string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			options.IncludeXmlComments(xmlPath, true);
		}

		/// <summary>
		/// Настройки объектов документов API для <see cref="SwaggerGenOptions"/>
		/// </summary>
		/// <param name="options">Экземпляр опций</param>
		private static void ConfigureSwaggerDocuments(this SwaggerGenOptions options)
		{
			var testInfo = new OpenApiInfo
			{
				Version = SWAGGER_API_VERSION,
				Title = "ORDERS API",
				Description = "Документ для тестового API",
			};

			options.SwaggerDoc("orders", testInfo);
		}

		/// <summary>
		/// Добавление возможности аутентификации
		/// </summary>
		//private static void ConfigureSwaggerAuth(this SwaggerGenOptions options)
		//{
		//	var securityScheme = new OpenApiSecurityScheme
		//	{
		//		Name = "JWT Аутентификация",
		//		Description = "Введите JWT Bearer token",
		//		In = ParameterLocation.Header,
		//		Type = SecuritySchemeType.Http,
		//		Scheme = "bearer",
		//		BearerFormat = "JWT",
		//		Reference = new OpenApiReference
		//		{
		//			Id = JwtBearerDefaults.AuthenticationScheme,
		//			Type = ReferenceType.SecurityScheme
		//		}
		//	};
		//	options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
		//	options.AddSecurityRequirement(new OpenApiSecurityRequirement
		//	{
		//		{securityScheme, new string[] { }}
		//	});
		//}
	}
}
