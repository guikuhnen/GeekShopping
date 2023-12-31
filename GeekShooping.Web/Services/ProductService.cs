﻿using GeekShopping.Web.Models;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services
{
	public class ProductService : IProductService
	{
		public readonly HttpClient _client;
		public const string BasePath = "api/v1/product";

		public ProductService(HttpClient client)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<ProductViewModel> Create(ProductViewModel product, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.PostAsJson(BasePath, product);

			if (response.IsSuccessStatusCode)
				return await response.ReadContentAs<ProductViewModel>();
			else
				throw new Exception($"Something went wrong calling the API: " + $"{response.ReasonPhrase}");
		}

		public async Task<IEnumerable<ProductViewModel>> FindAll(string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.GetAsync(BasePath);

			return await response.ReadContentAs<List<ProductViewModel>>();
		}

		public async Task<ProductViewModel> FindById(long id, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.GetAsync($"{BasePath}/{id}");

			return await response.ReadContentAs<ProductViewModel>();
		}

		public async Task<ProductViewModel> Update(ProductViewModel product, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.PutAsJson(BasePath, product);

			if (response.IsSuccessStatusCode)
				return await response.ReadContentAs<ProductViewModel>();
			else
				throw new Exception($"Something went wrong calling the API: " + $"{response.ReasonPhrase}");
		}

		public async Task<bool> Delete(long id, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.DeleteAsync($"{BasePath}/{id}");

			if (response.IsSuccessStatusCode)
				return await response.ReadContentAs<bool>();
			else
				throw new Exception($"Something went wrong calling the API: " + $"{response.ReasonPhrase}");
		}
	}
}
