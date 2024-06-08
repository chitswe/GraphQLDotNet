using Data.Ef;

public class EntityConnection<E> where E: Entity{
    public PageInfo PageInfo{get; private set;}
    public List<E> Edges{get;  set;}
    public EntityConnection(PageInfo? pageInfo=null){
        if (pageInfo==null)
            pageInfo = new PageInfo(){
                HasNextPage =false,
                HasPreviousPage=false,
                CurrentPage = 0,
                PageSize=80,
                RowCount=0,
                PageCount=0
            };
        this.PageInfo = pageInfo;
        this.Edges = new List<E>();
    }
}

public class PageInfo{
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public long RowCount { get; set; }
    public long PageCount { get; set; } 
}