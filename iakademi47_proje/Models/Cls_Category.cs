using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace iakademi47_proje.Models
{
	public class Cls_Category
	{
		iakademi47Context context = new iakademi47Context();

		public async Task<List<Category>> CategorySelect()
		{
			List<Category> categories = await context.Categories.ToListAsync();
			return categories;
		}

		public List<Category> CategorySelectMain()
		{
			List<Category> categories = context.Categories.Where(c => c.ParentID == 0).ToList();
			return categories;
		}

		public static string CategoryInsert(Category category)
		{
			//metod static olduğu için
			using (iakademi47Context context = new iakademi47Context())
			{
				try
				{
					Category cat = context.Categories.FirstOrDefault(c => c.CategoryName.ToLower() == category.CategoryName.ToLower());

					if (cat == null)
					{
						context.Add(category);
						context.SaveChanges();
						return "başarılı";
					}
					else
					{
						return "zaten var";
					}
				}
				catch (Exception)
				{

					return "başarısız";
				}
			}
		}

		public async Task<Category> CategoryDetails(int? id)
		{
			//gelen idye ait category bildiği
			//select * from Categories where CategoryID = id ado.net
			//entityframework core 
			//tip degiskenadi = sorgu


			//string? CategoryName = context.Categories.FirstOrDefault(c => c.CategoryID == id).CategoryName;
			//int ParentID = sorgu


			//bir kaydın bütün kolonları
			//Category category = sorgu
			//Category category = await context.Categories.FindAsync(id);
			//Category category1 = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);

			//bütün kategorileri 
			//List<Category> categories = sorgu 

			//ORM = ado.net, entityframework core
			// List<Category> categories = sorgu

			//List<Category> categories = context.Categories.ToList();
			//List<Category> categories2 = context.Categories.Where(c => c.fiyat > 20).ToList();

			Category category = await context.Categories.FindAsync(id);
			return category;
		}

		//bool asnwer = Cls_Category.CategoryUpdate(category);
		//public + static var ya da yok + geri dönüş tipi +
		public static bool CategoryUpdate(Category category)
		{
			//metod static olduğu için
			using (iakademi47Context context = new iakademi47Context())
			{
				try
				{
					var existing = context.Set<Category>().Find(category.CategoryID);
					if (existing != null)
					{
						context.Entry(existing).CurrentValues.SetValues(category);
						context.SaveChanges();
					}

					return true;
				}
				catch (Exception)
				{

					return false;
				}
			}
		}

		public static bool CategoryDelete(int id)
		{
			try
			{
				using (iakademi47Context context = new iakademi47Context())
				{
					Category category = context.Categories.FirstOrDefault(c => c.CategoryID == id);

					List<Category> categories = context.Categories.Where(c => c.ParentID == id).ToList();
					//silinecek olan ana kategoriye ait alt kategoriler varsa onları da pasif yapıyoruz
					foreach (var item in categories)
					{
						item.Active = false;
					}

					context.SaveChanges();
					return true;
				}
			}
			catch (Exception)
			{

				return false;
			}
		}
	}
}
