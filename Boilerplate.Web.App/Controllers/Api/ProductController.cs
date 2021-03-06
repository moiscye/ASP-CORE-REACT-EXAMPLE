﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boilerplate.Web.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Boilerplate.Web.App.Controllers.Api
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly SalesDBContext _context;

    public ProductController(SalesDBContext context)
    {
      _context = context;

      if (_context.Product.Count() == 0)
      {
        // Create a new TodoItem if collection is empty,
        // which means you can't delete all TodoItems.

        _context.Product.Add(new Product { Name = "Item1", Price = 10 });
        _context.SaveChanges();
      }
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int page, int numPerPage)
    {
      if (page == -1 & numPerPage == -1)
      {
        return await _context.Product.ToListAsync();
      }
      var product = await _context.Product.ToListAsync();
      var result = product.Skip((page - 1) * numPerPage).Take(numPerPage);
      return Ok(result);
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
      var product = await _context.Product.FindAsync(id);

      if (product == null)
      {
        return NotFound();
      }

      return product;
    }

    // POST: api/Products
    [HttpPost]
    public async Task<ActionResult<Product>> PostTodoItem(Product product)
    {
      _context.Product.Add(product);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }


    // PUT: api/Products/
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, Product item)
    {
      if (id != item.Id)//fix this one to query the db
      {
        return BadRequest();
      }

      _context.Entry(item).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/Products
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
      var item = await _context.Product.FindAsync(id);

      if (item == null)
      {
        return NotFound();
      }

      _context.Product.Remove(item);
      await _context.SaveChangesAsync();

      return NoContent();
    }




  }
}
