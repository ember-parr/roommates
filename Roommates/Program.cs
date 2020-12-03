﻿using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;


namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            bool runProgram = true;
            while (runProgram)
             {
                 string selection = GetMenuSelection();

                 switch (selection)
                 {
                     case ("Show all rooms"):
                         List<Room> rooms = roomRepo.GetAllRooms();
                         foreach (Room r in rooms)
                         {
                             Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                         }
                         Console.Write("Press any key to continue");
                         Console.ReadKey();
                         break;
                     case ("Search for room"):
                         Console.Write("Room Id: ");
                         int id = int.Parse(Console.ReadLine());

                         Room room = roomRepo.GetById(id);

                         Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                         Console.Write("Press any key to continue");
                         Console.ReadKey();
                         break;
                     case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                     case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAllChores();
                        foreach(Chore c in chores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());
                        Chore chore = choreRepo.GetById(choreId);
                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore Name");
                        string choreName = Console.ReadLine();
                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };
                        choreRepo.Insert(choreToAdd);
                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all roommates"):
                        List<Roommate> roommates = roommateRepo.GetAllRoommates();
                        foreach(Roommate r in roommates)
                        {
                            Console.WriteLine($"{r.Id} - {r.FirstName} pays {r.RentPortion}% of the rent & lives in the {r.RoomId}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for roommates"):
                        Console.Write("Search roommate by Id: ");
                        int roomieId = int.Parse(Console.ReadLine());
                        Roommate roomie = roommateRepo.GetById(roomieId);
                        Console.WriteLine($"{roomie.Id} - {roomie.FirstName} pays {roomie.RentPortion}% of the rent & lives in the {roomie.RoomId}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        foreach(Chore c in unassignedChores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                     case ("Exit"):
                         runProgram = false;
                         break;
                 }
             }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
            "Show all rooms",
            "Search for room",
            "Add a room",
            "Show all chores",
            "Search for chore",
            "Add a chore",
            "Show all roommates",
            "Search for roommates",
            "Show unassigned chores",
            "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }


        }
            
    }
}
