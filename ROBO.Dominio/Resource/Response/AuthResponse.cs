namespace ROBO.Dominio.Resource.Response
{
    public class AuthResponse
    {
        public AuthResponse()
        {
        }

        public string Token { get; set; }
        public RoboResponse Robo { get; set; }
    }
}
