using Employee_Creation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Employee_Creation.Controllers
{
    public class HomeController : Controller
    {

       
        private readonly string connectionString = @"Server=.; Database=Training; User Id=sa; Password=mspl.123;";

        public ActionResult Employee()
        {
            var employees = GetAllEmployees();
            return View(employees);
        }

        private List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("ManageEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Display");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                ID = (int)reader["ID"],
                                Name = reader["Name"].ToString(),
                                DateOfBirth = (DateTime)reader["DateOfBirth"],
                                Email = reader["Email"].ToString(),
                                PicturePath = reader["Picture"].ToString()
                            });
                        }
                    }
                }
            }

            return employees;
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee employee, HttpPostedFileBase Picture)
        {
            if (Picture != null)
            {
                // Save the picture to the server
                string pictureName = Path.GetFileName(Picture.FileName);
                string path = Path.Combine(Server.MapPath("~/Images/"), pictureName);
                Picture.SaveAs(path);
                employee.PicturePath = pictureName;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("ManageEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Add");
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);

                    if (Picture != null)
                    {                      
                        cmd.Parameters.AddWithValue("@Picture", employee.PicturePath);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Picture", DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Employee");
        }


        [HttpPost]
        public ActionResult DeleteEmployee(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("ManageEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Delete");
                    cmd.Parameters.AddWithValue("@ID",id);

                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Employee");
        }

        [HttpGet]
        public JsonResult GetEmployee(int id)
        {
            Employee employee = GetAllEmployees().FirstOrDefault(e => e.ID == id);
            if (employee != null)
            {
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        ID = employee.ID,
                        Name = employee.Name,
                        DateOfBirth = employee.DateOfBirth.ToString("yyyy-MM-dd"),
                        Email = employee.Email,
                        PicturePath = employee.PicturePath
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Employee not found" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditEmployee(Employee employee, HttpPostedFileBase Picture)
        {
            if (Picture != null)
            {
                // Save the new picture to the server
                string pictureName = Path.GetFileName(Picture.FileName);
                string path = Path.Combine(Server.MapPath("~/Images/"), pictureName);
                Picture.SaveAs(path);
                employee.PicturePath = pictureName;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("ManageEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Update");
                    cmd.Parameters.AddWithValue("@ID", employee.ID);
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@Picture", employee.PicturePath);
                    //if (Picture != null)
                    //{

                    //    cmd.Parameters.AddWithValue("@Picture", employee.PicturePath);
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@Picture", DBNull.Value);
                    //}

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Employee");
        }



    }
}