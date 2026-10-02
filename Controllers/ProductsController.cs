using Microsoft.AspNetCore.Mvc;
using ProductMaintenance.Constants;
using ProductMaintenance.Exceptions;
using ProductMaintenance.Models;
using ProductMaintenance.Repositories;
using ProductMaintenance.RequestModels;

namespace ProductMaintenance.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        bool alreadyExists = await _repository.CheckExistsAsync(request.Name.Trim(), cancellationToken);

        if (alreadyExists)
        {
            throw new ConflictException(ErrorMessages.PRODUCT_ALREADY_EXISTS);
        }

        var product = new Product
        {
            Name = request.Name.Trim(),
            Price = request.Price,
            Stock = request.Stock
        };

        await _repository.AddAsync(product, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        bool exists = await _repository.CheckExistsAsync(id, cancellationToken);

        if (!exists)
        {
            throw new NotFoundException(ErrorMessages.PRODUCT_NOT_FOUND);
        }

        await _repository.DeleteAsync(id, cancellationToken);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductListItem>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync(cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(ErrorMessages.PRODUCT_NOT_FOUND);
        }

        return Ok(product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        Product? product = await _repository.GetByIdAsync(id, cancellationToken);
        
        if (product is null)
        {
            throw new NotFoundException(ErrorMessages.PRODUCT_NOT_FOUND);
        }

        product.Name = request.Name.Trim();
        product.Price = request.Price;
        product.Stock = request.Stock;

        bool assignedToOtherProduct = await _repository.CheckNameExistsAsync(id, product.Name, cancellationToken);

        if (assignedToOtherProduct)
        {
            throw new ConflictException(ErrorMessages.PRODUCT_NAME_ALREADY_ASSIGNED);
        }

        await _repository.UpdateAsync(product, cancellationToken);

        return Ok();
    }    
}
