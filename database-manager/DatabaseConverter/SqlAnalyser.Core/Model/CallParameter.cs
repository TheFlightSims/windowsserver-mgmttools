namespace SqlAnalyser.Model
{
    public class CallParameter
    {
        public TokenInfo Name { get; set; }
        public TokenInfo Value { get; set; }
        public ParameterType ParameterType { get; set; }
        public bool IsDescription { get; set; }
    }
}
