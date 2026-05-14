using DAL.EF;
using DAL.EF.Tables;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos
{
    public class UserManagementRepo
    {
         HotelManagementContext db;

        public UserManagementRepo(HotelManagementContext db)
        {
            this.db = db;
        }

        // --- CRUD OPERATIONS ---
        public List<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return db.Users.Find(id);
        }

        public bool CreateUser(User user)
        {
            db.Users.Add(user);
            return db.SaveChanges() > 0;
        }

        public bool UpdateUser(User user)
        {
            var existing = db.Users.Find(user.UserId);
            if (existing != null)
            {
                existing.Name = user.Name;
                existing.Email = user.Email;
                existing.Status = user.Status;
                // Note: Role is updated via the specific AssignRole method
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public bool DeleteUser(int id)
        {
            var user = db.Users.Find(id);

            if (user == null)
                return false;

            // Delete related employee
            var employee = db.Employees.FirstOrDefault(e => e.UserId == id);

            if (employee != null)
            {
                db.Employees.Remove(employee);
            }

            // Delete related customer
            var customer = db.Customers.FirstOrDefault(c => c.UserId == id);

            if (customer != null)
            {
                db.Customers.Remove(customer);
            }

            // Delete user
            db.Users.Remove(user);

            return db.SaveChanges() > 0;
        }

        // --- BEYOND CRUD: ASSIGN ROLES ---
        public bool AssignRole(int userId, string newRole)
        {
            var user = db.Users.Find(userId);
            if (user != null)
            {
                user.Role = newRole;
                return db.SaveChanges() > 0;
            }
            return false;
        }
        public bool ChangeStatus(int userId, string ns)
        {
            var user = db.Users.Find(userId);
            if (user != null)
            {
                user.Status = ns;
                return db.SaveChanges() > 0;
            }
            return false;
        }
    }
}