namespace DeploymentToolkit.Scripting.Tests
{
    public class ExpectedScript
    {
        public string Name { get; set; }
        public string Script { get; set; }
        public string Environment { get; set; }
        public bool Result { get; set; }
        public ExpectedConditon TestCondition { get; set; }
    }
}
