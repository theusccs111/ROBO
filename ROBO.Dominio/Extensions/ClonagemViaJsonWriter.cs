using Newtonsoft.Json;

namespace ROBO.Dominio.Extensions
{
    public class ClonagemViaJsonWriter : JsonTextWriter
    {
        public ClonagemViaJsonWriter(TextWriter textWriter) : base(textWriter) { }

        public int Depth { get; private set; }

        public override void WriteStartObject()
        {
            Depth++;
            base.WriteStartObject();
        }

        public override void WriteEndObject()
        {
            Depth--;
            base.WriteEndObject();
        }
    }
}