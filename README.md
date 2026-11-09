# Parcial #3 – Herramientas de la Programación Aplicada III (.NET)

**Fecha de entrega:** 9 de noviembre de 2026
**Grupo:** 1IL133 · **II Semestre 2026**

---

## 📌 Contenido y propósito

Este repositorio contiene la parte práctica del Parcial #3. Reúne tres programas de consola en C# que aplican:

1. **Programación orientada a objetos:** una clase abstracta, herencia, un arreglo del tipo padre y polimorfismo.
2. **Serialización JSON** con la biblioteca Newtonsoft.Json.
3. **Seguridad de contraseñas:** un login que guarda solo el hash de BCrypt en una base de datos MySQL y registra cada intento de acceso para su análisis posterior.

**Arquitectura:** una solución de .NET (`Parcial3.sln`) con un proyecto de consola independiente para cada ejercicio. El ejercicio 3 se conecta a MySQL con ADO.NET (`MySql.Data`) y usa consultas parametrizadas para prevenir la inyección SQL.

---

## 🛠️ Tecnologías

| Tecnología | Uso |
|---|---|
| C# / .NET 10 | Lenguaje y plataforma |
| Newtonsoft.Json | Serialización de objetos a JSON (ejercicio 2) |
| BCrypt.Net-Next | Generación y verificación de hashes de contraseñas (ejercicio 3) |
| MySQL 8.0 + MySql.Data | Base de datos del login (ejercicio 3) |
| Visual Studio 2026 / VS Code | Entorno de desarrollo |
| Git y GitHub | Control de versiones |

---

## 👤 Autor

| | |
|---|---|
| **Estudiante** | Elvis Li |
| **Institución** | Universidad Tecnológica de Panamá (UTP) |
| **Facultad** | Facultad de Ingeniería de Sistemas Computacionales |
| **Campus** | Víctor Levi Sasso |
| **Asignatura** | Herramientas de la Programación Aplicada III (.NET) |
| **Grupo** | 1IL133 |
| **Instructora** | Ing. Irina Fong |

---

## ✅ Problemas resueltos

### Ejercicio 1: Jerarquía de figuras con polimorfismo
- Clase abstracta `Figura` con los métodos abstractos `CalcularArea()` y `CalcularPerimetro()`.
- Clases derivadas `Rectangulo`, `Cuadrado` y `Triangulo` (el área del triángulo se calcula con la fórmula de Herón).
- Arreglo unidimensional `Figura[]` que guarda objetos de distintas clases hijas.
- Recorrido polimórfico con `foreach` que muestra el nombre de la clase, su área y su perímetro.

### Ejercicio 2: Trabajar con JSON usando Newtonsoft.Json
- Creación de un objeto con los datos de una persona y su conversión a una cadena JSON con `JsonConvert.SerializeObject()`.

### Ejercicio 3: Login con BCrypt y MySQL
- Registro de usuarios: se genera el hash con `BCrypt.HashPassword()` (factor de trabajo 12) y se guarda en la base de datos.
- Inicio de sesión: la contraseña se verifica con `BCrypt.Verify()`.
- Si el acceso es exitoso, se muestra el nombre del usuario.
- Cada intento, fallido o exitoso, se guarda en la tabla `intentos_login` con el usuario y la fecha.
- La contraseña se oculta con `*` al escribirla.

---

## 📸 Capturas

### Ejercicio 1: Figuras
![Ejercicio 1](capturas/ejercicio1.png)

### Ejercicio 2: JSON
![Ejercicio 2](capturas/ejercicio2.png)

### Ejercicio 3: Login con BCrypt
![Registro e inicio de sesión](capturas/ejercicio3_login.png)

### Ejercicio 3: Tablas en MySQL
![Intentos en MySQL](capturas/ejercicio3_mysql.png)

---

## 📁 Estructura de directorios

```
Parcial3/
├── Parcial3.sln                    # Solución con los 3 proyectos
├── README.md
├── .gitignore
├── capturas/                       # Evidencia de la ejecución
├── Ejercicio1_Figuras/
│   ├── Ejercicio1_Figuras.csproj
│   └── Program.cs                  # Figura, Rectangulo, Cuadrado, Triangulo y Main
├── Ejercicio2_Json/
│   ├── Ejercicio2_Json.csproj
│   └── Program.cs                  # Serialización con Newtonsoft.Json
└── Ejercicio3_LoginBCrypt/
    ├── Ejercicio3_LoginBCrypt.csproj
    ├── Program.cs                  # Login, registro e historial de intentos
    └── BaseDatos/
        ├── LoginBCrypt.sql         # Script de creación de la BD y sus tablas
        └── backup_loginbcrypt.sql  # Backup de la BD exportado desde MySQL Workbench
```

### Base de datos `loginbcrypt`

| Tabla | Columnas |
|---|---|
| `usuarios` | `id`, `nombre_usuario` (único), `nombre_completo`, `password_hash`, `fecha_creacion` |
| `intentos_login` | `id`, `nombre_usuario`, `exitoso` (1 = exitoso, 0 = fallido), `fecha` |

---

## ▶️ Ejecución

**Requisitos:** .NET SDK 10 y MySQL Server 8.0.

```bash
git clone https://github.com/TU_USUARIO/Parcial3.git
cd Parcial3
```

**Ejercicio 1**
```bash
cd Ejercicio1_Figuras
dotnet run
```

**Ejercicio 2**
```bash
cd Ejercicio2_Json
dotnet run
```

**Ejercicio 3**
1. Restaure la base de datos en MySQL Workbench: ejecute `BaseDatos/LoginBCrypt.sql` o importe el backup `BaseDatos/backup_loginbcrypt.sql` (**Server → Data Import**).
2. En `Program.cs`, cambie `TU_CONTRASEÑA` en `CadenaConexion` por la contraseña del usuario `root` de MySQL.
3. Ejecute:
   ```bash
   cd Ejercicio3_LoginBCrypt
   dotnet run
   ```
4. Opción **1**: registrar un usuario. Opción **2**: iniciar sesión. Opción **3**: ver el historial de intentos.

**Consultas para revisar los intentos en MySQL:**
```sql
USE loginbcrypt;
SELECT * FROM intentos_login ORDER BY fecha DESC;

SELECT nombre_usuario, SUM(exitoso) AS exitosos, SUM(NOT exitoso) AS fallidos
FROM intentos_login
GROUP BY nombre_usuario;
```

---

## 📚 Referencias

- [Documentación de C# – Microsoft Learn](https://learn.microsoft.com/es-es/dotnet/csharp/)
- [Clases y miembros abstractos – Microsoft Learn](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/keywords/abstract)
- [Newtonsoft.Json – Documentación oficial](https://www.newtonsoft.com/json/help/html/Introduction.htm)
- [BCrypt.Net-Next – Repositorio en GitHub](https://github.com/BcryptNet/bcrypt.net)
- [MySQL Connector/NET – Documentación](https://dev.mysql.com/doc/connector-net/en/)
- [Sintaxis básica de Markdown – GitHub Docs](https://docs.github.com/es/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax)
