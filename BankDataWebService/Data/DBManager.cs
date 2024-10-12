using BankDataWebService.Models;
using System.Data.SQLite;

namespace BankDataWebService.Data
{
    public class DBManager
    {
        private static string connectionString = "Data Source=bank.db; Version=3;";

        private static Random random = new Random();
        public static bool CreateAccountTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                    CREATE TABLE Account (
                        AccountNumber INTEGER PRIMARY KEY AUTOINCREMENT,
                        Balance INTEGER,
                        HolderName TEXT NOT NULL,
                        PhoneNumber INTEGER,
                        Email TEXT NOT NULL
                    )";
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                Console.WriteLine("Table Account Created Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
            }
            return false;
        }

        public static bool CreateUserTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                    CREATE TABLE User (
                        Email TEXT PRIMARY KEY,
                        UserName TEXT,
                        Address TEXT NOT NULL,
                        Password TEXT NOT NULL,
                        Phone INTEGER,
                    )";
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                Console.WriteLine("Table User Created Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
            }
            return false;
        }

        public static bool CreateTransactionTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                    CREATE TABLE Transaction (
                        TransactionID INTEGER PRIMARY KEY AUTOINCREMENT,
                        TransactionName TEXT,
                        TransactionAmount INTEGER,
                        TransactionType TEXT NOT NULL CHECK (TransactionType IN ('Deposit' 'Withdraw')),
                        TransactionTime DATETIME DEFAULT CURRENT_TIMESTAMP,
                    )";
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                Console.WriteLine("Table Transaction Created Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
            }
            return false;
        }

        public static bool insertAccount(Account account)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        INSERT INTO Account(AccountNumber, Balance, HolderName, PhoneNumber, Email)
                        VALUES (@AccountNumber, @Balance, @HolderName, @PhoneNumber, @Email)";

                        command.Parameters.AddWithValue("@AccountNumber", account.accountNumber);
                        command.Parameters.AddWithValue("@Balance", account.balance);
                        command.Parameters.AddWithValue("@HolderName", account.holderName);
                        command.Parameters.AddWithValue("@PhoneNumber", account.phoneNumber);
                        command.Parameters.AddWithValue("@Email", account.email);

                        int rowsInserted = command.ExecuteNonQuery();

                        connection.Close();
                        if (rowsInserted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static bool insertUser(User user)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        INSERT INTO User(UserName, Email, Address, Password, Phone)
                        VALUES (@UserName, @Email, @Address, @Password, @Phone)";

                        command.Parameters.AddWithValue("@UserName", user.userName);
                        command.Parameters.AddWithValue("@Email", user.email);
                        command.Parameters.AddWithValue("@Address", user.address);
                        command.Parameters.AddWithValue("@Password", user.password);
                        command.Parameters.AddWithValue("@Phone", user.phone);

                        int rowsInserted = command.ExecuteNonQuery();

                        connection.Close();
                        if (rowsInserted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static bool insertTransaction(Models.Transaction transaction)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        INSERT INTO Transaction(UserID, UserName, Email, Address, Password, Phone)
                        VALUES (@UserID, @UserName, @Email, @Address, @Password, @Phone)";

                        command.Parameters.AddWithValue("@TransactionID", transaction.transactionID);
                        command.Parameters.AddWithValue("@TransactionName", transaction.transactionName);
                        command.Parameters.AddWithValue("@TransactionAmount", transaction.transactionAmount);
                        command.Parameters.AddWithValue("@TransactionAmount", transaction.transactionType);
                        command.Parameters.AddWithValue("@TransactionTime", transaction.transactionTime);

                        int rowsInserted = command.ExecuteNonQuery();

                        connection.Close();
                        if (rowsInserted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static bool deleteAccount(Account account)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"DELETE FROM Account WHERE AccountNumber = @AccountNumber";
                        command.Parameters.AddWithValue("@AccountNumber", account.accountNumber);

                        int rowsDeleted = command.ExecuteNonQuery();
                        connection.Close();
                        if (rowsDeleted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static bool deleteUser(User user)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"DELETE FROM User WHERE UserName = @UserName";
                        command.CommandText = $"DELETE FROM User WHERE Email = @Email";

                        command.Parameters.AddWithValue("@UserName", user.userName);
                        command.Parameters.AddWithValue("@Email", user.email);

                        int rowsDeleted = command.ExecuteNonQuery();
                        connection.Close();
                        if (rowsDeleted > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static bool updateAccount(Account account)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"UPDATE Account SET Balance = @Balance, HolderName = @HolderName, PhoneNumber = @PhoneNumebr, Email =@Email WHERE AccountNumber = @AccountNumber, ";

                        command.Parameters.AddWithValue("@AccountNumber", account.accountNumber);
                        command.Parameters.AddWithValue("@Balance", account.balance);
                        command.Parameters.AddWithValue("@HolderName", account.holderName);
                        command.Parameters.AddWithValue("@PhoneNumber", account.phoneNumber);
                        command.Parameters.AddWithValue("@Email", account.email);

                        int rowsUpdated = command.ExecuteNonQuery();
                        connection.Close();
                        if (rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static bool updateUser(User user)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"UPDATE User SET UserName = @UserName, Email = @Email, Address = @Address, Password =@Password, Phone = @Phone";

                        command.Parameters.AddWithValue("@UserName", user.userName);
                        command.Parameters.AddWithValue("@Email", user.email);
                        command.Parameters.AddWithValue("@Address", user.address);
                        command.Parameters.AddWithValue("@Password", user.password);
                        command.Parameters.AddWithValue("@Phone", user.phone);

                        int rowsUpdated = command.ExecuteNonQuery();
                        connection.Close();
                        if (rowsUpdated > 0)
                        {
                            return true;
                        }
                    }
                    connection.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false;
        }

        public static List<Account> getAllAcounts()
        {
            List<Account> accountsList = new List<Account>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Account";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Account account = new Account();
                                account.accountNumber = Convert.ToInt32(reader["AccountNumber"]);
                                account.balance = Convert.ToInt32(reader["Balance"]);
                                account.email = reader["Email"].ToString();
                                account.phoneNumber = Convert.ToInt32(reader["PhoneNumber"]);
                                account.holderName = reader["HolderName"].ToString();

                                accountsList.Add(account);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return accountsList;
        }

        public static List<User> getAllUsers()
        {
            List<User> userList = new List<User>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM User";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User();
                                user.userName = reader["UserName"].ToString();
                                user.phone = Convert.ToInt32(reader["Phone"]);
                                user.email = reader["Email"].ToString();
                                user.address = reader["Address"].ToString();
                                user.password = reader["Password"].ToString();
                                userList.Add(user);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return userList;
        }

        public static List<Models.Transaction> getAllHistory()
        {
            List<Models.Transaction> transactionList = new List<Models.Transaction>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Transaction";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Models.Transaction transaction = new Models.Transaction();
                                transaction.transactionID = Convert.ToInt32(reader["UserID"]);
                                transaction.transactionAmount = Convert.ToInt32(reader["TransactionAmount"]);
                                transaction.transactionName = reader["TransactionName"].ToString();
                                transaction.transactionType = reader["TransactionType"].ToString();
                                transactionList.Add(transaction);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return transactionList;
        }

        public static Account getByAccountNumber(int accountNumber)
        {
            Account account = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Account WHERE AccountNumber = @AccountNumber";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account = new Account();
                                account.accountNumber = Convert.ToInt32(reader["AccountNumber"]);
                                account.balance = Convert.ToInt32(reader["Balance"]);
                                account.email = reader["Email"].ToString();
                                account.phoneNumber = Convert.ToInt32(reader["PhoneNumber"]);
                                account.holderName = reader["HolderName"].ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return account;
        }

        public static User getByUserName(string userName)
        {
            User user = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM User WHERE UserID = @UserID";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User();
                                user.userName = reader["UserName"].ToString();
                                user.phone = Convert.ToInt32(reader["Phone"]);
                                user.email = reader["Email"].ToString();
                                user.address = reader["Address"].ToString();
                                user.password = reader["Password"].ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return user;
        }

        public static User getByUserEmail(String email)
        {
            User user = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM User WHERE Email = @Email";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User();
                                user.email = reader["Email"].ToString();
                                user.userName = reader["UserName"].ToString();
                                user.phone = Convert.ToInt32(reader["Phone"]);
                                user.address = reader["Address"].ToString();
                                user.password = reader["Password"].ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return user;
        }

        public static List<Account> generateAccounts(int accountNumber, int count)
        {
            var accountsList = new List<Account>();
            for (int i = 0; i < count; i++)
            {
                accountsList.Add(new Account
                {
                    accountNumber = (accountNumber * 10) + i + 1,
                    balance = random.Next(10000, 99999),
                    holderName = holderName,

                }
            }
        }
    }
}
