using CompetitionGame.Command;

namespace CompetitionGame.Decorators
{
    public abstract class Decorator<T, TResult> : ICommandHandler<T, TResult>
    {
        protected readonly ICommandHandler<T, TResult> Decorated;

        protected Decorator(ICommandHandler<T, TResult> decorated)
        {
            Decorated = decorated;
        }

        public virtual TResult Handle(T dataObject)
        {
            return Decorated.Handle(dataObject);
        }
    }
}
