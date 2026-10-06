using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppForSEII.API.Data
{
    public class SeedData
    {
        public static void Initialize(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            List<string> rolesNames = new List<string>
            {
                "Administrator",
                "Employee",
                "Customer"
            };

            // ---------------- ROLES ----------------

            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding the Roles in the Database.");
            }


            // ---------------- USUARIOS ----------------

            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            try
            {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding the Users in the Database.");
            }


            // ---------------- TIPOS ----------------

            try
            {
                SeedTipos(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding TipoDeporte and TipoMaterial.");
            }


            // ---------------- RESERVAS DE PISTAS ----------------

            try
            {
                SeedReservas(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding Reservas in the Database.");
            }


            // ---------------- COMPETICIONES ----------------

            try
            {
                SeedCompeticiones(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding Competiciones in the Database.");
            }


            // ---------------- CLASES DEPORTIVAS ----------------

            try
            {
                SeedClasesDeportivas(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding Clases Deportivas in the Database.");
            }


            // ---------------- ALQUILER DE MATERIAL ----------------

            try
            {
                SeedAlquilerMaterial(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding Alquiler de Material in the Database.");
            }
        }


        // ============================================================
        // ROLES
        // ============================================================

        public static void SeedRoles(
            RoleManager<IdentityRole> roleManager,
            List<string> roles)
        {
            foreach (string roleName in roles)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();

                    role.Name = roleName;
                    role.NormalizedName = roleName;

                    roleManager.CreateAsync(role).Wait();
                }
            }
        }


        // ============================================================
        // USUARIOS
        // ============================================================

        public static void SeedUsers(
            UserManager<ApplicationUser> userManager,
            List<string> roles)
        {
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser(
                    "1",
                    "Elena",
                    "Navarro Martínez",
                    "elena@uclm.es",
                    "12345678A",
                    25,
                    "Mujer"
                );

                user.EmailConfirmed = true;

                var result =
                    userManager.CreateAsync(user, "Password1234%");

                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager
                        .AddToRoleAsync(user, roles[0])
                        .Wait();
                }
            }


            if (userManager.FindByNameAsync("peter@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser(
                    "3",
                    "Peter",
                    "Jackson",
                    "peter@uclm.es",
                    "87654321B",
                    25,
                    "Hombre"
                );

                user.EmailConfirmed = true;

                var result =
                    userManager.CreateAsync(user, "OtherPass12$");

                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager
                        .AddToRoleAsync(user, roles[2])
                        .Wait();
                }
            }
        }


        // ============================================================
        // DATOS BASE: TIPO DEPORTE + TIPO MATERIAL
        // ============================================================

        public static void SeedTipos(ApplicationDbContext dbContext)
        {
            // TipoDeporte

            if (!dbContext.TipoDeporte.Any())
            {
                TipoDeporte futbol = new TipoDeporte
                {
                    Nombre = "Fútbol",
                    NombreTipoDeporte = "Fútbol",
                    Competiciones = "Liga local",
                    Descripcion = "Deporte de equipo"
                };

                dbContext.TipoDeporte.Add(futbol);
            }


            // TipoMaterial

            if (!dbContext.TiposMaterial.Any())
            {
                TipoMaterial tipoMaterial = new TipoMaterial
                {
                    NombreTipoMaterial = "Balón"
                };

                dbContext.TiposMaterial.Add(tipoMaterial);
            }


            dbContext.SaveChanges();
        }


        // ============================================================
        // RESERVA DE PISTAS
        // ============================================================

        public static void SeedReservas(ApplicationDbContext dbContext)
        {
            TipoDeporte tipoDeporte =
                dbContext.TipoDeporte.First();


            // -------- PISTA --------

            Pista? pista =
                dbContext.Pistas.FirstOrDefault();

            if (pista == null)
            {
                pista = new Pista
                {
                    NombrePista = "Pista de fútbol 1",
                    NPersonas = "10",
                    Precio = 20.00,
                    Stock = 1,
                    TipoDeporte = tipoDeporte
                };

                dbContext.Pistas.Add(pista);
            }


            // -------- RESERVA --------

            Reserva? reserva =
                dbContext.Reservas.FirstOrDefault();

            if (reserva == null)
            {
                reserva = new Reserva
                {
                    NombreCliente = "Elena",
                    Apellidos = "Navarro Martínez",
                    Dni = "12345678A",
                    FechaReserva = DateTime.Now.AddDays(1),
                    MetodoPago = MetodoPago.Bizum,
                    PrecioTotal = 20.00
                };

                dbContext.Reservas.Add(reserva);
            }


            // Necesitamos los IDs de Pista y Reserva

            dbContext.SaveChanges();


            // -------- PISTA RESERVADA --------

            if (!dbContext.PistaReservada.Any())
            {
                PistaReservada pistaReservada =
                    new PistaReservada
                    {
                        Cantidad = 1,

                        IdPista = pista.IdPista,
                        IdReserva = reserva.Id,

                        Observaciones =
                            "Reserva inicial de pista",

                        Precio = 20.00m,

                        Pista = pista,
                        Reserva = reserva
                    };

                dbContext.PistaReservada.Add(pistaReservada);

                dbContext.SaveChanges();
            }
        }


        // ============================================================
        // COMPETICIONES
        // ============================================================

        public static void SeedCompeticiones(
            ApplicationDbContext dbContext)
        {
            TipoDeporte tipoDeporte =
                dbContext.TipoDeporte.First();


            // -------- COMPETICIÓN --------

            Competicion? competicion =
                dbContext.Competiciones.FirstOrDefault();

            if (competicion == null)
            {
                competicion = new Competicion
                {
                    Nombre = "Torneo de fútbol",
                    Fecha = DateTime.Now.AddDays(10),
                    Lugar = "Albacete",
                    Plazas = 20,
                    Precio = 10.00m,

                    TipoDeporte = tipoDeporte
                };

                dbContext.Competiciones.Add(competicion);
            }


            // -------- INSCRIPCIÓN A COMPETICIÓN --------

            InscripcionComp? inscripcion =
                dbContext.InscripcionesComp.FirstOrDefault();

            if (inscripcion == null)
            {
                inscripcion = new InscripcionComp
                {
                    FechaInscripcion = DateTime.Now,
                    PrecioTotal = 10.00m,

                    MetodoPago = "Bizum",

                    NombreUsuario = "Elena",
                    ApellidosUsuario = "Navarro Martínez",

                    DNI = "12345678A",
                    Telefono = "600000000"
                };

                dbContext.InscripcionesComp.Add(inscripcion);
            }


            // Conseguimos IDs

            dbContext.SaveChanges();


            // -------- COMPETICIÓN INSCRITA --------

            if (!dbContext.CompeticionesInscritas.Any())
            {
                CompeticionInscrita competicionInscrita =
                    new CompeticionInscrita
                    {
                        CompeticionId = competicion.Id,

                        InscripcionId = inscripcion.Id,

                        Competicion = competicion,

                        Inscripcion = inscripcion,

                        ProblemasFisicos = null
                    };


                dbContext
                    .CompeticionesInscritas
                    .Add(competicionInscrita);

                dbContext.SaveChanges();
            }
        }


        // ============================================================
        // CLASES DEPORTIVAS
        // ============================================================

        public static void SeedClasesDeportivas(
            ApplicationDbContext dbContext)
        {
            TipoDeporte tipoDeporte =
                dbContext.TipoDeporte.First();


            ApplicationUser? usuario =
                dbContext.ApplicationUsers
                    .FirstOrDefault(
                        u => u.UserName == "elena@uclm.es"
                    );


            if (usuario == null)
            {
                return;
            }


            // -------- CLASE DEPORTIVA --------

            ClaseDeportiva? clase =
                dbContext.ClaseDeportiva.FirstOrDefault();

            if (clase == null)
            {
                clase = new ClaseDeportiva
                {
                    Descripcion =
                        "Clase de iniciación al fútbol",

                    FechaHora =
                        DateTime.Now.AddDays(3),

                    Lugar =
                        "Pista principal",

                    Monitor =
                        "Carlos",

                    Nivel =
                        "Iniciación",

                    PlazasDisponibles = 12,

                    PrecioUnitario = 8.00m,

                    TipoDeporte = tipoDeporte
                };


                dbContext.ClaseDeportiva.Add(clase);
            }


            // -------- INSCRIPCIÓN --------

            Inscripcion? inscripcion =
                dbContext.Inscripcion.FirstOrDefault();

            if (inscripcion == null)
            {
                inscripcion = new Inscripcion
                {
                    FechaInscripcion =
                        DateTime.Now,

                    PrecioTotal =
                        8.00m,

                    DatosPago =
                        "Pago realizado",

                    MetodoPago =
                        MetodoPago.Bizum,

                    Cliente =
                        usuario
                };


                dbContext.Inscripcion.Add(inscripcion);
            }


            dbContext.SaveChanges();


            // -------- CLASE INSCRITA --------

            if (!dbContext.ClaseInscritas.Any())
            {
                ClaseInscrita claseInscrita =
                    new ClaseInscrita
                    {
                        PlazasReservadas = 1,

                        Precio = 8.00m,

                        Observaciones =
                            "Sin observaciones",

                        ClaseDeportivaId =
                            clase.Id,

                        InscripcionId =
                            inscripcion.Id,

                        ClaseDeportiva =
                            clase
                    };


                dbContext.ClaseInscritas.Add(
                    claseInscrita
                );

                dbContext.SaveChanges();
            }
        }


        // ============================================================
        // ALQUILER DE MATERIAL
        // ============================================================

        public static void SeedAlquilerMaterial(
            ApplicationDbContext dbContext)
        {
            TipoDeporte tipoDeporte =
                dbContext.TipoDeporte.First();


            TipoMaterial tipoMaterial =
                dbContext.TiposMaterial.First();


            // -------- MATERIAL --------

            Material? material =
                dbContext.Materials.FirstOrDefault();

            if (material == null)
            {
                material = new Material
                {
                    Nombre =
                        "Balón de fútbol",

                    Cantidad =
                        10,

                    Precio =
                        5.00,

                    TipoDeporte =
                        tipoDeporte,

                    TipoMaterial =
                        tipoMaterial
                };


                dbContext.Materials.Add(material);
            }


            // -------- ALQUILER --------

            Alquiler? alquiler =
                dbContext.Alquileres.FirstOrDefault();

            if (alquiler == null)
            {
                alquiler = new Alquiler
                {
                    NombreUsuario =
                        "Elena",

                    ApellidosUsuario =
                        "Navarro Martínez",

                    DNI =
                        "12345678A",

                    FechaAlquiler =
                        DateTime.Now,

                    MetodoPago =
                        "Bizum",

                    NumeroTelefono =
                        "600000000",

                    PrecioTotal =
                        5.00m
                };


                dbContext.Alquileres.Add(alquiler);
            }


            // Generamos IdMaterial e IdAlquiler

            dbContext.SaveChanges();


            // -------- MATERIAL ALQUILADO --------

            if (!dbContext.MaterialesAlquilados.Any())
            {
                MaterialAlquilado materialAlquilado =
                    new MaterialAlquilado
                    {
                        IdMaterial =
                            material.IdMaterial,

                        IdAlquiler =
                            alquiler.IdAlquiler,

                        Cantidad =
                            1,

                        Descripcion =
                            "Balón de fútbol alquilado",

                        Precio =
                            5.00m,

                        Material =
                            material,

                        Alquiler =
                            alquiler
                    };


                dbContext.MaterialesAlquilados.Add(
                    materialAlquilado
                );

                dbContext.SaveChanges();
            }
        }
    }
}