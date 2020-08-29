# Introduction

During my recent work spent generally on developing RESTful APIs, I noticed that a REST call is built in kind od "stream" or "pipeline" way. It means:
- some input is taken for processing
- taken input is validated
- if validation fails then API returns error, otherwise:
- processing moves forward, sometimes fetches more data (like querying DB) and the process verifies if fetched data can be processed with the given input
- processing is summarized/finalized, operation result is created and sometimes wrapped into a generic object (response)

To address this scenario, I created simple library that gives you a posibillity to process input like a pipeline. 

First of all you will need to know base error type returned from your code:
```c#
public class GenericError // no specific base type required, can be struct
{
    public string Message { get; set; }
    public int SomeCode { get; set; }    
}

//....
{
    IFlowBuilder<TFilteringError> builder = 
        new StandardBuilder()
            .UseErrorType<GenericError>();
}

```

When you know what type of errors your code might return then you can do some tuning:


```c#
builder
    .OnChanging(() =>
        {
            // this lambda would be executed before any Apply() or Finalize() invocation
        }
    ).OnChanged(() =>
        {
            // this lambda would be executed after any Apply() or Finalize() invocation
        });

````

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
        if (_context.Set<Author>().Count(a => a.Id == authorId) == 0)
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
    private IFlowBuilder<TFilteringError> _builder;
    public BookService(DBContext context, IFlowBuilder<TFilteringError> builder)
    {
        _context = context;
        _builder = builder;
    }

    public Result<Book> Create(string title, DateTime published, int authorId)
    {
        var (res, _, _) = _builder
            .For((title, published, authorId))
            .Check(x => x.title, t => !string.IsNullOrWhiteSpace(t), () => new TitleError())
            .Check(x => x.published, p => t.Year >= 2000, () => new YearError())
            .Check(x => x.authorId,
                        aId => _context
                                    .Set<Author>()
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
                return newBook;
            })
            .Project(v => new Result { Value = v })
            .Sink();
        return res.Value;
    }
}

```

OOTB Exception handling:
```c#
[Fact]
public void Throw_Catch()
{
    var (res, ex, _) = _builder
        .For("input")
        .Finalize(x => { throw new NotImplementedException(); return x; })
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
    var (res, ex, errors) = _builder
        .For("input")
        .Apply(x => 10)
        .Check(x => x == 0, () => new NotZero())
        .Finalize(x => { throw new NotImplementedException(); return x; })
        .Sink();
    
    Assert.True(res.Failed);
    Assert.IsNull(ex);
    Assert.Single(errors);
}

```

You can notify 3rd party components that something happened (but specific integration you need to do on yourself - 
no integration with any libraries has been done so far)

```c#
// LatePublishEventReceiver will raise all events when Sink() is done
// ImmediatePublishEventReceiver will raise event when Raise() is called
class SampleLatepublishEventReceiver : LatePublishEventReceiver
{
    protected virtual Action CreatePublisher<TEvent>(TEvent e) => () => Console.WriteLine("Hello World!");
}

// ...

new StandardBuilder()
    .UseErrorType<GenericError>()
    .EnableEventSubscription(new GenericEventReceiverFactory<SampleLatepublishEventReceiver>())
    .For(default(int))
    .Raise(x => new TestingEvent())
    .Finalize(x => x)
    .Sink();

```

You can map Exception to your custom error type and deconstruct pipeline result to
```c#
var (result, errors)
```
instead of
```c#
var (result, exception, errors)
```
How to map Exception to Error:
```c#
public void MapExceptionToErrorExample()
{
    var (_, errors) = _builder
        .MapExceptionToErrorOnDeconstruct(ex => new GenericError { Message = ex.Message, SomeCode = ex.HResult })
        .For(0)
        .Finalize(x =>
        {
            throw new Exception();
            return x;
        })
        .Sink();
    Assert.Single(errors);
}
```
