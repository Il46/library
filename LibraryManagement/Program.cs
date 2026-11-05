using System.Globalization;

using (var db = new LibraryContext())
{
    db.Database.EnsureCreated();
}

var library = new Library();
library.ShowWelcomeMessage();
Library.ShowLibraryName();

while (true)
{
    library.ShowMenu();
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.Write("Введите название книги: ");
            var title = Console.ReadLine() ?? string.Empty;
            Console.Write("Введите автора: ");
            var author = Console.ReadLine() ?? string.Empty;
            var year = ReadInteger("Введите год издания: ");
            library.AddBook(title, author, year);
            break;
        case "2":
            library.ShowBooks();
            break;
        case "3":
            Console.Write("Введите название книги для поиска: ");
            var searchTitle = Console.ReadLine() ?? string.Empty;
            var book = library.FindBook(searchTitle);
            if (book is null)
            {
                Console.WriteLine("Книга не найдена.");
            }
            else
            {
                book.DisplayInfo();
            }
            break;
        case "4":
            Console.Write("Введите имя читателя: ");
            var firstName = Console.ReadLine() ?? string.Empty;
            Console.Write("Введите фамилию читателя: ");
            var lastName = Console.ReadLine() ?? string.Empty;
            Console.Write("Введите номер телефона: ");
            var phone = Console.ReadLine() ?? string.Empty;
            library.AddReader(firstName, lastName, phone);
            break;
        case "5":
            library.ShowReaders();
            break;
        case "6":
            Console.Write("Введите название книги: ");
            var borrowTitle = Console.ReadLine() ?? string.Empty;
            Console.Write("Введите имя и фамилию читателя (как в списке читателей): ");
            var readerName = Console.ReadLine() ?? string.Empty;
            library.BorrowBook(borrowTitle, readerName);
            break;
        case "7":
            Console.Write("Введите название книги: ");
            var returnTitle = Console.ReadLine() ?? string.Empty;
            library.ReturnBook(returnTitle);
            break;
        case "8":
            library.ShowStatistics();
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Неизвестная команда. Выберите пункт меню от 0 до 8.");
            break;
    }
}

static int ReadInteger(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();
        if (int.TryParse(input, NumberStyles.Integer, CultureInfo.CurrentCulture, out var value))
        {
            return value;
        }

        Console.WriteLine("Введите целое число.");
    }
}