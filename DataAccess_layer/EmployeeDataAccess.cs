using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using EmployeeManagement.Models;

namespace EmployeeManagement.DataAccess_layer
{
    public class EmployeeDataAccess
    {
        string strcon = ConfigurationManager.ConnectionStrings["EmployeeCon"].ToString();

        //Get all emp
        public List<Employee> GetAllEmployeeList()
        {
            List<Employee> empList = new List<Employee>();

            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    SqlCommand command = con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "EmployeeSelect_SP";

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dtEmp = new DataTable();

                    con.Open();
                    dataAdapter.Fill(dtEmp);
                    con.Close();

                    foreach (DataRow dr in dtEmp.Rows)
                    {
                        empList.Add(new Employee
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Name = dr["name"].ToString(),
                            Designation = dr["designation"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving employee data. Please try again later.", ex);
            }

            return empList;
        }

        //Get the employee record by id
        public List<Employee> GetEmployeeById(int Id)
        {
            List<Employee> empList = new List<Employee>();

            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    SqlCommand command = con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetEmployeeById_Sp";
                    command.Parameters.AddWithValue("@id", Id);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dtEmp = new DataTable();

                    con.Open();
                    dataAdapter.Fill(dtEmp);
                    con.Close();

                    foreach (DataRow dr in dtEmp.Rows)
                    {
                        empList.Add(new Employee
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Name = dr["name"].ToString(),
                            Designation = dr["designation"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving employee data. Please try again later.", ex);
            }

            return empList;
        }
        //Insert new Employee
        public bool InsertNewEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    SqlCommand command = new SqlCommand("Employee_SP", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", employee.Id);
                    command.Parameters.AddWithValue("@name", employee.Name);
                    command.Parameters.AddWithValue("@designation", employee.Designation);

                    con.Open();
                    command.ExecuteNonQuery();
                }

                
                return true;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error inserting employee: " + ex.Message);
                
                return false;
            }
        }

        //Update previously present employees
        public bool UpdateEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    SqlCommand command = new SqlCommand("UpdateEmployee_SP", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", employee.Id);
                    command.Parameters.AddWithValue("@name", employee.Name);
                    command.Parameters.AddWithValue("@designation", employee.Designation);

                    con.Open();
                    command.ExecuteNonQuery();
                }


                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error inserting employee: " + ex.Message);

                return false;
            }
        }

        //Delete Employee
        public bool DeleteEmployee(int id)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand("EmployeeDelete_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    return true; // Deletion successful
                }
                catch (SqlException)
                {
                    // Error occurred during deletion
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

    }
}