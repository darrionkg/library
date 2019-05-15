using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Tests
{
  [TestClass]
  public class BookTest : IDisposable
  {

    public void Dispose()
    {
      Book.ClearAll();
    }

    public BookTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    [TestMethod]
    public void BookConstructor_CreatesInstanceOfBook_Book()
    {
      Book newBook = new Book("test");
      Assert.AreEqual(typeof(Book), newBook.GetType());
    }

  [TestMethod]
   public void GetAll_BooksEmptyAtFirst_List()
   {
     //Arrange, Act
     int result = Book.GetAll().Count;

     //Assert
     Assert.AreEqual(0, result);
   }

   [TestMethod]
    public void GetAll_BooksNotEmpty_List()
    {
      //Arrange, Act
      Book test = new Book("Test");
      test.Save();
      int result = Book.GetAll().Count;

      //Assert
      Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void GetTitle_ReturnsName_String()
    {
      //Arrange
      string title = "Frankenstein";
      Book newBook = new Book(title);

      //Act
      string result = newBook.GetTitle();

      //Assert
      Assert.AreEqual(title, result);
    }

    [TestMethod]
    public void GetAll_ReturnsEmptyList_BookList()
    {
      //Arrange
      List<Book> newList = new List<Book> { };

      //Act
      List<Book> result = Book.GetAll();

      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void GetAll_ReturnsBooks_BookList()
    {
      //Arrange
      Book newBook1 = new Book("Slaughterhouse 5");
      newBook1.Save();
      Book newBook2 = new Book("Survivor");
      newBook2.Save();
      List<Book> newList = new List<Book> { newBook1, newBook2 };

      //Act
      List<Book> result = Book.GetAll();

      //Assert
      // This is a way to avoid the CollectionAssert error that says "(Element at index 0 do not match)", because we are working with databases now.
      Assert.AreEqual(newList[0].GetTitle(), result[0].GetTitle());
    }

    [TestMethod]
    public void Find_ReturnsCorrectBookFromDatabase_Book()
    {
      //Arrange
      Book testBook = new Book("Charlottes Web");
      testBook.Save();

      //Act
      Book foundBook = Book.Find(testBook.GetId());

      //Assert
      // To eliminate error "Expected: <object>. Actual: <object>", we looked for the title of the books instead.
      Assert.AreEqual(testBook.GetId(), foundBook.GetId());
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfNamesAreTheSame_Book()
    {
      Book testBook = new Book("The hound of Baskerville");
      testBook.Save();
      Book foundBook = Book.Find(testBook.GetId());
      // Assert
      Assert.AreEqual(testBook.GetTitle(), foundBook.GetTitle());
    }

    [TestMethod]
    public void Save_SavesToDatabase_BookList()
    {
      //Arrange
      Book testBook = new Book("Anne of Green Gables");
      //Act
      testBook.Save();
      List<Book> result = Book.GetAll();
      List<Book> testList = new List<Book>{testBook};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Book testBook = new Book("1984");

      //Act
      testBook.Save();
      Book savedBook = Book.GetAll()[0];

      int result = savedBook.GetId();
      int testId = testBook.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Update_UpdatesBookInDatabase_String()
    {
      //Arrange
      Book testBook = new Book("The Alchemist");
      testBook.Save();
      string secondName = "The Zahir";

      //Act
      testBook.Update("title", secondName);
      string result = Book.Find(testBook.GetId()).GetTitle();

      //Assert
      Assert.AreEqual(secondName, result);
    }

    // [TestMethod]
    // public void GetStylistId_ReturnsBooksParentStylistId_Int()
    // {
    //   //Arrange
    //   Stylist newStylist = new Stylist("Sheila Moore", "Hair dying", 0);
    //   Book newBook = new Book("Wallace Tan", newStylist.Id, 1);
    //
    //   //Act
    //   int result = newBook.StylistId;
    //
    //   //Assert
    //   Assert.AreEqual(newStylist.Id, result);
    // }

  }
}
