using WebApplication1.Models.Enum;

namespace WebApplication1.Models {
    public class WhereSQL {
        public string[] SQLClauses { get; set; } = new string[0];
        public WhereUnion WhereUnion { get; set; } = WhereUnion.and;
        public string GetClausule() { return $" {((SQLClauses.Length > 0) ? "WHERE" : "")} {String.Join($" { WhereUnion.ToString().Split(".").LastOrDefault()!.ToUpper()} ",SQLClauses)}"; }
    }
}
