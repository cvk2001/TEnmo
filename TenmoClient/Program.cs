﻿using System;
using System.Collections.Generic;
using TenmoClient.Data;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();

        static void Main(string[] args)
        {
            Run();
        }
        private static void Run()
        {
            int loginRegister = -1;
            while (loginRegister != 1 && loginRegister != 2)
            {
                Console.WriteLine("Welcome to TEnmo!");
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Register");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out loginRegister))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (loginRegister == 1)
                {
                    while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                    {
                        LoginUser loginUser = consoleService.PromptForLogin();
                        API_User user = authService.Login(loginUser);
                        if (user != null)
                        {
                            UserService.SetLogin(user);
                        }
                    }
                }
                else if (loginRegister == 2)
                {
                    bool isRegistered = false;
                    while (!isRegistered) //will keep looping until user is registered
                    {
                        LoginUser registerUser = consoleService.PromptForLogin();
                        isRegistered = authService.Register(registerUser);
                        if (isRegistered)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Registration successful. You can now log in.");
                            loginRegister = -1; //reset outer loop to allow choice for login
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }

            MenuSelection();
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    //TODO insert variable here and make magic happen.
                    APIService account = new APIService();
                    API_Account apiAccount = account.GetAccount(UserService.GetUserId());
                    Console.WriteLine($"Your current account balance is: {apiAccount.Balance:c2}");
                }
                else if (menuSelection == 2)
                {

                }
                else if (menuSelection == 3)
                {

                }
                else if (menuSelection == 4)
                {
                    APIService users = new APIService();
                    List<API_User> userList = users.GetUsers();
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine("User Id".PadRight(10) + "Name");
                    Console.WriteLine("-------------------------------");
                    foreach (API_User user in userList)
                    {
                        Console.WriteLine(user.UserId.ToString().PadRight(10) + user.Username);
                       
                    }
                    Console.WriteLine("-------------------------------");
                    int answer = -1;
                    while(answer <0)
                    {
                        Console.Write("Enter ID of user you are sending to (0 to cancel): ");
                        string input = Console.ReadLine();
                        if(!int.TryParse(input, out answer))
                        {
                            Console.WriteLine("Invalid input. Please enter a only a number");
                            answer = -1;
                        }
                        else
                        {
                            answer = int.Parse(input);
                        }
                        

                    }
                    if (answer != 0)
                    {
                        API_Account account = users.GetAccount(answer);
                        int amount = -1;
                        while (amount < 0)
                        {
                            Console.Write("Enter amount: ");
                            string input = Console.ReadLine();
                            if (!int.TryParse(input, out amount))
                            {
                                Console.WriteLine("Invalid input. Please enter a only a number");
                                amount = -1;
                            }
                            else
                            {
                                amount = int.Parse(input);
                                if (amount < 0)
                                {
                                    Console.WriteLine("Enter a positive amount");
                                }
                            }

                        }
                    }




                }
                else if (menuSelection == 5)
                {

                }
                else if (menuSelection == 6)
                {
                    Console.WriteLine("");
                    UserService.SetLogin(new API_User()); //wipe out previous login info
                    Run(); //return to entry point
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }
        
    }
}
