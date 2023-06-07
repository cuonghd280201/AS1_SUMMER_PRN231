using BuisinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFlowerBouquestRepository
    {
        Task<IEnumerable<FlowerBouquet>> GetFlowers();
        Task<IEnumerable<FlowerBouquet>> GetFlowers(int categoryId);

        Task<IEnumerable<FlowerBouquet>> GetFlowersSupplier(int supplierId);
        Task<FlowerBouquet> GetFlowersById(int flowerId);

        Task<FlowerBouquet> UpdateFlowerBouquest(FlowerBouquet updateFlower);

        Task<FlowerBouquet> AddFlowerBouquest(FlowerBouquet newFlower);

        Task DeleteFlowerBouquest(int flowerId);

        
    }
}
