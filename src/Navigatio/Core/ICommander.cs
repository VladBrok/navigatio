namespace Navigatio;

public interface ICommander
{
    CommandData? Get(string name);
    IEnumerable<CommandData> GetAll();
}
