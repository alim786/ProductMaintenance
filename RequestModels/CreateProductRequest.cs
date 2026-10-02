using ProductMaintenance.Constants;
using System.ComponentModel.DataAnnotations;

namespace ProductMaintenance.RequestModels;

public class CreateProductRequest
{
    [Required]
    [StringLength(ProductConstants.PRODUCT_NAME_MAXLENGTH, MinimumLength = ProductConstants.PRODUCT_NAME_MINLENGTH)]
    public string Name { get; set; } = string.Empty;

    [Range(ProductConstants.PRODUCT_PRICE_MIN, ProductConstants.PRODUCT_PRICE_MAX)]
    public decimal Price { get; set; }

    [Range(ProductConstants.PRODUCT_STOCK_MIN, ProductConstants.PRODUCT_STOCK_MAX)]
    public int Stock { get; set; }
 }
