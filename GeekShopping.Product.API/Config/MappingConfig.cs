using AutoMapper;
using GeekShopping.Product.API.Data.ValueObjects;

namespace GeekShopping.Product.API.Config
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<ProductVO, Models.Product>();
				config.CreateMap<Models.Product, ProductVO>();
			});

			return mappingConfig;
		}
	}
}
