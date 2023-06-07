using BuisinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System.Data;

namespace FlowerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository supplierRepository;

        public SuppliersController(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        // api/Suppliers
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSupplier()
        {
            try
            {
                return StatusCode(200, await supplierRepository.GetSupplier());
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // api/Supplier/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSupplier(int categoryId)
        {
            try
            {
                Supplier supplier = await supplierRepository.GetSupplier(categoryId);
                if (supplier == null)
                {
                    return StatusCode(404, "Supplier is not existed!!");
                }
                return StatusCode(200, supplier);
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
