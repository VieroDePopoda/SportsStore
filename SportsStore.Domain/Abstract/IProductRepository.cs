using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Abstract
{
	public interface IProductRepository
	{
		IQueryable<Product> Products { get; }

		void Save(Product product);

		Product Delete(int productID);
	}
}
