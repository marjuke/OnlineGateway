namespace Gateway.Model
{
    public record class Contact(Guid id,string value)
    {
        public Contact(string value)
        : this(Guid.Empty, value) { }
    }
}
