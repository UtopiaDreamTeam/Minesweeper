namespace Minesweeper.Core.Interfaces
{
    public interface IRunOnMainThread
    {
        public void Run(Action action);
    }
}
