using Basket.Data.CacheSerialization;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Data.Repository;

public class CachedBasketRepository(
    IBasketRepository repository,
    IDistributedCache cache
    ) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (!asNoTracking)
        {
            return await repository.GetBasketAsync(userName, false, cancellationToken);
        }

        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            var basketJsonDtoResult = JsonSerializer.Deserialize<ShoppingCartJsonDto>(cachedBasket)!;
            return basketJsonDtoResult.ToDomain();
        }

        var basket = await repository.GetBasketAsync(userName, asNoTracking, cancellationToken);
        var basketJsonDto = ShoppingCartJsonDto.ToJsonDto(basket);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basketJsonDto), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> CreateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.CreateBasketAsync(basket, cancellationToken);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteBasketAsync(userName, cancellationToken);
        await cache.RemoveAsync(userName, cancellationToken);

        return result;
    }

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        var result = await repository.SaveChangesAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrEmpty(userName))
        {
            await cache.RemoveAsync(userName, cancellationToken);
        }

        return result;
    }
}
