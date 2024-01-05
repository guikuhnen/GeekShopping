using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services
{
	public interface IProductService
	{
		Task<ProductModel> Create(ProductModel product, string token);

		Task<IEnumerable<ProductModel>> FindAll(string token);

		Task<ProductModel> FindById(long id, string token);

		Task<ProductModel> Update(ProductModel product, string token);

		Task<bool> Delete(long id, string token);
	}
}
