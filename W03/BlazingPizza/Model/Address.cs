
using System.ComponentModel.DataAnnotations;

namespace BlazingPizza;

public class Address
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public string Line1 { get; set; } = "";

    public string Line2 { get; set; } = "";

    [Required]
    public string City { get; set; } = "";

    [Required]
    public string Region { get; set; } = "";

    [Required]
    public string PostalCode { get; set; } = "";
}
