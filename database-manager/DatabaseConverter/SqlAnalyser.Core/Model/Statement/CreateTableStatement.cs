using DatabaseInterpreter.Model;

namespace SqlAnalyser.Model
{
    public class CreateTableStatement : CreateStatement
    {
        public override DatabaseObjectType ObjectType => DatabaseObjectType.Table;
        public TableInfo TableInfo { get; set; }
    }
}
