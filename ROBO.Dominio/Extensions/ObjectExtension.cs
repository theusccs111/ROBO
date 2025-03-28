namespace ROBO.Dominio.Extensions
{
    public static class ObjectExtension
    {
        public static T Clonar<T>(this T objetoParaClonar)
        {
            return objetoParaClonar.Clonar(1);
        }

        public static T ClonarProfundo<T>(this T objetoParaClonar, params string[] propriedadesParaIgnorar)
        {
            return objetoParaClonar.Clonar(int.MaxValue, propriedadesParaIgnorar);
        }
        public static T ClonarProfundo<T>(this T objetoParaClonar)
        {
            return objetoParaClonar.Clonar(int.MaxValue);
        }

        public static T Clonar<T>(this T objetoParaClonar, int profundidade, params string[] propriedadesParaIgnorar)
        {
            var json = ClonagemViaJsonSerializer.SerializeObject(objetoParaClonar, profundidade, propriedadesParaIgnorar);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static T Clonar<T>(this T objetoParaClonar, int profundidade)
        {
            return objetoParaClonar.Clonar(profundidade, new string[] { });
        }
    }
}
