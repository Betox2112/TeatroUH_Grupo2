using System;
using System.Collections.Generic;
using System.Text;

namespace TeatroUH.Application.Interfaces
{
    public interface ICheckoutService
    {
        int CreateOrder(string customerName, string customerEmail);
    
    
    
        Task<bool> ConfirmPurchaseAsync();
    }


}
