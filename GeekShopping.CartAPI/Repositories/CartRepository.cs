using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Models;
using GeekShopping.CartAPI.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repositories
{
	public class CartRepository : ICartRepository
	{
		private readonly MySQLContext _context;
		private IMapper _mapper;

		public CartRepository(MySQLContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<CartVO> SaveOrUpdateCart(CartVO cartVO)
		{
			Cart cart = _mapper.Map<Cart>(cartVO);

			//Checks if the product is already saved in the database if it does not exist then save
			var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cartVO.CartDetails.FirstOrDefault().ProductId);

			if (product == null)
			{
				_context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
				await _context.SaveChangesAsync();
			}

			//Check if CartHeader is null
			var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

			if (cartHeader == null)
			{
				//Create CartHeader and CartDetails
				_context.CartHeaders.Add(cart.CartHeader);
				await _context.SaveChangesAsync();

				cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
				cart.CartDetails.FirstOrDefault().Product = null;

				_context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
				await _context.SaveChangesAsync();
			}
			else
			{
				//If CartHeader is not null
				//Check if CartDetails has same product
				var cartDetail = await _context.CartDetails.AsNoTracking()
					.FirstOrDefaultAsync(p => p.ProductId == cartVO.CartDetails.FirstOrDefault().ProductId
						&& p.CartHeaderId == cartHeader.Id);

				if (cartDetail == null)
				{
					//Create CartDetails
					cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
					cart.CartDetails.FirstOrDefault().Product = null;

					_context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
					await _context.SaveChangesAsync();
				}
				else
				{
					//Update product count and CartDetails
					cart.CartDetails.FirstOrDefault().Product = null;
					cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;
					cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
					cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;

					_context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
					await _context.SaveChangesAsync();
				}
			}

			return _mapper.Map<CartVO>(cart);
		}

		public async Task<CartVO> FindCartByUserId(string userId)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> ApplyCoupon(string userId, string couponCode)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> RemoveCoupon(string userId)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> RemoveFromCart(long cartDetailsId)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> ClearCart(string userId)
		{
			throw new NotImplementedException();
		}
	}
}
