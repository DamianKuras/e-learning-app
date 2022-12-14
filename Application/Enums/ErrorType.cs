using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Enums
{
    public enum ErrorType
    {
        BadRequest=400,
        Unauthorized=401,
        NotFound=404,

        InternalServerError = 500
    }
}
