using Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Models
{
    public class Result<T>
    {
        public T Payload { get; set; }
        public bool IsError { get; set; }

        public ErrorType ErrorType {get;set;}
        public List<string> Errors { get; set; } = new List<string>();
    }
}
