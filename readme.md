# Introduction

During my recent work spent generally on developing RESTful APIs, I noticed that a REST call is built in kind od "stream" or "pipeline" way. It means:
- some input is taken for processing
- taken input is validated
- if validation fails then API returns error, otherwise:
- processing moves forward, sometimes fetches more data (like querying DB) and the process verifies if fetched data can be processed with the given input
- processing is summarized/finalized, operation result is created and sometimes wrapped into a generic object (respose)

To address this scenario, I created simple library that gives you a posibillity to process input like in a pipeline. 

Below you can find a simple code sample that creates new Book entity, with a given title publish date and author (given as author id):

```c#
public class BookService
{
    private DBContext _context;
    public BookService(DBContext context)
    {
        _context = context;
    }

    public Result<Book> Create(string title, DateTime published, int authorId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("nope");
        }
        if (published.Year < 2000)
        {
            throw new ArgumentException("I don't like old millennium");
        }
        if (_context.Entities<Author>().Count(a => a.Id == authorId) == 0)
        {
            throw new KeyNotFoundException("Sorry, given author does not exist");
        }
        var newBook = new Book 
        {
            AuthorId = authorId,
            Title = title,
            Published = published
        };
        _context.Attach(newBook);
        _context.SaveChanges();
        return new Result { Value = newBook };
    }
}
```

With PipeSharp you can write the following code:

```c#
public class BookService
{
    private DBContext _context;
    private IFlowFactory _flowFactory;
    public BookService(DBContext context, IFlowFactory flowFactory)
    {
        _context = context;
        _flowFactory = flowFactory;
    }

    public Result<Book> Create(string title, DateTime published, int authorId)
    {
        var (res, _, _) = _flowFactory
            .For((title, published, authorId))
            .Check(x => x.title, t => !string.IsNullOrWhiteSpace(t), () => new TitleError())
            .Check(x => x.published, p => t.Year >= 2000, () => new YearError())
            .Check(x => x.authorId,
                        aId => _context
                                    .Entities<Author>()
                                    .Count(a => a.Id == aId) == 1, 
                        () => new NoAuthorError())
            .Finalize(x => 
            {
                var newBook = new Book 
                {
                    AuthorId = authorId,
                    Title = title,
                    Published = published
                };
                _context.Attach(newBook);
                _context.SaveChanges();
            })
            .Project(v => new Result { Value = v })
            .Sink();
        retutn res;
    }
}

```

OOTB Exception handling:
```c#
[Fact]
public void Throw_Catch()
{
    var (res, ex, _) = _factory
        .For("input")
        .Finalize(x => throw new NotImplementedException())
        .Sink();
    
    Assert.True(res.Failed);
    Assert.IsType<NotImplementedException>(ex);
}
```

When you check input or applied changes it does not mean you throw exception:

```c#
[Fact]
public void CheckFailed_ValidationErrorExpected()
{
    var (res, ex, filteringErrors) = _factory
        .For("input")
        .Apply(x => 10)
        .Check(x => x == 0, () => new NotZero())
        .Finalize(x => throw new NotImplementedException())
        .Sink();
    
    Assert.True(res.Failed);
    Assert.IsNull(ex);
    Assert.Single(filteringErrors);
}

```
