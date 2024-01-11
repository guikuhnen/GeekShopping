using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Models.Context;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace GeekShopping.CartAPI.Repositories
{
	public class CouponRepository : ICouponRepository
	{
		public readonly HttpClient _client;

		public CouponRepository(HttpClient client)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<CouponVO> GetCouponByCouponCode(string couponCode, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.GetAsync($"/api/v1/coupon/{couponCode}");

			var content = await response.Content.ReadAsStringAsync();

			if (response.StatusCode != HttpStatusCode.OK)
				return new CouponVO() { CouponCode = string.Empty };

			return JsonSerializer.Deserialize<CouponVO>(content, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
		}
	}
}
