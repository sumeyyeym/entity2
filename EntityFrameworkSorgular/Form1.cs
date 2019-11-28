using System;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Windows.Forms;

namespace EntityFrameworkSorgular
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        NorthwindEntities db = new NorthwindEntities();

        private void btnOrnek1_Click(object sender, EventArgs e)
        {
            #region Soru
            // Fiyatı 20 ile 50 arasında olan ürünlerin Id,Ürün Adı, Fiyatı, Stok Miktarını ve Kategorisini getiren sorgu 
            #endregion


            // Linq To Entity

            //dataGridView1.DataSource = db.Products
            //    .Where(x => x.UnitPrice >= 20 && x.UnitPrice <= 50)
            //    .Select(x => new
            //    {
            //        UrunNo = x.ProductID,
            //        UrunAdi = x.ProductName,
            //        StokAdet = x.UnitsInStock,
            //        Fiyat = x.UnitPrice
            //    })
            //    .OrderBy(x => x.Fiyat) // Fiyat bilgisine göre artan sırada (fakirden => zengine)
            //    //.OrderByDescending(x => x.StokAdet) => stok adedine göre azalan sırada  (zenginden => züğürte)
            //    .ToList();

            // Linq To Sql 


            var result = from p in db.Products
                         where p.UnitPrice >= 20 && p.UnitPrice <= 50
                         orderby p.UnitPrice ascending
                         select new
                         {
                             UrunNo = p.ProductID,
                             UrunAdi = p.ProductName,
                             StokAdet = p.UnitsInStock,
                             Fiyat = p.UnitPrice
                         };


            dataGridView1.DataSource = result.ToList();

        }

        private void btnOrnek2_Click(object sender, EventArgs e)
        {
            #region Soru
            // Siparişler tablosundan MusteriSirketAdi, CalisanAdiSoyadi, SiparisId, SiparisTarihi ve KargoSirketiAdi getiren sorgu 
            #endregion

            // lazy loading

            //dataGridView1.DataSource = db.Orders.Select(x => new
            //{
            //    Musteri        = x.Customer.CompanyName,
            //    Personel       = x.Employee.FirstName + " " + x.Employee.LastName,
            //    SiparisNo      = x.OrderID,
            //    SiparisTarihi  = x.OrderDate,
            //    KargoAdi       = x.Shipper.CompanyName
            //}).ToList();


            var result = from x in db.Orders
                         select new
                         {
                             Musteri = x.Customer.CompanyName,
                             Personel = x.Employee.FirstName + " " + x.Employee.LastName,
                             SiparisNo = x.OrderID,
                             SiparisTarihi = x.OrderDate,
                             KargoAdi = x.Shipper.CompanyName
                         };

            dataGridView1.DataSource = result.ToList();

        }

        private void btnOrnek3_Click(object sender, EventArgs e)
        {
            #region Sorgu
            // CompanyName içerisinde restaurant geçen müşterilerin listelenmesi 
            #endregion


            //dataGridView1.DataSource = db.Customers.Where(x => x.CompanyName.Contains("restaurant")).ToList();


            var result = from c in db.Customers
                         where c.CompanyName.Contains("restaurant")
                         select c;

            dataGridView1.DataSource = result.ToList();
        }

        private void btnOrnek4_Click(object sender, EventArgs e)
        {
            #region Soru 
            // Kategorisi Beverages olan ve UrunAdi:Kola, Fiyati:5.00, StokAdet:500 olan ürün ekleyiniz.

            //Category category = db.Categories.FirstOrDefault(x => x.CategoryName == "Beverages");
            //Product product = new Product();
            //product.ProductName = "Kola 1";
            //product.UnitPrice = 5.00M;
            //product.UnitsInStock = 500;
            //product.CategoryID = category.CategoryID;

            //db.Products.Add(product);
            //db.SaveChanges();


            db.Categories.FirstOrDefault(x => x.CategoryName == "Beverages").Products.Add(new Product
            {
                ProductName = "Kola 2",
                UnitPrice = 5.00M,
                UnitsInStock = 500
            });
            db.SaveChanges();




            dataGridView1.DataSource = db.Products
               .Where(x => x.ProductName.StartsWith("Kola"))
                .Select(x => new
                {
                    x.ProductName,
                    x.UnitPrice,
                    x.UnitsInStock,
                    x.Category.CategoryName
                }).ToList();

            #endregion

        }

        private void btnOrnek5_Click(object sender, EventArgs e)
        {
            #region Soru
            // Çalışanların Adını, Soyadını, Doğum Tarihini ve Yaşını Getiren Sorgu Yazınız 

            #endregion

            //dataGridView1.DataSource = db.Employees.Select(x => new
            //{
            //    x.FirstName,
            //    x.LastName,
            //    x.BirthDate,
            //    Age = DateTime.Now.Year - x.BirthDate.Value.Year,
            //    Yas = SqlFunctions.DateDiff("Year", x.BirthDate, DateTime.Now)
            //}).ToList();

            var result = from x in db.Employees
                         select new
                         {
                             x.FirstName,
                             x.LastName,
                             x.BirthDate,
                             Age = DateTime.Now.Year - x.BirthDate.Value.Year,
                             Yas = SqlFunctions.DateDiff("Year", x.BirthDate, DateTime.Now)
                         };

            dataGridView1.DataSource = result.ToList();

            // using System.Data.Entity.SqlServer;   => SqlFunctions.DateDiff
        }

        private void btnOrnek6_Click(object sender, EventArgs e)
        {
            #region Soru
            // Kategorilerine stoktaki ürün sayısını veren sorgu   
            #endregion

            //dataGridView1.DataSource = db.Products
            //    .GroupBy(x => new
            //    {
            //        x.CategoryID,
            //        x.Category.CategoryName,
            //        x.Category.Description
            //    })
            //    .Select(x => new
            //    {
            //        x.Key.CategoryID,
            //        x.Key.CategoryName,
            //        x.Key.Description,
            //        Toplam = x.Sum(s => s.UnitsInStock)
            //    }).ToList();


            var result = from p in db.Products
                         group p by new
                         {
                             p.CategoryID,
                             p.Category.CategoryName,
                             p.Category.Description
                         } into g
                         select new
                         {
                             g.Key.CategoryID, 
                             g.Key.CategoryName,
                             g.Key.Description,
                             Toplam = g.Sum(p => p.UnitsInStock)
                         };

            dataGridView1.DataSource = result.ToList(); 
        }
    }
}
