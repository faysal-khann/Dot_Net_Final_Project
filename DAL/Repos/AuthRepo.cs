using DAL.EF;
using DAL.EF.Tables;
using System.Linq;

namespace DAL.Repos
{
    public class AuthRepo
    {
        private readonly HotelManagementContext db;

        public AuthRepo(HotelManagementContext db)
        {
            this.db = db;
        }

        public User Authenticate(string email, string password)
        {
            // Finds the user with matching email and password
            return db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}