using DotNetAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Books.Models;

namespace DotNetAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BooksService _booksService;

    public BooksController(BooksService booksService) =>
        _booksService = booksService;

    /// <summary>
    /// Get List of Books with pagination.
    /// </summary>
    [HttpGet]
    public async Task<object> Get(int page = 1, int perPage = 10)
    {
        var skip = (page - 1) * perPage;
        var limit = perPage;

        var books = await _booksService.GetAsync(skip, limit);

        var totalCount = await _booksService.GetCountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / perPage);

        return new
        {
            status = 200,
            message = "Success",
            pagination = new
            {
                page,
                perPage,
                totalPages,
                totalCount
            },
            data = books,
        };
    }

    /// <summary>
    /// Get BOOK by ID.
    /// </summary>
    [HttpGet("{id:length(24)}")]
    public async Task<object> Get(string id)
    {
        var book = await _booksService.GetAsync(id);

        if (book is null)
        {
            return new {
                status = 404,
                message = "Book not found",
                data = new {}
            };
        };

        return new {
            status = 200,
            message = "Success",
            data = book,
        };
    }

    /// <summary>
    /// Create a new Book.
    /// </summary>
    [HttpPost]
    public async Task<object> Post(Book newBook)
    //  IActionResult
    {
        await _booksService.CreateAsync(newBook);

        return new {
            status = 201,
            message = "Successfull created record",
            data = CreatedAtAction(nameof(Get), new { id = newBook.ID }, newBook).Value,
        };
    }

    /// <summary>
    /// Update a Book.
    /// </summary>
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Book updatedBook)
    {
        var book = await _booksService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedBook.ID = book.ID;

        await _booksService.UpdateAsync(id, updatedBook);

        return NoContent();
    }

    /// <summary>
    /// Delete a Book.
    /// </summary>
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _booksService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await _booksService.RemoveAsync(id);

        return NoContent();
    }
}