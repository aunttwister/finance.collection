namespace Financial.Collection.Domain.DTOs
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}
