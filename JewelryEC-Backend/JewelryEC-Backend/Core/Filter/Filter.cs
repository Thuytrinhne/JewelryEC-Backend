namespace JewelryEC_Backend.Core.Filter
{
    public class Filter
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public string Logic { get; set; } // This is the nested "logic" property for the inner filters array
        public List<Filter> Filters { get; set; } // Nested filters array
    }
}
