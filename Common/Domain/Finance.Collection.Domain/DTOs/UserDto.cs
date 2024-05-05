namespace Financial.Collection.Domain.DTOs
{
    public class UserDto
    {
        public UserDto()
        {
            
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<RoleDto> Roles { get; set; }
        public List<NoteDto> Notes { get; set; }
        public List<TickerListDto> TickerLists { get; set; }
        public ConfigurationDto Configuration { get; set; }
    }
}
