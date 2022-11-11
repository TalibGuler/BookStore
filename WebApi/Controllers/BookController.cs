using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller
{

    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {

        private static List<Book> BookList = new List<Book>(){
            new Book{
                Id=1,
                Title="Learn Startup",
                GenreId=1, //Personal Growth
                PageCount=200,
                PublishDate = new DateTime(2001,06,12)
            },

               new Book{
                Id=2,
                Title="Hearland",
                GenreId=2, //Science Fiction
                PageCount=250,
                PublishDate = new DateTime(2010,05,23)
            },

               new Book{
                Id=3,
                Title="Dune",
                GenreId=2, //Science Fiction
                PageCount=540,
                PublishDate = new DateTime(2001,12,21)
            }
        };

        [HttpGet]
        public List<Book> GetBooks()
        {
            var bookList = BookList.OrderBy(x => x.Id).ToList<Book>();
            return bookList;
        }

        [HttpGet("{id}")]
        public Book GetById(int id)
        {
            var book = BookList.Where(book => book.Id == id).SingleOrDefault();
            if (book == null)
            {
                return null;
            }
            else
                return book;
        }

        // [HttpGet]
        // public Book Get([FromQuery] string id)
        // {
        //     var book = BookList.Where(book => book.Id == Convert.ToInt32(id)).SingleOrDefault();
        //     if (book == null)
        //     {
        //         return null;
        //     }
        //     else
        //         return book;
        // }

        [HttpPost]
        public IActionResult AddBook ([FromBody] Book newBook){
            var book = BookList.SingleOrDefault(x => x.Title == newBook.Title);
            if(book is not null)
            return BadRequest();

            BookList.Add(newBook);
            return Ok();
        }


        [HttpPut("{id}")]
        public IActionResult UpdateBook (int id , [FromBody] Book updateBook){

            var book = BookList.SingleOrDefault(x => x.Id == id);

            if(book is null)
            return BadRequest();

            book.GenreId = updateBook.GenreId != default ? updateBook.GenreId: book.GenreId;
            book.PageCount = updateBook.PageCount != default ? updateBook.PageCount: book.PageCount;
            book.PublishDate = updateBook.PublishDate != default ? updateBook.PublishDate : book.PublishDate;
            book.Title = updateBook.Title != default ? updateBook.Title: book.Title;

            return Ok();

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook (int id){
            var book = BookList.SingleOrDefault(x => x.Id == id);
            if(book is null)
            return BadRequest();

            BookList.Remove(book);
            return Ok();
        }
    }

}