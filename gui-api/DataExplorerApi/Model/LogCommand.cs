using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExplorerApi.Model
{
    public class LogCommand : IRequest<Unit>
    {
        public string Identifier { get; set; }
        public string Message { get; set; }
    }
}
