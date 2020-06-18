using CompetitionGame.Command;
using CompetitionGame.Data;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CompetitionGame.Decorators
{
    public class StopwatchDecorator : Decorator<ExternalRequest, ExternalData>
    {
        private ILogger _logger { get; }
        private readonly Stopwatch _stopwatch;

        public StopwatchDecorator(ICommandHandler<ExternalRequest, ExternalData> decorated, ILogger logger) : base(decorated)
        {
            _logger = logger;
        }

        public override ExternalData Handle(ExternalRequest dataObject)
        {
            _logger.LogInformation("[ExternalService] Starting timer for ExternalService");
            _stopwatch.Start();

            var result = base.Handle(dataObject);

            _stopwatch.Stop();
            _logger.LogInformation("[ExternalService] Stopping timer for ExternalService, time elapsed: {0}", _stopwatch.Elapsed);

            return result;
        }
    }
}
