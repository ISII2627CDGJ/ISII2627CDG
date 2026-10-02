namespace AppForSEII.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
    }

    public ApplicationUser(string id, string name, string surname, string userName, string dni, int age, string sex)
    {
        Id = id;
        Name = name;
        Surname = surname;
        UserName = userName;
        Email = userName;
        DNI = dni;
        Age = age;
        Sex = sex;
    }

    [StringLength(50)]
    public string? Name { get; set; }

    [StringLength(50)]
    public string? Surname { get; set; }

    [StringLength(10)]
    public string? DNI { get; set; }

    public int Age { get; set; }

    [StringLength(20)]
    public string? Sex { get; set; }

    public List<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();


    public ApplicationUser(string id, string name, string surname, string userName, string dni, int age, string sex, string email)
        : this(id, name, surname, userName, dni, age, sex)
    {
        Email = email;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ApplicationUser other)
        {
            return Id == other.Id &&
                   Name == other.Name &&
                   Surname == other.Surname &&
                   UserName == other.UserName &&
                   DNI == other.DNI &&
                   Age == other.Age &&
                   Sex == other.Sex &&
                   Email == other.Email;
        }
        return false;
    }

  
}