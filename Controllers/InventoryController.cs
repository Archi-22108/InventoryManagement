using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
    public class InventoryController : Controller
    {
        string cs = ConfigurationManager
                    .ConnectionStrings["InventoryDB"]
                    .ConnectionString;

        // Display all products
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Products";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Product p = new Product();

                    p.ProductId = (int)reader["ProductId"];
                    p.ProductName = reader["ProductName"].ToString();
                    p.Category = reader["Category"].ToString();
                    p.Quantity = (int)reader["Quantity"];
                    p.Price = (decimal)reader["Price"];

                    products.Add(p);
                }

                reader.Close();
            }

            return View(products);
        }
    }
}