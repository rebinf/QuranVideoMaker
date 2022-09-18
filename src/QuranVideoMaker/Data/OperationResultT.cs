using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuranVideoMaker.Data
{
    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }

        public OperationResult(bool success, string message, T data) : base(success, message)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
