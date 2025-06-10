namespace EcoinverHub_api.Models.Dto.Update
{
    public class UpdateUsuarioDto
    {
        public string UserName { get; set; }    // Si quieres permitir cambiar el nombre de usuario
        public string Email { get; set; }       // Permitir actualizar email
        public int? RoleId { get; set; }        // Si quieres permitir cambiar el rol (opcional)
        public string? Password { get; set; }    // Si quieres permitir cambiar la contraseña (opcional)
        public string name {  get; set; }
        public string lastname { get; set; }
    }
}
