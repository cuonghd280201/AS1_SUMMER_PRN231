﻿using BuisinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public async Task<IEnumerable<OrderDetail>> GetOrderDetail(int orderId)
        {
            return await OrderDetailDAO.Instance.GetOrderDetails(orderId);
        }
    }
}
