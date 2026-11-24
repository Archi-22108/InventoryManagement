using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
    public class ProductController : Controller
    {
        string cs = ConfigurationManager
                    .ConnectionStrings["InventoryDB"]
                    .ConnectionString;

        // READ: Display all products
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Products";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Product p = new Product();

                    p.ProductId = (int)dr["ProductId"];
                    p.ProductName = dr["ProductName"].ToString();
                    p.Category = dr["Category"].ToString();
                    p.Quantity = (int)dr["Quantity"];
                    p.Price = (decimal)dr["Price"];

                    products.Add(p);
                }

                dr.Close();
            }

            return View(products);
        }

        // CREATE: Display form
        public ActionResult Create()
        {
            return View();
        }

        // CREATE: Save product
        [HttpPost]
        public ActionResult Create(Product p)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Products
                    (ProductName, Category, Quantity, Price)
                    VALUES (@ProductName, @Category, @Quantity, @Price)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Category", p.Category);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // UPDATE: Display existing product
        public ActionResult Edit(int id)
        {
            Product p = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Products WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    p = new Product();

                    p.ProductId = (int)dr["ProductId"];
                    p.ProductName = dr["ProductName"].ToString();
                    p.Category = dr["Category"].ToString();
                    p.Quantity = (int)dr["Quantity"];
                    p.Price = (decimal)dr["Price"];
                }

                dr.Close();
            }

            if (p == null)
                return HttpNotFound();

            return View(p);
        }

        // UPDATE: Save changes
        [HttpPost]
        public ActionResult Edit(Product p)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE Products
                    SET ProductName = @ProductName,
                        Category = @Category,
                        Quantity = @Quantity,
                        Price = @Price
                    WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@ProductId", p.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Category", p.Category);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // DELETE: Remove product
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Products WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}