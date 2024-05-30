using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using FilmsDomain.Model;
using Microsoft.IdentityModel.Tokens;

namespace FilmsInfrastructure.Controllers
{
    public class QueriesController : Controller
    {
        private const string CONNECTION = "Server=DESKTOP-9FE8NR6\\SQLEXPRESS;Database=DBFilms;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true";

        private const string Q1_PATH = @"C:\Users\Asus\source\repos\src\FilmsMVC\FilmsInfrastructure\Queries\П1.sql";
        private const string Q2_PATH = @"C:\Users\Asus\source\repos\src\FilmsMVC\FilmsInfrastructure\Queries\П2.sql";
        private const string Q3_PATH = @"C:\Users\Asus\source\repos\src\FilmsMVC\FilmsInfrastructure\Queries\П3.sql";
        private const string Q4_PATH = @"C:\Users\Asus\source\repos\src\FilmsMVC\FilmsInfrastructure\Queries\П4.sql";
        private const string Q5_PATH = @"C:\Users\Asus\source\repos\src\FilmsMVC\FilmsInfrastructure\Queries\П5.sql";
        private const string Q11_PATH = @"C:\Users\Asus\source\repos\src\FilmsMVC\FilmsInfrastructure\Queries\П6.sql";

        private const string Q6_PATH = @"C:\Users\Asus\source\repos\src\FilmsMVC\FilmsInfrastructure\Queries\С1.sql";
        private const string Q7_PATH = @"C:\Users\Asus\source\repos\src\FilmsMVC\FilmsInfrastructure\Queries\С2.sql";
        private const string Q8_PATH = @"C:\Users\Asus\source\repos\src\FilmsMVC\FilmsInfrastructure\Queries\С3.sql";
        private const string Q9_PATH = @"C:\Users\Asus\source\repos\src\FilmsMVC\FilmsInfrastructure\Queries\С4.sql";
        private const string Q10_PATH = @"C:\Users\Asus\source\repos\src\FilmsMVC\FilmsInfrastructure\Queries\С5.sql";

        private const string ERR_FILM = "Фільми, що задовольняють дану умову, відсутні.";
        private const string ERR_ACTORS = "Актори, що задовольняють дану умову, відсутні.";
        private const string ERR_CUSTOMERS = "Клієнти, що задовольняють дану умову, відсутні.";
        private const string ERR_GENRES = "Жанри, що задовольняють дану умову, відсутні.";

        private readonly DbfilmsContext _context;
        public QueriesController(DbfilmsContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Query1(Query queryModel)
        {
            if (string.IsNullOrEmpty(queryModel.CustomerName))
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.QuantityError = "Поле не повинно бути порожнім";
                return View("Index", queryModel);
            }

            string query = System.IO.File.ReadAllText(Q1_PATH);

            queryModel.FilmNames = new List<string>();
            queryModel.QueryName = "П1";

            try
            {
                using (var connection = new SqlConnection(CONNECTION))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerName", queryModel.CustomerName);

                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.FilmNames.Add(reader.GetString(0));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.ErrorName = ERR_FILM;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                queryModel.ErrorFlag = 1;
                queryModel.ErrorName = "Виникла помилка при виконанні запиту: " + ex.Message;
            }

            return RedirectToAction("Results", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Query2(Query queryModel)
        {
            if (string.IsNullOrEmpty(queryModel.FilmName))
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.QuantityError = "Поле не повинно бути порожнім";
                return View("Index", queryModel);
            }

            string query = System.IO.File.ReadAllText(Q2_PATH);

            queryModel.ActorNames = new List<string>();
            queryModel.QueryName = "П2";

            try
            {
                using (var connection = new SqlConnection(CONNECTION))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FilmName", queryModel.FilmName);

                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.ActorNames.Add(reader.GetString(0));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.ErrorName = ERR_ACTORS;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                queryModel.ErrorFlag = 1;
                queryModel.ErrorName = "Виникла помилка при виконанні запиту: " + ex.Message;
            }

            return RedirectToAction("Results", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Query3(Query queryModel)
        {
            if (string.IsNullOrEmpty(queryModel.DirectorName))
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.QuantityError = "Поле не повинно бути порожнім";
                return View("Index", queryModel);
            }

            string query = System.IO.File.ReadAllText(Q3_PATH);

            queryModel.FilmNames = new List<string>();
            queryModel.QueryName = "П3";

            try
            {
                using (var connection = new SqlConnection(CONNECTION))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DirectorName", queryModel.DirectorName);

                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.FilmNames.Add(reader.GetString(0));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.ErrorName = ERR_FILM;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                queryModel.ErrorFlag = 1;
                queryModel.ErrorName = "Виникла помилка при виконанні запиту: " + ex.Message;
            }

            return RedirectToAction("Results", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Query4(Query queryModel)
        {
            if (string.IsNullOrEmpty(queryModel.CustomerName))
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.QuantityError = "Поле не повинно бути порожнім";
                return View("Index", queryModel);
            }

            //string customerCheckQuery = "SELECT COUNT(*) FROM Customers WHERE Name = @CustomerName";
            string query = System.IO.File.ReadAllText(Q4_PATH);

            queryModel.TotalPrice = 0;
            queryModel.QueryName = "П4";

            try
            {
                using (var connection = new SqlConnection(CONNECTION))
                {
                    connection.Open();

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerName", queryModel.CustomerName);

                        var result = command.ExecuteScalar();
                        queryModel.TotalPrice = result != DBNull.Value ? Convert.ToSingle(result) : 0;

                        if (queryModel.TotalPrice == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.ErrorName = ERR_FILM;
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                queryModel.ErrorFlag = 1;
                queryModel.ErrorName = "Виникла помилка при виконанні запиту: " + ex.Message;
            }

            return RedirectToAction("Results", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Query5(Query queryModel)
        {
            if (string.IsNullOrEmpty(queryModel.GenreName) || !queryModel.ReleaseYear.HasValue || queryModel.ReleaseYear < 1900 || queryModel.ReleaseYear > 2024)
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.QuantityError = "";
                return View("Index", queryModel);
            }

            string query = System.IO.File.ReadAllText(Q5_PATH);

            queryModel.FilmNames = new List<string>();
            queryModel.TrailerLinks = new List<string>();
            queryModel.QueryName = "П5";

            try
            {
                using (var connection = new SqlConnection(CONNECTION))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GenreName", queryModel.GenreName);
                        command.Parameters.AddWithValue("@ReleaseYear", queryModel.ReleaseYear);

                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.FilmNames.Add(reader.GetString(0));
                                queryModel.TrailerLinks.Add(reader.IsDBNull(1) ? null : reader.GetString(1));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.ErrorName = ERR_FILM;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                queryModel.ErrorFlag = 1;
                queryModel.ErrorName = "Виникла помилка при виконанні запиту: " + ex.Message;
            }

            return RedirectToAction("Results", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Query6(Query queryModel)
        {
            if (!queryModel.Price.HasValue || queryModel.Price < 0)
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.QuantityError6 = "Будь ласка, введіть коректну ціну.";
                return View("Index", queryModel);
            }

            string query = System.IO.File.ReadAllText(Q6_PATH);

            queryModel.CustomerNames = new List<string>();
            queryModel.CustomerEmails = new List<string>();
            queryModel.QueryName = "С1";

            try
            {
                using (var connection = new SqlConnection(CONNECTION))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GivenPrice", queryModel.Price.Value);

                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.CustomerNames.Add(reader.GetString(0));
                                queryModel.CustomerEmails.Add(reader.GetString(1));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.ErrorName = ERR_CUSTOMERS;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                queryModel.ErrorFlag = 1;
                queryModel.ErrorName = "Виникла помилка при виконанні запиту: " + ex.Message;
            }

            return RedirectToAction("Results", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Query7(Query queryModel)
        {
            if (string.IsNullOrEmpty(queryModel.FilmName))
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.QuantityError7 = "Будь ласка, введіть назву фільму.";
                return View("Index", queryModel);
            }

            var filmId = _context.Films.Where(f => f.Name == queryModel.FilmName).Select(f => f.Id).FirstOrDefault();

            string query = System.IO.File.ReadAllText(Q7_PATH);

            queryModel.FilmNames = new List<string>();
            queryModel.QueryName = "С2";

            try
            {
                using (var connection = new SqlConnection(CONNECTION))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FilmId", filmId);

                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.FilmNames.Add(reader.GetString(0));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.ErrorName = ERR_FILM;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                queryModel.ErrorFlag = 1;
                queryModel.ErrorName = "Виникла помилка при виконанні запиту: " + ex.Message;
            }

            return RedirectToAction("Results", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Query8(Query queryModel)
        {
            if (string.IsNullOrEmpty(queryModel.CustomerName))
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.QuantityError8 = "Будь ласка, введіть ім'я покупця.";
                return View("Index", queryModel);
            }

            var customerId = _context.Customers.Where(c => c.Name == queryModel.CustomerName).Select(c => c.Id).FirstOrDefault();

            string query = System.IO.File.ReadAllText(Q8_PATH);

            queryModel.CustomerNames = new List<string>();
            queryModel.CustomerEmails = new List<string>();
            queryModel.QueryName = "С3";

            try
            {
                using (var connection = new SqlConnection(CONNECTION))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);

                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.CustomerNames.Add(reader.GetString(0));
                                queryModel.CustomerEmails.Add(reader.GetString(1));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.ErrorName = ERR_CUSTOMERS;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                queryModel.ErrorFlag = 1;
                queryModel.ErrorName = "Виникла помилка при виконанні запиту: " + ex.Message;
            }

            return RedirectToAction("Results", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Query9(Query queryModel)
        {
            if (string.IsNullOrEmpty(queryModel.CustomerName))
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.QuantityError8 = "Будь ласка, введіть ім'я покупця.";
                return View("Index", queryModel);
            }

            var customerId = _context.Customers.Where(c => c.Name == queryModel.CustomerName).Select(c => c.Id).FirstOrDefault();

            string query = System.IO.File.ReadAllText(Q9_PATH);

            queryModel.CustomerNames = new List<string>();
            queryModel.CustomerEmails = new List<string>();
            queryModel.QueryName = "С4";

            try
            {
                using (var connection = new SqlConnection(CONNECTION))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);

                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.CustomerNames.Add(reader.GetString(0));
                                queryModel.CustomerEmails.Add(reader.GetString(1));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.ErrorName = ERR_CUSTOMERS;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                queryModel.ErrorFlag = 1;
                queryModel.ErrorName = "Виникла помилка при виконанні запиту: " + ex.Message;
            }

            return RedirectToAction("Results", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Query10(Query queryModel)
        {
            if (string.IsNullOrEmpty(queryModel.ActorName))
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.QuantityError10 = "Будь ласка, введіть ім'я актора.";
                return View("Index", queryModel);
            }

            string query = System.IO.File.ReadAllText(Q10_PATH);

            queryModel.ActorNames = new List<string>();
            queryModel.QueryName = "С5";

            try
            {
                using (var connection = new SqlConnection(CONNECTION))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ActorName", queryModel.ActorName);

                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.ActorNames.Add(reader.GetString(0));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.ErrorName = ERR_ACTORS;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                queryModel.ErrorFlag = 1;
                queryModel.ErrorName = "Виникла помилка при виконанні запиту: " + ex.Message;
            }

            return RedirectToAction("Results", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Query11(Query queryModel)
        {
            if (string.IsNullOrEmpty(queryModel.CustomerName))
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.QuantityError11 = "Будь ласка, введіть ім'я покупця.";
                return View("Index", queryModel);
            }

            string query = System.IO.File.ReadAllText(Q11_PATH);

            queryModel.GenreNames = new List<string>();
            queryModel.QueryName = "П6";

            try
            {
                using (var connection = new SqlConnection(CONNECTION))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerName", queryModel.CustomerName);

                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.GenreNames.Add(reader.GetString(0));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.ErrorName = ERR_GENRES;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                queryModel.ErrorFlag = 1;
                queryModel.ErrorName = "Виникла помилка при виконанні запиту: " + ex.Message;
            }

            return RedirectToAction("Results", queryModel);
        }

        public IActionResult Results(Query queryResult)
        {
            return View(queryResult);
        }

    }
}
