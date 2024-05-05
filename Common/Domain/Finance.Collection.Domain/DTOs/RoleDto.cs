namespace Financial.Collection.Domain.DTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserDto> Users { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}
