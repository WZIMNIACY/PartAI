using System.Threading.Tasks;

public interface ILLM
{
    string ModelName { get; set; }
    Task<string> Ask(string prompt);
}
