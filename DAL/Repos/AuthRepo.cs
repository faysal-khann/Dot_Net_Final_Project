using DAL.EF;
using DAL.EF.Tables;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
            return db.Users.FirstOrDefault(u => u.Email == email && u.Password == GetMd5(password));
        }


        string GetMd5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // lowercase hex
                }

                return sb.ToString();
            }
        }
    }
}