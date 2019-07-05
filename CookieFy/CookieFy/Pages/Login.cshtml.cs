using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using CookieFy.Data;
using CookieFy.Includes;
using CookieFy.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CookieFy.Pages {

    [AllowAnonymous]
    public class Login : PageModel {

        [Required]
        [BindProperty]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [BindProperty]
        [Display(Name = "Lembrar-me")]
        public bool RememberMe { get; set; }

        //instanciando a classe login por injeção
        private ILoginService _loginService;

        // recebe a instância da classe login
        public Login(ILoginService loginService) {

            _loginService = loginService;
        }

        // método chamado quando o utilizador carrega em login, na página login
        public IActionResult OnPostLoginUser() {

            TryUpdateModelAsync(this);

            // para o caso de as credenciais não terem sido preenchidas. Mostra mensagens de erro...
            if (!ModelState.IsValid) return Page();

            // verificação dupla
            if (Email == null || Password == null) {
                ModelState.AddModelError("", "Preencha os campos");
                return Page();
            }

            // desta forma, não mais é possível alterar o valor das strings
            const string AdminUsername = "admin";
            const string PasswordUsername = "admin123";

            var user = (dynamic)null;
            var claims = (dynamic)null;

            // login do ADMIN
            if (Email.Equals(AdminUsername) && Password.Equals(PasswordUsername)) {
                claims = new[]
                {
                     new Claim(ClaimTypes.Name, "Administrador"),
                     new Claim(ClaimTypes.Role, "Admin")
                };
                goto Checked;
            }

            // faz a busca do utilizador e verifica se existe na tabela clientes ou na tabela profissionais
            User userLogin = _loginService.AuthenticateUser(Email, Password);

            // se não encontrou nada então algo falhou
            if (userLogin == null) {
                ModelState.AddModelError("", "Username ou Password inválidas.");
                return Page();
            }
            // se encontrou alguma coisa
            else
            {
                user = userLogin;
                claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "User")
                };
            }

            Checked:
            //faz autenticação via Cookie
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties {
                    IsPersistent = this.RememberMe,
                    ExpiresUtc = DateTime.UtcNow.AddDays(30)
                });

            // redireciona para a Index novamente, porém já autenticado
            return RedirectToPage("/Index").WithSuccess("Login", "efetuado com sucesso.", "3000");
        }

        // metodo chamado quando carrega em logout
        public IActionResult OnGetLogout() {

            // remove o cookie
            Response.Cookies.Delete("CookieFyLogin");

            return RedirectToPage("/Index").WithSuccess("Logout", "efetuado com sucesso.", "3000");
        }

    }
}