using GeekShopping.Product.API.Data.ValueObjects;

namespace GeekShopping.Product.API.Repositories
{
	public interface IProductRepository
	{
		Task<ProductVO> Create(ProductVO productVO);

		Task<IEnumerable<ProductVO>> FindAll();

		Task<IEnumerable<ProductVO>> FindById(long id);

		Task<ProductVO> Update(ProductVO productVO);

		Task<bool> Delete(long id);
	}
}
