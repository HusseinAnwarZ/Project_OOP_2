using System;
using System.Collections.Generic;

class Books
{
    public string NameOfBook { get; set; }
    public string NameOfAuthor { get; set; }
    public string BookDepartMent { get; set; }

    // Book objects are created and managed by the Department
    public Books(string nameOfBook, string nameOfAuthor, string bookDepartment)
    {
        NameOfBook = nameOfBook;
        NameOfAuthor = nameOfAuthor;
        BookDepartMent = bookDepartment;
    }
}

abstract class Library
{
    public abstract void Add(string bookName, string authorName);
    public abstract void Search(string bookOrAuthorName);
    public abstract void Delete(string bookOrAuthorName);
    public abstract void DisplayBooks();
}

class Department : Library
{
    private List<Books> bookList = new List<Books>();//
    public string DepartmentName { get; set; }

    public Department(string departmentName)
    {
        DepartmentName = departmentName;
    }

    // Composition: Department is responsible for creating and managing the lifecycle of Books
    public override void Add(string bookName, string authorName)
    {
        Books newBook = new Books(bookName, authorName, DepartmentName);
        bookList.Add(newBook);  // Add the new Book to the Department's book list
        Console.WriteLine($"Added book: {bookName} by {authorName} to {DepartmentName}.");
    }

    public override void Search(string bookOrAuthorName)
    {
        bool found = false;
        foreach (Books book in bookList)
        {
            if (book.NameOfBook.Contains(bookOrAuthorName) || book.NameOfAuthor.Contains(bookOrAuthorName))
            {
                Console.WriteLine($"Found: {book.NameOfBook}, Author: {book.NameOfAuthor}, Department: {book.BookDepartMent}");
                found = true;
            }
        }

        if (found!=true)
        {
            Console.WriteLine("No matching books found.");
        }
    }

    // In composition, books will be destroyed with the department
    public override void Delete(string bookOrAuthorName)
    {
        var bookToRemove = bookList.Find(b => b.NameOfBook.Contains(bookOrAuthorName) || b.NameOfAuthor.Contains(bookOrAuthorName));

        if (bookToRemove != null)
        {
            bookList.Remove(bookToRemove);  // The Book is removed, and its lifecycle is tied to the department
            Console.WriteLine($"Removed book: {bookToRemove.NameOfBook} by {bookToRemove.NameOfAuthor}.");
        }
        else
        {
            Console.WriteLine($"No book found to delete with the name or author: {bookOrAuthorName}");
        }
    }

    public override void DisplayBooks()
    {
        Console.WriteLine($"Books in {DepartmentName}:");
        if (bookList.Count == 0)
        {
            Console.WriteLine("No books in this department.");
        }
        else
        {
            foreach (var book in bookList)
            {
                Console.WriteLine($"Book: {book.NameOfBook}, Author: {book.NameOfAuthor}, Department: {book.BookDepartMent}");
            }
        }
    }

    // The department destructor will clean up the list of books (if necessary, depending on your design)
    //~Department()
    //{
    //    // Destructor: When the Department is destroyed, all associated Books will be cleaned up.
    //    bookList.Clear();
    //}
}


// {"hussein",Library l }


class DepartmentFactory
{
    private Dictionary <string, Library> departments = new Dictionary<string, Library>();
    //a[0]
    public Library GetDepartment(string departmentName)
    {
        if (departments.ContainsKey(departmentName) != true)
        {
            departments[departmentName] = new Department(departmentName);
            Console.WriteLine($"Created new department: {departmentName}");
        }
        return departments[departmentName];
    }
}

class Program
{
    static void Main(string[] args)
    {
        DepartmentFactory departmentFactory = new DepartmentFactory();

        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Add a Book");
            Console.WriteLine("2. Search for a Book");
            Console.WriteLine("3. Delete a Book");
            Console.WriteLine("4. Display Books in a Department");
            Console.WriteLine("5. Exit");
            Console.Write("Enter choice: ");
            string choice = Console.ReadLine();

            if (choice == "5")
            {
                break;
            }

            Console.Write("Enter department name: ");
            string departmentName = Console.ReadLine();

            Library department = departmentFactory.GetDepartment(departmentName);

            switch (choice)
            {
                case "1":
                    Console.Write("Enter book name: ");
                    string bookName = Console.ReadLine();
                    Console.Write("Enter author name: ");
                    string authorName = Console.ReadLine();
                    department.Add(bookName, authorName);
                    break;

                case "2":
                    Console.Write("Enter book or author name to search: ");
                    string searchQuery = Console.ReadLine();
                    department.Search(searchQuery);
                    break;

                case "3":
                    Console.Write("Enter book or author name to delete: ");
                    string deleteQuery = Console.ReadLine();
                    department.Delete(deleteQuery);
                    break;

                case "4":
                    department.DisplayBooks();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
