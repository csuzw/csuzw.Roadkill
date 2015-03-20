
namespace csuzw.Roadkill.Core
{
    public interface ITextPlugin
    {
        string BeforeParse(string markupText);
        string AfterParse(string html);
    }
}
