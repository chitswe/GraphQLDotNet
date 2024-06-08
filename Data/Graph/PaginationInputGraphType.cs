using GraphQL.Types;

namespace Data;

public class PaginationInputGraphType: InputObjectGraphType<PaginationParameter>
{
    public PaginationInputGraphType()
        {

            Name = "PaginationInput";
            Field(p => p.Page, nullable: true);
            Field(p => p.PageSize, nullable: true);
            Field(p => p.Skip, nullable: true);
            Field(p => p.Take, nullable: true);
        }
        public static QueryArgument<PaginationInputGraphType> BuildQueryArgument()
        {
            return new QueryArgument<PaginationInputGraphType> { Name = "pagination", DefaultValue = new PaginationParameter { Page = 1, PageSize = 20 } };
        }
}
