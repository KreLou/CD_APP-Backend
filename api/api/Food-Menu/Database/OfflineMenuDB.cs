﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using BAGCST.api.FoodMenu.Models;

namespace BAGCST.api.FoodMenu.Database
{
    public class OfflineMenuDB: IMenuDB
    {

        private readonly IMealDB mealDB;
        private string menu_filename = Path.Combine(Environment.CurrentDirectory,"offlineDB","Files","menus.csv");

        public OfflineMenuDB(IMealDB mealDB)
        {
            this.mealDB = mealDB;
        }


        /// <summary>
        /// Creates the string output for menu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string ConvertFromMenuToString(MenuItem item)
        {
            return
                item.MenuID + ";" +
                item.Meal.MealID + ";" +
                item.Price + ";" +
                item.Date;
        }


        /// <summary>
        /// Creates the meunItem output for menu from the String 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private MenuItem ConvertFromStringToMenuItem(string line)
        {
            string[] args = line.Split(";");
            MenuItem item = new MenuItem
            {
                MenuID = Convert.ToInt32(args[0]),
                Meal = mealDB.getMealItem(Convert.ToInt32(args[1])),
                Price = decimal.Parse(args[2]),
                Date = DateTime.Parse(args[3]).Date
            };
            return item;
        }

        /// <summary>
        /// Search for Menu in file, return menu or null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MenuItem getMenuItem(int id)
        {
            MenuItem menu = null;

            using (StreamReader sr = new StreamReader(menu_filename))
            {
                string line;
                //end if end of file or menu is found
                while ((line = sr.ReadLine()) != null && menu == null)
                {
                    // if the id in the line is the id from Menu 
                    int menu_id = (int)Convert.ToInt64(line.Split(";")[0]);
                    if (menu_id == id)
                    {
                        // then get the menu
                        menu = ConvertFromStringToMenuItem(line);
  
                    }
                }
            }

            return menu;
        }

        /// <summary>
        /// Create new menu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public MenuItem saveNewMenu(MenuItem item)
        {
            // get all Menus 
            MenuItem[] menus = getAllMenus();
            int max = 0;
            foreach (MenuItem menu in menus)
            {
                if(menu.MenuID >= max)
                {
                    max = menu.MenuID;
                }
               
            }
            // the new id will be the maxId in the file + 1
            max++;
            item.MenuID = max;
            
            // save item 
            File.AppendAllLines(menu_filename, new String[] { ConvertFromMenuToString(item) });

            // return item 
            return item;
        }

        /// <summary>
        /// edits the Menu based on the given Menu except for the ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>menu</returns>
        public MenuItem editMenu(int id,MenuItem item)
        {
            string tempFile = Path.GetTempFileName();
            using (StreamWriter writer = new StreamWriter(tempFile))
            using (StreamReader reader = new StreamReader(menu_filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (Convert.ToInt32(line.Split(";")[0]) == id)
                    {
                        item.MenuID = id;
                        line =id + ";" +
                            item.Meal.MealID + ";" +
                            item.Price + ";" +
                            item.Date;
                        // save the line in the file 
                        writer.WriteLine(line);
                    }
                    else
                    {
                        // save the line in the file 
                        writer.WriteLine(line);
                    }

                }
            }
            // delete the old item 
            File.Delete(menu_filename);
            // save the new item 
            File.Move(tempFile, menu_filename);
            // return the new item 
            return getMenuItem(id);
        }

        /// <summary>
        /// deletes the Menu based on the given ID
        /// </summary>
        /// <param name="id"></param>
        public void deleteMenu(int id)
        {
            string tempFile = Path.GetTempFileName();
            using (StreamWriter writer = new StreamWriter(tempFile))
            using (StreamReader reader = new StreamReader(menu_filename))
            {
                string line;
                // end if the item not exist 
                while ((line = reader.ReadLine()) != null)
                {
                    // if the item exist und id  null 
                    if (Convert.ToInt32(line.Split(";")[0]) != id)
                    {
                        // do no thing 
                        writer.WriteLine(line);
                    }
                }
            }
            // else delete the item 
            File.Delete(menu_filename);
            File.Move(tempFile, menu_filename);
        }

        /// <summary>
        /// Search for all active Menu in file 
        /// </summary>
        /// <returns></returns>
        public MenuItem[] getAllMenus()
        {
            // list for all items
            List<MenuItem> list = new List<MenuItem>();

            using (StreamReader sr = new StreamReader(this.menu_filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // if item found 
                    MenuItem menu = ConvertFromStringToMenuItem(line);
                    // add this item to the list 
                    list.Add(menu);

                }
            }
            return list.ToArray();
        }

        public MenuItem[] getFilterdMenus(DateTime startDate, DateTime endDate, int[] placeIDs)
        {
            MenuItem[] allMenuItems = getAllMenus();

            allMenuItems = allMenuItems
                .Where(menu => menu.Date >= startDate)
                .Where(menu => menu.Date <= endDate)
                .Where(menu => placeIDs.Contains(menu.Meal.Place.PlaceID))
                .ToArray();
            return allMenuItems;
        }
    }
}
