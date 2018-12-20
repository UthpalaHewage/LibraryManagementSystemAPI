using AutoMapper;
using LibrarySystem.Dtos;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace LibrarySystem.Controllers
{
    public class BooksController : ApiController
    {
        LibraryContext _context;
        public BooksController()
        {
            _context = new LibraryContext();
        }

        [HttpGet]
        //GET /api/Books
        //public IEnumerable<Book> GetBooks()

        public IHttpActionResult GetBooks()
        {
            return Ok(_context.Books.ToList().Select(Mapper.Map<Book, BookDto>));
        }

        [HttpGet]
        //GET /api/Books/1
        public IHttpActionResult GetBook(int id)
        {

            Book book = _context.Books.SingleOrDefault(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
                //throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Ok(Mapper.Map<Book, BookDto>(book));
        }

        [HttpPost]
        //POST /api/Books
        public IHttpActionResult CreateBook(BookDto bookDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var book = Mapper.Map<BookDto, Book>(bookDto);
            _context.Books.Add(book);
            _context.SaveChanges();

            bookDto.Id = book.Id;

            return Created(new Uri(Request.RequestUri + "/" + book.Id), bookDto);

        }


        //PUT /api/Books/1
        [HttpPut]
        public void UpdateBook(int id, BookDto bookDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var bookInDb = _context.Books.SingleOrDefault(c => c.Id == id);

            if (bookInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            Mapper.Map(bookDto, bookInDb);

            //Mapper.Map<BookDto, Book>(bookDto, bookInDb);
            /*
            bookInDb.BookNumber = book.BookNumber;
            bookInDb.BookName = book.BookName;
            bookInDb.BookPubName = book.BookPubName;
            bookInDb.BookPrice = book.BookPrice;
            bookInDb.Author = book.Author;
            bookInDb.BookBuyYear = book.BookBuyYear;
            bookInDb.BookAvailability = book.BookAvailability;
            */

            _context.SaveChanges();
        }

        //DELETE /api/Books/1
        [HttpDelete]
        public void DeleteBook(int id)
        {
            var bookInDb = _context.Books.SingleOrDefault(c => c.Id == id);

            if (bookInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Books.Remove(bookInDb);
            _context.SaveChanges();
        }

    }
}
