namespace EcoinverHub_api.Models.Dto.Create
{
    public class CreateUsuarioDto
    {
        public string UserName { get; set; }
        
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; } // Opcional
        public string name { get; set; }
        public string lastname { get; set; }
    }
}
