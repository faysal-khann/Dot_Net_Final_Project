using System;
using System.Collections.Generic;
using System.Text;
using DAL.EF;
using DAL.EF.Tables;
namespace DAL.Repos
{
    public class RegistrationRepo
    {
        HotelManagementContext db;
        public RegistrationRepo(HotelManagementContext db)
        {
            this.db = db;
        }

        public bool RegisterCustomer(User user, Customer customer)
        {
            db.Users.Add(user);
            db.SaveChanges();

            customer.UserId = user.UserId;
            db.Customers.Add(customer);
            return db.SaveChanges() > 0;
        }

        // Register employee
        public bool RegisterEmployee(User user, Employee employee)
        {
            db.Users.Add(user);
            db.SaveChanges();

            employee.UserId = user.UserId;
            db.Employees.Add(employee);
            return db.SaveChanges() > 0;
        }


    }
}
