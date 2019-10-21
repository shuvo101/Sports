using Serialize.Linq.Nodes;

namespace IMTestProject.Common.QuerySerialize
{
    public class QueryOptionsNodes
    {
        public ExpressionNode IncludeExpressionNode { get; set; }
        public ExpressionNode FilterExpressionNode { get; set; }
        public ExpressionNode SortingExpressionNode { get; set; }
        public Pagination Pagination { get; set; }
    }
}