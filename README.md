# NewDotNet
A sample API project built in ASP.NET 7 using MongoDB Database.

Running the Project:
```bash
dotnet run
```

Swagger: http://localhost:5269/swagger/index.html

## API Routes

#### Get List of Books
Fetch the list of books with pagination params.
```
GET {{host}}/api/v1/books?page=1&perPage=10
```

#### Get a Book by ID
Fetch a book by it's ID.
```
GET {{host}}/api/v1/books/{{id}}
```

#### Create a Book
Create a new record of the book.
```
POST {{host}}/api/v1/books

Request Body:
{
    "Name": "Book Name",
    "Price": 9.99,
    "Category": "Category Name",
    "Author": "Author Name"
}
```

#### Update a Book
```
POST {{host}}/api/v1/books/{{id}}

Request Body:
{
    "Name": "Book Name",
    "Price": 9.99,
    "Category": "Category Name",
    "Author": "Author Name"
}
```

#### Delete a Book
```
DELETE {{host}}/api/v1/books/{{id}}
```
