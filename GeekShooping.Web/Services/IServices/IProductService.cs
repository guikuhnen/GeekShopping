using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices
{
	public interface IProductService
	{
		Task<ProductModel> Create(ProductModel product);

		Task<IEnumerable<ProductModel>> FindAll();

		Task<ProductModel> FindById(long id);

		Task<ProductModel> Update(ProductModel product);

		Task<bool> Delete(long id);
	}
}
