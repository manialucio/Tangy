using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Models
{
    public class SuccessModelDto:ResponseModelDto
    {
        public SuccessModelDto()
        {
            StatusCode = 200;   

        }
        public object Data { get; set; }
    }
}
