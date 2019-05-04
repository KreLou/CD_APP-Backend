﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    interface IGroupsDB
    {
        /// <summary>
        /// search for Group by ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Group|null</returns>
        Group getGroup(int id);

        /// <summary>
        /// get all Groups
        /// </summary>
        /// <returns>Array of Rights with length>=0</returns>
        Group[] getAllGroups();

        /// <summary>
        /// creates a Group
        /// </summary>
        /// <param name="item">Group</param>
        /// <returns>full Group</returns>
        Group createGroup(Group item);

        /// <summary>
        /// edit Group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>edited Group|null</returns>
        Group editGroup(int id, Group item);

        /// <summary>
        /// delete Group
        /// </summary>
        /// <param name="id">ID</param>
        void deleteGroup(int id);

        /// <summary>
        /// returns all Groups for the User, identified by the given UserID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int[] getGroupsByUser(long userID);

        /// <summary>
        /// sets the given GroupIDs for the User, identified by the given UserID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupIDs"></param>
        void setGroupsForUser(long userID, int[] groupIDs);
    }
}