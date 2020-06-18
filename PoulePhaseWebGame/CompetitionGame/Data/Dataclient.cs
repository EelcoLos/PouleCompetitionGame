using CompetitionGame.Command;
using System.IO;

namespace CompetitionGame.Data
{
    public class Dataclient : ICommandHandler<ExternalRequest,ExternalData>
    {
        public ExternalData Handle(ExternalRequest request)
        {
            string json;
            using (StreamReader r = new StreamReader("ExternalData.json"))
            {
                json = r.ReadToEnd();
            }
            return new ExternalData { Json = json };
        }
    }

    /// <summary>
    /// Any Request type of object
    /// </summary>
    public class ExternalRequest {}

    /// <summary>
    ///  any type of response of an external service
    /// </summary>
    public class ExternalData { public string Json;}
}
