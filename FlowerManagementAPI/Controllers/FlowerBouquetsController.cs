using BuisinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System;

namespace FlowerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class FlowerBouquetsController : ControllerBase
    {
        private readonly IFlowerBouquestRepository flowerBouquestRepository;

        public FlowerBouquetsController(IFlowerBouquestRepository flowerBouquestRepository)
        {
            this.flowerBouquestRepository = flowerBouquestRepository;
        }

        // api/Flowers
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FlowerBouquet>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFlowers()
        {
            try
            {
             
                return StatusCode(200, await flowerBouquestRepository.GetFlowers());
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

        // api/Flowers/category/5
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(typeof(IEnumerable<FlowerBouquet>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFlowers(int categoryId)
        {
            try
            {
                return StatusCode(200, await flowerBouquestRepository.GetFlowers(categoryId));
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

        //api/Flowers/supplier/1
        [HttpGet("supllier/{supplierId}")]
        [ProducesResponseType(typeof(IEnumerable<FlowerBouquet>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFlowersSupplier(int supplierId)
        {
            try
            {
                return StatusCode(200, await flowerBouquestRepository.GetFlowersSupplier(supplierId));
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



        // api/Flowers/1
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FlowerBouquet), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFlowerById(int id)
        {
            try
            {
                FlowerBouquet flowerBouquet = await flowerBouquestRepository.GetFlowersById(id);
                if(flowerBouquet == null)
                {
                    return StatusCode(404, "Flower is not exist!");
                }
                return StatusCode(200, flowerBouquet);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(400, ex.Message);
            
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // api/Flowers/1

        //[HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> PutFlower([FromForm]int id, FlowerBouquet flowerBouquet)
        {
            if(id != flowerBouquet.FlowerBouquetId)
            {
                return StatusCode(400, "Id is not !");
            }
            try
            {
                await flowerBouquestRepository.UpdateFlowerBouquest(flowerBouquet);
                return StatusCode(204, "Update successfully");
            }
            catch (ApplicationException ex)
            {
                return StatusCode(400, ex.Message);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // api/Flowers

        [HttpPost]
        [ProducesResponseType(typeof(FlowerBouquet), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> PostFlower(FlowerBouquet flowerBouquet)
        {
            try
            {
                FlowerBouquet addFlower;
                addFlower = await flowerBouquestRepository.AddFlowerBouquest(flowerBouquet);
                return StatusCode(201, addFlower);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(400, ex.Message);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        //   api/Flowers/1

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteFLower(int id)
        {
            try
            {
                await flowerBouquestRepository.DeleteFlowerBouquest(id);
                return StatusCode(204, "Delete successfully");
            }
            catch (ApplicationException ex)
            {
                return StatusCode(400, ex.Message);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }





    }
}
