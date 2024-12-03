using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Riptide;

namespace PowerLevelNetwork.Utils
{
    public class ClientCalculate
    {
    //Output the information in our ClientStorage to run comparisons
    //RESULTS:
    //What is the OVERALL total of the cards (totals.sum)
    //Do all of the card suits match (foreach in types => if (i == a){int match++} if(match == length){matching = true;})
    //How many SPECIAL cards are there (foreach in types => if (i == special){int power++})
    //How many card are there in general (totals.count)
    public static int TotalCalc(ClientStorage storage)
    {
        int overall = storage.totals.Sum();
        return overall;
    }

    public static int CountCalc(ClientStorage storage)
    {
        int length = storage.totals.Count;
        return length;
    }

    public static bool MatchCalc(ClientStorage storage)
    {
        int previous_i = -1;
        int current_i;
        int counted_i = 0;
        int total = storage.types.Count;
        foreach(int i in storage.types)
        {
            current_i = i;
            if(previous_i == -1)
            {
                counted_i++;
                previous_i = current_i;
            }
            if(current_i == previous_i)
            {
                counted_i++;
                previous_i = current_i;
            }
            else
            {
                break;
            }
        }
        if(counted_i >= total || total == 1)
            return true;
        else
            return false;
    }

    public static int SpecialCalc(ClientStorage storage)
    {
        int special = 2;
        int counted_i = 0;
        foreach(int i in storage.types)
            if(i == special)
            {counted_i++;}
        return counted_i;
    }

    }

    public struct ClientInfo
    {
    public string name;
    public List<int> cards;

        public ClientInfo(string username, List<int> usercards)
        {
            name = username;
            cards = usercards;
        }

        public void OutName(out string username)
        {
            username = name;
        }

        public void OutCards(out List<int> usercards)
        {
            usercards = cards;
        }
    }

    public struct ClientStorage
    {
    public List<int> types;
    public List<int> totals;

        public ClientStorage(List<int> uservalues, List<int> usertotals)
        {
            types = uservalues;
            totals = usertotals;
        }

        public void OutValues(out List<int> uservalues)
        {
            uservalues = types;
        }

        public void OutTotals(out List<int> usertotals)
        {
            usertotals = totals;
        }
    }

    public struct ClientResults
    {
    public int result_T;
    public int result_C;
    public int result_S;
    public bool result_M;

        public ClientResults(int T, int C, int S, bool M)
        {
            result_T = T;
            result_C = C;
            result_S = S;
            result_M = M;
        }
    }

}