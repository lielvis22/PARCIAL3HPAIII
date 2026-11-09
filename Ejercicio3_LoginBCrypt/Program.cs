using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace Ejercicio3_LoginBCrypt
{
    internal class Program
    {
        private const string CadenaConexion =
            "Server=localhost;Port=3306;Database=loginbcrypt;Uid=root;Pwd=elvisli22";

        private const int FactorTrabajo = 12;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool salir = false;

            while (!salir)
            {
                Console.WriteLine("\n===== LOGIN CON BCRYPT =====");
                Console.WriteLine("1. Registrar usuario (generar hash)");
                Console.WriteLine("2. Iniciar sesión (verificar con BCrypt.Verify)");
                Console.WriteLine("3. Ver historial de intentos");
                Console.WriteLine("0. Salir");
                Console.Write("Opción: ");

                try
                {
                    switch (Console.ReadLine()?.Trim())
                    {
                        case "1": RegistrarUsuario(); break;
                        case "2": IniciarSesion(); break;
                        case "3": MostrarIntentos(); break;
                        case "0": salir = true; break;
                        default: Console.WriteLine("Opción no válida."); break;
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error de base de datos: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            }
        }

        // ---------- 1. Generar el hash y guardarlo en la base de datos ----------
        private static void RegistrarUsuario()
        {
            string usuario = LeerTexto("Nombre de usuario: ");
            string nombreCompleto = LeerTexto("Nombre completo: ");
            string password = LeerPassword("Contraseña: ");

            if (password.Length < 6)
            {
                Console.WriteLine("La contraseña debe tener al menos 6 caracteres.");
                return;
            }

            // BCrypt genera automáticamente una sal aleatoria y la incluye en el hash.
            string hash = BCrypt.Net.BCrypt.HashPassword(password, FactorTrabajo);
            Console.WriteLine($"Hash generado: {hash}");

            using var conexion = new MySqlConnection(CadenaConexion);
            conexion.Open();

            // Consultas parametrizadas para evitar inyección SQL.
            using (var existe = new MySqlCommand(
                "SELECT COUNT(*) FROM usuarios WHERE nombre_usuario = @usuario", conexion))
            {
                existe.Parameters.AddWithValue("@usuario", usuario);
                if (Convert.ToInt32(existe.ExecuteScalar()) > 0)
                {
                    Console.WriteLine("Ese nombre de usuario ya existe.");
                    return;
                }
            }

            using var insertar = new MySqlCommand(
                "INSERT INTO usuarios (nombre_usuario, nombre_completo, password_hash) " +
                "VALUES (@usuario, @nombre, @hash)", conexion);
            insertar.Parameters.AddWithValue("@usuario", usuario);
            insertar.Parameters.AddWithValue("@nombre", nombreCompleto);
            insertar.Parameters.AddWithValue("@hash", hash);
            insertar.ExecuteNonQuery();

            Console.WriteLine("Usuario registrado correctamente. Solo se guardó el hash, no la contraseña.");
        }

        // ---------- 2. Verificar la contraseña con BCrypt.Verify() ----------
        private static void IniciarSesion()
        {
            string usuario = LeerTexto("Usuario: ");
            string password = LeerPassword("Contraseña: ");

            using var conexion = new MySqlConnection(CadenaConexion);
            conexion.Open();

            string? hashGuardado = null;
            string? nombreCompleto = null;

            using (var buscar = new MySqlCommand(
                "SELECT password_hash, nombre_completo FROM usuarios WHERE nombre_usuario = @usuario", conexion))
            {
                buscar.Parameters.AddWithValue("@usuario", usuario);
                using var lector = buscar.ExecuteReader();
                if (lector.Read())
                {
                    hashGuardado = lector.GetString(0);
                    nombreCompleto = lector.GetString(1);
                }
            }

            // Compara la contraseña escrita con el hash almacenado.
            bool exitoso = hashGuardado != null && BCrypt.Net.BCrypt.Verify(password, hashGuardado);

            RegistrarIntento(conexion, usuario, exitoso);

            if (exitoso)
            {
                // Acceso exitoso: se muestra el nombre del usuario.
                Console.WriteLine($"\n¡Acceso concedido! Bienvenido(a), {nombreCompleto} ({usuario}).");
            }
            else
            {
                Console.WriteLine("\nAcceso denegado: usuario o contraseña incorrectos.");
            }
        }

        // ---------- Guardar intentos fallidos y exitosos ----------
        private static void RegistrarIntento(MySqlConnection conexion, string usuario, bool exitoso)
        {
            using var cmd = new MySqlCommand(
                "INSERT INTO intentos_login (nombre_usuario, exitoso, fecha) VALUES (@usuario, @exitoso, NOW())",
                conexion);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@exitoso", exitoso);
            cmd.ExecuteNonQuery();
        }

        private static void MostrarIntentos()
        {
            using var conexion = new MySqlConnection(CadenaConexion);
            conexion.Open();

            using var cmd = new MySqlCommand(
                "SELECT nombre_usuario, exitoso, fecha FROM intentos_login ORDER BY fecha DESC LIMIT 20", conexion);
            using var lector = cmd.ExecuteReader();

            Console.WriteLine($"\n{"Usuario",-20}{"Resultado",-12}{"Fecha",-20}");
            Console.WriteLine(new string('-', 52));
            while (lector.Read())
            {
                string resultado = lector.GetBoolean(1) ? "Exitoso" : "Fallido";
                Console.WriteLine($"{lector.GetString(0),-20}{resultado,-12}{lector.GetDateTime(2):dd/MM/yyyy HH:mm:ss}");
            }
        }

        // ---------- Utilidades de entrada ----------
        private static string LeerTexto(string mensaje)
        {
            string? valor;
            do
            {
                Console.Write(mensaje);
                valor = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(valor));
            return valor;
        }

        // Lee la contraseña mostrando '*' (equivalente a PasswordChar en Windows Forms).
        private static string LeerPassword(string mensaje)
        {
            Console.Write(mensaje);
            var sb = new StringBuilder();
            ConsoleKeyInfo tecla;
            while ((tecla = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (tecla.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Length--;
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(tecla.KeyChar))
                {
                    sb.Append(tecla.KeyChar);
                    Console.Write('*');
                }
            }
            Console.WriteLine();
            return sb.ToString();
        }
    }
}