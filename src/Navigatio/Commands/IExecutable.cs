namespace Navigatio.Commands;

public interface IExecutable
{
    bool Execute(params string[] args);
}
