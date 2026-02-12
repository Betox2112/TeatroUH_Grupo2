using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeatroUH.Application.DTOs;
using TeatroUH.Application.Interfaces;
using TeatroUH.Domain.Entities;
using TeatroUH.Infrastructure.Data;
using Newtonsoft.Json;

namespace TeatroUH.Web.Controllers
{
    public class CuentaController : Controller
    {
        private readonly IAuthService _authService;
        private readonly TeatroUHDbContext _db;

        public CuentaController(IAuthService authService, TeatroUHDbContext db)
        {
            _authService = authService;
            _db = db;
        }

        // ---------- PÁGINA PRINCIPAL ----------
        public IActionResult Index()
        {
            // Recupera datos de sesión para mostrar en la vista
            var email = HttpContext.Session.GetString("USER_EMAIL");
            var role = HttpContext.Session.GetString("USER_ROLE");

            ViewBag.Email = email;
            ViewBag.Role = role;

            return View();
        }

        // ---------- HISTORIAL DE RESERVAS ----------
        public async Task<IActionResult> MisReservas()
        {
            var email = HttpContext.Session.GetString("USER_EMAIL");
            if (string.IsNullOrWhiteSpace(email))
                return RedirectToAction("Login");

            email = email.Trim().ToLower();

            // Busca reservas asociadas al correo del usuario
            var reservas = await _db.Reservations
                .Include(r => r.Items)
                    .ThenInclude(i => i.Showtime)
                        .ThenInclude(s => s.Play)
                .Where(r => r.CustomerEmail != null && r.CustomerEmail.ToLower() == email)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(reservas);
        }

        // ---------- REGISTER ----------
        [HttpGet]
        public IActionResult Register()
        {
            // Devuelve la vista de registro vacía
            return View(new RegisterDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            // 1. Validación de Google reCAPTCHA
            var captchaResponse = Request.Form["g-recaptcha-response"];
            using (var httpClient = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "secret", "6Lc7p2gsAAAAAL9gwLhacrDp-sjqBlz9qeIKtTOZ" }, // 
                    { "response", captchaResponse }
                };

                var content = new FormUrlEncodedContent(values);
                var response = await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                var jsonString = await response.Content.ReadAsStringAsync();

                dynamic jsonData = JsonConvert.DeserializeObject(jsonString);

                if (jsonData.success != true)
                {
                    // Si el captcha falla, se muestra error y se regresa a la vista
                    ModelState.AddModelError("", "Captcha inválido, intente de nuevo.");
                    return View(model);
                }
            }

            // 2. Validaciones básicas de campos
            if (string.IsNullOrWhiteSpace(model.Nombre))
                ModelState.AddModelError(nameof(model.Nombre), "El nombre es requerido.");

            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError(nameof(model.Email), "El email es requerido.");

            if (string.IsNullOrWhiteSpace(model.Password))
                ModelState.AddModelError(nameof(model.Password), "La contraseña es requerida.");

            if (!ModelState.IsValid)
                return View(model);

            // 3. Crear usuario y registrar en el sistema
            var user = new User
            {
                Nombre = model.Nombre.Trim(),
                Email = model.Email.Trim(),
                Password = model.Password,
                Role = Role.Comprador
            };

            var ok = _authService.Register(user);
            if (!ok)
            {
                ModelState.AddModelError("", "Ese correo ya está registrado o los datos son inválidos.");
                return View(model);
            }

            // 4. Auto-login: guardar datos en sesión
            HttpContext.Session.SetString("USER_EMAIL", user.Email);
            HttpContext.Session.SetString("USER_ROLE", user.Role.ToString());

            // 5. Mensaje de éxito y redirección al Home
            TempData["SuccessMessage"] = "¡Tu registro fue exitoso! Bienvenido al sistema.";
            return RedirectToAction("Index");
        }

        // ---------- LOGIN ----------
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            // 1. Validación de Google reCAPTCHA
            var captchaResponse = Request.Form["g-recaptcha-response"];
            using (var httpClient = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "secret", "6Lc7p2gsAAAAAL9gwLhacrDp-sjqBlz9qeIKtTOZ" }, 
                    { "response", captchaResponse }
                };

                var content = new FormUrlEncodedContent(values);
                var response = await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                var jsonString = await response.Content.ReadAsStringAsync();

                dynamic jsonData = JsonConvert.DeserializeObject(jsonString);

                if (jsonData.success != true)
                {
                    ModelState.AddModelError("", "Captcha inválido, intente de nuevo.");
                    return View(model);
                }
            }

            // 2. Validaciones básicas
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError(nameof(model.Email), "El email es requerido.");

            if (string.IsNullOrWhiteSpace(model.Password))
                ModelState.AddModelError(nameof(model.Password), "La contraseña es requerida.");

            if (!ModelState.IsValid)
                return View(model);

            // 3. Intentar login
            var email = model.Email.Trim();
            var user = _authService.Login(email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Credenciales incorrectas.");
                return View(model);
            }

            // 4. Guardar datos en sesión
            HttpContext.Session.SetString("USER_EMAIL", user.Email);
            HttpContext.Session.SetString("USER_ROLE", user.Role.ToString());

            // 5. Mensaje de éxito y redirección
            TempData["SuccessMessage"] = "Ingreso exitoso. ¡Bienvenido!";
            return RedirectToAction("Index");
        }

        // ---------- LOGOUT ----------
        public IActionResult Logout()
        {
            // Limpia la sesión y redirige al Home
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Has cerrado sesión correctamente.";
            return RedirectToAction("Index");
        }
    }
}
