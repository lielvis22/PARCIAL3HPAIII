using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

var persona = new
{
    Nombre = "Elvis Li",
    Edad = 21,
    Carrera = "Ingenieria en Sistemas"
};

string json = JsonConvert.SerializeObject(persona);

Console.WriteLine(json);
