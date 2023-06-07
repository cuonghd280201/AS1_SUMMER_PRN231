using BuisinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FlowerBouquestRepository : IFlowerBouquestRepository
    {
        public async Task<FlowerBouquet> AddFlowerBouquest(FlowerBouquet newFlower)
        {
            return await FlowerBouquestDAO.Instance.AddFlowerBouquest(newFlower);
        }

        public async Task DeleteFlowerBouquest(int flowerId)
        {
             await FlowerBouquestDAO.Instance.DeleteFlowerBouquest(flowerId);
        }

        public async Task<IEnumerable<FlowerBouquet>> GetFlowers()
        {
            return await FlowerBouquestDAO.Instance.GetFlowers();
        }

        public async Task<IEnumerable<FlowerBouquet>> GetFlowers(int categoryId)
        {
            return await FlowerBouquestDAO.Instance.GetFlowers(categoryId);
        }

        public async Task<FlowerBouquet> GetFlowersById(int flowerId)
        {
            return await FlowerBouquestDAO.Instance.GetFlowersById(flowerId);
        }

        public async Task<IEnumerable<FlowerBouquet>> GetFlowersSupplier(int supplierId)
        {
            return await FlowerBouquestDAO.Instance.GetFlowersSupplier(supplierId);
        }

        public async Task<FlowerBouquet> UpdateFlowerBouquest(FlowerBouquet updateFlower)
        {
            return await FlowerBouquestDAO.Instance.UpdateFlowerBouquest(updateFlower);
        }
    }
}
