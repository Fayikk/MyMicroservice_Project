using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Base
{
    public class BaseResponse
    {
        public object Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess {get; set;}   
    }
}