namespace CompetitionGame.Command
{
    public interface ICommandHandler<in T, out TResult>
    {
        TResult Handle(T request);
    }
}