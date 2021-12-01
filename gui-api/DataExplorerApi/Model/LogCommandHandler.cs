using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataExplorerApi.Model
{
    public class LogCommandHandler : IRequestHandler<LogCommand>
    {
        private readonly ILogger<LogCommandHandler> _logger;

        public LogCommandHandler(ILogger<LogCommandHandler> logger) => _logger = logger;

        public Task<Unit> Handle(LogCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("---- Received message: {Message} ----", request.Message);
            return Task.FromResult(Unit.Value);
        }
    }
}
