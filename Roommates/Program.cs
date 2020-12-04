using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;
using System.IO;
using System.Linq;


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
                    case ("Show unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        foreach(Chore c in unassignedChores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign a chore"):
                        Console.WriteLine();
                        List<Chore> choreToAssign = choreRepo.GetUnassignedChores();
                        Console.WriteLine(" - - - Currently unassigned chores - - - ");
                        foreach(Chore c in choreToAssign)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.Write(" \n Which chore would you like to assign? ");
                        int choreChosen = int.Parse(Console.ReadLine());
                        Console.WriteLine();

                        List<Roommate> roommateToAssign = roommateRepo.GetAllRoommates();
                        foreach(Roommate r in roommateToAssign)
                        {
                            Console.WriteLine($"{r.Id} - {r.FirstName}");
                        }
                        Console.Write(" \n Which roommate would you like to assign the chore to? ");
                        int roommateChosen = int.Parse(Console.ReadLine());
                        Console.WriteLine();
                        choreRepo.AssignChore(roommateChosen, choreChosen);
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show chore counts"):
                        List<string> choreCounts = choreRepo.GetChoreCounts();
                        foreach(string c in choreCounts)
                        {
                            Console.WriteLine(c);
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAllRooms();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine($"Room has been successfully updated");
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
            "Show unassigned chores",
            "Assign a chore",
            "Show chore counts",
            "Update a room",
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
