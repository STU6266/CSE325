
using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    // GET: /pizza
    [HttpGet]
    public ActionResult<List<Pizza>> GetAll()
    {
        return PizzaService.GetAll();
    }

    // GET: /pizza/1
    [HttpGet("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = PizzaService.Get(id);

        if (pizza is null)
            return NotFound();

        return pizza;
    }

    // POST: /pizza
    [HttpPost]
    public IActionResult Create(Pizza pizza)
    {
        PizzaService.Add(pizza);

        return CreatedAtAction(
            nameof(Get),
            new { id = pizza.Id },
            pizza);
    }

    // PUT: /pizza/1
    [HttpPut("{id}")]
    public IActionResult Update(int id, Pizza pizza)
    {
        if (id != pizza.Id)
            return BadRequest();

        if (PizzaService.Get(id) is null)
            return NotFound();

        PizzaService.Update(pizza);
        return NoContent();
    }

    // DELETE: /pizza/1
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (PizzaService.Get(id) is null)
            return NotFound();

        PizzaService.Delete(id);
        return NoContent();
    }
}
