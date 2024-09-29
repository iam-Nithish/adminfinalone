using Myntra.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Myntra.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
     
        // Details action method to fetch and display product details
       //display
            public ActionResult ProductList()
            {
                List<Product> products = new List<Product>();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyntraEntities"].ConnectionString))
                {
                    string query = "SELECT * FROM Products";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductPrice = reader["ProductPrice"] as decimal?,
                            ProductDescription = reader["ProductDescription"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            Category = reader["Category"].ToString(),
                            Status = reader["Status"].ToString(),
                            Color = reader["Color"].ToString(),
                            Brand = reader["Brand"].ToString()
                        };
                        products.Add(product);
                    }
                }

                return View(products);
            }


            ///Create
            public ActionResult AddProducs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProducs(Product product, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Images/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                product.ProductImage = "~/Images/" + file.FileName;
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyntraEntities"].ConnectionString))
            {
                string query = "INSERT INTO Products (ProductName, ProductPrice, ProductDescription, ProductImage, Category, Status, Color, Brand) VALUES (@Name, @Price, @Desc, @Image, @Category, @Status, @Color, @Brand)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.ProductPrice);
                cmd.Parameters.AddWithValue("@Desc", product.ProductDescription);
                cmd.Parameters.AddWithValue("@Image", product.ProductImage);
                cmd.Parameters.AddWithValue("@Category", product.Category); // New field
                cmd.Parameters.AddWithValue("@Status", product.Status);     // New field
                cmd.Parameters.AddWithValue("@Color", product.Color);       // New field
                cmd.Parameters.AddWithValue("@Brand", product.Brand);       // New field

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return RedirectToAction("AddProducs");
        }
        //edit
        [HttpGet]
        public ActionResult UpdateProducts(int id)
        {
            Product product = null;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyntraEntities"].ConnectionString))
            {
                string query = "SELECT * FROM Products WHERE ProductID = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    product = new Product
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductPrice = reader["ProductPrice"] as decimal?,
                        ProductDescription = reader["ProductDescription"].ToString(),
                        ProductImage = reader["ProductImage"].ToString(),
                        Category = reader["Category"].ToString(),
                        Status = reader["Status"].ToString(),
                        Color = reader["Color"].ToString(),
                        Brand = reader["Brand"].ToString()
                    };
                }
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult UpdateProducts(Product product, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Images/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                product.ProductImage = "~/Images/" + file.FileName;
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyntraEntities"].ConnectionString))
            {
                string query = "UPDATE Products SET ProductName = @Name, ProductPrice = @Price, ProductDescription = @Desc, ProductImage = @Image, Category = @Category, Status = @Status, Color = @Color, Brand = @Brand WHERE ProductID = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", product.ProductID);
                cmd.Parameters.AddWithValue("@Name", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.ProductPrice);
                cmd.Parameters.AddWithValue("@Desc", product.ProductDescription);
                cmd.Parameters.AddWithValue("@Image", product.ProductImage);
                cmd.Parameters.AddWithValue("@Category", product.Category);
                cmd.Parameters.AddWithValue("@Status", product.Status);
                cmd.Parameters.AddWithValue("@Color", product.Color);
                cmd.Parameters.AddWithValue("@Brand", product.Brand);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return RedirectToAction("ProductList");
        }
    







public ActionResult MyProfile()
            {
            return View();
            }
    } 
}