using CompetitionGame.Command;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CompetitionGame.Decorators
{
    public class StopwatchDecorator : Decorator<MatchRequest, MatchResult>
    {
        private ILogger _logger { get; }
        private readonly Stopwatch _stopwatch;

        public StopwatchDecorator(ICommandHandler<MatchRequest, MatchResult> decorated, ILogger logger) : base(decorated)
        {
            _logger = logger;
        }

        public override MatchResult Handle(MatchRequest dataObject)
        {
            _logger.LogInformation("[Match] Starting timer for Match");
            _stopwatch.Start();

            var result = base.Handle(dataObject);

            _stopwatch.Stop();
            _logger.LogInformation("[Match] Stopping timer for Match, time elapsed: {0}", _stopwatch.Elapsed);

            return result;
        }
    }
}
