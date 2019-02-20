﻿using api.Interfaces;
using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace api.offlineDB
{
    public class OfflineMenuDB:IMenuDB
    {

        private string menu_filename = Environment.CurrentDirectory + "\\offlineDB\\Files\\menus.csv";

        /// <summary>
        /// Creates the string output for menu
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        private string writeLine(MenuItem menu)
        {
            return menu.MenuID + ";" + menu.MealID + ";" + menu.Price + ";" + menu.Date ;
        }
        /// <summary>
        /// Search for Menu in file, return menu or null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MenuItem GetMenuItem(int id)
        {
            MenuItem menu = null;

            using (StreamReader sr = new StreamReader(menu_filename))
            {
                string line;
                //end if end of file or menu is found
                while ((line = sr.ReadLine()) != null && menu == null)
                {
                    int menu_id = (int)Convert.ToInt64(line.Split(";")[0]);
                    if (menu_id == id)
                    {
                        string[] args = line.Split(";");
                        menu = new MenuItem()
                        {
                            MenuID = menu_id,
                            MealID = (int)Convert.ToInt64(args[1]),
                            Price = decimal.Parse( args[2]),
                            Date = DateTime.Parse(args[3]).Date

                        };
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
            MenuItem[] menus = GetMenus(item.Date);
            int max = 0;
            foreach (MenuItem menu in menus)
            {
                if(menu.MenuID >= max)
                {
                    max = menu.MenuID;
                }
               
            }
            max++;
            item.MenuID = max;
            
            // save item 
            File.AppendAllLines(menu_filename, new String[] { this.writeLine(item) });

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

                      
                        writer.WriteLine(id + ";" + item.MealID + ";" + item.Price + ";" + item.Date  );
                    }
                    else
                    {
                        writer.WriteLine(line);
                    }
                }
            }
            File.Delete(menu_filename);
            File.Move(tempFile, menu_filename);
            return GetMenuItem(id);
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
                while ((line = reader.ReadLine()) != null)
                {
                    if (Convert.ToInt32(line.Split(";")[0]) != id)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
            File.Delete(menu_filename);
            File.Move(tempFile, menu_filename);
        }

        /// <summary>
        /// Search for all active Menu in file 
        /// </summary>
        /// <param name="Date">Date</param>
        /// <returns></returns>
        public MenuItem[] GetMenus(DateTime date)
        {
            List<MenuItem> list = new List<MenuItem>();
            MenuItem[] menus = GetMenus();
            foreach(MenuItem item in menus)
            {
                if(item.Date== date)
                {
                    list.Add(item);
                }
            }
    
            return list.ToArray();
        }

        /// <summary>
        /// Search for all active Menu in file 
        /// </summary>
        /// <returns></returns>
        public MenuItem[] GetMenus()
        {
            List<MenuItem> list = new List<MenuItem>();

            using (StreamReader sr = new StreamReader(this.menu_filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] args = line.Split(";");
                    MenuItem menu = new MenuItem()
                    {
                        MenuID = (int)Convert.ToInt64(args[0]),
                        MealID = (int)Convert.ToInt64(args[1]),
                        Price = decimal.Parse(args[2]),
                        Date = DateTime.Parse(args[3]).Date
                    };
                    list.Add(menu);

                }
            }
            return list.ToArray();
        }


    }
}
