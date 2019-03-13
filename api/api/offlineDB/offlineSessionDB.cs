﻿using api.Interfaces;
using api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace api.offlineDB
{
    public class offlineSessionDB : ISessionDB
    {
        private string filePath = Path.Combine(Environment.CurrentDirectory, "offlineDB", "Files", "sessions.csv");
        private string convertToLine(SessionItem item)
        {
            return $"{item.InternalID};" +
                $"{item.DeviceID};" +
                $"{item.UserID};" +
                $"{item.StartTime};" +
                $"{item.ExpirationTime};" +
                $"{item.Token}";
        }
        private SessionItem convertToItem(string line)
        {
            string[] args = line.Split(';');
            return new SessionItem
            {
                InternalID = (long)Convert.ToInt64(args[0]),
                DeviceID = (long)Convert.ToInt64(args[1]),
                UserID = (long)Convert.ToInt64(args[2]),
                StartTime = Convert.ToDateTime(args[3]),
                ExpirationTime = Convert.ToDateTime(args[4]),
                Token = args[5]
            };
        }
        public SessionItem createNewSession(SessionItem item)
        {
            item.InternalID = getNextFreeSessionID();
            string writeLine = convertToLine(item);

            File.AppendAllLines(filePath, new string[] { writeLine });
            return item;
        }

        public SessionItem findExistingSession(long userID, long deviceID)
        {
            SessionItem[] sessions = getAllActiveSessions();
            SessionItem[] possibleItems = sessions
                .Where(x => x.DeviceID == deviceID && x.UserID == userID).ToArray();
            if (possibleItems.Length == 1)
            {
                return possibleItems[0];
            }
            else if (possibleItems.Length == 0)
            {
                return null;
            }
            throw new Exception("No Unique Session found");
        }

        public SessionItem[] getAllSessions()
        {
            List<SessionItem> sessions = new List<SessionItem>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    SessionItem item = convertToItem(line);
                    sessions.Add(item);
                }
            }

            return sessions.ToArray();
        }

        public SessionItem[] getAllActiveSessions()
        {
            DateTime now = DateTime.Now;
            return getAllSessions()
                .Where(x => x.ExpirationTime <= now)
                .Where(x => x.StartTime >= now)
                .ToArray();
        }

        private long getNextFreeSessionID()
        {
            if (getAllSessions().Length >= 2)
            {
                SessionItem itemWithHighestID = getAllSessions().Aggregate((x1, x2) => x1.InternalID > x2.InternalID ? x1 : x2);
                return itemWithHighestID.InternalID + 1;
            } else if (getAllSessions().Length == 1)
            {
                return getAllSessions()[0].InternalID + 1;
            }
            return 1;
        }
    }
}
