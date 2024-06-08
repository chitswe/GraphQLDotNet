using Data.Graph;
using GraphQL.Types;

public class PageInfoGraphType : ObjectGraphType<PageInfo>
    {
        public PageInfoGraphType()
        {
            Field(x => x.HasNextPage, nullable: true, typeof(NonNullGraphType<BooleanGraphType>)).Description("If true, next page is availabe.");
            Field(x => x.HasPreviousPage, nullable: true, typeof(NonNullGraphType<BooleanGraphType>)).Description("If true, previous page is available");
            Field(x => x.RowCount, nullable: true, typeof(NonNullGraphType<IntGraphType>)).Description("Total number of rows to be fetched");
            Field(x => x.PageCount, nullable: true, typeof(NonNullGraphType<IntGraphType>)).Description("Total number of pages to be fetched");
            Field(x => x.CurrentPage, nullable: true, typeof(NonNullGraphType<IntGraphType>)).Description("Current page number of result set.");
            Field(x => x.PageSize, nullable: true, typeof(NonNullGraphType<IntGraphType>)).Description("Number of rows in a page");
        }
    }