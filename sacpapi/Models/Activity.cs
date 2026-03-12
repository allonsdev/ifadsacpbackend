
namespace sacpapi.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string NameInShort { get; set; }
        public int OutputId { get; set; }

    }
}
