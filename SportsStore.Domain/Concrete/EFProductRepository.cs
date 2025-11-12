using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
	public class EFProductRepository : IProductRepository
	{
		private EFDbContext context;

		public EFProductRepository(EFDbContext _context)
		{
			context = _context;
		}

		public IQueryable<Product> Products
		{
			get {  return context.Products; }
		}

		public void Save(Product product)
		{
			if (product.ProductID == 0)
			{
				context.Products.Add(product);
			}
			else
			{
				var dbEntry = context.Products.Find(product.ProductID);

				if (dbEntry != null)
				{
					dbEntry.Name = product.Name;
					dbEntry.Description = product.Description;
					dbEntry.Price = product.Price;
					dbEntry.Category = product.Category;
					dbEntry.ImageData = product.ImageData;
					dbEntry.ImageMimeType = product.ImageMimeType;
				}
			}

			context.SaveChanges();
		}

		public Product Delete(int productID)
		{
			var dbEntry = context.Products.Find(productID);

			if (dbEntry != null)
			{
				context.Products.Remove(dbEntry);
				context.SaveChanges();
			}

			return dbEntry;
		}
	}
}
