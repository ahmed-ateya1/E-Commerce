namespace E_Commerce.Core.Dtos.VoteDto
{
    public class VoteResponse : VoteBase
    {
        public Guid VoteID { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string VoteType { get; set; }
    }
}
