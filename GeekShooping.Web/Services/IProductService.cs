using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services
{
	public interface IProductService
	{
		Task<ProductViewModel> Create(ProductViewModel product, string token);

		Task<IEnumerable<ProductViewModel>> FindAll(string token);

		Task<ProductViewModel> FindById(long id, string token);

		Task<ProductViewModel> Update(ProductViewModel product, string token);

		Task<bool> Delete(long id, string token);
	}
}
