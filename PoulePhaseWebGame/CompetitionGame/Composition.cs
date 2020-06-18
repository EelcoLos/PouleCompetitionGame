using Autofac;
using CompetitionGame.Command;
using CompetitionGame.Data;
using CompetitionGame.Decorators;
using CompetitionGame.Evaluators;
using CompetitionGame.Factories;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using Microsoft.Extensions.Logging;

namespace CompetitionGame
{
    public static class Composition
    {
        public static void RegisterTypes(this ContainerBuilder builder)
        {
            builder.RegisterType<FootballMatchFactory>().As<MatchFactory>();
            builder.RegisterType<PoissonEvaluator>().As<IPoissonEvaluator>();
            builder.RegisterType<ExternalDataService>().As<ICommandHandler<DataRequest, DataResult>>();
            builder.RegisterType<RoundRobinTournamentSelector>().As<ICommandHandler<RoundRobinRequest, RoundRobinResult>>();
            builder.RegisterType<MatchHandler>().As<ICommandHandler<MatchRequest, MatchResult>>();
            builder.RegisterType<PoissonPotentialOutcomeCalculator>().As<ICommandHandler<CalculatePotentialOutcomeRequest, CalculatePotentialOutcomeResult>>();


            builder.RegisterType<Dataclient>().Named<ICommandHandler<ExternalRequest, ExternalData>>("real");
            builder.RegisterDecorator<ICommandHandler<ExternalRequest, ExternalData>>(
                (c, inner) =>
                    new StopwatchDecorator(inner, c.Resolve<ILogger>()),
                fromKey: "real")
                .Named<ICommandHandler<ExternalRequest, ExternalData>>("decorator1");
        }
    }
}