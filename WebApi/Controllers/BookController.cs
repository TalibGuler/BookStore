using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.BookOperations.CreateBook;
using WebApi.BookOperations.GetBooks;
using WebApi.DBOperations;
using static WebApi.BookOperations.CreateBook.CreateBookCommand;

namespace WebApi.Controller
{

    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {

        private readonly BookStoreDbContext context;

        public BookController(BookStoreDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            GetBooksQuery query = new GetBooksQuery(context);
            var result = query.Handle();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public Book GetById(int id)
        {
            var book = context.Books.Where(book => book.Id == id).SingleOrDefault();
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
        public IActionResult AddBook([FromBody] CreateBookModel newbook)
        {
            CreateBookCommand command = new CreateBookCommand(context);
            try
            {
                command.Model = newbook;
                command.Handle();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book updateBook)
        {

            var book = context.Books.SingleOrDefault(x => x.Id == id);

            if (book is null)
                return BadRequest();

            book.GenreId = updateBook.GenreId != default ? updateBook.GenreId : book.GenreId;
            book.PageCount = updateBook.PageCount != default ? updateBook.PageCount : book.PageCount;
            book.PublishDate = updateBook.PublishDate != default ? updateBook.PublishDate : book.PublishDate;
            book.Title = updateBook.Title != default ? updateBook.Title : book.Title;

            context.SaveChanges();

            return Ok();

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = context.Books.SingleOrDefault(x => x.Id == id);
            if (book is null)
                return BadRequest();

            context.Books.Remove(book);
            context.SaveChanges();
            return Ok();
        }
    }

}