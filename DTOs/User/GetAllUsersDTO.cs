namespace CRM.DTOs.User
{
    public class GetAllUsersDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; } 
    }
}
