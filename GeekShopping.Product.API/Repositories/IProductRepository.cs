using GeekShopping.ProductAPI.Data.ValueObjects;

namespace GeekShopping.ProductAPI.Repositories
{
	public interface IProductRepository
	{
		Task<ProductVO> Create(ProductVO productVO);

		Task<IEnumerable<ProductVO>> FindAll();

		Task<ProductVO> FindById(long id);

		Task<ProductVO> Update(ProductVO productVO);

		Task<bool> Delete(long id);
	}
}
