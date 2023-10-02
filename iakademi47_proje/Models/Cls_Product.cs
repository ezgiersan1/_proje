using Microsoft.EntityFrameworkCore;


namespace iakademi47_proje.Models
{
	public class Cls_Product
	{
		iakademi47Context context = new iakademi47Context();

		public async Task<List<Product>> ProductSelect()
		{
			List<Product> products = await context.Products.ToListAsync();
			return products;
		}


		public static string ProductInsert(Product product)
		{
			//metod static oldugu icin
			using (iakademi47Context context = new iakademi47Context())
			{
				try
				{
					Product pro = context.Products.FirstOrDefault(c => c.ProductName.ToLower() == product.ProductName.ToLower());

					if (pro == null)
					{
						product.AddDate = DateTime.Now;
						context.Add(product);
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



		public List<Product> ProductSelect(string mainPageName,string subPageName, int pageNumber)
		{
			List<Product> products;
			
			if (mainPageName == "slider")
			{
				products = context.Products.Where(p => p.StatusID == 1).Take(8).ToList();
			}
			else if(mainPageName == "new")
			{
				if (subPageName == "")
				{
					//home/index
					//select top 8 * from Products order by AddDate desc -- ado.net
					//entityframeworkcore -- products = context.Products.OrderByDescending(p => p.AddDate).Take(8).ToList();
					//proje açılışı = .Net core (önceki versiyon .Net Framework)
					products = context.Products.OrderByDescending(p => p.AddDate).Take(8).ToList();
				}
				else
				{
					if (pageNumber == 0)
					{
						//alt sayfa
						products = context.Products.OrderByDescending(p => p.AddDate).Take(4).ToList();
					}
					else
					{
						//alt sayfa AJAX 
						products = context.Products.OrderByDescending(p => p.AddDate).Skip(4*pageNumber).Take(4).ToList();
					}
				}

			}
			else if (mainPageName == "Special")
			{
				if (subPageName == "")
				{
					//Home/Index
					products = context.Products.Where(p => p.StatusID == 3).Take(8).ToList();
				}
				else
				{
					if (pageNumber == 0)
					{
						//alt sayfa
						products = context.Products.Where(p => p.StatusID == 3).Take(4).ToList();
					}
					else
					{
						//alt sayfa AJAX
						products = context.Products.Where(p => p.StatusID == 3).Skip(4 * pageNumber).Take(4).ToList();
					}
				}

			}
			else if (mainPageName == "Discounted")
			{
				if (subPageName == "")
				{
				
					products = context.Products.OrderByDescending(p => p.Discount).Take(8).ToList();
				}
				else
				{
					if (pageNumber == 0)
					{
						
						products = context.Products.OrderByDescending(p => p.Discount).Take(4).ToList();
					}
					else
					{
						
						products = context.Products.OrderByDescending(p => p.Discount).Skip(4 * pageNumber).Take(4).ToList();
					}
				}

			}
            else if (mainPageName == "Highlighted")
            {
                if (subPageName == "")
                {

                    products = context.Products.OrderByDescending(p => p.Highlighted).Take(8).ToList();
                }
                else
                {
                    if (pageNumber == 0)
                    {

                        products = context.Products.OrderByDescending(p => p.Highlighted).Take(4).ToList();
                    }
                    else
                    {

                        products = context.Products.OrderByDescending(p => p.Highlighted).Skip(4 * pageNumber).Take(4).ToList();
                    }
                }

            }
            else if (mainPageName == "TopSeller")
			{
				products = context.Products.OrderByDescending(p => p.TopSeller).Take(8).ToList();
			}
			else if (mainPageName == "Star")
			{
				products = context.Products.Where(p => p.StatusID == 4).OrderBy(p => p.ProductName).Take(8).ToList();
			}
			else if (mainPageName == "Featured")
			{
				products = context.Products.Where(p => p.StatusID == 5).OrderBy(p => p.ProductName).Take(8).ToList();
			}
			else 
			{
				products = context.Products.Where(p => p.StatusID == 6).OrderBy(p => p.ProductName).Take(8).ToList();
			}
			return products;
		}

		public Product ProductDetails(string v)
		{
			Product product = context.Products.FirstOrDefault(p => p.StatusID == 2);
			return product;
		}


		public static void Highlighted_Increase(int id)
		{
			using (iakademi47Context context = new iakademi47Context())
			{
				Product? product = context.Products.FirstOrDefault(p => p.ProductID == id);

				product.Highlighted += 1;
				context.Update(product);
				context.SaveChanges();
			}
		}


	}
}
