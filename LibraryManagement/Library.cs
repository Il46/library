using System;
using System.Collections.Generic;
using System.Linq;

public class Library
{
    public static int TotalBooks 
    {
        get 
        {
            using var db = new LibraryContext();
            return db.Books.Count();
        }
    }

    public static void ShowLibraryName()
    {
        Console.WriteLine("==============================\n     УПРАВЛЕНИЕ БИБЛИОТЕКОЙ\n==============================");
    }

    public static decimal CalculateFine(int overdueDays)
    {
        return overdueDays * 100;
    }

    public static int GetMaxBooks(int first, int second)
    {
        return Math.Max(first, second);
    }

    public void ShowWelcomeMessage()
    {
        Console.WriteLine("Добро пожаловать в систему управления библиотекой!");
    }

    public void AddBook(string title, string author, int year)
    {
        using var db = new LibraryContext();
        var book = new Book { Title = title, Author = author, Year = year, IsAvailable = true };
        db.Books.Add(book);
        db.SaveChanges();
        Console.WriteLine("Книга успешно добавлена!");
    }

    public void AddReader(string firstName)
    {
        using var db = new LibraryContext();
        db.Readers.Add(new Reader { FirstName = firstName });
        db.SaveChanges();
        Console.WriteLine("Читатель успешно добавлен!");
    }

    public void AddReader(string firstName, string lastName)
    {
        using var db = new LibraryContext();
        db.Readers.Add(new Reader { FirstName = firstName, LastName = lastName });
        db.SaveChanges();
        Console.WriteLine("Читатель успешно добавлен!");
    }

    public void AddReader(string firstName, string lastName, string phone)
    {
        using var db = new LibraryContext();
        db.Readers.Add(new Reader { FirstName = firstName, LastName = lastName, PhoneNumber = phone });
        db.SaveChanges();
        Console.WriteLine("Читатель успешно добавлен!");
    }

    public void ShowBooks()
    {
        using var db = new LibraryContext();
        var books = db.Books.ToList();
        foreach (var book in books)
        {
            book.DisplayInfo();
        }
    }

    public void ShowReaders()
    {
        using var db = new LibraryContext();
        var readers = db.Readers.ToList();
        foreach (var r in readers)
        {
            Console.WriteLine($"Имя: {r.FirstName} {r.LastName}, Телефон: {r.PhoneNumber}");
        }
    }

    public int GetBookCount()
    {
        using var db = new LibraryContext();
        return db.Books.Count();
    }

    public bool IsBookAvailable(string title)
    {
        using var db = new LibraryContext();
        var book = FindBook(db.Books.AsEnumerable(), title, exactMatch: true);
        return book is not null && book.IsAvailable;
    }

    public bool IsReaderRegistered(string name)
    {
        using var db = new LibraryContext();
        return db.Readers.Any(r => r.FirstName.ToLower() == name.ToLower());
    }

    public Book? FindBook(string title)
    {
        using var db = new LibraryContext();
        return FindBook(db.Books.AsEnumerable(), title, exactMatch: false);
    }

    public void BorrowBook(string title, string readerName)
    {
        using var db = new LibraryContext();
        var book = FindBook(db.Books.AsEnumerable(), title, exactMatch: true);
        var normalizedReaderName = NormalizeWhitespace(readerName);
        var reader = db.Readers
            .AsEnumerable()
            .FirstOrDefault(r =>
                string.Equals(NormalizeWhitespace(r.FirstName), normalizedReaderName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(NormalizeWhitespace($"{r.FirstName} {r.LastName}"), normalizedReaderName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(NormalizeWhitespace($"{r.LastName} {r.FirstName}"), normalizedReaderName, StringComparison.OrdinalIgnoreCase));

        if (book is null)
        {
            Console.WriteLine($"Книга «{title}» не найдена. Проверьте название в списке книг.");
            return;
        }

        if (reader is null)
        {
            Console.WriteLine($"Читатель «{readerName}» не найден. Введите имя и фамилию, как в списке читателей.");
            return;
        }

        if (!book.IsAvailable)
        {
            Console.WriteLine($"Книга «{book.Title}» уже выдана.");
            return;
        }

        book.IsAvailable = false;
        db.SaveChanges();
        Console.WriteLine($"Книга «{book.Title}» успешно выдана читателю {reader.FirstName} {reader.LastName}.");
    }

    private static string NormalizeWhitespace(string value) =>
        string.Join(' ', value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

    private static Book? FindBook(IEnumerable<Book> books, string title, bool exactMatch)
    {
        var normalizedTitle = NormalizeWhitespace(title);
        if (normalizedTitle.Length == 0)
        {
            return null;
        }

        return books.FirstOrDefault(book =>
        {
            var candidateTitle = NormalizeWhitespace(book.Title);
            return exactMatch
                ? string.Equals(candidateTitle, normalizedTitle, StringComparison.OrdinalIgnoreCase)
                : candidateTitle.Contains(normalizedTitle, StringComparison.OrdinalIgnoreCase);
        });
    }

    public void ReturnBook(string title)
    {
        using var db = new LibraryContext();
        var book = FindBook(db.Books.AsEnumerable(), title, exactMatch: true);
        if (book is null)
        {
            Console.WriteLine($"Книга «{title}» не найдена. Проверьте название в списке книг.");
            return;
        }

        if (book.IsAvailable)
        {
            Console.WriteLine($"Книга «{book.Title}» уже находится в библиотеке.");
            return;
        }

        book.IsAvailable = true;
        db.SaveChanges();
        Console.WriteLine($"Книга «{book.Title}» успешно возвращена в библиотеку.");
    }

    public void ShowStatistics()
    {
        using var db = new LibraryContext();
        int total = db.Books.Count();
        int available = db.Books.Count(b => b.IsAvailable);
        int borrowed = total - available;
        int readersCount = db.Readers.Count();

        Console.WriteLine($"Общее количество книг: {total}");
        Console.WriteLine($"Доступно книг: {available}");
        Console.WriteLine($"Выдано книг: {borrowed}");
        Console.WriteLine($"Зарегистрировано читателей: {readersCount}");
    }

    public void ShowMenu()
    {
        Console.WriteLine("\n1. Добавить книгу");
        Console.WriteLine("2. Показать все книги");
        Console.WriteLine("3. Найти книгу");
        Console.WriteLine("4. Добавить читателя");
        Console.WriteLine("5. Показать читателей");
        Console.WriteLine("6. Выдать книгу");
        Console.WriteLine("7. Вернуть книгу");
        Console.WriteLine("8. Статистика библиотеки");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите действие: ");
    }
}