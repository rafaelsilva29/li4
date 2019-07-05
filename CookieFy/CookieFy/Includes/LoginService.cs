using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieFy.Data;
using CookieFy.Models;

namespace CookieFy.Includes{

    public interface ILoginService {

        User AuthenticateUser(string email, string password);
      
    }

    public class LoginService : ILoginService {

        private readonly CookieDbContext dbContext;

        public LoginService(CookieDbContext context) {

            dbContext = context;
        }

        public User AuthenticateUser(string email, string password) {

            // retorna null se os argumentos são null
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            // vai buscar o cliente à BD
            User user = dbContext.User.FirstOrDefault(u=> u.Email.Equals(email));

            // se for diferente de null e se a password corresponder, retorna o cliente
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password)) return user;

            return null;
        }
    }
}