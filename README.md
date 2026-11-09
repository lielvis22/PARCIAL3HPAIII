# Parcial #3 – Herramientas de la Programación Aplicada III (.NET)

| Carpeta | Contenido |
|---|---|
| `Ejercicio1_Figuras` | Clase abstracta `Figura`, clases `Rectangulo`, `Cuadrado`, `Triangulo`, arreglo `Figura[]` y recorrido polimórfico |
| `Ejercicio2_Json` | Serialización de un objeto a JSON con Newtonsoft.Json |
| `Ejercicio3_LoginBCrypt` | Login con BCrypt.Net-Next y SQL Server (usuarios + historial de intentos) |

## Cómo ejecutar

```bash
cd Ejercicio1_Figuras && dotnet run
cd Ejercicio2_Json && dotnet run
```

### Ejercicio 3
1. Abra `Ejercicio3_LoginBCrypt/BaseDatos/LoginBCrypt.sql` en SQL Server Management Studio y ejecútelo (crea la BD `LoginBCrypt` y las tablas `Usuarios` e `IntentosLogin`).
2. Si su instancia no es `localhost`, cambie `CadenaConexion` en `Program.cs` (ej. `localhost\SQLEXPRESS`).
3. `cd Ejercicio3_LoginBCrypt && dotnet run`
4. Opción 1 → registrar un usuario (se genera y guarda el hash). Opción 2 → iniciar sesión (`BCrypt.Verify()`); si es exitoso, muestra el nombre del usuario. Opción 3 → ver intentos.
5. Backup para el repositorio: ejecute el `BACKUP DATABASE` que está al final del script y copie el `.bak` a `Ejercicio3_LoginBCrypt/BaseDatos/`.
