namespace PMPK.Models
{
    public class Document : BaseType
    {
        public override int Id { get; set; }
        public int ChildrenId { get; set; }
        public Children Children { get; set; }
        public Passport Passport { get; set; }
    }
}
